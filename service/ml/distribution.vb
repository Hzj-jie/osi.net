
Option Explicit On
Option Infer Off
Option Strict On

Public Interface distribution
    Function possibility(ByVal v As Double) As Double
    Function cumulative_distribute(ByVal v As Double) As Double
    Function range_possibility(ByVal min As Double, ByVal max As Double) As Double
End Interface
