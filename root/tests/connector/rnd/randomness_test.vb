
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils

Public Class randomness_test
    Inherits [case]

    Private Shared ReadOnly lower As Int32 = 0
    Private Shared ReadOnly upper As Int32 = 0

    Shared Sub New()
        lower = 10
        upper = 1010
        If isreleasebuild() Then
            lower *= 20
            upper *= 20
        End If
    End Sub

    Private Shared Function rnd_case(ByVal d As _do(Of Int32, Int32, Int32)) As Boolean
        assert(Not d Is Nothing)
        Dim s() As Boolean = Nothing
        ReDim s(upper - lower - 1)
        For i As Int32 = 0 To (upper - lower) * 10 - 1
            s(d(lower, upper) - lower) = True
        Next
        Dim unset_count As Int32 = 0
        For i As Int32 = lower To upper - 1
            If Not s(i - lower) Then
                unset_count += 1
            End If
        Next
        assertion.less_or_equal(CDbl(unset_count) / (upper - lower), If(isreleasebuild(), 0.00025, 0.001))
        Return True
    End Function

    Private Shared Function rnd_int_case() As Boolean
        Return rnd_case(Function(ByRef x, ByRef y) rnd_int(x, y))
    End Function

    Private Shared Function rnd_uint_case() As Boolean
        Return rnd_case(Function(ByRef x, ByRef y) rnd_uint(x, y))
    End Function

    Public Overrides Function run() As Boolean
        Return rnd_int_case() AndAlso
               rnd_uint_case()
    End Function
End Class
