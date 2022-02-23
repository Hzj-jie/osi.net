
Imports osi.root.connector
Imports osi.root.lock
Imports lock_t = osi.root.lock.slimlock.monitorlock

Namespace counter
    Public Class locked_writer(Of T As icounter_writer)
        Implements icounter_writer

        Public ReadOnly writer As T = Nothing
        Private l As lock_t

        Public Sub New(ByVal writer As T)
            assert(writer IsNot Nothing)
            Me.writer = writer
        End Sub

        Public Sub write(ByVal s As String) Implements counter.icounter_writer.write
            l.locked(Sub() writer.write(s))
        End Sub
    End Class
End Namespace
