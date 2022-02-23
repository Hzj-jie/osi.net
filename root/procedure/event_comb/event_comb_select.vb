
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Class event_comb_select(Of T)
    Private Const default_wait_ms As Int64 = 1
    Private ReadOnly rcec As reference_count_event_comb_2
    Private ReadOnly q As qless2(Of pair(Of T, Action))
    Private ReadOnly s As vector(Of pair(Of T, Action))

    Public Sub New(ByVal init As Func(Of event_comb),
                   ByVal final As Func(Of event_comb),
                   Optional ByVal wait_ms As Int64 = default_wait_ms)
        rcec = reference_count_event_comb_2.ctor(init,
                                                 Function() As event_comb
                                                     Dim ec As event_comb = Nothing
                                                     Return New event_comb(Function() As Boolean
                                                                               ec = [select]()
                                                                               Return waitfor(ec) AndAlso
                                                                                      goto_next()
                                                                           End Function,
                                                                           Function() As Boolean
                                                                               If ec.end_result() Then
                                                                                   selected()
                                                                                   Return waitfor(wait_ms) AndAlso
                                                                                          goto_end()
                                                                               Else
                                                                                   Return False
                                                                               End If
                                                                           End Function)
                                                 End Function,
                                                 final)
        q = New qless2(Of pair(Of T, Action))()
        s = New vector(Of pair(Of T, Action))()
    End Sub

    Public Sub New(Optional ByVal wait_ms As Int64 = default_wait_ms)
        Me.New(Nothing, Nothing, wait_ms)
    End Sub

    Public Function queue(ByVal i As T) As event_comb
        Return sync_async(Sub()
                              rcec.bind()
                              Dim v As Action = Nothing
                              v = event_comb.wait()
                              Dim v2 As Action = Nothing
                              v2 = Sub()
                                       v()
                                       rcec.release()
                                   End Sub
                              q.emplace(pair.emplace_of(copy_no_error(i), v2))
                          End Sub)
    End Function

    Public Function empty() As Boolean
        Return q.empty()
    End Function

    Public Function size() As UInt32
        Return q.size()
    End Function

    Protected Overridable Function [select](ByVal i As T) As Boolean
        Return assert(False)
    End Function

    Protected Sub foreach(ByVal d As Func(Of pair(Of T, Action), Boolean))
        assert(d IsNot Nothing)
        assert(Not q.empty())
        Dim s As Int64 = 0
        s = q.size()
        While s > 0
            Dim p As pair(Of T, Action) = Nothing
            assert(q.pop(p))
            assert(p IsNot Nothing)
            If p.second IsNot Nothing Then
                If d(p) Then
                    [select](p)
                Else
                    q.emplace(p)
                End If
            End If
            s -= 1
        End While
    End Sub

    Protected Sub sync_select()
        foreach(Function(p As pair(Of T, Action)) As Boolean
                    Return [select](p.first)
                End Function)
    End Sub

    Protected Overridable Function [select]() As event_comb
        Return sync_async(AddressOf sync_select)
    End Function

    Protected Sub [select](ByVal i As pair(Of T, Action))
        assert(i IsNot Nothing)
        assert(i.second IsNot Nothing)
        'must use emplace_back to ensure the pair in q and s is the same instance
        s.emplace_back(i)
    End Sub

    Protected Function selected_count() As UInt32
        Return s.size()
    End Function

    Private Sub selected()
        If Not s.empty() Then
            For i As UInt32 = 0 To s.size() - uint32_1
                assert(s(i).second IsNot Nothing)
                s(i).second()
                s(i).second = Nothing
            Next
            s.clear()
        End If
    End Sub
End Class
