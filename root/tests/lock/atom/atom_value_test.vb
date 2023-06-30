
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class atom_value_test
    Inherits atom_test

    Private Class atom_value_case
        Inherits atom_case

        Public Const increment As Int32 = 7
        Private ReadOnly a As atom(Of Int32) = Nothing

        Public Sub New()
            a = New atom(Of Int32)()
        End Sub

        Public Overrides Function run() As Boolean
            a.modify(Sub(ByRef i As Int32)
                         i += increment
                     End Sub)
            Return True
        End Function

        Public Function result() As Int32
            Return a.get()
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atom_value_case()
    End Function

    Protected Overrides Sub validate(ByVal ac As atom_case)
        assert(Not ac Is Nothing)
        assert(TypeOf ac Is atom_value_case)
        assertion.equal(ac.direct_cast_to(Of atom_value_case)().result(), atom_value_case.increment * round * thread_count)
    End Sub
End Class
