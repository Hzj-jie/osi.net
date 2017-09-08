
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _object_extensions
    <Extension()> Public Function get_type_or_null(ByVal this As Object) As Type
        Return If(this Is Nothing, Nothing, this.GetType())
    End Function

    <Extension()> Public Function is_null_or_true(ByVal this As Object) As Boolean
        Dim r As Boolean = Nothing
        Return this Is Nothing OrElse
               (direct_cast(this, r) AndAlso r)
    End Function

    <Extension()> Public Function is_not_null_and_true(ByVal this As Object) As Boolean
        Dim r As Boolean = Nothing
        Return Not this Is Nothing AndAlso
               direct_cast(this, r) AndAlso
               r
    End Function
End Module
