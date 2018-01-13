
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock

' Control behavior in program level, without changing environment variables
<global_init(global_init_level.foundamental)>
Public NotInheritable Class suppress
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

        register(Of is_suppressed.compare_error_protector)(compare_error)
        register(Of is_suppressed.rebind_global_value_protector)(rebind_global_value_error)
        register(Of is_suppressed.alloc_error_protector)(alloc_error)
    End Sub

    Private Shared Sub register(Of PROTECTOR)(ByVal i As atomic_bool)
        assert(Not i Is Nothing)
        global_resolver(Of Func(Of Boolean), PROTECTOR).assert_first_register(
                Function() As Boolean
                    Return i.true_()
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
