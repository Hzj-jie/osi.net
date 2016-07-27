
Imports System.DateTime
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.lock

Public Module _fake
    Public Sub fake_processor_work(ByVal ms As Int64)
        Dim s As Int64 = 0
        s = Now().milliseconds()
        strict_wait_when(Function() Now().milliseconds() - s < ms)
    End Sub

    Public Sub fake_processor_ticks_work(ByVal ticks As Int64)
        Dim s As Int64 = 0
        s = nowadays.high_res_ticks()
        strict_wait_when(Function() nowadays.high_res_ticks() - s < ticks)
    End Sub

    <MethodImpl(MethodImplOptions.NoInlining)> _
    Public Sub do_nothing(ByVal v As Boolean)
    End Sub
End Module
