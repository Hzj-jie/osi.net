
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A ticks retriever to return ticks always larger than last return even in multiple threads.
' Thread A:
' while(!a_bool) {}
' long a_v = strong_high_res_ticks_retriever.ticks();
' Thread B:
' a_bool = true;
' long b_v = strong_high_res_ticks_retriever.ticks();
' assert(b_v >= a_v);
Public NotInheritable Class strong_high_res_ticks_retriever
    Private Shared ReadOnly my_type As Type = GetType(strong_high_res_ticks_retriever)
    Private Shared last As Int64 = min_int64

    Public Shared Function ticks() As Int64
        SyncLock my_type
            While True
                Dim v As Int64 = 0
                v = high_res_ticks_retriever.ticks()
                If v >= last Then
                    last = v
                    Return v
                End If
            End While
        End SyncLock
        assert(False)
        Return 0
    End Function

    Private Sub New()
    End Sub
End Class
