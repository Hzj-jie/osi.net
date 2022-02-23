
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
        assert(i IsNot Nothing)
        Return one_of(Of tuple(Of Double, Double), vector(Of Double)).of_second(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function union_with(ByVal i As tuple(Of Double, Double),
                                             ByVal j As tuple(Of Double, Double)) As tuple(Of Double, Double)
        Return tuple.of(min(i.first(), j.first()), max(i.second(), j.second()))
    End Function
End Module

Public Interface distribution
    ' f(v) = p{x = v}
    Function possibility(ByVal v As Double) As Double
    ' F(v) = p{x <= v}
    Function cumulative_distribute(ByVal v As Double) As Double
    ' p{min <= x <= max}
    Function range_possibility(ByVal min As Double, ByVal max As Double) As Double
    ' x is within range [a, b), or x is within collection (a, b, c, ...).
    Function parameter_space() As one_of(Of tuple(Of Double, Double), vector(Of Double))
    ' a range covering say 99.9% of range_possibility.
    ' The "significant" is definited per implementation without a standard rule, but should never less than 99%.
    ' This range is only used when parameter_space returns a range rather than a collection.
    Function significant_range() As tuple(Of Double, Double)
End Interface
