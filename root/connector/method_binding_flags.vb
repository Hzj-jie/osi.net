
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Public Structure method_binding_flags
    Private ReadOnly r As BindingFlags

    Public Sub New(ByVal i As BindingFlags)
        r = i Or BindingFlags.InvokeMethod
    End Sub

    Public Shared Widening Operator CType(ByVal this As method_binding_flags) As BindingFlags
        Return this.r
    End Operator

    Public Shared Widening Operator CType(ByVal this As BindingFlags) As method_binding_flags
        Return New method_binding_flags(this)
    End Operator
End Structure
