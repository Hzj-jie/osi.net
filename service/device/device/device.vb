
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils

Public MustInherit Class device
    Implements idevice

    Public Event closing() Implements idevice.closing
    Private ReadOnly disposer As once_action

    Public Sub New()
        disposer = New once_action(AddressOf close_device)
    End Sub

    Protected MustOverride Sub close()

    Private Sub close_device()
        RaiseEvent closing()
        close()
        assert(Not idevice_is_valid())
    End Sub

    Public Sub idevice_close() Implements idevice.close
#If 0 Then
        'may use async_sync to close the device
        'so there is possibility a deadlock
        'async_sync is locking current thread, while no other thread to run the real close procedure
        'ps. closing device should not block the followup procedure, such as creating device,
        'so queue the closing procedure into managed threadpool is not a bad idea
        'the only problem is, if the managed threadpool does not have enough threads to handle the request,
        'the closing may be delayed, and cause the resource exhausted trouble
        'FIXME: find a better solution
        If in_restricted_threadpool_thread() Then
            queue_in_managed_threadpool(AddressOf disposer.run)
        Else
            disposer.run()
        End If
#End If
        disposer.run()
        GC.SuppressFinalize(Me)
    End Sub

    Public Function closed() As Boolean Implements idevice.closed
        Return Not disposer.has()
    End Function

    Public MustOverride Function identity() As String Implements idevice.identity

    Protected MustOverride Function is_valid() As Boolean

    Public Function idevice_is_valid() As Boolean Implements idevice.is_valid
        Return Not closed() AndAlso is_valid()
    End Function

    Protected MustOverride Sub check()

    Public Sub idevice_check() Implements idevice.check
        If idevice_is_valid() Then
            check()
        End If
    End Sub

    Public NotOverridable Overrides Function ToString() As String
        Return identity()
    End Function

    Public NotOverridable Overrides Function GetHashCode() As Int32
        Dim s As String = Nothing
        s = identity()
        Return If(s Is Nothing, 0, s.GetHashCode())
    End Function

    Protected Overridable Function close_when_finalize() As Boolean
        Return True
    End Function

    Protected NotOverridable Overrides Sub Finalize()
        If close_when_finalize() Then
            idevice_close()
        End If
        MyBase.Finalize()
    End Sub
End Class
