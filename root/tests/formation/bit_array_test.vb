
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class bit_array_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New run_case(), 1024 * 1024)
    End Sub

    Private Class run_case
        Inherits [case]

        Private Const size As UInt32 = 1024 * 8 + 3
        Private ReadOnly b As bit_array
        Private ReadOnly v As vector(Of Boolean)

        Public Sub New()
            b = New bit_array()
            v = New vector(Of Boolean)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            b.resize(size)
            assertion.equal(b.size(), size)
            v.resize(size)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            If rnd_bool_trues(16) Then
                b.clear()
                v.clear()
                v.resize(size)
            End If
            Dim i As UInt32 = 0
            i = rnd_uint(0, size)
            Dim t As Boolean = False
            t = rnd_bool()
            assertion.equal(b(i), v(i))
            b(i) = t
            v(i) = t
            assertion.equal(b(i), v(i))
            assertion.equal(b(i), t)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            v.clear()
            v.shrink_to_fit()
            b.resize(0)
            Return MyBase.finish()
        End Function
    End Class
End Class
