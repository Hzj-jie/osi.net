
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class filtered_prebind_module
    Inherits filtered_module

    Private ReadOnly n As String
    Private ReadOnly i As Func(Of server.context, event_comb)

    Private Sub New(ByVal name As String, ByVal mi As MemberInfo, ByVal i As Func(Of server.context, event_comb))
        MyBase.New(mi)
        Me.n = name
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Public Shared Shadows Function [New](ByVal type As String,
                                         ByVal assembly As String,
                                         ByVal binding_flags As BindingFlags,
                                         ByVal function_name As String) As filtered_prebind_module
        Dim invoker As invoker(Of Func(Of server.context, event_comb)) = Nothing
        bind_module.bind(type, assembly, binding_flags, function_name, invoker)
        Dim f As Func(Of server.context, event_comb) = Nothing
        If Not invoker Is Nothing AndAlso invoker.pre_bind(f) Then
            Return New filtered_prebind_module(invoker.identity(), invoker.method_info(), f)
        Else
            Return Nothing
        End If
    End Function

    Protected Overrides Function execute(ByVal context As server.context) As event_comb
        Return i(context)
    End Function

    Public Overrides Function name() As String
        Return n
    End Function
End Class
