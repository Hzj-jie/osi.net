
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class event_comb
    Public Function repeat(ByVal c As UInt32) As event_comb
        Return repeat(c,
                      Function(ByVal i As UInt32) As event_comb
                          Return renew()
                      End Function)
    End Function

    Public Shared Function repeat(ByVal c As UInt32, ByVal f As Func(Of UInt32, event_comb)) As event_comb
        assert(f IsNot Nothing)
        Dim e As event_comb = Nothing
        Dim i As UInt32 = 0
        Return New event_comb(Function() As Boolean
                                  If Not e.end_result_or_null() Then
                                      Return False
                                  End If
                                  If i < c Then
                                      e = f(i)
                                      i += uint32_1
                                      Return waitfor(e)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function
End Class
