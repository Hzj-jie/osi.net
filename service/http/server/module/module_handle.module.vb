
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.argument

Partial Public Class module_handle
    Public Interface [module]
        Function context_received(ByVal context As server.context) As Boolean
    End Interface

    Private Shared ReadOnly module_constructor As vector(Of Func(Of String, String, BindingFlags, String, [module]))

    Shared Sub New()
        _new(module_constructor)
        module_constructor.emplace_back(AddressOf prebind_module.[New])
        module_constructor.emplace_back(AddressOf filtered_prebind_module.[New])
    End Sub

    Public Function add(ByVal m As [module]) As Boolean
        If m Is Nothing Then
            Return False
        Else
            v.push_back(m)
            Return True
        End If
    End Function

    Public Function add(ByVal type As String,
                        ByVal assembly As String,
                        ByVal binding_flags As BindingFlags,
                        ByVal function_name As String) As Boolean
        For i As UInt32 = 0 To module_constructor.size() - uint32_1
            If add(module_constructor(i)(type, assembly, binding_flags, function_name)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function add(ByVal v As var) As Boolean
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

        Return add(v(p_type), v(p_assembly), bf, v(p_function_name))
    End Function
End Class
