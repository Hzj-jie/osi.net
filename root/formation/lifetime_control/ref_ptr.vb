
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Public NotInheritable Class ref_ptr
    Public Shared Function [New](Of T)(ByVal p As T,
                                       Optional ByVal disposer As Action(Of T) = Nothing,
                                       Optional ByVal ref As UInt32 = uint32_1) As ref_ptr(Of T)
        Return New ref_ptr(Of T)(p, disposer, ref)
    End Function

    Public Shared Function [New](Of T)(Optional ByVal disposer As Action(Of T) = Nothing,
                                       Optional ByVal ref As UInt32 = uint32_1) As ref_ptr(Of T)
        Return New ref_ptr(Of T)(disposer, ref)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class ref_ptr(Of T)
    Inherits dispose_ptr(Of T)

    Private ReadOnly i As atomic_int
    Private ReadOnly create_stack_trace As String

    Private Shared Function build_create_stack_trace() As String
        Return If(isdebugmode(), callstack(), "##NOT_TRACE##")
    End Function

    Public Sub New(ByVal p As T,
                   Optional ByVal disposer As Action(Of T) = Nothing,
                   Optional ByVal ref As UInt32 = uint32_1)
        MyBase.New(p, disposer)
        assert(ref <= max_int32)
        i = New atomic_int(CInt(ref))
        create_stack_trace = build_create_stack_trace()
    End Sub

    Public Sub New(Optional ByVal disposer As Action(Of T) = Nothing,
                   Optional ByVal ref As UInt32 = uint32_1)
        Me.New(Nothing, disposer, ref)
    End Sub

    Public Function ref() As UInt32
        Dim r As Int32 = 0
        r = i.increment()
        assert(r > 0)
        Return CUInt(r)
    End Function

    Public Function unref() As UInt32
        Dim r As Int32 = 0
        r = i.decrement()
        assert(r >= 0)
        If r = 0 Then
            dispose()
        End If
        Return CUInt(r)
    End Function

    Public Function referred() As Boolean
        Return ref_count() > 0
    End Function

    Public Function ref_count() As UInt32
        Dim r As Int32 = 0
        r = i.get()
        assert(r >= 0)
        Return CUInt(r)
    End Function

    Protected Overrides Sub Finalize()
        assert(i.get() = 0, "ref_ptr @ ", create_stack_trace, " has not been fully dereferred.")
        MyBase.Finalize()
    End Sub
End Class
