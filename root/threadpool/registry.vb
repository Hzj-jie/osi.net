
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils

Public Module _auto_updating_resolver
    Public Function thread_pool() As slimqless2_threadpool2
        Return newable_global_instance(Of slimqless2_threadpool2).ref_new()
    End Function
End Module
