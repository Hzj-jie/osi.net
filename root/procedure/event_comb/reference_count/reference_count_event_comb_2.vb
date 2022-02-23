
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with reference_count_event_comb_2.vbp ----------
'so change reference_count_event_comb_2.vbp instead of this file


' TODO: Remove, use event_comb.flip_event

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock

Public Class reference_count_event_comb_2
    Public Shared Function start_after_trigger() As Boolean
        Return False
    End Function

    Private ReadOnly r As ref(Of singleentry)
    Private ReadOnly b As atomic_int
    Private ReadOnly i As Func(Of event_comb)
    Private ReadOnly w As Func(Of event_comb)
    Private ReadOnly f As Func(Of event_comb)

    Protected Sub New(ByVal init As Func(Of event_comb),
                      ByVal work As Func(Of event_comb),
                      ByVal final As Func(Of event_comb))
        Me.i = init
        Me.w = work
        Me.f = final

        Me.r = New ref(Of singleentry)()
        Me.b = New atomic_int()
        start(weak_ref.of(Me))
    End Sub

    Protected Sub New()
        Me.New(Nothing, Nothing, Nothing)
    End Sub

    Public Function bind() As Boolean
        'assert(b >= 0)
        binding_count()
        Return (b.increment() = 1)
    End Function

    Public Function release() As Boolean
        assert(binding())
        Return (b.decrement() = 0)
    End Function

    Public Function expired() As Boolean
        Return r.not_in_use()
    End Function

    Public Function running() As Boolean
        Return r.in_use()
    End Function

    Public Function binding_count() As UInt32
        Dim c As Int32 = 0
        c = (+b)
        assert(c >= 0)
        Return CUInt(c)
    End Function

    Public Function binding() As Boolean
        Return binding_count() > 0
    End Function

    Private Shared Sub start(ByVal wp As weak_ref(Of reference_count_event_comb_2))
        assert(wp IsNot Nothing)
        assert_begin(New event_comb(Function() As Boolean
                                        Return waitfor(Function() As Boolean
                                                           Dim rcec As reference_count_event_comb_2 = Nothing
                                                           Return Not wp.get(rcec) OrElse
                                                                  rcec.binding()
                                                       End Function) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Dim rcec As reference_count_event_comb_2 = Nothing
                                        If wp.get(rcec) Then
                                            Return waitfor(rcec.start()) AndAlso
                                                   goto_prev()
                                        Else
                                            Return goto_end()
                                        End If
                                    End Function))
    End Sub

    Private Function run() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If ec.end_result_or_null() Then
                                      If binding() Then
                                          ec = work()
                                          Return waitfor(ec)
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      raise_error(error_type.warning,
                                                  "reference_count_event_comb_2.work failed")
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Function start() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  assert(r.mark_in_use())
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
                                                  "reference_count_event_comb_2.init failed")
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  ec = final()
                                  Return waitfor_or_null(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  r.release()
                                  If ec.end_result_or_null() Then
                                      Return goto_end()
                                  Else
                                      raise_error(error_type.warning,
                                                  "reference_count_event_comb_2.final failed")
                                      Return False
                                  End If
                              End Function)
    End Function

    Protected Overridable Function init() As event_comb
        Return If(i Is Nothing, Nothing, i())
    End Function

    Protected Overridable Function work() As event_comb
        assert(w IsNot Nothing)
        Return w()
    End Function

    Protected Overridable Function final() As event_comb
        Return If(f Is Nothing, Nothing, f())
    End Function


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ctors.vbp ----------
'so change ctors.vbp instead of this file


    Public Shared Shadows Function disposer(ByVal rcec As reference_count_event_comb_2,
                                            ByRef r As disposer(Of reference_count_event_comb_2)) As Boolean
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

    Public Shared Shadows Function disposer(ByVal rcec As reference_count_event_comb_2) _
                                           As disposer(Of reference_count_event_comb_2)
        Dim r As disposer(Of reference_count_event_comb_2) = Nothing
        assert(disposer(rcec, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of event_comb),
                                        ByRef r As reference_count_event_comb_2) As Boolean
        Return ctor(Nothing, work, Nothing, r)
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of event_comb)) As reference_count_event_comb_2
        Dim r As reference_count_event_comb_2 = Nothing
        assert(ctor(work, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal init As Func(Of event_comb),
                                        ByVal work As Func(Of event_comb),
                                        ByVal final As Func(Of event_comb),
                                        ByRef r As reference_count_event_comb_2) As Boolean
        If work Is Nothing Then
            Return False
        Else
            r = New reference_count_event_comb_2(init, work, final)
            Return True
        End If
    End Function

    Public Shared Shadows Function ctor(ByVal init As Func(Of event_comb),
                                        ByVal work As Func(Of event_comb),
                                        ByVal final As Func(Of event_comb)) As reference_count_event_comb_2
        Dim r As reference_count_event_comb_2 = Nothing
        assert(ctor(init, work, final, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of Boolean),
                                        ByVal interval_ms As Int64,
                                        ByRef r As reference_count_event_comb_2) As Boolean
        If work Is Nothing Then
            Return False
        Else
            Return ctor(Function() sync_async(Function() If(work(), True, waitfor(interval_ms))),
                        r)
        End If
    End Function

    Public Shared Shadows Function ctor(ByVal work As Func(Of Boolean),
                                        ByVal interval_ms As Int64) As reference_count_event_comb_2
        Dim r As reference_count_event_comb_2 = Nothing
        assert(ctor(work, interval_ms, r))
        Return r
    End Function

    Public Shared Shadows Function ctor(ByVal work As Action,
                                        ByRef r As reference_count_event_comb_2) As Boolean
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

    Public Shared Shadows Function ctor(ByVal work As Action) As reference_count_event_comb_2
        Dim r As reference_count_event_comb_2 = Nothing
        assert(ctor(work, r))
        Return r
    End Function
'finish ctors.vbp --------
End Class
'finish reference_count_event_comb_2.vbp --------
