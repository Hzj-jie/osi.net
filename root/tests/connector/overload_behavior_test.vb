
Imports osi.root.utt

Public Class overload_behavior_test
    Inherits [case]

    Private Interface I
    End Interface

    Private Class B
        Implements I
    End Class

    Private Class D
        Inherits B
    End Class

    Private Shared Function f(Of T)(ByVal i As T) As Int32
        Return 0
    End Function

    Private Shared Function f(ByVal i As I) As Int32
        Return 1
    End Function

    Private Shared Function f(ByVal i As B) As Int32
        Return 2
    End Function

    Public Overrides Function run() As Boolean
        Dim a As Object = Nothing
        assertion.equal(f(a), 0)
        Dim b As B = Nothing
        assertion.equal(f(b), 2)
        Dim c As D = Nothing
        assertion.equal(f(c), 0)
        Dim d As I = Nothing
        assertion.equal(f(d), 1)
        Return True
    End Function
End Class
