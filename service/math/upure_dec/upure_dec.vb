
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class upure_dec
    Private ReadOnly d As big_uint

    Public Sub New()
        d = New big_uint()
    End Sub

    Public Sub New(ByVal d As Double)
        Me.New()
        replace_by(d)
    End Sub

    Public Sub New(ByVal d As Single)
        Me.New()
        replace_by(d)
    End Sub

    Public Sub replace_by(ByVal d As Double)
        assert(d < 1)
        assert(d > 0)
        Me.d.replace_by(1 / d)
    End Sub

    Public Sub replace_by(ByVal d As Single)
        replace_by(CDbl(d))
    End Sub
End Class
