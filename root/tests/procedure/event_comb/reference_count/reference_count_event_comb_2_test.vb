
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with reference_count_event_comb_2_test.vbp ----------
'so change reference_count_event_comb_2_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with reference_count_event_comb_test.vbp ----------
'so change reference_count_event_comb_test.vbp instead of this file


Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.lock
Imports osi.root.utt

Public Class reference_count_event_comb_2_test
    Inherits [case]

    Private Class case1
        Inherits [case]

        Public ReadOnly rcec As reference_count_event_comb_2
        Public ReadOnly c1 As atomic_int
        Public ReadOnly c2 As ref(Of unchecked_int)

        Public Sub New()
            c1 = New atomic_int()
            c2 = New ref(Of unchecked_int)()
            rcec = ctor()
        End Sub

        Protected Overridable Function ctor() As reference_count_event_comb_2
            Return reference_count_event_comb_2.ctor(
                       Sub()
                           c1.increment()
                           c2.increment()
                       End Sub)
        End Function

        Public NotOverridable Overrides Function run() As Boolean
            rcec.bind()
            sleep(rnd_int(1, 20))
            rcec.release()
            Return True
        End Function
    End Class

    Private Class case2
        Inherits case1

        Public ReadOnly init_runs As ref(Of unchecked_int)
        Public ReadOnly final_runs As ref(Of unchecked_int)

        Public Sub New()
            MyBase.New()
            init_runs = New ref(Of unchecked_int)()
            final_runs = New ref(Of unchecked_int)()
        End Sub

        Protected Overrides Function ctor() As reference_count_event_comb_2
            Return reference_count_event_comb_2.ctor(
                       Function() As event_comb
                           Return sync_async(Sub()
                                                 init_runs.increment()
                                             End Sub)
                       End Function,
                       Function() As event_comb
                           Return sync_async(Sub()
                                                 c1.increment()
                                                 c2.increment()
                                             End Sub)
                       End Function,
                       Function() As event_comb
                           Return sync_async(Sub()
                                                 final_runs.increment()
                                             End Sub)
                       End Function)
        End Function
    End Class

    Private Class case3
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim r As reference_count_event_comb_1 = Nothing
            r = reference_count_event_comb_1.ctor(Function() As event_comb
                                                      Return Nothing
                                                  End Function)
            For i As Int32 = 0 To 1000
                assert(r.bind())
                sleep(10)
                assert(r.release())
            Next
            Return True
        End Function
    End Class

    Private Overloads Function run(ByVal rc As case1, ByVal c As [case]) As Boolean
        assert(Not rc Is Nothing)
        assert(Not c Is Nothing)
        If c.run() Then
            sleep()
            assert_true(rc.rcec.expired())
            assert_false(rc.rcec.running())
            assert_equal(+(rc.c1), +(rc.c2.p))
            assert_not_equal(+(rc.c1), 0)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function run_case() As Boolean
        Dim rc As case1 = Nothing
        rc = New case1()
        Dim c As [case] = Nothing
        c = multithreading(repeat(rc, 1024), Environment.ProcessorCount() << 2)
        If Not run(rc, c) Then
            Return False
        End If
        Dim last As Int32 = 0
        last = +(rc.c1)
        sleep()
        assert_equal(+(rc.c1), last)
        assert_equal(+(rc.c2.p), last)

        If Not run(rc, c) Then
            Return False
        End If
        assert_not_equal(+(rc.c1), last)
        assert_not_equal(+(rc.c2.p), last)

        Return True
    End Function

    Private Function run_case2() As Boolean
        Const repeat_count As Int32 = 8192
        Dim rc As case2 = Nothing
        rc = New case2()
        Dim c As [case] = Nothing
        c = repeat(rc, repeat_count)
        If Not run(rc, c) Then
            Return False
        End If
        Dim last As Int32 = 0
        last = +(rc.c1)
        sleep()
        assert_equal(+(rc.c1), last)
        assert_equal(+(rc.c2.p), last)
        If reference_count_event_comb_2.start_after_trigger() Then
            assert_equal(+(rc.init_runs.p), repeat_count)
            assert_equal(+(rc.final_runs.p), repeat_count)
        Else
            assert_more_and_less_or_equal(+(rc.init_runs.p), 0, repeat_count)
            assert_more_and_less_or_equal(+(rc.final_runs.p), 0, repeat_count)
            assert_equal(+(rc.init_runs.p), +(rc.final_runs.p))
        End If

        Return True
    End Function

    Private Function run_case3() As Boolean
        Dim c As [case] = Nothing
        c = New case3()
        Return c.run()
    End Function

    Public Overrides Function run() As Boolean
        Return run_case() AndAlso
               run_case2() AndAlso
               run_case3()
    End Function
End Class
'finish reference_count_event_comb_test.vbp --------
'finish reference_count_event_comb_2_test.vbp --------
