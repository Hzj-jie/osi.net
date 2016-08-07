
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with threadpool_test.vbp ----------
'so change threadpool_test.vbp instead of this file



Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.threadpool
Imports osi.root.utt

Public Class fast_threadpool_test
    Inherits [case]

    Private Const round As Int64 = 16 * 1024 * 1024
    Private ReadOnly inserted As atomic_int64
    Private ReadOnly executed As atomic_int64
    Private ReadOnly t As fast_threadpool

    Public Sub New()
        inserted = New atomic_int64()
        executed = New atomic_int64()
        t = New fast_threadpool()
        assert(Not t Is Nothing)
    End Sub

    Private Sub queue_job()
        t.queue_job(AddressOf execute)
    End Sub

    Private Sub execute()
        executed.increment()
        If inserted.increment() <= round Then
            queue_job()
        End If
    End Sub

    Public Overrides Function preserved_processors() As Int16
        Return t.thread_count()
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            inserted.set(0)
            executed.set(0)
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function run() As Boolean
        While inserted.increment() <= round
            queue_job()
        End While
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Using New thread_lazy()
            assert_true(timeslice_sleep_wait_until(Function() t.idle(),
                                                   minute_to_milliseconds(1)))
            assert_true(timeslice_sleep_wait_until(Function() (+executed) = round,
                                                   seconds_to_milliseconds(1)))
        End Using
        assert_true(t.[stop]())
        Return MyBase.finish()
    End Function
End Class
'finish threadpool_test.vbp --------
