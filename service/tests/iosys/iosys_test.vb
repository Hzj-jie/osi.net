
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.iosys

Public Class iosys_test
    Inherits [case]

    Protected Const flower_size As UInt32 = 1024

    Protected Class iosys_test_case
        Public ReadOnly value As Int32

        Public Sub New()
            Me.New(rnd_int())
        End Sub

        Private Sub New(ByVal v As Int32)
            value = v
        End Sub

        Public Shared Narrowing Operator CType(ByVal this As iosys_test_case) As command
            If this Is Nothing Then
                Return Nothing
            End If
            Return command.[New]().attach(this.value)
        End Operator

        Public Shared Narrowing Operator CType(ByVal this As command) As iosys_test_case
            If this Is Nothing Then
                Return Nothing
            End If
            Dim v As Int32 = 0
            If bytes_serializer.from_bytes(this.action(), v) Then
                Return New iosys_test_case(v)
            End If
            Return Nothing
        End Operator
    End Class

    Private Class iosys_test_ar
        Implements iagent(Of iosys_test_case), ireceiver(Of iosys_test_case)

        Private ReadOnly q As qless2(Of iosys_test_case)

        Public Event deliver(ByVal c As iosys_test_case,
                             ByRef failed As Boolean) Implements iagent(Of iosys_test_case).deliver

        Public Sub New()
            q = New qless2(Of iosys_test_case)()
        End Sub

        Public Function receive(ByVal c As iosys_test_case) _
                               As event_comb Implements ireceiver(Of iosys_test_case).receive
            If assertion.is_not_null(c) Then
                Dim lc As iosys_test_case = Nothing
                While assertion.is_true(q.pop(lc))
                    assert(Not lc Is Nothing)
                    If assertion.equal(c.value, lc.value) Then
                        Exit While
                    End If
                End While
            End If
            Return Nothing
        End Function

        Public Sub start(ByVal round As Int64, ByVal interval_ms As Int64)
            assertion.is_true(async_sync(New event_comb(Function() As Boolean
                                                      Return waitfor(Function() q.size() < flower_size,
                                                                     minutes_to_milliseconds(1)) AndAlso
                                                             goto_next()
                                                  End Function,
                                                  Function() As Boolean
                                                      assertion.less_or_equal(q.size(), flower_size)
                                                      Dim c As iosys_test_case = Nothing
                                                      c = New iosys_test_case()
                                                      Dim f As Boolean = False
                                                      RaiseEvent deliver(c, f)
                                                      assertion.is_false(f)
                                                      q.emplace(c)
                                                      If round = 0 Then
                                                          Return goto_next()
                                                      End If
                                                      round -= 1
                                                      Return waitfor(interval_ms) AndAlso
                                                             goto_begin()
                                                  End Function,
                                                  Function() As Boolean
                                                      Return waitfor(Function() q.empty(),
                                                                     minutes_to_milliseconds(1)) AndAlso
                                                             goto_end()
                                                  End Function)))
            assertion.is_true(q.empty())
        End Sub
    End Class

    Private ReadOnly round As Int64
    Private ReadOnly interval_ms As Int64

    Public Sub New()
        Me.New(32 * flower_size)
    End Sub

    Protected Sub New(ByVal round As Int64)
        Me.New(round, 0)
    End Sub

    Protected Sub New(ByVal round As Int64, ByVal interval_ms As Int64)
        assert(round > 0)
        assert(interval_ms >= 0)
        Me.round = round * If(isdebugbuild(), 1, 4)
        Me.interval_ms = interval_ms
    End Sub

    Protected Overridable Function create_iosys(ByRef first As iosys(Of iosys_test_case),
                                                ByRef last As iosys(Of iosys_test_case)) As Boolean
        first = New iosys(Of iosys_test_case)(flower_size)
        last = first
        Return True
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim f As iosys(Of iosys_test_case) = Nothing
        Dim l As iosys(Of iosys_test_case) = Nothing
        If assertion.is_true(create_iosys(f, l)) Then
            Dim ar As iosys_test_ar = Nothing
            ar = New iosys_test_ar()
            If assertion.is_true(f.listen(ar)) AndAlso
               assertion.is_true(l.notice(ar)) Then
                ar.start(round, interval_ms)
            End If
        End If
        assertion.is_true(If(object_compare(f, l) = 0, f.stop(), f.stop() AndAlso l.stop()))
        'wait a second to let the iosys stop working
        sleep()
        Return True
    End Function
End Class
