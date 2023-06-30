
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.constants

Public NotInheritable Class thread_random
    Private NotInheritable Class _r_holder
        'the Random class is not thread-safe,
        'when several threads get random number from one Random object at the same time, it will return 0
        <ThreadStatic> Public Shared r As Random

        Private Sub New()
        End Sub
    End Class

    Public Shared Function ref() As Random
        If _r_holder.r Is Nothing Then
            Const offset As Int32 = 6
            _r_holder.r = New Random(CInt(
                (Threading.Thread.CurrentThread().ManagedThreadId() And ((1 << offset) - 1)) +
                ((Now().milliseconds() << offset) And max_int32)))
        End If
        Return _r_holder.r
    End Function

    Public NotInheritable Class of_double
        Public Shared Function larger_than_0_and_less_or_equal_than_1() As Double
            Dim r As Double = Nothing
            r = ref().NextDouble()
            If r = 0 Then
                Return 1
            End If
            Return r
        End Function

        Public Shared Function larger_or_equal_than_0_and_less_than_1() As Double
            Return ref().NextDouble()
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
