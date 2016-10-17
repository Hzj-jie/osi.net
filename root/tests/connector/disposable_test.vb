
Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports osi.root.connector
Imports osi.root.utt

Public Class disposable_test
    Inherits [case]

    Private Class test_class
    End Class

    Private Class test_text_writer
        Inherits TextWriter

        Public Overrides ReadOnly Property Encoding() As Encoding
            Get
                Return Encoding.Unicode()
            End Get
        End Property
    End Class

    Private Shared Function default_case() As Boolean
        assert_not_nothing(disposable(Of Stream).D())
        assert_not_nothing(disposable(Of MemoryStream).D())
        assert_not_nothing(disposable(Of ManualResetEvent).D())
        assert_not_nothing(disposable(Of AutoResetEvent).D())
        assert_not_nothing(disposable(Of TcpClient).D())
        assert_not_nothing(disposable(Of UdpClient).D())
        assert_not_nothing(disposable(Of TextWriter).D())
        assert_not_nothing(disposable(Of IDisposable).D())
        Return True
    End Function

    Private Shared Function register_case() As Boolean
        assert_not_nothing(disposable(Of test_text_writer).D())
        Dim c As Int32 = 0
        disposable.register(Sub(x As test_text_writer)
                                c += 1
                                If assert_not_nothing(x) Then
                                    assert(close_writer(x))
                                End If
                            End Sub)
        disposable.dispose(New test_text_writer())
        assert_equal(c, 1)
        disposable.dispose(New test_text_writer())
        assert_equal(c, 2)
        disposable(Of test_text_writer).unregister()
        disposable.dispose(New test_text_writer())
        assert_equal(c, 2)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return default_case() AndAlso
               register_case()
    End Function
End Class
