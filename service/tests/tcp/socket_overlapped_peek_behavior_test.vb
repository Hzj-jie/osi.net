
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.service.tcp

Public Class socket_overlapped_peek_behavior_test
    Inherits [case]

    Private Const repeat_size As Int32 = 1024
    Private Const data_size As Int32 = 128
    Private Const overlapped_size As Int32 = 16
    Private Shared ReadOnly port As UInt16

    Shared Sub New()
        port = rnd_port()
    End Sub

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return 2
    End Function

    Private Sub client_thread(ByVal ready As AutoResetEvent)
        Dim c As TcpClient = Nothing
        c = New TcpClient()
        Try
            c.Connect(IPAddress.Loopback, port)
        Catch ex As Exception
            assert_true(False, ex)
            Return
        End Try
        For i As Int32 = 0 To repeat_size - 1
            assert_true(ready.WaitOne())
            sleep(rnd_int(1, 5))
            Dim b() As Byte = Nothing
            b = next_bytes(data_size)
            assert(array_size(b) = data_size)
            c.GetStream().Write(b, 0, data_size)
        Next
        c.Close()
    End Sub

    Public Overrides Function run() As Boolean
        Dim ready As AutoResetEvent = Nothing
        ready = New AutoResetEvent(False)
        Dim l As TcpListener = Nothing
        l = New TcpListener(IPAddress.Loopback, port)
        l.Start()
        queue_in_managed_threadpool(Sub()
                                        client_thread(ready)
                                    End Sub)
        Dim r As TcpClient = Nothing
        r = l.AcceptTcpClient()
        For i As Int32 = 0 To repeat_size - 1
            assert_true(ready.Set())
            Dim buffs As vector(Of Byte()) = Nothing
            buffs = New vector(Of Byte())()
            Dim whs As vector(Of WaitHandle) = Nothing
            whs = New vector(Of WaitHandle)()
            Dim buff() As Byte = Nothing
            For j As Int32 = 0 To overlapped_size - 1
                ReDim buff(data_size - 1)
                buffs.emplace_back(buff)
                whs.emplace_back(r.Client().BeginReceive(buff,
                                                         0,
                                                         data_size,
                                                         SocketFlags.Peek,
                                                         Sub(ar As IAsyncResult)
                                                             assert_equal(r.Client().EndReceive(ar), data_size)
                                                         End Sub,
                                                         Nothing).AsyncWaitHandle())
            Next
            WaitHandle.WaitAll(+whs)
            ReDim buff(data_size - 1)
            r.Client().BeginReceive(buff,
                                    0,
                                    data_size,
                                    SocketFlags.None,
                                    Sub(ar As IAsyncResult)
                                        assert_equal(r.Client().EndReceive(ar), data_size)
                                    End Sub,
                                    Nothing).AsyncWaitHandle().WaitOne()

            For j As Int32 = 0 To overlapped_size - 1
                assert_array_equal(buffs(j), buff)
            Next
        Next
        r.Close()
        l.Stop()
        ready.Close()
        Return True
    End Function
End Class
