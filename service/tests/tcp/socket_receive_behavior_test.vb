
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.service.tcp

'mustinherit for utt
Public MustInherit Class socket_receive_behavior_test(Of IMPL)
    Inherits [case]

    Private Const repeat_size As Int32 = 1024
    Private Const max_size As Int32 = 1024
    Private Shared ReadOnly port As UInt16
    Private ReadOnly receive_size As Int32
    Private ReadOnly receive_option As SocketFlags
    Private ReadOnly receive_before_send As Boolean

    Shared Sub New()
        port = rnd_port()
    End Sub

    Protected Sub New(ByVal receive_size As Int32,
                      ByVal receive_option As SocketFlags,
                      ByVal receive_before_send As Boolean)
        assert(receive_size >= 0)
        Me.receive_size = receive_size
        Me.receive_option = receive_option
        Me.receive_before_send = receive_before_send
    End Sub

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return 2
    End Function

    Private Sub client_thread(ByVal received As atomic_int, ByVal ready As AutoResetEvent)
        Dim c As TcpClient = Nothing
        c = New TcpClient()
        Try
            c.Connect(IPAddress.Loopback, port)
        Catch ex As Exception
            assertion.is_true(False, ex)
            Return
        End Try
        For i As Int32 = 0 To repeat_size - 1
            Dim last_received As Int32 = 0
            last_received = (+received)
            assertion.is_true(ready.WaitOne())
            If receive_before_send Then
                assertion.is_false(try_wait_when(Function() last_received = (+received), 4096))
                assertion.equal(last_received + 1, +received)
            Else
                assertion.is_true(try_wait_when(Function() last_received = (+received), 32))
                assertion.equal(last_received, +received)
            End If
            Dim b() As Byte = Nothing
            b = next_bytes(rnd_int(1, max_size + 1))
            c.GetStream().Write(b, 0, array_size(b))
            assertion.is_false(try_wait_when(Function() last_received = (+received), 4096))
            assertion.equal(last_received + 1, +received)
        Next
        c.Close()
    End Sub

    Public Overrides Function run() As Boolean
        Dim received As New atomic_int()
        Dim ready As New AutoResetEvent(False)
        Dim l As New TcpListener(IPAddress.Loopback, port)
        l.Start()
        managed_thread_pool.push(Sub()
                                     client_thread(received, ready)
                                 End Sub)
        Dim r As TcpClient = l.AcceptTcpClient()
        For i As Int32 = 0 To repeat_size - 1
            assertion.is_true(ready.Set())
            assert(receive_size <= max_size)
            Dim buff() As Byte = Nothing
            ReDim buff(max_size - 1)
            r.Client().BeginReceive(buff,
                                    0,
                                    receive_size,
                                    receive_option,
                                    Sub(ar As IAsyncResult)
                                        Try
                                            assertion.equal(r.Client().EndReceive(ar), receive_size)
                                        Catch ex As Exception
                                            assertion.is_true(False, ex)
                                        End Try
                                        received.increment()
                                    End Sub,
                                    Nothing).AsyncWaitHandle().WaitOne()
            r.Client().BeginReceive(buff,
                                    0,
                                    max_size,
                                    SocketFlags.None,
                                    Sub(ar As IAsyncResult)
                                        Dim rr As Int32 = 0
                                        Try
                                            rr = r.Client().EndReceive(ar)
                                        Catch ex As Exception
                                            assertion.is_true(False, ex)
                                        End Try
                                        assertion.more_and_less_or_equal(rr, 0, max_size)
                                    End Sub,
                                    Nothing).AsyncWaitHandle().WaitOne()
        Next
        r.Close()
        l.Stop()
        ready.Close()
        Return True
    End Function
End Class
