
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class type_attribute_signal_test
    Inherits [case]

    <type_attribute()>
    Private Class C1
        Shared Sub New()
            type_attribute.of(Of C1)().set(1)
        End Sub
    End Class

    <type_attribute()>
    Private Class C2
        Shared Sub New()
            type_attribute.of(Of C2)().set(2)
        End Sub
    End Class

    <type_attribute()>
    Private Class C3
        Shared Sub New()
            type_attribute.of(Of C3)().set(3)
        End Sub
    End Class

    <type_attribute()>
    Private Class C4
        Shared Sub New()
            type_attribute.of(Of C4).forward_from(Of C3)()
        End Sub
    End Class

    <type_attribute(type_attribute.init_mode.once)>
    Private Class C5
        Implements type_attribute.signal

        Private ReadOnly a As type_attribute

        Shared Sub New()
            type_attribute.of(Of C5)().set(5)
        End Sub

        Public Sub New(ByVal o As Object)
            assert(Not o Is Nothing)
            a = type_attribute.of(o)
        End Sub

        Public Function attribute() As type_attribute Implements type_attribute.signal.attribute
            Return a
        End Function
    End Class

    <type_attribute(type_attribute.init_mode.once)>
    Private Class C6
        Implements type_attribute.signal

        Private ReadOnly a As type_attribute

        Public Sub New(ByVal o As Object)
            assert(Not o Is Nothing)
            a = type_attribute.of(o)
        End Sub

        Public Function attribute() As type_attribute Implements type_attribute.signal.attribute
            Return a
        End Function
    End Class

    Public Overrides Function run() As Boolean
        assertion.equal(type_attribute.of(Of C1)().get(Of Int32), 1)
        assertion.equal(type_attribute.of(Of C2)().get(Of Int32), 2)
        assertion.equal(type_attribute.of(Of C3)().get(Of Int32), 3)
        assertion.equal(type_attribute.of(Of C4)().get(Of Int32), 3)
        assertion.equal(type_attribute.of(Of C5)().get(Of Int32)(), 5)
        assertion.is_false(type_attribute.has(Of C6)())

        assertion.equal(type_attribute.of(New C1()).get(Of Int32)(), 1)
        assertion.equal(type_attribute.of(New C2()).get(Of Int32)(), 2)
        assertion.equal(type_attribute.of(New C3()).get(Of Int32)(), 3)
        assertion.equal(type_attribute.of(New C4()).get(Of Int32)(), 3)

        assertion.equal(type_attribute.of(New C5(New C1())).get(Of Int32)(), 1)
        assertion.equal(type_attribute.of(New C5(New C2())).get(Of Int32)(), 2)
        assertion.equal(type_attribute.of(New C5(New C3())).get(Of Int32)(), 3)
        assertion.equal(type_attribute.of(New C5(New C4())).get(Of Int32)(), 3)

        assertion.equal(type_attribute.of(New C6(New C5(New C1()))).get(Of Int32)(), 1)
        assertion.equal(type_attribute.of(New C6(New C5(New C2()))).get(Of Int32)(), 2)
        assertion.equal(type_attribute.of(New C6(New C5(New C3()))).get(Of Int32)(), 3)
        assertion.equal(type_attribute.of(New C6(New C5(New C4()))).get(Of Int32)(), 3)
        Return True
    End Function
End Class
