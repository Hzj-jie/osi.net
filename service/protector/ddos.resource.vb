
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

Public NotInheritable Class ddos
    Private Shared ReadOnly fake_insert_string As String =
                                ":_""/\<>[]{}?&^%" + guid_str() + guid_str() + ":_""/\<>[]{}?&^%"

    Private NotInheritable Class count_resource
        Private ReadOnly m As map(Of String, UInt64)
        Private ReadOnly q As queue(Of String)
        Private ReadOnly l As ref(Of event_comb_lock)
        Private ReadOnly exp As ref(Of singleentry)

        Public Sub New(ByVal len As UInt32, ByVal fake_insert_interval_ms As UInt32)
            assert(len > 0)
            l = New ref(Of event_comb_lock)()
            exp = New ref(Of singleentry)()
            m = New map(Of String, UInt64)()
            q = New queue(Of String)()
            m(fake_insert_string) = len
            For i As UInt32 = 0 To len - uint32_1
                q.push(fake_insert_string)
            Next
            begin_lifetime_event_comb(exp,
                                      Function() As Boolean
                                          Return waitfor(fake_insert_interval_ms) AndAlso
                                                 goto_next()
                                      End Function,
                                      Function() As Boolean
                                          Return waitfor(insert(fake_insert_string, Nothing)) AndAlso
                                                 goto_begin()
                                      End Function)
        End Sub

        Protected Function length() As UInt32
            Return q.size()
        End Function

        Public Function insert(ByVal s As String, ByVal r As ref(Of UInt64)) As event_comb
            Return New event_comb(Function() As Boolean
                                      If s.null_or_empty() Then
                                          Return False
                                      Else
                                          Return waitfor(l) AndAlso
                                                 goto_next()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      q.push(s)
                                      Dim it As map(Of String, UInt64).iterator = Nothing
                                      it = m.find(s)
                                      If it = m.end() Then
                                          it = m.insert(s, 1).first
                                      Else
                                          With +it
                                              .second += uint64_1
                                          End With
                                      End If
                                      eva(r, (+it).second)

                                      s = q.front()
                                      assert(q.pop())
                                      it = m.find(s)
                                      assert(it <> m.end())
                                      With +it
                                          .second -= uint64_1
                                          If .second = 0 Then
                                              m.erase(it)
                                          End If
                                      End With
                                      l.release()
                                      Return goto_end()
                                  End Function)
        End Function

        Protected Overrides Sub Finalize()
            assert(exp.mark_in_use())
            MyBase.Finalize()
        End Sub
    End Class

    Private Class percentage_resource
        Private ReadOnly m As map(Of String, UInt64)
        Private ReadOnly t As UInt64
        Private ReadOnly l As ref(Of event_comb_lock)

        Public Sub New(ByVal total As UInt64)
            assert(total > 0)
            t = total
            m = New map(Of String, UInt64)()
            l = New ref(Of event_comb_lock)()
        End Sub

        Public Function insert(ByVal s As String, ByVal r As ref(Of Double)) As event_comb
            Return New event_comb(Function() As Boolean
                                      If s.null_or_empty() Then
                                          Return False
                                      Else
                                          Return waitfor(l) AndAlso
                                                 goto_next()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      Dim it As map(Of String, UInt64).iterator = Nothing
                                      it = m.find(s)
                                      If it = m.end() Then
                                          it = m.insert(s, 1).first
                                      Else
                                          With +it
                                              .second += uint64_1
                                          End With
                                      End If
                                      eva(r, (+it).second / t)
                                      l.release()
                                      Return goto_end()
                                  End Function)
        End Function

        Public Function release(ByVal s As String) As event_comb
            Return New event_comb(Function() As Boolean
                                      If s.null_or_empty() Then
                                          Return False
                                      Else
                                          Return waitfor(l) AndAlso
                                                 goto_next()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      Dim it As map(Of String, UInt64).iterator = Nothing
                                      it = m.find(s)
                                      If it = m.end() Then
                                          Return False
                                      Else
                                          With +it
                                              .second -= uint64_1
                                              If .second = 0 Then
                                                  m.erase(it)
                                              End If
                                          End With
                                      End If
                                      l.release()
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
