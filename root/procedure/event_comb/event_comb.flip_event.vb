
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.event

Partial Public Class event_comb
    Public Function repeat_when_high(Of T As flip_event)(ByVal f As flip_event.[New](Of T)) As T
        assert(f IsNot Nothing)
        Dim c As event_comb = Nothing
        Dim e As event_comb = Nothing
        Return f(flip_event.events.of(Sub()
                                          e = New event_comb(Function() As Boolean
                                                                 c = renew()
                                                                 Return waitfor(c)
                                                             End Function)
                                          assert_begin(e)
                                      End Sub,
                                      Sub()
                                          If e IsNot Nothing Then
                                              e.cancel()
                                          End If
                                          If c IsNot Nothing Then
                                              c.cancel()
                                          End If
                                      End Sub))
    End Function
End Class
