
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.utils

Partial Public NotInheritable Class module_binder
    Public Shared Function [New](ByVal type As String,
                                 ByVal assembly As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal function_name As String) As module_handle.named_module
        If String.IsNullOrEmpty(function_name) Then
            function_name = "process"
        End If

        Using scoped.atomic_bool(suppress.invoker_error)
            Dim m As module_handle.named_module = Nothing
            m = create_module(Of Func(Of server.context, Boolean)) _
                             (type,
                              assembly,
                              binding_flags,
                              function_name,
                              AddressOf delegate_module.[New],
                              AddressOf pre_or_post_alloc_bind)
            If m IsNot Nothing Then
                Return m
            End If

            m = create_module(Of _do_val_ref(Of server.context, event_comb, Boolean)) _
                             (type,
                              assembly,
                              binding_flags,
                              function_name,
                              AddressOf delegate_procedure_module.[New],
                              AddressOf pre_or_post_alloc_bind)
            If m IsNot Nothing Then
                Return m
            End If

            m = create_module(Of Action(Of server.context)) _
                             (type,
                              assembly,
                              binding_flags,
                              function_name,
                              AddressOf delegate_filtered_module.[New],
                              AddressOf pre_or_post_alloc_bind)
            If m IsNot Nothing Then
                Return m
            End If

            m = create_module(Of Func(Of server.context, event_comb)) _
                             (type,
                              assembly,
                              binding_flags,
                              function_name,
                              AddressOf delegate_filtered_procedure_module.[New],
                              AddressOf pre_or_post_alloc_bind)
            If m IsNot Nothing Then
                Return m
            End If
        End Using
        Return Nothing
    End Function

    Private Shared Function create_module _
            (Of delegate_type) _
            (ByVal type As String,
             ByVal assembly As String,
             ByVal binding_flags As BindingFlags,
             ByVal function_name As String,
             ByVal create As Func(Of context_filter, delegate_type, module_handle.module),
             ByVal pre_or_post_alloc_bind As _do_val_ref(Of invoker(Of delegate_type), delegate_type, Boolean)) _
            As module_handle.named_module
        assert(create IsNot Nothing)
        assert(pre_or_post_alloc_bind IsNot Nothing)

        Dim invoker As invoker(Of delegate_type) = Nothing
        If Not typeless_invoker.of(invoker).
                   with_type_name(type).
                   with_assembly_name(assembly).
                   with_binding_flags(binding_flags).
                   with_name(function_name).
                   build(invoker) Then
            Return Nothing
        End If

        Dim filter As context_filter = Nothing
        filter = context_filter.[New](invoker.method_info())
        Dim action As delegate_type = Nothing
        If Not pre_or_post_alloc_bind(invoker, action) Then
            Return Nothing
        End If
        Return New module_handle.named_module(invoker.identity(), create(filter, action))
    End Function

    Private Sub New()
    End Sub
End Class
