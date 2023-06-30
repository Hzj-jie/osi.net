
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.event

Partial Public Class event_comb
    Public Function repeat_when_high(Of T As flip_event)(ByVal f As flip_event.[New](Of T)) As T
        assert(Not f Is Nothing)
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
                                          If Not e Is Nothing Then
                                              e.cancel()
                                          End If
                                          If Not c Is Nothing Then
                                              c.cancel()
                                          End If
                                      End Sub))
    End Function
End Class
