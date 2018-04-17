
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

' A derived filtered_module implementation to get the execute() function from a pre-binding static function.
Public NotInheritable Class filtered_prebind_module
    Inherits filtered_module

    Private ReadOnly i As Func(Of server.context, event_comb)

    Private Sub New(ByVal mi As MemberInfo, ByVal i As Func(Of server.context, event_comb))
        MyBase.New(mi)
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
                                          As invoker(Of Func(Of server.context, event_comb))) _
                                         As module_handle.module
                                     assert(Not invoker Is Nothing)
                                     Dim i As Func(Of server.context, event_comb) = Nothing
                                     If invoker.pre_bind(i) Then
                                         Return New filtered_prebind_module(invoker.method_info(), i)
                                     Else
                                         Return Nothing
                                     End If
                                 End Function)
    End Function

    Protected Overrides Function execute(ByVal context As server.context) As event_comb
        Return i(context)
    End Function
End Class
