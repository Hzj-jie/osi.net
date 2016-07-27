
Imports System.Reflection

Public Module _cast
    Public Const cast_operator_binding_flags As BindingFlags = BindingFlags.Public Or BindingFlags.Static
    Public Const explicit_cast_operator As String = "op_Explicit"
    Public Const implicit_cast_operator As String = "op_Implicit"
End Module
