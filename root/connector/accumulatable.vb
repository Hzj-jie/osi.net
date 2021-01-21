
Option Explicit On
Option Infer Off
Option Strict On

Public Class accumulatable(Of T, T2, RT)
    Public Shared ReadOnly v As Boolean
    Public Shared ReadOnly ex As Exception

    Shared Sub New()
        Dim i As Object = Nothing
        Dim j As Object = Nothing
        i = alloc(Of T)()
        j = alloc(Of T2)()
        Try
            Dim k As RT = Nothing
            accumulatable(Of T, T2, RT).v = cast(Of RT).from(implicit_conversions.object_add(i, j), k)
        Catch ex As Exception
            accumulatable(Of T, T2, RT).ex = ex
            accumulatable(Of T, T2, RT).v = False
        End Try
    End Sub

    Protected Sub New()
    End Sub
End Class

Public NotInheritable Class accumulatable(Of T)
    Inherits accumulatable(Of T, T, T)
End Class