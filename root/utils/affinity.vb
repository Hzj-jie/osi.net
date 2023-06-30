
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Public Module _affinity
    Public Function set_thread_affinity(ByVal ParamArray processors() As UInt32) As Boolean
        If isemptyarray(processors) Then
            Return False
        Else
            Dim mask As Int64 = 0
            For i As Int32 = 0 To array_size_i(processors) - 1
                If processors(i) >= Environment.ProcessorCount() Then
                    Return False
                Else
                    mask = (mask Or (1 << CInt(processors(i))))
                End If
            Next

            Try
                current_process_thread().ProcessorAffinity() = New IntPtr(mask)
                Return True
            Catch ex As Exception
                raise_error(error_type.warning, "failed to set thread processor affinity, ex ", ex.Message())
                Return False
            End Try
        End If
    End Function

    Public Function loop_set_thread_affinity(ByVal ParamArray processors() As UInt32) As Boolean
        For i As Int32 = 0 To array_size_i(processors) - 1
            processors(i) = (processors(i) Mod CUInt(Environment.ProcessorCount()))
        Next
        Return set_thread_affinity(processors)
    End Function
End Module
