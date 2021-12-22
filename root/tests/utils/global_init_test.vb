
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.tests.root.utils.global_init_test_private_namespace

Namespace global_init_test_private_namespace
    <global_init(True)>
    Friend Module init_in_module
        Private ReadOnly i As New atomic_int()

        Public Sub init()
            i.increment()
        End Sub

        Public Function invoke_times() As Int32
            Return (+i)
        End Function
    End Module

    Friend NotInheritable Class init_in_module_cctor_holder
        Private Shared ReadOnly i As New atomic_int()

        Public Shared Sub execute()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function
    End Class

    <global_init(True)>
    Friend Module init_in_module_cctor
        Sub New()
            init_in_module_cctor_holder.execute()
        End Sub
    End Module

    Friend NotInheritable Class init_in_module_cctor_with_init_holder
        Private Shared ReadOnly i As New atomic_int()

        Public Shared Sub execute()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function
    End Class

    <global_init(True)>
    Friend Module init_in_module_cctor_with_init
        Sub New()
            init_in_module_cctor_with_init_holder.execute()
        End Sub

        Private Sub init()
        End Sub
    End Module
End Namespace

Public NotInheritable Class global_init_test
    Inherits [case]

    <global_init(True)>
    Public NotInheritable Class init_in_class_init
        Private Shared ReadOnly i As New atomic_int()

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
    Public NotInheritable Class init_in_class_init_not_once
        Private Shared ReadOnly i As New atomic_int()

        Public Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return (+i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class init_in_class_cctor_without_init_holder
        Private Shared ReadOnly i As New atomic_int()

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
    Public NotInheritable Class init_in_class_cctor_without_init
        Shared Sub New()
            init_in_class_cctor_without_init_holder.execute()
        End Sub

        Private Sub New()
        End Sub
    End Class

    <global_init(True)>
    Public NotInheritable Class init_in_class_cctor_and_init
        Private Shared ReadOnly i As New atomic_int()

        Shared Sub New()
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
    Private NotInheritable Class init_in_class_private_init
        Private Shared ReadOnly i As New atomic_int()

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return +i
        End Function
    End Class

    <global_init(False, global_init_level.other)>
    Private NotInheritable Class init_in_class_private_init_not_once
        Public Shared ReadOnly i As New atomic_int()

        Private Shared Sub init()
            i.increment()
        End Sub

        Public Shared Function invoke_times() As Int32
            Return +i
        End Function
    End Class

    Private NotInheritable Class should_not_invoke_cctor_until_init_holder
        Public Shared count As New atomic_int()

        Private Sub New()
        End Sub
    End Class

    <global_init(False, global_init_level.not_run)>
    Private NotInheritable Class should_not_invoke_cctor_until_init
        Shared Sub New()
            should_not_invoke_cctor_until_init_holder.count.increment()
        End Sub

        Private Shared Sub init()
            should_not_invoke_cctor_until_init_holder.count.increment()
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        type_lock(Of global_init).wait()
        init_in_class_private_init_not_once.i.set(0)
        global_init.execute()
        global_init.execute()
        global_init.execute(global_init_level.max - 1)

        assertion.more_or_equal(global_init.init_times(), 2 + 3)

        assertion.equal(init_in_class_init.invoke_times(), 1)

        assertion.equal(init_in_class_init_not_once.invoke_times(), global_init.init_times())

        assertion.equal(init_in_class_cctor_without_init_holder.invoke_times(), 1)

        assertion.equal(init_in_class_cctor_and_init.invoke_times(), 2)

        assertion.equal(init_in_class_private_init.invoke_times(), 1)

        assertion.equal(init_in_module.invoke_times(), 1)

        assertion.equal(init_in_module_cctor_holder.invoke_times(), 0)

        assertion.equal(init_in_module_cctor_with_init_holder.invoke_times(), 1)

        assertion.equal(init_in_class_private_init_not_once.invoke_times(), 2)

        assertions.of(+should_not_invoke_cctor_until_init_holder.count).equal(0)
        global_init.execute(global_init_level.not_run)
        assertions.of(+should_not_invoke_cctor_until_init_holder.count).equal(2)

        type_lock(Of global_init).release()
        Return True
    End Function
End Class
