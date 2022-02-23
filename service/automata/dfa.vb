
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.formation

'deterministic finite automaton
Public Class dfa(Of KEY_T,
                    TRANSITION_COUNT As _int64,
                    KEY_TO_INDEX As _to_uint32(Of KEY_T),
                    RESULT_T)
    Private Shared ReadOnly trans_count As Int64
    Private Shared ReadOnly trans_index As _to_uint32(Of KEY_T)

    Shared Sub New()
        trans_count = +(alloc(Of TRANSITION_COUNT)())
        assert(trans_count > 0)
        trans_index = alloc(Of KEY_TO_INDEX)()
        assert(trans_index IsNot Nothing)
    End Sub

    Private Shared Function transition_position(ByVal k As KEY_T) As UInt32
        Dim r As UInt32 = 0
        r = trans_index(k)
        assert(r < trans_count)
        Return r
    End Function

    Private Class status
        Private ReadOnly transitions() As transition

        Public Sub New()
            ReDim transitions(trans_count - 1)
        End Sub

        Public Function transition(ByVal k() As KEY_T,
                                   ByVal pos As UInt32,
                                   ByVal result As RESULT_T) As status
            assert(pos < array_size(k))
            Dim index As UInt32 = 0
            index = transition_position(k(pos))
            If transitions(index) IsNot Nothing AndAlso
               transitions(index).action(k, pos, result) Then
                Return transitions(index).next
            Else
                Return Nothing
            End If
        End Function

        Public Function insert(ByVal k As KEY_T,
                               ByVal next_status As status,
                               ByVal action As Func(Of KEY_T(), UInt32, RESULT_T, Boolean)) As Boolean
            assert(next_status IsNot Nothing)
            If action Is Nothing Then
                Return False
            Else
                Dim index As UInt32 = 0
                index = transition_position(k)
                If transitions(index) Is Nothing Then
                    transitions(index) = New transition(next_status, action)
                    Return True
                Else
                    Return False
                End If
            End If
        End Function
    End Class

    Private Class transition
        Public ReadOnly [next] As status
        Public ReadOnly action As Func(Of KEY_T(), UInt32, RESULT_T, Boolean)

        Public Sub New(ByVal [next] As status, ByVal action As Func(Of KEY_T(), UInt32, RESULT_T, Boolean))
            assert([next] IsNot Nothing)
            assert(action IsNot Nothing)
            Me.next = [next]
            Me.action = action
        End Sub
    End Class

    Public Const start_status As UInt32 = 0
    Public Const end_status As UInt32 = 1
    Public Const first_user_status As UInt32 = end_status + 1
    Private ReadOnly statuses As vector(Of status)

    Public Sub New()
        statuses = New vector(Of status)()
        prepare_status(start_status)
        prepare_status(end_status)
    End Sub

    Private Sub prepare_status(ByVal status_id As UInt32)
        assert(status_id >= 0)
        If status_id >= statuses.size() Then
            statuses.resize(status_id + 1)
        End If
        If statuses(status_id) Is Nothing Then
            statuses(status_id) = New status()
        End If
    End Sub

    Public Function insert(ByVal current_status As UInt32,
                           ByVal transition_variable As KEY_T,
                           ByVal next_status As UInt32,
                           ByVal action As Func(Of KEY_T(), UInt32, RESULT_T, Boolean)) As Boolean
        If current_status = end_status Then
            Return False
        Else
            prepare_status(current_status)
            prepare_status(next_status)
            Return statuses(current_status).insert(transition_variable, statuses(next_status), action)
        End If
    End Function

    Public Function insert(ByVal current_status As UInt32,
                           ByVal transition_variable As KEY_T,
                           ByVal next_status As UInt32) As Boolean
        Return insert(current_status, transition_variable, next_status, Function(x, y, z) True)
    End Function

    Public Function parse(ByVal k() As KEY_T,
                          ByVal start As UInt32,
                          ByVal count As UInt32,
                          ByVal result As RESULT_T,
                          ByRef ended As Boolean) As UInt32
        Dim i As UInt32 = 0
        Dim s As status = Nothing
        s = statuses(start_status)
        For i = start To start + count - 1
            s = s.transition(k, i, result)
            If s Is Nothing Then
                ended = False
                Exit For
            ElseIf object_compare(s, statuses(end_status)) = 0 Then
                ended = True
                i += 1
                Exit For
            End If
        Next
        Return i - start
    End Function

    Public Function parse(ByVal k() As KEY_T,
                          ByVal start As UInt32,
                          ByVal result As RESULT_T,
                          ByRef ended As Boolean) As UInt32
        Return parse(k, start, array_size(k) - start, result, ended)
    End Function

    Public Function parse(ByVal k() As KEY_T, ByVal result As RESULT_T, ByRef ended As Boolean) As UInt32
        Return parse(k, 0, array_size(k), result, ended)
    End Function

    Public Function parse(ByVal k() As KEY_T, ByVal result As RESULT_T) As UInt32
        Return parse(k, result, False)
    End Function
End Class
