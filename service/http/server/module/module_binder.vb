
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class module_binder
    Public Shared Function [New](ByVal type As String,
                                 ByVal assembly As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal function_name As String) As module_handle.named_module
        If String.IsNullOrEmpty(function_name) Then
            function_name = "process"
        End If

        Dim m As module_handle.named_module = Nothing
        m = create_module(Of Func(Of server.context, Boolean)) _
                         (type,
                          assembly,
                          binding_flags,
                          function_name,
                          AddressOf delegate_module.[New],
                          AddressOf _invoker.post_allocate_bind)
        If Not m Is Nothing Then
            Return m
        End If

        m = create_module(Of Action(Of server.context)) _
                         (type, assembly,
                          binding_flags,
                          function_name,
                          AddressOf delegate_filtered_module.[New],
                          AddressOf _invoker.post_allocate_bind)
        If Not m Is Nothing Then
            Return m
        End If

        m = create_module(Of Func(Of server.context, event_comb)) _
                         (type,
                          assembly,
                          binding_flags,
                          function_name,
                          AddressOf delegate_filtered_procedure_module.[New],
                          AddressOf _invoker.post_allocate_bind)
        If Not m Is Nothing Then
            Return m
        End If

        Return Nothing
    End Function

    Private Shared Function create_module(Of delegate_type) _
                                         (ByVal type As String,
                                          ByVal assembly As String,
                                          ByVal binding_flags As BindingFlags,
                                          ByVal function_name As String,
                                          ByVal create As Func(Of MethodInfo, delegate_type, module_handle.module),
                                          ByVal post_bind As Func(Of invoker(Of delegate_type), delegate_type)) _
                                         As module_handle.named_module
        assert(Not create Is Nothing)
        assert(Not post_bind Is Nothing)

        Dim invoker As invoker(Of delegate_type) = Nothing
        If Not typeless_invoker.[New](type, assembly, binding_flags, function_name, invoker) Then
            Return Nothing
        End If
        If Not invoker.valid() Then
            Return Nothing
        End If

        Dim action As delegate_type = Nothing
        If invoker.pre_bind(action) Then
            Return New module_handle.named_module(invoker.identity(), create(invoker.method_info(), action))
        End If

        Return New module_handle.named_module(invoker.identity(), create(invoker.method_info(), post_bind(invoker)))
    End Function

    Private NotInheritable Class delegate_module
        Implements module_handle.module

        Private ReadOnly filter As context_filter
        Private ReadOnly action As Func(Of server.context, Boolean)

        Private Sub New(ByVal mi As MethodInfo, ByVal action As Func(Of server.context, Boolean))
            assert(Not action Is Nothing)
            Me.filter = context_filter.[New](mi)
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal mi As MethodInfo,
                                     ByVal action As Func(Of server.context, Boolean)) As module_handle.module
            Return New delegate_module(mi, action)
        End Function

        Public Function context_received(ByVal context As server.context) As Boolean _
                                        Implements module_handle.module.context_received
            Return filter.select(context) AndAlso action(context)
        End Function
    End Class

    Private NotInheritable Class delegate_filtered_module
        Inherits filtered_module

        Private ReadOnly action As Action(Of server.context)

        Private Sub New(ByVal mi As MethodInfo, ByVal action As Action(Of server.context))
            MyBase.New(context_filter.[New](mi))
            assert(Not action Is Nothing)
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal mi As MethodInfo,
                                     ByVal action As Action(Of server.context)) As module_handle.module
            Return New delegate_filtered_module(mi, action)
        End Function

        Protected Overrides Sub process(ByVal context As server.context)
            action(context)
        End Sub
    End Class

    Private NotInheritable Class delegate_filtered_procedure_module
        Inherits filtered_procedure_module

        Private ReadOnly action As Func(Of server.context, event_comb)

        Private Sub New(ByVal mi As MethodInfo, ByVal action As Func(Of server.context, event_comb))
            MyBase.New(context_filter.[New](mi))
            assert(Not action Is Nothing)
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal mi As MethodInfo,
                                     ByVal action As Func(Of server.context, event_comb)) As module_handle.module
            Return New delegate_filtered_procedure_module(mi, action)
        End Function

        Protected Overrides Function execute(ByVal context As server.context) As event_comb
            Return action(context)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
