
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Structure one_of(Of T1, T2)
    Implements ICloneable, ICloneable(Of one_of(Of T1, T2)),
               IComparable, IComparable(Of one_of(Of T1, T2))

    Private ReadOnly t As tuple(Of Boolean, T1, T2)

    ' Just in case T1 == T2
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function of_first(ByVal v As T1) As one_of(Of T1, T2)
        Return New one_of(Of T1, T2)(tuple.emplace_of(True, v, [default](Of T2).null))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function of_second(ByVal v As T2) As one_of(Of T1, T2)
        Return New one_of(Of T1, T2)(tuple.emplace_of(False, [default](Of T1).null, v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal t As tuple(Of Boolean, T1, T2))
        Me.t = t
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function is_first() As Boolean
        Return t._1()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function first() As T1
        assert(is_first())
        Return t._2()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function is_second() As Boolean
        Return Not is_first()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function second() As T2
        assert(is_second())
        Return t._3()
    End Function

    Public Function map(Of R)(ByVal v1 As Func(Of T1, R), ByVal v2 As Func(Of T2, R)) As R
        assert(Not v1 Is Nothing)
        assert(Not v2 Is Nothing)
        If is_first() Then
            Return v1(first())
        End If
        Return v2(second())
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As one_of(Of T1, T2) Implements ICloneable(Of one_of(Of T1, T2)).Clone
        Return New one_of(Of T1, T2)(t.CloneT())
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of one_of(Of T1, T2))().from(obj, False))
    End Function

    Public Function CompareTo(ByVal other As one_of(Of T1, T2)) As Int32 _
            Implements IComparable(Of one_of(Of T1, T2)).CompareTo
        Return t.CompareTo(other.t)
    End Function
End Structure