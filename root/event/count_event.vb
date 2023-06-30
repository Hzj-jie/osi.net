
Imports System.Threading
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.template
Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class count_event
    Inherits count_event(Of action_event(Of _true))

    Public Sub New(ByVal init_value As UInt32)
        MyBase.New(init_value)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

Public Class weak_count_event
    Inherits count_event(Of weak_reference_event(Of _true))

    Public Sub New(ByVal init_value As UInt32)
        MyBase.New(init_value)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

Public Class count_event(Of EVENT_TYPE As {action_event(Of _true), New})
    Implements attachable_event
    Private ReadOnly e As EVENT_TYPE
    Private l As lock_t
    Private s As UInt32

    Public Sub New(ByVal init_value As UInt32)
        e = New EVENT_TYPE()
        s = init_value
    End Sub

    Public Sub New()
        Me.New(uint32_0)
    End Sub

    Public Function attach(ByVal v As iaction) As Boolean Implements attachable_event.attach
        If v Is Nothing OrElse Not v.valid() Then
            Return False
        Else
            Dim attached As Boolean = False
            l.wait()
            If marked() Then
                attached = False
            Else
                attached = True
                assert(e.attach(v))
            End If
            l.release()
            If Not attached Then
                v.run()
            End If
            Return True
        End If
    End Function

    Public Function marked() As Boolean Implements attachable_event.marked
        Return s = uint32_0
    End Function

    Public Function attached() As Boolean
        Return e.attached()
    End Function

    Public Function attached_count() As Int32
        Return e.attached_count()
    End Function

    Public Function increment() As UInt32
        Dim r As UInt32 = uint32_0
        l.wait()
        s += uint32_1
        r = s
        l.release()
        Return r
    End Function

    Public Function try_decrement(Optional ByRef decremented_value As UInt32 = uint32_0) As Boolean
        Dim r As Boolean = False
        l.wait()
        If marked() Then
            r = False
        Else
            r = True
            If s = uint32_1 Then
                s = uint32_0
                e.raise()
            Else
                s -= uint32_1
            End If
        End If
        decremented_value = s
        l.release()
        Return r
    End Function

    Public Function decrement() As UInt32
        Dim r As UInt32 = uint32_0
        assert(try_decrement(r))
        Return r
    End Function

    Public Sub clear()
        e.clear()
    End Sub

    Public Function wait(Optional ByVal ms As Int64 = npos) As Boolean
        Dim ms32 As Int32 = 0
        If ms > max_int32 OrElse (ms < 0 AndAlso ms <> npos) Then
            ms32 = Timeout.Infinite
        Else
            ms32 = CInt(ms)
        End If
        Dim are As ref_ptr(Of AutoResetEvent) = Nothing
        are = New ref_ptr(Of AutoResetEvent)(New AutoResetEvent(False), ref:=2)
        assert(attach(Sub()
                          assert((+are).Set())
                          are.unref()
                      End Sub))
        Dim r As Boolean = False
        r = (+are).WaitOne(ms32)
        are.unref()
        Return r
    End Function
End Class
