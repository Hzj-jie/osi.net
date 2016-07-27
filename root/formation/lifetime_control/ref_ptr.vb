
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Public Class ref_ptr(Of T)
    Inherits dispose_ptr(Of T)

    Private ReadOnly i As atomic_int
    Private ReadOnly create_stack_trace As String

    Private Shared Function build_create_stack_trace() As String
        Return If(isdebugmode(), callstack(), "##NOT_TRACE##")
    End Function

    Public Sub New(ByVal p As Func(Of T),
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing,
                   Optional ByVal ref As UInt32 = uint32_1)
        MyBase.New(p, init, disposer)
        i = New atomic_int(ref)
        create_stack_trace = build_create_stack_trace()
    End Sub

    Public Sub New(ByVal p As T,
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing,
                   Optional ByVal ref As UInt32 = uint32_1)
        MyBase.New(p, init, disposer)
        i = New atomic_int(ref)
        create_stack_trace = build_create_stack_trace()
    End Sub

    Public Sub New(Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing,
                   Optional ByVal ref As UInt32 = uint32_1)
        MyBase.New(init, disposer)
        i = New atomic_int(ref)
        create_stack_trace = build_create_stack_trace()
    End Sub

    Public Function ref() As UInt32
        Return i.increment()
    End Function

    Public Function unref() As UInt32
        Dim r As Int32 = 0
        r = i.decrement()
        assert(r >= 0)
        If r = 0 Then
            dispose()
        End If
        Return r
    End Function

    Protected Overrides Sub Finalize()
        assert(+i = 0, "ref_ptr @ ", create_stack_trace, " has not been fully dereferred.")
        MyBase.Finalize()
    End Sub
End Class
