
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with reference_count_event_comb_1.vbp ----------
'so change reference_count_event_comb_1.vbp instead of this file


' TODO: Remove, use event_comb.flip_event

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
Public Class reference_count_event_comb_1
    Public Shared Function start_after_trigger() As Boolean
        Return True
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event before_init()
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event after_final()
    Private ReadOnly r As reference_count_runner
    Private ec As event_comb

    Private ReadOnly i As Func(Of event_comb)
    Private ReadOnly w As Func(Of event_comb)
    Private ReadOnly f As Func(Of event_comb)

    Protected Sub New(ByVal init As Func(Of event_comb),
                      ByVal work As Func(Of event_comb),
                      ByVal final As Func(Of event_comb))
        Me.i = init
        Me.w = work
        Me.f = final
        Me.r = New reference_count_runner(Sub(this As reference_count_runner)
                                              assert(Not this Is Nothing)
                                              start()
                                              this.mark_started()
                                          End Sub,
                                          Sub(this As reference_count_runner)
                                              Dim c As event_comb = Nothing
                                              c = ec
                                              If Not c Is Nothing Then
                                                  c.cancel()
                                              End If
                                          End Sub)
    End Sub

    Protected Sub New()
        Me.New(Nothing, Nothing, Nothing)
    End Sub

    Public Function bind() As Boolean
        Return r.bind()
    End Function

    Public Function release() As Boolean
        Return r.release()
    End Function

    Public Function expired() As Boolean
        Return r.stopping() OrElse r.stopped()
    End Function

    Public Function running() As Boolean
        Return r.started()
    End Function

    Public Function binding_count() As UInt32
        Return r.binding_count()
    End Function

    Public Function binding() As Boolean
        Return r.binding()
    End Function

    Public Function stopped() As Boolean
        Return r.stopped()
    End Function

    Public Function wait_for_stop(ByVal ms As Int64) As Boolean
        Return r.wait_for_stop(ms)
    End Function

    Public Sub wait_for_stop()
        r.wait_for_stop()
    End Sub

    Private Function run() As event_comb
        ec = Nothing
        Return New event_comb(Function() As Boolean
                                  If ec.end_result_or_null() Then
                                      If r.stopping() Then
                                          Return goto_end()
                                      Else
                                          atomic.eva(ec, work())
                                          If r.stopping() Then
                                              Return goto_end()
                                          ElseIf waitfor(ec) Then
                                              Return True
                                          Else
                                              Return False
                                          End If
                                      End If
                                  Else
                                      If Not r.stopping() Then
                                          raise_error(error_type.warning,
                                                      "reference_count_event_comb_1.work failed")
                                      End If
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Sub start()
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        RaiseEvent before_init()
                                        ec = init()
                                        Return waitfor_or_null(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If ec.end_result_or_null() Then
                                            Return waitfor(run()) AndAlso
                                                   goto_next()
                                        Else
                                            raise_error(error_type.warning,
                                                        "reference_count_event_comb_1.init failed")
                                            Return goto_next()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        ec = final()
                                        Return waitfor_or_null(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        r.mark_stopped()
                                        RaiseEvent after_final()
                                        If ec.end_result_or_null() Then
                                            Return goto_end()
                                        Else
                                            raise_error(error_type.warning,
                                                        "reference_count_event_comb_1.final failed")
                                            Return False
                                        End If
                                    End Function))
    End Sub

    Protected Overridable Function init() As event_comb
        Return If(i Is Nothing, Nothing, i())
    End Function

    Protected Overridable Function work() As event_comb
        assert(Not w Is Nothing)
        Return w()
    End Function

    Protected Overridable Function final() As event_comb
        Return If(f Is Nothing, Nothing, f())
    End Function


    'the following code is generated by /osi/root/codegen/precompile/precompile.exe
    'with ctors.vbp ----------
    'so change ctors.vbp instead of this file


    Public Shared Shadows Function disposer(ByVal rcec As reference_count_event_comb_1,
                                            ByRef r As disposer(Of reference_count_event_comb_1)) As Boolean
        If rcec Is Nothing Then
            Return False
        Else
            rcec.bind()
            r = make_disposer(rcec,
                              disposer:=Sub(ad)
                                            rcec.release()
                                        End Sub)
            Return True
        End If
    End Function

    Public Shared Shadows Function disposer(ByVal rcec As reference_count_event_comb_1) _
                                           As disposer(Of reference_count_event_comb_1)
        Dim r As disposer(Of reference_count_event_comb_1) = Nothing
        assert(disposer(rcec, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of event_comb),
                                        ByRef r As reference_count_event_comb_1) As Boolean
        Return ctor(Nothing, work, Nothing, r)
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of event_comb)) As reference_count_event_comb_1
        Dim r As reference_count_event_comb_1 = Nothing
        assert(ctor(work, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal init As Func(Of event_comb),
                                        ByVal work As Func(Of event_comb),
                                        ByVal final As Func(Of event_comb),
                                        ByRef r As reference_count_event_comb_1) As Boolean
        If work Is Nothing Then
            Return False
        Else
            r = New reference_count_event_comb_1(init, work, final)
            Return True
        End If
    End Function

    Public Shared Shadows Function ctor(ByVal init As Func(Of event_comb),
                                        ByVal work As Func(Of event_comb),
                                        ByVal final As Func(Of event_comb)) As reference_count_event_comb_1
        Dim r As reference_count_event_comb_1 = Nothing
        assert(ctor(init, work, final, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of Boolean),
                                        ByVal interval_ms As Int64,
                                        ByRef r As reference_count_event_comb_1) As Boolean
        If work Is Nothing Then
            Return False
        Else
            Return ctor(Function() sync_async(Function() If(work(), True, waitfor(interval_ms))),
                        r)
        End If
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of Boolean),
                                        ByVal interval_ms As Int64) As reference_count_event_comb_1
        Dim r As reference_count_event_comb_1 = Nothing
        assert(ctor(work, interval_ms, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Action,
                                        ByRef r As reference_count_event_comb_1) As Boolean
        If work Is Nothing Then
            Return False
        Else
            Return ctor(Function() As Boolean
                            work()
                            Return True
                        End Function,
                        npos,
                        r)
        End If
    End Function

    Public Shared Shadows Function ctor(ByVal work As Action) As reference_count_event_comb_1
        Dim r As reference_count_event_comb_1 = Nothing
        assert(ctor(work, r))
        Return r
    End Function
    'finish ctors.vbp --------
End Class
'finish reference_count_event_comb_1.vbp --------
