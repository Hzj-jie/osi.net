
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.commander

Public Class bytes_ragent(Of CASE_T)
    Inherits command_ragent(Of CASE_T)

    Public Sub New(ByVal r As Func(Of ref(Of Byte()), event_comb))
        MyBase.New()
        start(r)
    End Sub

    Private Sub start(ByVal r As Func(Of ref(Of Byte()), event_comb))
        assert(Not r Is Nothing)
        Dim p As ref(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        p.renew()
                                        ec = r(p)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If ec.end_result() Then
                                            Dim c As command = Nothing
                                            If bytes_serializer.from_bytes(+p, c) Then
                                                MyBase.push(c)
                                            End If
                                            Return goto_begin()
                                        Else
                                            Return False
                                        End If
                                    End Function))
    End Sub
End Class
