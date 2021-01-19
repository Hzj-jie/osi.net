
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils

' TODO: Use flip_event
Partial Public MustInherit Class auto_device_exporter(Of T)
    Inherits device_exporter(Of T)
    Implements iauto_device_exporter(Of T)

    Private ReadOnly rcr As reference_count_runner
    Private ReadOnly check_interval_ms As Int64
    Private ReadOnly failure_wait_ms As Int64
    Private ReadOnly max_concurrent_generations As Int32
    Private ReadOnly count As atomic_int

    Protected Sub New(ByVal id As String,
                      ByVal check_interval_ms As Int64,
                      ByVal failure_wait_ms As Int64,
                      ByVal max_concurrent_generations As Int32)
        MyBase.New(id)
        assert(check_interval_ms = npos OrElse check_interval_ms > 0)
        assert(failure_wait_ms = npos OrElse failure_wait_ms > 0)
        assert(max_concurrent_generations = npos OrElse max_concurrent_generations > 0)
        Me.rcr = New reference_count_runner(AddressOf start_to_export)
        AddHandler Me.rcr.after_start, Sub()
                                           assert(MyBase.start())
                                       End Sub
        AddHandler Me.rcr.after_stop, Sub()
                                          assert(MyBase.stop())
                                      End Sub
        If check_interval_ms < 0 Then
            Me.check_interval_ms = constants.default_auto_generation_check_interval_ms
        Else
            Me.check_interval_ms = check_interval_ms
        End If
        If failure_wait_ms < 0 Then
            Me.failure_wait_ms = constants.default_auto_generation_failure_wait_ms
        Else
            Me.failure_wait_ms = failure_wait_ms
        End If
        Me.max_concurrent_generations = max_concurrent_generations
        Me.count = New atomic_int()
    End Sub

    Private Sub start_to_export(ByVal rcr As reference_count_runner)
        assert(object_compare(rcr, Me.rcr) = 0)
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        If stopping() Then
                                            rcr.mark_stopped()
                                            Return goto_end()
                                        Else
                                            Dim this_round As Int32 = 0
                                            this_round = (+count) - CInt(exported())
                                            assert(this_round >= 0)
                                            If this_round = 0 Then
                                                Return waitfor(check_interval_ms)
                                            Else
                                                If max_concurrent_generations > 0 Then
                                                    this_round = min(this_round, max_concurrent_generations)
                                                End If
                                                ec = create_devices(this_round)
                                                Return waitfor(ec) AndAlso
                                                       goto_next()
                                            End If
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        Return If(ec.end_result(), True, waitfor(failure_wait_ms)) AndAlso
                                               goto_begin()
                                    End Function))
        rcr.mark_started()
    End Sub

    Public Sub require(Optional ByVal c As UInt32 = uint32_1) Implements iauto_device_exporter(Of T).require
        Me.count.add(CInt(c))
    End Sub

    Protected MustOverride Function create_device(ByVal p As ref(Of idevice(Of T))) As event_comb

    Private Function create_devices(ByVal expected As Int32) As event_comb
        assert(expected > 0)
        Dim ecs() As event_comb = Nothing
        Dim p() As ref(Of idevice(Of T)) = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim ecs(expected - 1)
                                  ReDim p(expected - 1)
                                  For i As Int32 = 0 To expected - 1
                                      p(i) = New ref(Of idevice(Of T))()
                                      ecs(i) = create_device(p(i))
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim failed As Boolean = False
                                  For i As Int32 = 0 To expected - 1
                                      If Not ecs(i).end_result() OrElse
                                         p(i).empty() OrElse
                                         Not device_exported(+p(i)) Then
                                          failed = True
                                      End If
                                  Next
                                  Return Not failed AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public NotOverridable Overrides Function start() As Boolean
        Return rcr.bind()
    End Function

    Public Function starting() As Boolean Implements iauto_device_exporter(Of T).starting
        Return rcr.starting()
    End Function

    Public NotOverridable Overrides Function started() As Boolean
        Return rcr.started()
    End Function

    Public Sub wait_for_start() Implements iauto_device_exporter(Of T).wait_for_start
        rcr.wait_for_start()
    End Sub

    Public NotOverridable Overrides Function [stop]() As Boolean
        Return rcr.release()
    End Function

    Public Function stopping() As Boolean Implements iauto_device_exporter(Of T).stopping
        Return rcr.stopping()
    End Function

    Public NotOverridable Overrides Function stopped() As Boolean
        Return rcr.stopped()
    End Function

    Public Sub wait_for_stop() Implements iauto_device_exporter(Of T).wait_for_stop
        rcr.wait_for_stop()
    End Sub
End Class
