
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class global_init_order_test
    Inherits [case]

    <global_init(False, uint8_0)>
    Private Class case1
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    <global_init(False, uint8_1)>
    Private Class case2
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
            assert_equal(case1.initialized_times(), initialized_times())
            assert_equal(case4.initialized_times(), initialized_times())
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    <global_init(False, CByte(2))>
    Private Class case3
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
            assert_equal(case1.initialized_times(), initialized_times())
            assert_equal(case2.initialized_times(), initialized_times())
            assert_equal(case4.initialized_times(), initialized_times())
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    <global_init(False, uint8_0)>
    Private Class case4
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    Public Overrides Function run() As Boolean
        global_init.execute()
        assert_more(case1.initialized_times(), uint32_1)
        assert_more(case2.initialized_times(), uint32_1)
        assert_more(case3.initialized_times(), uint32_1)
        assert_more(case4.initialized_times(), uint32_1)
        assert_equal(case1.initialized_times(), case2.initialized_times())
        assert_equal(case1.initialized_times(), case3.initialized_times())
        assert_equal(case1.initialized_times(), case4.initialized_times())
        Return True
    End Function
End Class
