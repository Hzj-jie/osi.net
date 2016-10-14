
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class ref_ptr(Of T)
    Private ReadOnly [New] As Func(Of T)
    Private ReadOnly p As dispose_ptr(Of T)
    Private ReadOnly create_stack_trace As String
    Private r As UInt32
    Private l As lock_t

    Private Shared Function build_create_stack_trace() As String
        Return If(isdebugmode(), callstack(), "##NOT_TRACE##")
    End Function

    Public Sub New(ByVal [New] As Func(Of T),
                   Optional ByVal ref As UInt32 = uint32_1,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        assert(Not [New] Is Nothing)
        Me.[New] = [New]
        Me.p = New dispose_ptr(Of T)(disposer:=disposer)
        Me.create_stack_trace = build_create_stack_trace()
        Me.r = ref
        If Me.r > 0 Then
            Me.p.set(Me.[New]())
        End If
    End Sub

    Public Sub New(ByVal p As T,
                   Optional ByVal ref As UInt32 = uint32_1,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(Function() As T
                   Return p
               End Function,
               ref,
               disposer)
    End Sub

    Public Function ref() As UInt32
        Dim v As UInt32 = 0
        l.wait()
        r += uint32_1
        v = r
        If v = 1 Then
            p.set([New]())
        End If
        l.release()
        Return v
    End Function

    Public Function unref() As UInt32
        Dim v As UInt32 = 0
        l.wait()
        assert(r > 0)
        r -= uint32_1
        v = r
        If v = 0 Then
            p.dispose()
            p.clear()
        End If
        l.release()
        Return v
    End Function

    Public Function [get]() As T
        Return p.get()
    End Function

    Public Shared Operator +(ByVal this As ref_ptr(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.get()
        End If
    End Operator

    Protected Overrides Sub Finalize()
        assert(r = 0, "ref_ptr @ ", create_stack_trace, " has not been fully dereferred.")
        MyBase.Finalize()
    End Sub
End Class
