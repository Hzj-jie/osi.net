
Imports osi.root.connector
Imports osi.root.utt

Public Class implement_test
    Inherits [case]

    Private Interface I1
        Sub i1()
    End Interface

    Private Interface I2
        Sub i2()
    End Interface

    Private Interface I3
        Inherits I1, I2
    End Interface

    Private Class M
        Implements I1, I2

        Public Sub i1() Implements i1.i1
        End Sub

        Public Sub i2() Implements I2.i2
        End Sub
    End Class

    Private Class M2
        Inherits M
        Implements I3
    End Class

    Public Overrides Function run() As Boolean
        Dim m As M = Nothing
        m = New M()
        Dim i3 As I3 = Nothing
        assertion.is_false(cast(m, i3))
        assertion.is_null(i3)
        Dim m2 As M2 = Nothing
        m2 = New M2()
        assertion.is_true(cast(m2, i3))
        assertion.is_not_null(i3)

        assertion.is_true(m.GetType().GetInterface("I1") Is GetType(I1))
        assertion.is_true(m.GetType().GetInterface("I2") Is GetType(I2))
        assertion.is_false(m.GetType().GetInterface("I3") Is GetType(I3))
        assertion.is_null(m.GetType().GetInterface("I3"))
        assertion.equal(array_size(m.GetType().GetInterfaces()), CUInt(2))
        assertion.is_true(m2.GetType().GetInterface("I1") Is GetType(I1))
        assertion.is_true(m2.GetType().GetInterface("I2") Is GetType(I2))
        assertion.is_true(m2.GetType().GetInterface("I3") Is GetType(I3))
        assertion.equal(array_size(m2.GetType().GetInterfaces()), CUInt(3))
        Return True
    End Function
End Class
