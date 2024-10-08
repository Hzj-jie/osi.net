
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class global_init_order_test
    Inherits [case]

    <global_init(False, uint8_0)>
    Private NotInheritable Class case1
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
    Private NotInheritable Class case2
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
            assertion.equal(case1.initialized_times(), initialized_times())
            assertion.equal(case4.initialized_times(), initialized_times())
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    <global_init(False, CByte(2))>
    Private NotInheritable Class case3
        Private Shared ReadOnly i As atomic_uint32

        Shared Sub New()
            i = New atomic_uint32()
        End Sub

        Private Shared Sub init()
            i.increment()
            assertion.equal(case1.initialized_times(), initialized_times())
            assertion.equal(case2.initialized_times(), initialized_times())
            assertion.equal(case4.initialized_times(), initialized_times())
        End Sub

        Public Shared Function initialized_times() As UInt32
            Return (+i)
        End Function
    End Class

    <global_init(False, uint8_0)>
    Private NotInheritable Class case4
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
        type_lock(Of global_init).wait()
        global_init.execute()
        assertion.more(case1.initialized_times(), uint32_1)
        assertion.more(case2.initialized_times(), uint32_1)
        assertion.more(case3.initialized_times(), uint32_1)
        assertion.more(case4.initialized_times(), uint32_1)
        assertion.equal(case1.initialized_times(), case2.initialized_times())
        assertion.equal(case1.initialized_times(), case3.initialized_times())
        assertion.equal(case1.initialized_times(), case4.initialized_times())
        type_lock(Of global_init).release()
        Return True
    End Function
End Class
