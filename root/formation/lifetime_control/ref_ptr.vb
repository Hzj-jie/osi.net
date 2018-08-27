
Option Explicit On
Option Infer Off
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

' A ref-counted pointer to an object. It calls dispose() when the ref-count reaches 0 for the first time.
' Directly using this class is unsafe, calling ref() and get() in parallel with unref() may end-up getting disposed
' object. Usually this class should be used together with ref_map.
Public NotInheritable Class ref_ptr(Of T)
    Inherits disposer(Of T)

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
        ' If i has reached 0 already, using this object is unsafe.
        assert(r > 1)
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

    Public Function assert_getter() As getter(Of T)
        Return New _assert_getter(Me)
    End Function

    Public Function getter() As getter(Of T)
        Return New _getter(Me)
    End Function

    Private NotInheritable Class _assert_getter
        Implements getter(Of T)

        Private ReadOnly r As ref_ptr(Of T)

        Public Sub New(ByVal r As ref_ptr(Of T))
            assert(Not r Is Nothing)
            Me.r = r
        End Sub

        Public Function [get](ByRef k As T) As Boolean Implements getter(Of T).get
            assert(r.referred())
            k = r.get()
            Return True
        End Function
    End Class

    Private NotInheritable Class _getter
        Implements getter(Of T)

        Private ReadOnly r As ref_ptr(Of T)

        Public Sub New(ByVal r As ref_ptr(Of T))
            assert(Not r Is Nothing)
            Me.r = r
        End Sub

        Public Function [get](ByRef k As T) As Boolean Implements getter(Of T).get
            If r.referred() Then
                k = r.get()
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Class
