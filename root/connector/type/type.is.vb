
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _is
    <Extension()> Public Function [is](ByVal this As Type, ByVal base As Type) As Boolean
        If this Is Nothing AndAlso base Is Nothing Then
            Return True
        ElseIf this Is Nothing OrElse base Is Nothing Then
            Return False
        ElseIf this Is base Then
            Return True
        ElseIf base.IsGenericTypeDefinition() AndAlso
               this.IsGenericType() AndAlso
               Not this.IsGenericTypeDefinition() Then
            Return (this.GetGenericTypeDefinition().is(base))
        ElseIf base.IsInterface() Then
            Return implement(this, base)
        Else
            Return inherit(this, base)
        End If
    End Function

    <Extension()> Public Function [is](Of T)(ByVal this As Type) As Boolean
        Return [is](this, GetType(T))
    End Function
End Module