
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure

Public NotInheritable Class delegate_case
    Inherits [case]

    Private ReadOnly r As Func(Of Boolean)

    Public Sub New(ByVal v As Action)
        assert(v IsNot Nothing)
        r = Function() As Boolean
                v()
                Return True
            End Function
    End Sub

    Public Sub New(ByVal f As Func(Of Boolean))
        assert(f IsNot Nothing)
        r = f
    End Sub

    Public Overrides Function run() As Boolean
        Return r()
    End Function
End Class

Public NotInheritable Class delegate_event_comb_case
    Inherits event_comb_case

    Private ReadOnly r As Func(Of event_comb)

    Public Sub New(ByVal f As Func(Of event_comb))
        assert(f IsNot Nothing)
        r = f
    End Sub

    Public Overrides Function create() As event_comb
        Return r()
    End Function
End Class
