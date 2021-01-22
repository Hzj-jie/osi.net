
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with managed_threadpool_test.vbp ----------
'so change managed_threadpool_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with threadpool_test.vbp ----------
'so change threadpool_test.vbp instead of this file



Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.threadpool
Imports osi.root.utt

Public NotInheritable Class managed_threadpool_manual_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(rinne(New managed_threadpool_test(1024 * 1024), 1024 * 32))
    End Sub
End Class

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
Public NotInheritable Class managed_threadpool_test
    Inherits [case]

    Private ReadOnly round As Int64
    Private ReadOnly inserted As atomic_int64
    Private ReadOnly executed As atomic_int64
    Private t As managed_threadpool

    Public Sub New(ByVal round As Int64)
        assert(round > 0)
        Me.round = round
        Me.inserted = New atomic_int64()
        Me.executed = New atomic_int64()
    End Sub

    Public Sub New()
        Me.New(16 * 1024 * 1024)
    End Sub

    Private Sub push()
        t.push(AddressOf execute)
    End Sub

    Private Sub execute()
        executed.increment()
        If inserted.increment() <= round Then
            push()
        End If
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return CShort(osi.root.threadpool.threadpool.default_thread_count)
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            t = managed_threadpool.[global]
            assert(Not t Is Nothing)
            inserted.set(0)
            executed.set(0)
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function run() As Boolean
        While inserted.increment() <= round
            push()
            If (+inserted) - (+executed) > 1024 * 32 Then
                Exit While
            End If
        End While
        timeslice_sleep_wait_until(Function() (+inserted) >= round)
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Using New thread_lazy()
            assertion.is_true(timeslice_sleep_wait_until(Function() t.idle(),
                                                   minutes_to_milliseconds(1)))
            assertion.is_true(timeslice_sleep_wait_until(Function() (+executed) = round,
                                                   seconds_to_milliseconds(1)))
        End Using
        assertion.is_true(t.[stop]())
#If False Then
        t.join()
#End If
        Return MyBase.finish()
    End Function
End Class
'finish threadpool_test.vbp --------
'finish managed_threadpool_test.vbp --------
