
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _distribution
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_range(ByVal i As one_of(Of tuple(Of Double, Double), vector(Of Double))) _
                                      As Boolean
        Return i.is_first()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function range(ByVal i As one_of(Of tuple(Of Double, Double), vector(Of Double))) _
                                      As tuple(Of Double, Double)
        Return i.first()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function as_range(ByVal i As tuple(Of Double, Double)) _
                                      As one_of(Of tuple(Of Double, Double), vector(Of Double))
        Return one_of(Of tuple(Of Double, Double), vector(Of Double)).of_first(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_samples(ByVal i As one_of(Of tuple(Of Double, Double), vector(Of Double))) _
                                      As Boolean
        Return i.is_second()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function samples(ByVal i As one_of(Of tuple(Of Double, Double), vector(Of Double))) _
                                      As vector(Of Double)
        Return i.second()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function as_samples(ByVal i As vector(Of Double)) _
                                      As one_of(Of tuple(Of Double, Double), vector(Of Double))
        assert(Not i Is Nothing)
        Return one_of(Of tuple(Of Double, Double), vector(Of Double)).of_second(i)
    End Function
End Module

Public Interface distribution
    Function possibility(ByVal v As Double) As Double
    Function cumulative_distribute(ByVal v As Double) As Double
    Function range_possibility(ByVal min As Double, ByVal max As Double) As Double
    Function parameter_space() As one_of(Of tuple(Of Double, Double), vector(Of Double))
End Interface
