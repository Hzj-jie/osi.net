
Imports osi.root.connector
Imports osi.root.constants.system_perf

'test integer number, stack pushing and small memory allocation perf
Public Class fibonacci
    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Private Shared Function fibonacci(ByVal i As Int32) As Double
        assert(i < fibonacci_size)
        assert(i >= 0)
        If i = 0 OrElse i = 1 Then
            Return 1
        Else
            Return fibonacci(i - 1) + fibonacci(i - 2)
        End If
    End Function

    Public Shared Sub run()
        fibonacci(fibonacci_size - 1)
    End Sub
End Class
