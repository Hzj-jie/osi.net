
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utt

Public Class atom_reference_test
    Inherits atom_test

    Private Class atom_reference_case
        Inherits atom_case

        Private ReadOnly a As atom(Of StringBuilder) = Nothing

        Public Sub New()
            a = New atom(Of StringBuilder)(New StringBuilder())
        End Sub

        Public Overrides Function run() As Boolean
            a.modify(Sub(ByRef s As StringBuilder)
                         assert(Not s Is Nothing)
                         Dim co As Boolean = True
                         Dim i As Int32 = 0
                         While co
                             If strlen(s) <= i Then
                                 s.Append(character._0)
                             End If
                             If s(i) = character._9 Then
                                 co = True
                                 s(i) = character._0
                             Else
                                 assert(s(i) >= character._0 AndAlso s(i) < character._9)
                                 co = False
                                 s(i) = Convert.ToChar(Convert.ToInt32(s(i)) + 1)
                             End If
                             i += 1
                         End While
                     End Sub)
            Return True
        End Function

        Public Function result() As String
            Return Convert.ToString(a.get())
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atom_reference_case()
    End Function

    Protected Overrides Sub validate(ByVal ac As atom_case)
        assert(Not ac Is Nothing)
        assert(TypeOf ac Is atom_reference_case)
        assertion.equal(ac.direct_cast_to(Of atom_reference_case)().result().reverse(),
                     Convert.ToString(round * thread_count))
    End Sub
End Class
