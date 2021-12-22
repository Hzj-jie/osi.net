
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class global_init_test2
    Private NotInheritable Class should_not_invoke_cctor_holder
        Public Shared count As New atomic_int()
    End Class

    <global_init(False, global_init_level.not_run)>
    Private NotInheritable Class should_not_invoke_cctor_init
        Shared Sub New()
            should_not_invoke_cctor_holder.count.increment()
        End Sub

        Private Shared Sub init()
            should_not_invoke_cctor_holder.count.increment()
        End Sub
    End Class

    <test>
    Private Shared Sub should_not_invoke_cctor_before_init()
        type_lock(Of global_init).wait()
        assertions.of(+should_not_invoke_cctor_holder.count).equal(0)
        global_init.execute(global_init_level.not_run - 1)
        assertions.of(+should_not_invoke_cctor_holder.count).equal(0)
        global_init.execute(global_init_level.not_run)
        assertions.of(+should_not_invoke_cctor_holder.count).equal(2)
        type_lock(Of global_init).release()
    End Sub

    Private Sub New()
    End Sub
End Class
