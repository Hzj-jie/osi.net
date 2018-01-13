
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with first_const_pair.vbp ----------
'so change first_const_pair.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with pair.vbp ----------
'so change pair.vbp instead of this file


#Const IS_CONST = ("first_const_" = "const_")
#Const IS_FIRST_CONST = ("first_const_" = "first_const_")

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

Public NotInheritable Class first_const_pair(Of FT, ST)
    Implements IComparable(Of first_const_pair(Of FT, ST)), IComparable,
               ICloneable(Of first_const_pair(Of FT, ST)), ICloneable

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
        bytes_serializer.fixed.register(Function(ByVal i As first_const_pair(Of FT, ST), ByVal o As MemoryStream) As Boolean
                                            Return bytes_serializer.append_to(i.first_or_null(), o) AndAlso
                                                   bytes_serializer.append_to(i.second_or_null(), o)
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As first_const_pair(Of FT, ST)) As Boolean
                                            Dim f As FT = Nothing
                                            Dim s As ST = Nothing
                                            If bytes_serializer.consume_from(i, f) AndAlso
                                               bytes_serializer.consume_from(i, s) Then
                                                o = New first_const_pair(Of FT, ST)(f, s)
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

#If Not IS_CONST Then
    Public Sub New()
    End Sub
#End If

    Public Shared Function make_first_const_pair(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return New first_const_pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    Public Shared Function make_first_const_pair(ByVal first As FT) As first_const_pair(Of FT, ST)
        Return make_first_const_pair(first, Nothing)
    End Function

    Public Shared Function make_first_const_pair(ByVal second As ST) As first_const_pair(Of FT, ST)
        Return make_first_const_pair(Nothing, second)
    End Function

    Public Shared Function make_first_const_pair() As first_const_pair(Of FT, ST)
        Return make_first_const_pair(Nothing, Nothing)
    End Function

    Public Shared Function emplace_make_first_const_pair(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return New first_const_pair(Of FT, ST)(first, second)
    End Function

    Public Shared Function emplace_make_first_const_pair(ByVal first As FT) As first_const_pair(Of FT, ST)
        Return emplace_make_first_const_pair(first, Nothing)
    End Function

    Public Shared Function emplace_make_first_const_pair(ByVal second As ST) As first_const_pair(Of FT, ST)
        Return emplace_make_first_const_pair(Nothing, second)
    End Function

    Public Shared Function emplace_make_first_const_pair() As first_const_pair(Of FT, ST)
        Return emplace_make_first_const_pair(Nothing, Nothing)
    End Function

#If Not IS_CONST AndAlso Not IS_FIRST_CONST Then
    Public Shared Function move(ByVal that As first_const_pair(Of FT, ST)) As first_const_pair(Of FT, ST)
        If that Is Nothing Then
            Return Nothing
        Else
            Dim r As first_const_pair(Of FT, ST) = Nothing
            r = New first_const_pair(Of FT, ST)()
            r.first = that.first
            r.second = that.second
            that.first = Nothing
            that.second = Nothing
            Return r
        End If
    End Function
#End If

    Public Function CompareTo(ByVal other As first_const_pair(Of FT, ST)) As Int32 _
                             Implements IComparable(Of first_const_pair(Of FT, ST)).CompareTo
        Dim c As Int32 = 0
        c = object_compare(Me, other)
        If c = object_compare_undetermined Then
            assert(Not other Is Nothing)
            c = compare(first, other.first)
            If c = 0 Then
                Return compare(second, other.second)
            Else
                Return c
            End If
        Else
            Return c
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of first_const_pair(Of FT, ST))(obj, False))
    End Function

    Public Shared Operator =(ByVal this As first_const_pair(Of FT, ST), ByVal that As first_const_pair(Of FT, ST)) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator =(ByVal this As first_const_pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <>(ByVal this As first_const_pair(Of FT, ST), ByVal that As first_const_pair(Of FT, ST)) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator <>(ByVal this As first_const_pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Function CloneT() As first_const_pair(Of FT, ST) Implements ICloneable(Of first_const_pair(Of FT, ST)).Clone
        Return New first_const_pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
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
        Return make_const_pair(first, second)
    End Function

    Public Function emplace_to_const_pair() As const_pair(Of FT, ST)
        Return emplace_make_const_pair(first, second)
    End Function
#End If
#If Not IS_FIRST_CONST Then
    Public Function to_first_const_pair() As first_const_pair(Of FT, ST)
        Return make_first_const_pair(first, second)
    End Function

    Public Function emplace_to_first_const_pair() As first_const_pair(Of FT, ST)
        Return emplace_make_first_const_pair(first, second)
    End Function
#End If
#If IS_CONST OrElse IS_FIRST_CONST Then
    Public Function to_pair() As pair(Of FT, ST)
        Return make_pair(first, second)
    End Function

    Public Function emplace_to_pair() As pair(Of FT, ST)
        Return emplace_make_pair(first, second)
    End Function
#End If
End Class

Public Module _first_const_pair
    Public Function make_first_const_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).make_first_const_pair(first, second)
    End Function

    Public Function make_first_const_pair(Of FT, ST)(ByVal first As FT) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).make_first_const_pair(first)
    End Function

    Public Function make_first_const_pair(Of FT, ST)(ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).make_first_const_pair(second)
    End Function

    Public Function make_first_const_pair(Of FT, ST)() As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).make_first_const_pair()
    End Function

    Public Function emplace_make_first_const_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_make_first_const_pair(first, second)
    End Function

    Public Function emplace_make_first_const_pair(Of FT, ST)(ByVal first As FT) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_make_first_const_pair(first)
    End Function

    Public Function emplace_make_first_const_pair(Of FT, ST)(ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_make_first_const_pair(second)
    End Function

    Public Function emplace_make_first_const_pair(Of FT, ST)() As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_make_first_const_pair()
    End Function

    <Extension()> Public Function first_or_null(Of FT, ST)(ByVal i As first_const_pair(Of FT, ST)) As FT
        Return If(i Is Nothing, Nothing, i.first)
    End Function

    <Extension()> Public Function second_or_null(Of FT, ST)(ByVal i As first_const_pair(Of FT, ST)) As ST
        Return If(i Is Nothing, Nothing, i.second)
    End Function

    <Extension()> Public Function to_array(Of T)(ByVal i() As first_const_pair(Of T, T)) As T(,)
        If isemptyarray(i) Then
            Return Nothing
        Else
            Dim r(,) As T = Nothing
            ReDim r(CInt(array_size(i) - uint32_1), 2 - 1)
            For j As Int32 = 0 To CInt(array_size(i) - uint32_1)
                If Not i(j) Is Nothing Then
                    r(j, 0) = i(j).first
                    r(j, 1) = i(j).second
                End If
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function to_two_dimensional_array(Of T)(ByVal i() As first_const_pair(Of T, T)) As T()()
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
'finish first_const_pair.vbp --------
