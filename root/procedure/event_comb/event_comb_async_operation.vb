
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports error_type = osi.root.constants.error_type

Public Class event_comb_async_operation
    Inherits event_comb

    Private Shared ReadOnly UNFINISHED_IASYNCRESULT As Int64
    Private finished As singleentry
    Private end_suc As Boolean = False

    Shared Sub New()
        UNFINISHED_IASYNCRESULT = counter.register_counter("UNFINISHED_IASYNCRESULT")
    End Sub

    Private Shared Shadows Function current() As event_comb_async_operation
        Dim o As event_comb_async_operation = Nothing
        o = DirectCast(event_comb.current(), event_comb_async_operation)
        assert(Not o Is Nothing)
        Return o
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Sub New(ByVal begin As Func(Of AsyncCallback, IAsyncResult),
                    ByVal [end] As Action(Of Boolean, IAsyncResult),
                    ByVal original_end As [Delegate])
        MyBase.New(New Func(Of Boolean)() _
                   {Function() As Boolean
                        Dim w As Action = Nothing
                        w = wait()
                        'if the void from wait()'s return has been called before goto_next(),
                        'the goto_next() will not take effect
                        'but the new change in event_comb.private_waitfor makes it safe
                        assert(Not begin Is Nothing)
                        Dim c As event_comb_async_operation = Nothing
                        c = current()
                        Try
                            begin(Sub(a As IAsyncResult)
                                      Try
                                          [end](Not c.finished.mark_in_use(), a)
                                          c.end_suc = True
                                      Catch ex As Exception
                                          If event_comb_trace Then
                                              raise_error(error_type.warning,
                                                          "failed to end ",
                                                          original_end.method_identity(),
                                                          ", callstack, ",
                                                          c.callstack(),
                                                          ", ex ",
                                                          ex.Message)
                                          End If
                                          c.end_suc = False
                                      Finally
                                          counter.decrease(UNFINISHED_IASYNCRESULT)
                                          w()
                                      End Try
                                  End Sub)
                        Catch ex As Exception
                            If event_comb_trace Then
                                raise_error(error_type.warning,
                                            "failed to begin ",
                                            begin.method_identity(),
                                            ", callstack, ",
                                            c.callstack(),
                                            ", ex ",
                                            ex.Message)
                            End If
                            Return False
                        End Try
                        counter.increase(UNFINISHED_IASYNCRESULT)
                        Return goto_next()
                    End Function,
                    Function() As Boolean
                        Return current().end_suc AndAlso
                               goto_end()
                    End Function})
        attach_suspending(Sub() finished.mark_in_use())
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub New(ByVal begin As Func(Of AsyncCallback, IAsyncResult),
                   ByVal [end] As Action(Of IAsyncResult))
        Me.New(begin,
               Sub(e As Boolean, ar As IAsyncResult)
                   assert(Not [end] Is Nothing)
                   [end](ar)
               End Sub,
               [end])
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Shared Function ctor(Of T)(ByVal begin As Func(Of AsyncCallback, IAsyncResult),
                                      ByVal [end] As Func(Of IAsyncResult, T),
                                      Optional ByVal result As pointer(Of T) = Nothing) As event_comb
        If use_promise_for_event_comb_async_operation Then
            Return event_comb.from(promise.[New](begin, [end]), result)
        Else
            Return New event_comb_async_operation(begin,
                                              Sub(e As Boolean, ar As IAsyncResult)
                                                  Dim r As T = Nothing
                                                  assert(Not [end] Is Nothing)
                                                  r = [end](ar)
                                                  If Not e Then
                                                      eva(result, r)
                                                  End If
                                              End Sub,
                                              [end])
        End If
    End Function
End Class
