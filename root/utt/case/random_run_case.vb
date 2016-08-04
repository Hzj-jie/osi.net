
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils

' MustInherit to avoid utt-host to load this empty case.
Public MustInherit Class random_run_case
    Inherits [case]

    Private ReadOnly r As random_run(Of Boolean)

    Protected Sub New()
        r = New random_run(Of Boolean)()
    End Sub

    Public NotOverridable Overrides Function run() As Boolean
        Return r.select()
    End Function

    Public Sub insert_calls(ByVal ParamArray cs() As pair(Of Double, Func(Of Boolean)))
        r.insert_calls(cs)
    End Sub

    Public Sub insert_call(ByVal percentage As Double, ByVal d As Func(Of Boolean))
        r.insert_call(percentage, d)
    End Sub

    Public Sub insert_call(ByVal percentage As Double, ByVal d As Action)
        assert(Not d Is Nothing)
        r.insert_call(percentage,
                      Function() As Boolean
                          d()
                          Return True
                      End Function)
    End Sub
End Class

Public Class random_run(Of RT)
    Private ReadOnly calls As vector(Of pair(Of Double, Func(Of RT))) = Nothing

    Public Sub New()
        calls = New vector(Of pair(Of Double, Func(Of RT)))()
    End Sub

    Public Sub insert_calls(ByVal ParamArray cs() As pair(Of Double, Func(Of RT)))
        assert(calls.push_back(cs))
    End Sub

    Public Sub insert_call(ByVal percentage As Double, ByVal d As Func(Of RT))
        assert(percentage >= 0 AndAlso percentage <= 1)
        assert(Not d Is Nothing)
        If percentage > 0 Then
            insert_calls(make_pair(percentage, d))
        End If
    End Sub

    Public Function [select]() As RT
        assert(Not calls.empty())
        Dim j As Double = 0
        j = rnd_double(0, 1)
        For i As Int64 = 0 To calls.size() - 1
            If j < calls(i).first Then
                Return calls(i).second()
            Else
                j -= calls(i).first
            End If
        Next
        Return Nothing
    End Function
End Class
