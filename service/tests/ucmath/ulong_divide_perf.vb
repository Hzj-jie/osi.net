
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ulong_divide_perf.vbp ----------
'so change ulong_divide_perf.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with operator_perf.vbp ----------
'so change operator_perf.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_perf.vbp ----------
'so change binary_operator_perf.vbp instead of this file


Imports osi.root.connector
Imports osi.root.utt

Public NotInheritable Class ulong_divide_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New ulong_divide_case())
    End Sub

    Private Class ulong_divide_case
        Inherits [case]

        Private ReadOnly r1 As UInt64
        Private ReadOnly r2 As UInt64

        Public Sub New()
            r1 = rnd_uint64()
            r2 = 0
            While r2 = 0
                r2 = rnd_uint64()
            End While
            Dim r As UInt64 = 0
            r = r1 \ r2
        End Sub

        Public Overrides Function run() As Boolean
            For i As Int64 = 0 To 1073741824L - 1
                Dim r As UInt64 = 0
                r = r1 \ r2
            Next
            Return True
        End Function
    End Class
End Class
'finish binary_operator_perf.vbp --------
'finish operator_perf.vbp --------
'finish ulong_divide_perf.vbp --------
