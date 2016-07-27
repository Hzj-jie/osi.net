
#If 0 Then
Imports osi.root.constants.system_perf

Public Class thread_static_operator
    <ThreadStatic()> Private Shared a As Int64

    Public Shared Sub run()
        For i As Int32 = 0 To thread_static_size - 1
            a += 1
            a -= 1
            If a = 0 Then
                a = 1
            End If
            a = 0
        Next
    End Sub
End Class
#End If
