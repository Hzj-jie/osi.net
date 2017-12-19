
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class bind_module
    Public Shared Function [New](Of delegate_type) _
                                (ByVal type As String,
                                 ByVal assembly As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal function_name As String,
                                 ByVal creator As Func(Of invoker(Of delegate_type), module_handle.module)) _
                                As module_handle.named_module
        Return bind_module(Of delegate_type).[New](type, assembly, binding_flags, function_name, creator)
    End Function
End Class

Public MustInherit Class bind_module(Of delegate_type)
    Implements module_handle.module

    Protected Sub New()
    End Sub

    Public Shared Function [New](ByVal type As String,
                                 ByVal assembly As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal function_name As String,
                                 ByVal creator As Func(Of invoker(Of delegate_type), module_handle.module)) _
                                As module_handle.named_module
        assert(Not creator Is Nothing)
        If String.IsNullOrEmpty(function_name) Then
            function_name = "process"
        End If

        Dim invoker As invoker(Of delegate_type) = Nothing
        If typeless_invoker.[New](type, assembly, binding_flags, function_name, invoker) Then
            assert(Not invoker Is Nothing)
            Dim m As module_handle.module = Nothing
            m = creator(invoker)
            If m Is Nothing Then
                Return Nothing
            Else
                Return New module_handle.named_module(invoker.identity(), m)
            End If
        Else
            Return Nothing
        End If
    End Function

    Protected MustOverride Function execute(ByVal context As server.context, ByRef o As event_comb) As Boolean

    Public Function context_received(ByVal context As server.context) As Boolean _
                                    Implements module_handle.module.context_received
        Return procedure_handle.process_context(context, AddressOf execute)
    End Function
End Class
