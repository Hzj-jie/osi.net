
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.formation

Public Module _stream
#If 0 Then
    Private ReadOnly empty_buff() As Byte

    Sub New()
        '-1 means nothing?
        ReDim empty_buff(0)
        assert(npos < 0)
    End Sub
#End If

    'not only whether the stream can write, but also try to write an empty buff
    <Extension()> Public Function can_read(ByVal i As Stream) As Boolean
        Return Not i Is Nothing AndAlso i.CanRead()
#If 0 Then
        If i Is Nothing OrElse Not i.CanRead() Then
            Return False
        Else
            Try
                i.read(empty_buff, 0, 0)
                Return True
            Catch
                Return False
            End Try
        End If
#End If
    End Function

    <Extension()> Public Function can_write(ByVal i As Stream) As Boolean
        Return Not i Is Nothing AndAlso i.CanWrite()
#If 0 Then
        If i Is Nothing OrElse Not i.CanWrite() Then
            Return False
        Else
            Try
                i.write(empty_buff, 0, 0)
                Return True
            Catch
                Return False
            End Try
        End If
#End If
    End Function

    <Extension()> Public Function data_available(ByVal i As NetworkStream) As ternary
        If i Is Nothing Then
            Return ternary.unknown
        Else
            Try
                If i.DataAvailable() Then
                    Return ternary.true
                Else
                    Return ternary.false
                End If
            Catch
                Return ternary.unknown
            End Try
        End If
    End Function
End Module
