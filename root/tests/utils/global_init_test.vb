
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.tests.root.utils.global_init_test_private_namespace

Namespace global_init_test_private_namespace
    <global_init(True)>
    Friend Module global_init_case_6
        Private ReadOnly i As atomic_int

        Sub New()
            i = New atomic_int()
        End Sub

        Public Sub init()
            i.increment()
        End Sub

        Public Function invoke_times() As Int32
            Return (+i)
        End Function
    End Module

    Friend Class global_init_case_7_holder
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Public Shared Sub execute()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function
    End Class

    <global_init(True)>
    Friend Module global_init_case_7
        Sub New()
            global_init_case_7_holder.execute()
        End Sub
    End Module

    Friend Class global_init_case_8_holder
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Public Shared Sub execute()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function
    End Class

    <global_init(True)>
    Friend Module global_init_case_8
        Sub New()
            global_init_case_8_holder.execute()
        End Sub

        Private Sub init()
        End Sub
    End Module
End Namespace

Public Class global_init_test
    Inherits [case]

    <global_init(True)>
    Public Class global_init_case_1
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Public Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function

        Private Sub New()
        End Sub
    End Class

    <global_init(False)>
    Public Class global_init_case_2
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Public Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Class global_init_case_3_holder
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Public Shared Sub execute()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function

        Private Sub New()
        End Sub
    End Class

    <global_init(True)>
    Public Class global_init_case_3

        Shared Sub New()
            global_init_case_3_holder.execute()
        End Sub

        Private Sub New()
        End Sub
    End Class

    <global_init(True)>
    Public Class global_init_case_4
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
            i.increment()
        End Sub

        Public Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return +i
        End Function
    End Class

    <global_init(True)>
    Private Class global_init_case_5
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return +i
        End Function
    End Class

    <global_init(False, global_init_level.other)>
    Private Class global_init_case_9
        Private Shared ReadOnly i As atomic_int

        Shared Sub New()
            i = New atomic_int()
        End Sub

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return +i
        End Function
    End Class

    Public Overrides Function run() As Boolean
        type_lock(Of global_init).wait()
        global_init.execute()
        global_init.execute()
        global_init.execute(global_init_level.max - 1)
        assert_more_or_equal(global_init.init_times(), 2 + 3)
        assert_equal(global_init_case_1.invoke_times(), 1)
        'when the first global_init.execute is called, this assembly has not been loaded yet
        assert_equal(global_init_case_2.invoke_times(), global_init.init_times() - 1)
        assert_equal(global_init_case_3_holder.invoke_times(), 1)
        assert_equal(global_init_case_4.invoke_times(), 2)
        assert_equal(global_init_case_5.invoke_times(), 1)
        assert_equal(global_init_case_6.invoke_times(), 1)
        assert_equal(global_init_case_7_holder.invoke_times(), 0)
        assert_equal(global_init_case_8_holder.invoke_times(), 1)
        assert_equal(global_init_case_9.invoke_times(), global_init.init_times() - 2)
        type_lock(Of global_init).release()
        Return True
    End Function
End Class
