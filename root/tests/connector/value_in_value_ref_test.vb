
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class value_in_value_ref_test
    Private Structure s
        Private v As Int32

        Public Sub f()
            v = 1
        End Sub

        Public Function g() As Int32
            Return v
        End Function
    End Structure

    Private Shared Sub f(ByRef x As s)
        g(x)
    End Sub

    Private Shared Sub g(ByRef x As s)
        x.f()
    End Sub

    <test>
    Private Shared Sub run()
        Dim s As s
        assertion.equal(s.g(), 0)
        f(s)
        assertion.equal(s.g(), 1)
    End Sub
End Class
