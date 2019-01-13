
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with pair.vbp ----------
'so change pair.vbp instead of this file


#Const IS_CONST = ("" = "const_")
#Const IS_FIRST_CONST = ("" = "first_const_")
#Const IS_CLASS = ("Class" = "Class")

Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

Public NotInheritable Class pair
    Public Shared Function [of](Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(first, second)
    End Function

    Public Shared Function emplace_of(Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(first, second)
    End Function

    Public Shared Function [of](Of FT, ST)(ByVal i as KeyValuePair(Of FT, ST)) As pair(Of FT, ST)
        Return pair(Of FT, ST).from_key_value_pair(i)
    End Function

    Public Shared Function emplace_of(Of FT, ST)(ByVal i as KeyValuePair(Of FT, ST)) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_from_key_value_pair(i)
    End Function

    Private Sub New()
    End Sub
End Class

#If IS_CLASS Then
Public NotInheritable Class pair(Of FT, ST)
#Else
Public Class pair(Of FT, ST)
#End If
    Implements IComparable(Of pair(Of FT, ST)), IComparable,
               ICloneable(Of pair(Of FT, ST)), ICloneable

#If IS_CONST Then
    Public ReadOnly first As FT
    Public ReadOnly second As ST
#ElseIf IS_FIRST_CONST Then
    Public ReadOnly first As FT
    Public second As ST
#Else
    Public first As FT
    Public second As ST
#End If

    Shared Sub New()
        bytes_serializer.fixed.register(Function(ByVal i As pair(Of FT, ST), ByVal o As MemoryStream) As Boolean
                                            Return bytes_serializer.append_to(i.first_or_null(), o) AndAlso
                                                   bytes_serializer.append_to(i.second_or_null(), o)
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As pair(Of FT, ST)) As Boolean
                                            Dim f As FT = Nothing
                                            Dim s As ST = Nothing
                                            If bytes_serializer.consume_from(i, f) AndAlso
                                               bytes_serializer.consume_from(i, s) Then
                                                o = New pair(Of FT, ST)(f, s)
                                                Return True
                                            Else
                                                Return False
                                            End If
                                        End Function)
    End Sub

    Private Sub New(ByVal first As FT, ByVal second As ST)
        Me.first = first
        Me.second = second
    End Sub

#If Not IS_CONST AndAlso IS_CLASS Then
    Public Sub New()
    End Sub
#End If

    Public Shared Function make_pair(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return New pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    Public Shared Function make_pair(ByVal first As FT) As pair(Of FT, ST)
        Return make_pair(first, Nothing)
    End Function

    Public Shared Function make_pair(ByVal second As ST) As pair(Of FT, ST)
        Return make_pair(Nothing, second)
    End Function

    Public Shared Function make_pair() As pair(Of FT, ST)
        Return make_pair(Nothing, Nothing)
    End Function

    Public Shared Function emplace_make_pair(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return New pair(Of FT, ST)(first, second)
    End Function

    Public Shared Function emplace_make_pair(ByVal first As FT) As pair(Of FT, ST)
        Return emplace_make_pair(first, Nothing)
    End Function

    Public Shared Function emplace_make_pair(ByVal second As ST) As pair(Of FT, ST)
        Return emplace_make_pair(Nothing, second)
    End Function

    Public Shared Function emplace_make_pair() As pair(Of FT, ST)
        Return emplace_make_pair(Nothing, Nothing)
    End Function

    Public Shared Function from_key_value_pair(ByVal i As KeyValuePair(Of FT, ST)) As pair(Of FT, ST)
        Return make_pair(i.Key(), i.Value())
    End Function

    Public Shared Function emplace_from_key_value_pair(ByVal i As KeyValuePair(Of FT, ST)) As pair(Of FT, ST)
        Return emplace_make_pair(i.Key(), i.Value())
    End Function

#If Not IS_CONST AndAlso Not IS_FIRST_CONST Then
    Public Shared Function move(ByVal that As pair(Of FT, ST)) As pair(Of FT, ST)
#If IS_CLASS Then
        If that Is Nothing Then
            Return Nothing
        End If
#End If
        Dim r As pair(Of FT, ST) = Nothing
        r = New pair(Of FT, ST)()
        r.first = that.first
        r.second = that.second
        that.first = Nothing
        that.second = Nothing
        Return r
    End Function
#End If

    Public Function CompareTo(ByVal other As pair(Of FT, ST)) As Int32 _
                             Implements IComparable(Of pair(Of FT, ST)).CompareTo
#If IS_CLASS Then
        If other Is Nothing Then
            Return 1
        End If
#End If
        Dim c As Int32 = 0
        c = compare(first, other.first)
        If c = 0 Then
            Return compare(second, other.second)
        Else
            Return c
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of pair(Of FT, ST))().from(obj, False))
    End Function

    Public Shared Operator =(ByVal this As pair(Of FT, ST), ByVal that As pair(Of FT, ST)) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator =(ByVal this As pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <>(ByVal this As pair(Of FT, ST), ByVal that As pair(Of FT, ST)) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator <>(ByVal this As pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Function CloneT() As pair(Of FT, ST) Implements ICloneable(Of pair(Of FT, ST)).Clone
        Return New pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(first) + ", " + Convert.ToString(second)
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return (If(first Is Nothing, 0, first.GetHashCode()) Xor If(second Is Nothing, 0, second.GetHashCode()))
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return compare(Me, obj) = 0
    End Function

#If Not IS_CONST Then
    Public Function to_const_pair() As const_pair(Of FT, ST)
        Return const_pair.of(first, second)
    End Function

    Public Function emplace_to_const_pair() As const_pair(Of FT, ST)
        Return const_pair.emplace_of(first, second)
    End Function
#End If
#If Not IS_FIRST_CONST Then
    Public Function to_first_const_pair() As first_const_pair(Of FT, ST)
        Return first_const_pair.of(first, second)
    End Function

    Public Function emplace_to_first_const_pair() As first_const_pair(Of FT, ST)
        Return first_const_pair.emplace_of(first, second)
    End Function
#End If
#If IS_CONST OrElse IS_FIRST_CONST Then
    Public Function to_pair() As pair(Of FT, ST)
        Return pair.of(first, second)
    End Function

    Public Function emplace_to_pair() As pair(Of FT, ST)
        Return pair.emplace_of(first, second)
    End Function
#End If
#If Not IS_CONST AndAlso Not IS_FIRST_CONST AndAlso Not IS_CLASS Then
    Public Shared Widening Operator CType(ByVal this As pair(Of FT, ST)) As pair(Of FT, ST)
        Return pair.emplace_of(this.first, this.second)
    End Operator
#End If
End Class

' TODO: Remove
Public Module _pair
    Public Function make_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair.of(first, second)
    End Function

    Public Function make_pair(Of FT, ST)(ByVal first As FT) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(first)
    End Function

    Public Function make_pair(Of FT, ST)(ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(second)
    End Function

    Public Function make_pair(Of FT, ST)() As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair()
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair.emplace_of(first, second)
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal first As FT) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(first)
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(second)
    End Function

    Public Function emplace_make_pair(Of FT, ST)() As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair()
    End Function

    <Extension()> Public Function first_or_null(Of FT, ST)(ByVal i As pair(Of FT, ST)) As FT
#If IS_CLASS Then
        Return If(i Is Nothing, Nothing, i.first)
#Else
        Return i.first
#End If
    End Function

    <Extension()> Public Function second_or_null(Of FT, ST)(ByVal i As pair(Of FT, ST)) As ST
#If IS_CLASS Then
        Return If(i Is Nothing, Nothing, i.second)
#Else
        Return i.second
#End If
    End Function

    <Extension()> Public Function to_array(Of T)(ByVal i() As pair(Of T, T)) As T(,)
        If isemptyarray(i) Then
            Return Nothing
        Else
            Dim r(,) As T = Nothing
            ReDim r(CInt(array_size(i) - uint32_1), 2 - 1)
            For j As Int32 = 0 To CInt(array_size(i) - uint32_1)
#If IS_CLASS Then
                If i(j) Is Nothing Then
                    Continue For
                End If
#End If
                r(j, 0) = i(j).first
                r(j, 1) = i(j).second
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function to_two_dimensional_array(Of T)(ByVal i() As pair(Of T, T)) As T()()
        If isemptyarray(i) Then
            Return Nothing
        Else
            Dim r()() As T = Nothing
            ReDim r(CInt(array_size(i) - uint32_1))
            For j As Int32 = 0 To CInt(array_size(i) - uint32_1)
                ReDim r(j)(2 - 1)
                r(j)(0) = i(j).first
                r(j)(1) = i(j).second
            Next
            Return r
        End If
    End Function
End Module
'finish pair.vbp --------
