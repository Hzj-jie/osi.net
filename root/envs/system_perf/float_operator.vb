
Imports osi.root.constants.system_perf

Public Class float_operator
    Public Shared Sub run()
        Dim t As Double = 0
        For i As Int32 = 0 To float_operator_size - 1
            Dim x As Double = 0
            x = (Math.PI + If(i And 1 = 1, 1.0, 0.0)) * (Math.E - If(i And 1 = 1, 1.0, 0.0))
            x += If(i And 2 = 1, 1.0, -1.0)
            x /= If(i And 4 = 1, 1.1, 0.9)
            t += x
        Next
    End Sub
End Class
