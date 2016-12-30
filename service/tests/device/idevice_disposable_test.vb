
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.device

Public Class idevice_disposable_test
    Inherits [case]

    Private Class test_class
        Implements idevice

        Public Event closing As idevice.closingEventHandler Implements idevice.closing
        Private _close_count As Int32

        Public Sub New()
            _close_count = 0
        End Sub

        Public Function close_count() As Int32
            Return _close_count
        End Function

        Public Sub check() Implements idevice.check
        End Sub

        Public Sub close() Implements idevice.close
            _close_count += 1
        End Sub

        Public Function closed() As Boolean Implements idevice.closed
            Return _close_count > 0
        End Function

        Public Function identity() As String Implements idevice.identity
            Return String.Empty
        End Function

        Public Function is_valid() As Boolean Implements idevice.is_valid
            Return Not closed()
        End Function
    End Class

    Public Overrides Function run() As Boolean
        Dim d As test_class = Nothing
        d = New test_class()
        For i As Int32 = 1 To 100
            disposable.dispose(d)
            assert_equal(d.close_count(), i)
        Next
        Return True
    End Function
End Class
