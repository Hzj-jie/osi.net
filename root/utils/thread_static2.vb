
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation

' ~23x slower than internal <ThreadStatic()> or connector.thread_static.
Public Class thread_static2(Of T)
    Private ReadOnly m As hashmapless(Of UInt32, T, _uint32_to_uint32)

    Public Sub New()
        m = New hashmapless(Of UInt32, T, _uint32_to_uint32)(63)
    End Sub

    Private Function current_id() As UInt32
        Dim r As Int32 = 0
        r = current_thread_id() - 1
        assert(r >= 0)
        Return CUInt(r)
    End Function

    Public Function [get]() As T
        Dim r As T = Nothing
        If m.get(current_id(), r) Then
            Return r
        End If
        Return Nothing
    End Function

    Public Sub [set](ByVal i As T)
        m(current_id()) = i
    End Sub

    Public Sub clear()
        m.clear()
    End Sub

    Public Shared Operator +(ByVal this As thread_static2(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.get()
    End Operator
End Class
