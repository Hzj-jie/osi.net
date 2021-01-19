
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple7.vbp ----------
'so change tuple7.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple.vbp ----------
'so change tuple.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

' To reduce complexity, tuple is always const, use ref if the fields are required to be muttable.
Partial Public Structure tuple(Of T1, T2, T3, T4, T5, T6, T7)
    Implements ICloneable, ICloneable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7)),
               IComparable, IComparable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7))

    Private __1 As T1

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _1() As T1
        Return __1
    End Function

    Private __2 As T2

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _2() As T2
        Return __2
    End Function

#If 7 > 2 Then
    Private __3 As T3

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _3() As T3
        Return __3
    End Function
#End If

#If 7 > 3 Then
    Private __4 As T4

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _4() As T4
        Return __4
    End Function
#End If

#If 7 > 4 Then
    Private __5 As T5

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _5() As T5
        Return __5
    End Function
#End If

#If 7 > 5 Then
    Private __6 As T6

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _6() As T6
        Return __6
    End Function
#End If

#If 7 > 6 Then
    Private __7 As T7

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _7() As T7
        Return __7
    End Function
#End If

#If 7 > 7 Then
    Private __8 As T8

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function _8() As T8
        Return __8
    End Function
#End If

#If 7 = 2 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2)
#ElseIf 7 = 3 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3)
#ElseIf 7 = 4 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3,
                   ByVal _4 As T4)
#ElseIf 7 = 5 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3,
                   ByVal _4 As T4,
                   ByVal _5 As T5)
#ElseIf 7 = 6 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3,
                   ByVal _4 As T4,
                   ByVal _5 As T5,
                   ByVal _6 As T6)
#ElseIf 7 = 7 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3,
                   ByVal _4 As T4,
                   ByVal _5 As T5,
                   ByVal _6 As T6,
                   ByVal _7 As T7)
#ElseIf 7 = 8 Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal _1 As T1,
                   ByVal _2 As T2,
                   ByVal _3 As T3,
                   ByVal _4 As T4,
                   ByVal _5 As T5,
                   ByVal _6 As T6,
                   ByVal _7 As T7,
                   ByVal _8 As T8)
#Else
    Unexpected SIZE (7)
#End If
        Me.__1 = _1
        Me.__2 = _2
#If 7 > 2 Then
        Me.__3 = _3
#End If
#If 7 > 3 Then
        Me.__4 = _4
#End If
#If 7 > 4 Then
        Me.__5 = _5
#End If
#If 7 > 5 Then
        Me.__6 = _6
#End If
#If 7 > 6 Then
        Me.__7 = _7
#End If
#If 7 > 7 Then
        Me.__8 = _8
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As tuple(Of T1, T2, T3, T4, T5, T6, T7) _
                                Implements ICloneable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7)).Clone
#If 7 = 2 Then
        Return tuple.of(__1, __2)
#ElseIf 7 = 3 Then
        Return tuple.of(__1, __2, __3)
#ElseIf 7 = 4 Then
        Return tuple.of(__1, __2, __3, __4)
#ElseIf 7 = 5 Then
        Return tuple.of(__1, __2, __3, __4, __5)
#ElseIf 7 = 6 Then
        Return tuple.of(__1, __2, __3, __4, __5, __6)
#ElseIf 7 = 7 Then
        Return tuple.of(__1, __2, __3, __4, __5, __6, __7)
#ElseIf 7 = 8 Then
        Return tuple.of(__1, __2, __3, __4, __5, __6, __7, __8)
#Else
        Unexpected SIZE (7)
#End If
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal other As tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Int32 _
                             Implements IComparable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7)).CompareTo
        Dim r As Int32 = 0

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__1, other.__1)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__2, other.__2)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#If 7 > 2 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__3, other.__3)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
#If 7 > 3 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__4, other.__4)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
#If 7 > 4 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__5, other.__5)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
#If 7 > 5 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__6, other.__6)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
#If 7 > 6 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__7, other.__7)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
#If 7 > 7 Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_compare.vbp ----------
'so change tuple_compare.vbp instead of this file


    r = compare(__8, other.__8)
    If r <> 0 Then
        Return r
    End If
'finish tuple_compare.vbp --------
#End If
        Return 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of tuple(Of T1, T2, T3, T4, T5, T6, T7))(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As tuple(Of T1, T2, T3, T4, T5, T6, T7),
                             ByVal that As tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
        Return compare(this, that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As tuple(Of T1, T2, T3, T4, T5, T6, T7),
                             ByVal that As Object) As Boolean
        Return compare(this, that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As tuple(Of T1, T2, T3, T4, T5, T6, T7),
                              ByVal that As tuple(Of T1, T2, T3, T4, T5, T6, T7)) As Boolean
        Return compare(this, that) <> 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As tuple(Of T1, T2, T3, T4, T5, T6, T7),
                              ByVal that As Object) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Overrides Function ToString() As String
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        r.Append(Convert.ToString(__1))
        r.Append(", ")
        r.Append(Convert.ToString(__2))
#If 7 > 2 Then
        r.Append(", ")
        r.Append(Convert.ToString(__3))
#End If
#If 7 > 3 Then
        r.Append(", ")
        r.Append(Convert.ToString(__4))
#End If
#If 7 > 4 Then
        r.Append(", ")
        r.Append(Convert.ToString(__5))
#End If
#If 7 > 5 Then
        r.Append(", ")
        r.Append(Convert.ToString(__6))
#End If
#If 7 > 6 Then
        r.Append(", ")
        r.Append(Convert.ToString(__7))
#End If
#If 7 > 7 Then
        r.Append(", ")
        r.Append(Convert.ToString(__8))
#End If
        Return Convert.ToString(r)
    End Function

    Public Overrides Function GetHashCode() As Int32
        Dim r As Int32 = 0
        r = r Xor If(__1 Is Nothing, 0, __1.GetHashCode())
        r = r Xor If(__2 Is Nothing, 0, __2.GetHashCode())
#If 7 > 2 Then
        r = r Xor If(__3 Is Nothing, 0, __3.GetHashCode())
#End If
#If 7 > 3 Then
        r = r Xor If(__4 Is Nothing, 0, __4.GetHashCode())
#End If
#If 7 > 4 Then
        r = r Xor If(__5 Is Nothing, 0, __5.GetHashCode())
#End If
#If 7 > 5 Then
        r = r Xor If(__6 Is Nothing, 0, __6.GetHashCode())
#End If
#If 7 > 6 Then
        r = r Xor If(__7 Is Nothing, 0, __7.GetHashCode())
#End If
#If 7 > 7 Then
        r = r Xor If(__8 Is Nothing, 0, __8.GetHashCode())
#End If
        Return r
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return compare(Me, obj) = 0
    End Function
End Structure
'finish tuple.vbp --------
'finish tuple7.vbp --------
