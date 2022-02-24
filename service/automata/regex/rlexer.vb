
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class rlexer
    Private ReadOnly rs As vector(Of regex)
    Private ReadOnly type_choice As match_choice
    Private ReadOnly word_choice As match_choice

    Public Sub New(ByVal rs As vector(Of regex),
                   ByVal type_choice As match_choice,
                   ByVal word_choice As match_choice)
        assert(Not rs Is Nothing)
        assert(rs.find(Nothing) = npos)
        Me.rs = rs
        Me.type_choice = type_choice
        Me.word_choice = word_choice
    End Sub

    Public Sub New(ByVal type_choice As match_choice,
                   ByVal word_choice As match_choice)
        Me.New(New vector(Of regex)(), type_choice, word_choice)
    End Sub

    Public Sub New()
        Me.New(New vector(Of regex)(), match_choice.greedy, match_choice.greedy)
    End Sub

    Public Function regex_count() As UInt32
        Return rs.size()
    End Function

    Public Shared Function create(ByVal rule_file As String, ByRef o As rlexer) As Boolean
        Dim r As rule = Nothing
        r = New rule()
        Dim e As rule.exporter = Nothing
        If r.parse(rule_file) AndAlso r.export(e) Then
            assert(Not e Is Nothing)
            o = e.rlexer
            assert(Not o Is Nothing)
            Return True
        End If
        Return False
    End Function
End Class
