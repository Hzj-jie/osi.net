
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class async_preparer
    Public Shared Function [New](Of T)(ByVal d As Func(Of ref(Of T), event_comb)) As async_preparer(Of T)
        Return New async_preparer(Of T)(d)
    End Function

    Public Shared Function new_sync(Of T)(ByVal d As T) As async_preparer(Of T)
        Return New async_preparer(Of T)(Function(p As ref(Of T)) As event_comb
                                            Return sync_async(Sub() eva(p, d))
                                        End Function)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class async_preparer(Of T)
    Implements async_getter(Of T)

    Private ReadOnly finish_get As signal_event
    Private ReadOnly d As Func(Of ref(Of T), event_comb)
    Private ReadOnly inited As ManualResetEvent
    Private state As ternary
    Private v As T

    Public Sub New(ByVal d As Func(Of ref(Of T), event_comb))
        assert(d IsNot Nothing)
        Me.finish_get = New signal_event()
        Me.d = d
        Me.inited = New ManualResetEvent(False)
        Me.state = ternary.unknown
        Me.v = Nothing
        start_prepare()
    End Sub

    Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
        Return inited
    End Function

    Protected Overridable Function prepare(ByVal p As ref(Of T)) As event_comb
        assert(d IsNot Nothing)
        Return d(p)
    End Function

    Private Sub start_prepare()
        assert(state.unknown_())
        assert(finish_get IsNot Nothing)
        assert(v Is Nothing OrElse type_info(Of T).is_valuetype)
        Dim ec As event_comb = Nothing
        Dim r As ref(Of T) = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        r = New ref(Of T)()
                                        ec = prepare(r)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If ec.end_result() Then
                                            v = (+r)
                                        End If
                                        state = ec.end_result()
                                        finish_get.mark()
                                        assert(inited.Set())
                                        Return goto_end()
                                    End Function))
    End Sub

    Public Function alive() As ternary Implements async_getter(Of T).alive
        Return state
    End Function

    Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
        If initialization_succeeded() Then
            r = v
            Return True
        Else
            Return False
        End If
    End Function

    Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
        Return New event_comb(Function() As Boolean
                                  If initialized() Then
                                      Return initialization_succeeded() AndAlso
                                             eva(r, v) AndAlso
                                             goto_end()
                                  Else
                                      assert(finish_get IsNot Nothing)
                                      Return waitfor(finish_get) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return initialization_succeeded() AndAlso
                                         eva(r, v) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Overrides Sub Finalize()
        inited.Close()
        MyBase.Finalize()
    End Sub
End Class
