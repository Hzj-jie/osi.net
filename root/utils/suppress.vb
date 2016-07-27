
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock

' Control behavior in program level, without changing environment variables
<global_init(global_init_level.foundamental)>
Public Class suppress
    Public Shared ReadOnly compare_error As atomic_bool
    Public Shared ReadOnly invoker_error As atomic_bool
    Public Shared ReadOnly valuer_error As atomic_bool
    Public Shared ReadOnly pending_io_punishment As atomic_bool
    Public Shared ReadOnly rebind_global_value_error As atomic_bool
    Public Shared ReadOnly alloc_error As atomic_bool

    Shared Sub New()
        compare_error = New atomic_bool(env_bool(env_keys("suppress", "compare", "error")))
        invoker_error = New atomic_bool(env_bool(env_keys("suppress", "invoker", "error")))
        valuer_error = New atomic_bool(env_bool(env_keys("suppress", "valuer", "error")))
        pending_io_punishment = New atomic_bool(env_bool(env_keys("suppress", "pending", "io", "punishment")) OrElse
                                                env_bool(env_keys("disable", "pending", "io", "punishment")))
        rebind_global_value_error =
            New atomic_bool(env_bool(env_keys("suppress", "rebind", "global", "value", "error")) OrElse
                            env_bool(env_keys("suppress", "rebind", "error")))
        alloc_error = New atomic_bool(env_bool(env_keys("suppress", "alloc", "error")))

        binder(Of Func(Of Boolean), suppress_compare_error_binder_protector).set_global(
            Function() As Boolean
                Return compare_error.true_()
            End Function)
        binder(Of Func(Of Boolean), suppress_rebind_global_value_error_binder_protector).set_global(
            Function() As Boolean
                Return rebind_global_value_error.true_()
            End Function)
        binder(Of Func(Of Boolean), suppress_alloc_error_binder_protector).set_global(
            Function() As Boolean
                Return alloc_error.true_()
            End Function)
    End Sub

    Public Shared Function init_state() As Boolean
        Return compare_error.init_state() AndAlso
               invoker_error.init_state() AndAlso
               valuer_error.init_state() AndAlso
               pending_io_punishment.init_state() AndAlso
               rebind_global_value_error.init_state() AndAlso
               alloc_error.init_state()
    End Function

    Private Sub New()
    End Sub

    Private Shared Sub init()
    End Sub
End Class
