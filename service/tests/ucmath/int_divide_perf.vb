
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with int_divide_perf.vbp ----------
'so change int_divide_perf.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with operator_perf.vbp ----------
'so change operator_perf.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_perf.vbp ----------
'so change binary_operator_perf.vbp instead of this file


Imports osi.root.connector
Imports osi.root.utt

Public Class int_divide_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New int_divide_case())
    End Sub

    Private Class int_divide_case
        Inherits [case]

        Private ReadOnly r1 As Int32
        Private ReadOnly r2 As Int32

        Public Sub New()
            r1 = rnd_int()
            r2 = 0
            While r2 = 0
                r2 = rnd_int()
            End While
			Dim r As Int32 = 0
			r = r1 \ r2
        End Sub

        Public Overrides Function run() As Boolean
            For i As Int64 = 0 To 1073741824 - 1
                Dim r As Int32 = 0
				r = r1 \ r2
            Next
            Return True
        End Function
    End Class
End Class
'finish binary_operator_perf.vbp --------
'finish operator_perf.vbp --------
'finish int_divide_perf.vbp --------
