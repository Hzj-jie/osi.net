
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module _finalizer
    <Extension()> Public Function has_finalizer(ByVal this As Type) As Boolean
        Return this.finalizer() IsNot Nothing
    End Function

    <Extension()> Public Function finalizer(ByVal this As Type) As MethodInfo
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.GetMethod("Finalize",
                                  BindingFlags.NonPublic Or
                                  BindingFlags.Instance Or
                                  BindingFlags.DeclaredOnly)
        End If
    End Function
End Module