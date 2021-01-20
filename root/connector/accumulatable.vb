
Option Explicit On
Option Infer Off
Option Strict On

Public Class accumulatable(Of T, T2, RT)
    Public Shared ReadOnly v As Boolean = executor.instance.v
    Public Shared ReadOnly ex As Exception = executor.instance.ex

    Private NotInheritable Class executor
        Public Shared ReadOnly instance As executor = New executor()

        Public ReadOnly v As Boolean
        Public ReadOnly ex As Exception

        Private Sub New()
            Dim i As Object = Nothing
            Dim j As Object = Nothing
            i = alloc(Of T)()
            j = alloc(Of T2)()
            Try
                Dim k As RT = Nothing
                Me.v = cast(Of RT).from(implicit_conversions.object_add(i, j), k)
            Catch ex As Exception
                Me.ex = ex
                Me.v = False
            End Try
        End Sub
    End Class

    Protected Sub New()
    End Sub
End Class

Public NotInheritable Class accumulatable(Of T)
    Inherits accumulatable(Of T, T, T)

    Private Sub New()
    End Sub
End Class