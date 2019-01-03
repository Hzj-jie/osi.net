
Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
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
        assertion.is_not_null(disposable(Of Stream).D())
        assertion.is_not_null(disposable(Of MemoryStream).D())
        assertion.is_not_null(disposable(Of ManualResetEvent).D())
        assertion.is_not_null(disposable(Of AutoResetEvent).D())
        assertion.is_not_null(disposable(Of TcpClient).D())
        assertion.is_not_null(disposable(Of UdpClient).D())
        assertion.is_not_null(disposable(Of TextWriter).D())
        assertion.is_not_null(disposable(Of IDisposable).D())
        Return True
    End Function

    Private Shared Function register_case() As Boolean
        assertion.is_not_null(disposable(Of test_text_writer).D())
        Dim c As Int32 = 0
        disposable.register(Sub(x As test_text_writer)
                                c += 1
                                If assertion.is_not_null(x) Then
                                    assert(close_writer(x))
                                End If
                            End Sub)
        disposable.dispose(New test_text_writer())
        assertion.equal(c, 1)
        disposable.dispose(New test_text_writer())
        assertion.equal(c, 2)
        disposable(Of test_text_writer).unregister()
        disposable.dispose(New test_text_writer())
        assertion.equal(c, 2)
        Return True
    End Function

    Private Interface test_interface
    End Interface

    Private Class test_class(Of T)
        Implements test_interface
    End Class

    Private Shared Function register_type_case() As Boolean
        Dim disposed As vector(Of test_interface) = Nothing
        disposed = New vector(Of test_interface)()
        disposable.register(GetType(test_interface),
                            Sub(ByVal i As Object)
                                Dim t As test_interface = Nothing
                                assertion.is_true(direct_cast(i, t))
                                disposed.emplace_back(t)
                            End Sub)
        Dim a As test_class(Of Int32) = Nothing
        a = New test_class(Of Int32)()
        Dim b As test_class(Of Int64) = Nothing
        b = New test_class(Of Int64)()
        Dim c As test_class(Of Boolean) = Nothing
        c = New test_class(Of Boolean)()
        disposable.dispose(a)
        disposable.dispose(b)
        disposable.dispose(c)

        If assertion.equal(disposed.size(), CUInt(3)) Then
            assertion.equal(object_compare(a, disposed(0)), 0)
            assertion.equal(object_compare(b, disposed(1)), 0)
            assertion.equal(object_compare(c, disposed(2)), 0)
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return default_case() AndAlso
               register_case() AndAlso
               register_type_case()
    End Function
End Class
