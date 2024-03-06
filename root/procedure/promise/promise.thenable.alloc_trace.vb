
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils

Partial Public NotInheritable Class promise
    Partial Private Class thenable
        Private Sub trace_start()
            counter.instance_count_counter(Of promise).alloc()
        End Sub

        Private Sub trace_stop()
            counter.instance_count_counter(Of promise).dealloc()
        End Sub
    End Class
End Class
