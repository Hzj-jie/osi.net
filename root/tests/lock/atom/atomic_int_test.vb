
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class atomic_int_test
    Inherits atom_test

    Private Class atomic_int_case
        Inherits atom_case

        Private ReadOnly a As atomic_int = Nothing

        Public Sub New()
            a = New atomic_int()
        End Sub

        Public Overrides Function run() As Boolean
            a.increment()
            Return True
        End Function

        Public Function result() As Int32
            Return a.get()
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atomic_int_case()
    End Function

    Protected Overrides Sub validate(ByVal ac As atom_case)
        assert(ac IsNot Nothing)
        assert(TypeOf ac Is atomic_int_case)
        assertion.equal(ac.direct_cast_to(Of atomic_int_case)().result(), round * thread_count)
    End Sub
End Class
