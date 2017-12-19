
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class bind_module
    Public Shared Sub bind(Of delegate_type)(ByVal type As String,
                                             ByVal assembly As String,
                                             ByVal binding_flags As BindingFlags,
                                             ByVal function_name As String,
                                             ByRef o As invoker(Of delegate_type))
        o = bind_module(Of delegate_type).bind(type, assembly, binding_flags, function_name)
    End Sub
End Class

Public MustInherit Class bind_module(Of delegate_type)
    Implements module_handle.module

    Private ReadOnly n As String

    Protected Sub New(ByVal name As String)
        n = name
    End Sub

    Public Shared Function bind(ByVal type As String,
                                ByVal assembly As String,
                                ByVal binding_flags As BindingFlags,
                                ByVal function_name As String) As invoker(Of delegate_type)
        If String.IsNullOrEmpty(function_name) Then
            function_name = "process"
        End If

        Dim invoker As invoker(Of delegate_type) = Nothing
        If typeless_invoker.[New](type, assembly, binding_flags, function_name, invoker) Then
            Return invoker
        Else
            Return Nothing
        End If
    End Function

    Protected Shared Function [New](Of R As bind_module(Of delegate_type)) _
                                   (ByVal type As String,
                                    ByVal assembly As String,
                                    ByVal binding_flags As BindingFlags,
                                    ByVal function_name As String,
                                    ByVal creator As Func(Of invoker(Of delegate_type), R)) As R
        assert(Not creator Is Nothing)
        Dim invoker As invoker(Of delegate_type) = Nothing
        invoker = bind(type, assembly, binding_flags, function_name)
        If invoker Is Nothing Then
            Return Nothing
        Else
            Return creator(invoker)
        End If
    End Function

    Protected MustOverride Function execute(ByVal context As server.context, ByRef o As event_comb) As Boolean

    Public Function context_received(ByVal context As server.context) As Boolean _
                                    Implements module_handle.module.context_received
        Return procedure_handle.process_context(context, AddressOf execute)
    End Function

    Public Function name() As String Implements module_handle.module.name
        Return n
    End Function
End Class
