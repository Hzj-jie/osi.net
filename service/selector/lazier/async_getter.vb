
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Public Interface async_getter(Of T)
    Function alive() As ternary
    Function [get](ByRef r As T) As Boolean
    Function [get](ByVal r As ref(Of T)) As event_comb
    Function initialized_wait_handle() As WaitHandle
End Interface

Public Module _async_getter
    Public Class async_getter_downgrader(Of T, IT As T)
        Implements async_getter(Of T)

        Private ReadOnly a As async_getter(Of IT)

        Public Sub New(ByVal a As async_getter(Of IT))
            assert(Not a Is Nothing)
            Me.a = a
        End Sub

        Public Function alive() As ternary Implements async_getter(Of T).alive
            Return a.alive()
        End Function

        Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
            Dim ec As event_comb = Nothing
            Dim p As ref(Of IT) = Nothing
            Return New event_comb(Function() As Boolean
                                      p = New ref(Of IT)()
                                      ec = a.get(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return ec.end_result() AndAlso
                                             eva(r, +p) AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
            Dim t As IT = Nothing
            If a.get(t) Then
                r = t
                Return True
            Else
                Return False
            End If
        End Function

        Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
            Return a.initialized_wait_handle()
        End Function
    End Class

    <Extension()> Public Sub wait_until_initialized(Of T)(ByVal this As async_getter(Of T))
        assert(Not this Is Nothing)
        assert(this.initialized_wait_handle().WaitOne())
    End Sub

    <Extension()> Public Function wait_until_initialized(Of T)(ByVal this As async_getter(Of T),
                                                               ByVal timeout_ms As Int64) As Boolean
        If timeout_ms < 0 OrElse timeout_ms > max_int32 Then
            wait_until_initialized(Of T)(this)
            Return True
        Else
            assert(Not this Is Nothing)
            Return this.initialized_wait_handle().WaitOne(CInt(timeout_ms))
        End If
    End Function

    <Extension()> Public Function wait_get(Of T)(ByVal this As async_getter(Of T), ByRef r As T) As Boolean
        assert(Not this Is Nothing)
        this.wait_until_initialized()
        assert(this.initialized())
        Return this.get(r)
    End Function

    <Extension()> Public Function initialized(Of T)(ByVal this As async_getter(Of T)) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.alive().notunknown()
        End If
    End Function

    <Extension()> Public Function not_initialized(Of T)(ByVal this As async_getter(Of T)) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.alive.unknown_()
        End If
    End Function

    <Extension()> Public Function initialization_succeeded(Of T)(ByVal this As async_getter(Of T)) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.alive().true_()
        End If
    End Function

    <Extension()> Public Function initialization_failed(Of T)(ByVal this As async_getter(Of T)) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.alive.false_()
        End If
    End Function
End Module
