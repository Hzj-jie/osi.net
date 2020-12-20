
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _unordered_set
    <Extension()> Public Function stream(Of T)(ByVal this As unordered_set(Of T)) As stream(Of T)
        Return New stream(Of T).container(Of unordered_set(Of T))(this)
    End Function
End Module
