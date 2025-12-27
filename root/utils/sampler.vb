
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class sampler
    Public Shared ReadOnly all As New sampler(1)

    Private ReadOnly sr As Double

    Public Sub New(ByVal sr As Double)
        assert(sr >= 0 AndAlso sr <= 1)
        Me.sr = sr
    End Sub

    Public Function sampled() As Boolean
        If sr = 1 Then
            Return True
        End If
        Return thread_random.of_double.larger_or_equal_than_0_and_less_than_1() < sr
    End Function

    Public Function not_sampled() As Boolean
        Return Not sampled()
    End Function
End Class
