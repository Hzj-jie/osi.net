
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants.system_perf

Public NotInheritable Class memory_access
    Private Shared r As Int32 = 0

    Public Shared Sub run()
        r += 1
        For i As Int32 = 0 To memory_access_size - 1
            Dim s As Int32 = 0
            s = memory_access_memory_size + (i << 10) + (r << 10)
            Dim a() As Byte = Nothing
            ReDim a(s - 1)
            For j As Int32 = 1 To 16
                a(rnd_int(0, s)) = 7
            Next
            Erase a
        Next
        For i As Int32 = 0 To memory_access_size - 1
            Dim x As memory_access = Nothing
            x = New memory_access()
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
