
#Const USE_FAST_THREAD_POOL = False

#If USE_FAST_THREAD_POOL Then

Public Module _auto_updating_resolver
    Public Function thread_pool() As fast_threadpool
        Return fast_threadpool.instance
    End Function
End Module

Public Module _registry
    Public Sub register_managed_threadpool()
    End Sub

    Public Sub register_qless_threadpool()
    End Sub

    Public Sub register_slimqless2_threadpool()
    End Sub

    Public Sub register_slimheapless_threadpool()
    End Sub

    Public Function using_default_ithreadpool() As Boolean
        Return True
    End Function
End Module

#Else

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _auto_updating_resolver
    Private ReadOnly p As disposer(Of weak_pointer(Of ithreadpool))

    Sub New()
        p = resolver.auto_updating_resolve(Of ithreadpool)()
    End Sub

    Public Function thread_pool() As ithreadpool
        Dim o As ithreadpool = Nothing
        Return If((+p).get(o), o, managed_threadpool.global)
    End Function
End Module

<global_init(global_init_level.threading_and_procedure)>
Public Module _registry
    Public Sub register_managed_threadpool()
        managed_threadpool.register()
    End Sub

    Public Sub register_qless_threadpool()
        qless_threadpool.register()
    End Sub

    Public Sub register_slimqless2_threadpool()
        slimqless2_threadpool.register()
    End Sub

    Public Sub register_slimheapless_threadpool()
        slimheapless_threadpool.register()
    End Sub

    Public Function using_default_ithreadpool() As Boolean
        Return TypeOf thread_pool() Is slimqless2_threadpool
    End Function

    Private Sub init()
        register_slimqless2_threadpool()
    End Sub
End Module

#End If
