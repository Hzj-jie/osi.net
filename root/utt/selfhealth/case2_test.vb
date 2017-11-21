
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock

Public Class case2_test
    Inherits [case]

    <attributes.test>
    <attributes.reserved_processors(5)>
    Private Class case2_case
        Inherits cd_object(Of case2_case)

        Public Const multi_threading_case_thread_count As UInt32 = 4
        Public Const multi_threading_case_repeat_count As UInt32 = 1000
        Private Shared ReadOnly _prepare_runs As atomic_int
        Private Shared ReadOnly _finish_runs As atomic_int
        Private Shared ReadOnly _d_runs As atomic_int
        Private Shared ReadOnly _multi_threading_case_runs As atomic_int

        Shared Sub New()
            _prepare_runs = New atomic_int()
            _finish_runs = New atomic_int()
            _d_runs = New atomic_int()
            _multi_threading_case_runs = New atomic_int()
        End Sub

        Public Shared Function prepare_runs() As UInt32
            Return CUInt(+_prepare_runs)
        End Function

        Public Shared Function finish_runs() As UInt32
            Return CUInt(+_finish_runs)
        End Function

        Public Shared Function d_runs() As UInt32
            Return CUInt(+_d_runs)
        End Function

        Public Shared Function multi_threading_case_runs() As UInt32
            Return CUInt(+_multi_threading_case_runs)
        End Function

        <attributes.test>
        <attributes.reserved_processors(10)>
        <attributes.repeat(5)>
        Private Sub d()
            _d_runs.increment()
        End Sub

        <attributes.test>
        <attributes.repeat(10)>
        Private Function e() As Boolean
            Return False
        End Function

        <attributes.prepare>
        <attributes.repeat(100)>
        <attributes.reserved_processors(10000)>
        Private Sub prepare1()
            _prepare_runs.increment()
        End Sub

        <attributes.prepare>
        Private Sub prepare2()
            _prepare_runs.increment()
            _prepare_runs.increment()
        End Sub

        <attributes.finish>
        Private Sub finish1()
            _finish_runs.increment()
        End Sub

        <attributes.finish>
        Private Sub finish2()
            _finish_runs.increment()
            _finish_runs.increment()
        End Sub

        <attributes.finish>
        Private Sub finish3()
            _finish_runs.increment()
            _finish_runs.increment()
            _finish_runs.increment()
            _finish_runs.increment()
        End Sub

        <attributes.test>
        <attributes.multi_threading(multi_threading_case_thread_count)>
        <attributes.repeat(multi_threading_case_repeat_count)>
        Private Shared Sub multi_threading_case()
            _multi_threading_case_runs.increment()
            Dim thread_id As Int32 = 0
            thread_id = multithreading_case_wrapper.thread_id()
            assert_more_or_equal_and_less(thread_id, 0, CInt(multi_threading_case_thread_count))
            Static b(multi_threading_case_thread_count - uint32_1) As Boolean
            assert_false(b(thread_id))
            b(thread_id) = False
            assert_false(b(thread_id))
            b(thread_id) = True
            assert_true(b(thread_id))
            b(thread_id) = False
            Static c(multi_threading_case_thread_count - uint32_1) As UInt32
            c(thread_id) += uint32_1
            assert_less_or_equal(c(thread_id), multi_threading_case_repeat_count)
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        Dim cases As vector(Of [case]) = Nothing
        cases = case2.create(GetType(case2_case))
        assert_equal(cases.size(), CUInt(3))

        assert_true(cases(0).prepare())
        assert_true(cases(0).run())
        assert_true(cases(0).finish())
        assert_equal(case2_case.constructed(), CUInt(3))
        assert_equal(case2_case.prepare_runs(), CUInt(3))
        assert_equal(case2_case.finish_runs(), CUInt(7))
        assert_equal(case2_case.d_runs(), CUInt(5))
        assert_equal(case2_case.multi_threading_case_runs(), uint32_0)

        assert_true(cases(1).prepare())
        assert_false(cases(1).run())
        assert_equal(case2_case.constructed(), CUInt(3))
        assert_equal(case2_case.prepare_runs(), CUInt(6))
        assert_equal(case2_case.finish_runs(), CUInt(7))
        assert_equal(case2_case.d_runs(), CUInt(5))
        assert_equal(case2_case.multi_threading_case_runs(), uint32_0)

        assert_true(cases(2).prepare())
        assert_true(cases(2).run())
        assert_true(cases(2).finish())
        assert_equal(case2_case.constructed(), CUInt(3))
        assert_equal(case2_case.prepare_runs(), CUInt(9))
        assert_equal(case2_case.finish_runs(), CUInt(14))
        assert_equal(case2_case.d_runs(), CUInt(5))
        assert_equal(case2_case.multi_threading_case_runs(),
                     case2_case.multi_threading_case_repeat_count * case2_case.multi_threading_case_thread_count)
        Return True
    End Function
End Class
