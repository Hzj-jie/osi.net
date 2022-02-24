
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock

' Control behavior in program level, without changing environment variables
<global_init(global_init_level.functor)>
Public NotInheritable Class suppress
    Public Shared ReadOnly compare_error As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "compare", "error")))
    Public Shared ReadOnly invoker_error As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "invoker", "error")))
    Public Shared ReadOnly valuer_error As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "valuer", "error")))
    Public Shared ReadOnly pending_io_punishment As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "pending", "io", "punishment")) OrElse
                        env_bool(env_keys("disable", "pending", "io", "punishment")))
    Public Shared ReadOnly rebind_global_value_error As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "rebind", "global", "value", "error")) OrElse
                        env_bool(env_keys("suppress", "rebind", "error")))
    Public Shared ReadOnly alloc_error As atomic_bool =
        New atomic_bool(env_bool(env_keys("suppress", "alloc", "error")))
    Private Shared ReadOnly m As hashmapless(Of String, atomic_bool) = hashmapless(Of String, atomic_bool).tiny()

    Private Shared Sub register(Of PROTECTOR)(ByVal i As atomic_bool)
        assert(Not i Is Nothing)
        global_resolver(Of Func(Of Boolean), PROTECTOR).assert_first_register(
                Function() As Boolean
                    Return i.true_()
                End Function)
    End Sub

    Public Shared Function [on](ByVal key As String) As atomic_bool
        Return m(key)
    End Function

    Private Shared Function m_init_state() As Boolean
        Dim r As Boolean = True
        m.foreach(Function(ByVal k As String, ByVal v As atomic_bool) As Boolean
                      If Not v.init_state() Then
                          r = False
                      End If
                      Return r
                  End Function)
        Return r
    End Function

    Public Shared Function init_state() As Boolean
        Return compare_error.init_state() AndAlso
               invoker_error.init_state() AndAlso
               valuer_error.init_state() AndAlso
               pending_io_punishment.init_state() AndAlso
               rebind_global_value_error.init_state() AndAlso
               alloc_error.init_state() AndAlso
               m_init_state()
    End Function

    Private Shared Sub init()
        register(Of is_suppressed.compare_error_protector)(compare_error)
        register(Of is_suppressed.rebind_global_value_protector)(rebind_global_value_error)
        register(Of is_suppressed.alloc_error_protector)(alloc_error)
        global_resolver(Of Func(Of String, Boolean), is_suppressed).assert_first_register(
            Function(ByVal key As String) As Boolean
                Dim a As atomic_bool = Nothing
                If Not m.get(key, a) Then
                    Return False
                End If
                assert(Not a Is Nothing)
                Return +a
            End Function)
    End Sub

    Private Sub New()
    End Sub
End Class
