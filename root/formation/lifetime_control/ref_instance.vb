
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class ref_instance
    Public Shared Function [New](Of T)(ByVal n As Func(Of T),
                                       Optional ByVal ref As UInt32 = uint32_1,
                                       Optional ByVal disposer As Action(Of T) = Nothing) As ref_instance(Of T)
        Return New ref_instance(Of T)(n, ref, disposer)
    End Function

    Public Shared Function [New](Of T)(Optional ByVal ref As UInt32 = uint32_1,
                                       Optional ByVal disposer As Action(Of T) = Nothing) As ref_instance(Of T)
        Return New ref_instance(Of T)(ref, disposer)
    End Function

    Private Sub New()
    End Sub
End Class

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
Public NotInheritable Class ref_instance(Of T)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event created()
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event disposed()
    Private ReadOnly [New] As Func(Of T)
    Private ReadOnly p As disposer(Of T)
    Private ReadOnly create_stack_trace As String
    Private r As Int32
    Private l As lock_t

    Private Shared Function build_create_stack_trace() As String
        Return If(isdebugmode(), callstack(), "##NOT_TRACE##")
    End Function

    Public Sub New(ByVal [New] As Func(Of T),
                   Optional ByVal ref As UInt32 = uint32_1,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        If [New] Is Nothing Then
            Me.[New] = AddressOf alloc(Of T)
        Else
            Me.[New] = [New]
        End If
        Me.p = New disposer(Of T)(disposer:=disposer)
        Me.create_stack_trace = build_create_stack_trace()
        assert(ref <= max_int32)
        Me.r = CInt(ref)
        If Me.r > 0 Then
            Me.p.set(Me.[New]())
        End If
    End Sub

    Public Sub New(Optional ByVal ref As UInt32 = uint32_1,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(Nothing, ref, disposer)
    End Sub

    Public Function ref() As UInt32
        Dim v As UInt32 = 0
        l.wait()
        r += 1
        assert(r > 0)
        v = CUInt(r)
        If r = 1 Then
            p.set([New]())
            RaiseEvent created()
        End If
        l.release()
        Return v
    End Function

    Public Function unref() As UInt32
        Dim v As UInt32 = 0
        l.wait()
        assert(r > 0)
        r -= 1
        v = CUInt(r)
        If v = 0 Then
            p.dispose()
            p.clear()
            RaiseEvent disposed()
        End If
        l.release()
        Return v
    End Function

    Public Function [get]() As T
        Dim r As T = Nothing
        l.wait()
        r = p.get()
        l.release()
        Return r
    End Function

    Public Function referred() As Boolean
        Return ref_count() > 0
    End Function

    Public Function ref_count() As UInt32
        Dim x As Int32 = 0
        x = atomic.read(r)
        assert(x >= 0)
        Return CUInt(x)
    End Function

    Public Shared Operator +(ByVal this As ref_instance(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.get()
    End Operator

    Public Function assert_getter() As getter(Of T)
        Return New _assert_getter(Me)
    End Function

    Public Function getter() As getter(Of T)
        Return New _getter(Me)
    End Function

    Private NotInheritable Class _assert_getter
        Implements getter(Of T)

        Private ReadOnly r As ref_instance(Of T)

        Public Sub New(ByVal r As ref_instance(Of T))
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

        Private ReadOnly r As ref_instance(Of T)

        Public Sub New(ByVal r As ref_instance(Of T))
            assert(Not r Is Nothing)
            Me.r = r
        End Sub

        Public Function [get](ByRef k As T) As Boolean Implements getter(Of T).get
            If r.referred() Then
                k = r.get()
                Return True
            End If
            Return False
        End Function
    End Class
End Class
