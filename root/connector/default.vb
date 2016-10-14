
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class [default](Of T)
    Public Shared ReadOnly null As T

    Shared Sub New()
        null = Nothing
    End Sub

    Private Class disposer_retriever
        Public Shared ReadOnly D As Action(Of T)

        Shared Sub New()
            If GetType(T).inherit(GetType(Stream)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            Dim s As Stream = Nothing
                            s = cast(Of Stream)(x)
                            assert(Not s Is Nothing)
                            s.Flush()
                            s.Close()
                            s.Dispose()
                        End If
                    End Sub
            ElseIf GetType(T).inherit(GetType(WaitHandle)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            cast(Of WaitHandle)(x).Close()
                        End If
                    End Sub
            ElseIf GetType(T).inherit(GetType(TcpClient)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            cast(Of TcpClient)(x).Close()
                        End If
                    End Sub
            ElseIf GetType(T).inherit(GetType(UdpClient)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            cast(Of UdpClient)(x).Close()
                        End If
                    End Sub
            ElseIf GetType(T).inherit(GetType(Socket)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            cast(Of Socket)(x).Close()
                        End If
                    End Sub
            ElseIf GetType(T).inherit(GetType(TextWriter)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            assert(close_writer(direct_cast(Of TextWriter)(x)))
                        End If
                    End Sub
            ElseIf GetType(T).implement(GetType(IDisposable)) Then
                D = Sub(x As T)
                        If Not x Is Nothing Then
                            cast(Of IDisposable)(x).Dispose()
                        End If
                    End Sub
            Else
                D = Nothing
            End If
        End Sub
    End Class

    Public Shared Function disposer() As Action(Of T)
        Return disposer_retriever.D
    End Function
End Class
