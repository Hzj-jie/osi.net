
Imports System.Threading
Imports osi.root.constants.system_perf

Public Class atomic_operator
    Public Shared Sub run()
        For i As Int32 = 0 To atomic_operator_size - 1
            Dim a As Int64
            Interlocked.CompareExchange(a, 1, 0)
            Interlocked.Exchange(a, 1)
            Interlocked.Increment(a)
            Interlocked.Decrement(a)
            Interlocked.Add(a, 1)
            Interlocked.Read(a)
        Next
    End Sub
End Class
