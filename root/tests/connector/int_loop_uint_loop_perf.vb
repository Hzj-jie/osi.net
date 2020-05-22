
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt

Public NotInheritable Class int_loop_uint_loop_perf
    Inherits performance_comparison_case_wrapper

    Private Const loops As Int32 = max_int32 - 1

    Public Sub New()
        MyBase.New(New int_loop(), New uint_loop())
    End Sub

    Private NotInheritable Class int_loop
        Inherits [case]

        Public Overrides Function run() As Boolean
            For i As Int32 = 0 To loops
            Next
            Return True
        End Function
    End Class

    Private NotInheritable Class uint_loop
        Inherits [case]

        Public Overrides Function run() As Boolean
            For i As UInt32 = CUInt(0) To CUInt(loops) Step uint32_1
            Next
            Return True
        End Function
    End Class
End Class
