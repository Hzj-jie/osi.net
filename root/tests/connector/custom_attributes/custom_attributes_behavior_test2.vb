
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

' This test shows the attributes are generated at each time it involved.
Public Class custom_attributes_behavior_test2
    Inherits [case]

    Private Shared ReadOnly i As atomic_int

    Shared Sub New()
        i = New atomic_int()
    End Sub

    Private Class test
        Inherits Attribute

        Public ReadOnly i As Int32

        Public Sub New()
            custom_attributes_behavior_test2.i.increment()
            Me.i = +(custom_attributes_behavior_test2.i)
        End Sub
    End Class

    <test()>
    Private Class C
    End Class

    <test()>
    Private Class C2
    End Class

    <test()>
    Private Class C3
    End Class

    Public Overrides Function run() As Boolean
        assertion.equal(+i, 0)
        Dim a As test = Nothing
        assertion.is_true(GetType(C).custom_attribute(Of test)(a))
        assertion.equal(a.i, 1)
        assertion.is_true(GetType(C2).custom_attribute(Of test)(a))
        assertion.equal(a.i, 2)
        assertion.is_true(GetType(C3).custom_attribute(Of test)(a))
        assertion.equal(a.i, 3)
        assertion.equal(+i, 3)
        assertion.is_true(GetType(C).custom_attribute(Of test)(a))
        assertion.equal(a.i, 4)
        assertion.is_true(GetType(C2).custom_attribute(Of test)(a))
        assertion.equal(a.i, 5)
        assertion.is_true(GetType(C3).custom_attribute(Of test)(a))
        assertion.equal(a.i, 6)
        assertion.equal(+i, 6)
        Return True
    End Function
End Class
