
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class prebind_module
    Inherits bind_module(Of _do_val_ref(Of server.context, event_comb, Boolean))

    Private ReadOnly i As _do_val_ref(Of server.context, event_comb, Boolean)

    Private Sub New(ByVal i As _do_val_ref(Of server.context, event_comb, Boolean))
        MyBase.New()
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Public Shared Shadows Function [New](ByVal type As String,
                                         ByVal assembly As String,
                                         ByVal binding_flags As BindingFlags,
                                         ByVal function_name As String) As module_handle.named_module
        Return bind_module.[New](type,
                                 assembly,
                                 binding_flags,
                                 function_name,
                                 Function(ByVal invoker _
                                          As invoker(Of _do_val_ref(Of server.context, event_comb, Boolean))) _
                                         As module_handle.module
                                     assert(Not invoker Is Nothing)
                                     Dim i As _do_val_ref(Of server.context, event_comb, Boolean) = Nothing
                                     If invoker.pre_bind(i) Then
                                         Return New prebind_module(i)
                                     Else
                                         Return Nothing
                                     End If
                                 End Function)
    End Function

    Protected Overrides Function execute(ByVal context As server.context, ByRef o As event_comb) As Boolean
        Return i(context, o)
    End Function
End Class
