
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports invoker_type = osi.root.utils.invoker(Of osi.root.delegates._do_val_ref(Of osi.service.http.server.context,
                                                                                   osi.root.procedure.event_comb,
                                                                                   Boolean))

Public NotInheritable Class module_handle
    Private ReadOnly i As _do_val_ref(Of server.context, event_comb, Boolean)

    Private Sub New(ByVal i As _do_val_ref(Of server.context, event_comb, Boolean))
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Public Shared Function [New](ByVal invoker As invoker_type) As module_handle
        If invoker Is Nothing Then
            Return Nothing
        End If

        Dim v As _do_val_ref(Of server.context, event_comb, Boolean) = Nothing
        If invoker.pre_bind(v) Then
            Return New module_handle(v)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function [New](ByVal type As String,
                                 ByVal assembly As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal function_name As String) As module_handle
        If String.IsNullOrEmpty(function_name) Then
            function_name = "process"
        End If

        Dim invoker As invoker_type = Nothing
        If typeless_invoker.[New](type, assembly, binding_flags, function_name, invoker) Then
            Return [New](invoker)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function [New](ByVal v As var) As module_handle
        If v Is Nothing Then
            Return Nothing
        End If

        Const p_type As String = "type"
        Const p_assembly As String = "assembly"
        Const p_binding_flags As String = "binding-flags"
        Const p_function_name As String = "function"
        v.bind(p_type, p_assembly, p_binding_flags, p_function_name)
        Dim bf As BindingFlags = Nothing
        If Not bf.method_from_str(v(p_binding_flags)) Then
            Return Nothing
        End If

        Return [New](v(p_type), v(p_assembly), bf, v(p_function_name))
    End Function

    Public Function context_received(ByVal ctx As server.context) As Boolean
        Dim ec As event_comb = Nothing
        If i(ctx, ec) Then
            procedure_handle.process_context(ctx, ec)
            Return True
        Else
            Return False
        End If
    End Function
End Class
