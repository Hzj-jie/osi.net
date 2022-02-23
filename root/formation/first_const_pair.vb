
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

Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class first_const_pair
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of FT, ST)(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).of(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(Of FT, ST)(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_of(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of FT, ST)(ByVal i as KeyValuePair(Of FT, ST)) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).of(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(Of FT, ST)(ByVal i as KeyValuePair(Of FT, ST)) As first_const_pair(Of FT, ST)
        Return first_const_pair(Of FT, ST).emplace_of(i)
    End Function

    Private Sub New()
    End Sub
End Class

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
                                            End If
                                            Return False
                                        End Function)
        json_serializer.register(Function(ByVal i As first_const_pair(Of FT, ST), ByVal o As StringWriter) As Boolean
                                     If Not json_serializer.to_str(i.first_or_null(), o) Then
                                         Return False
                                     End If
                                     o.Write(":")
                                     If Not json_serializer.to_str(i.second_or_null(), o) Then
                                         Return False
                                     End If
                                     Return True
                                 End Function)
        string_serializer.register(Function(ByVal i As first_const_pair(Of FT, ST), ByVal o As StringWriter) As Boolean
                                       If Not string_serializer.to_str(i.first_or_null(), o) Then
                                           Return False
                                       End If
                                       o.Write(":")
                                       If Not string_serializer.to_str(i.second_or_null(), o) Then
                                           Return False
                                       End If
                                       Return True
                                   End Function,
                                   Function(ByVal sr As StringReader, ByRef o As first_const_pair(Of FT, ST)) As Boolean
                                       assert(sr IsNot Nothing)
                                       Dim i As String = sr.ReadToEnd()
                                       Dim pos As Int32 = i.strindexof(":")
                                       If pos = npos Then
                                           Return False
                                       End If
                                       Dim f As FT = Nothing
                                       Dim s As ST = Nothing
                                       If Not string_serializer.from_str(i.strleft(CUInt(pos)), f) OrElse
                                          Not string_serializer.from_str(i.strmid(CUInt(pos + 1)), s) Then
                                           Return False
                                       End If
                                       o = New first_const_pair(Of FT, ST)(f, s)
                                       Return True
                                   End Function)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal first As FT, ByVal second As ST)
        Me.first = first
        Me.second = second
    End Sub

#If Not IS_CONST Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
    End Sub
#End If

    Public Shared ReadOnly first_getter As Func(Of first_const_pair(Of FT, ST), FT) =
        Function(ByVal p As first_const_pair(Of FT, ST)) As FT
            assert(p IsNot Nothing)
            Return p.first
        End Function

    Public Shared ReadOnly first_or_null_getter As Func(Of first_const_pair(Of FT, ST), FT) =
        Function(ByVal p As first_const_pair(Of FT, ST)) As FT
            If p Is Nothing Then
                Return Nothing
            End If
            Return p.first
        End Function

    Public Shared ReadOnly second_getter As Func(Of first_const_pair(Of FT, ST), ST) =
        Function(ByVal p As first_const_pair(Of FT, ST)) As ST
            assert(p IsNot Nothing)
            Return p.second
        End Function

    Public Shared ReadOnly second_or_null_getter As Func(Of first_const_pair(Of FT, ST), ST) =
        Function(ByVal p As first_const_pair(Of FT, ST)) As ST
            If p Is Nothing Then
                Return Nothing
            End If
            Return p.second
        End Function

    Public Shared ReadOnly first_comparer As Func(Of first_const_pair(Of FT, ST), first_const_pair(Of FT, ST), Int32) =
        Function(ByVal l As first_const_pair(Of FT, ST),
                 ByVal r As first_const_pair(Of FT, ST)) As Int32
            assert(l IsNot Nothing)
            assert(r IsNot Nothing)
            Return compare(l.first, r.first)
        End Function

    Public Shared ReadOnly second_comparer As Func(Of first_const_pair(Of FT, ST), first_const_pair(Of FT, ST), Int32) =
        Function(ByVal l As first_const_pair(Of FT, ST),
                 ByVal r As first_const_pair(Of FT, ST)) As Int32
            assert(l IsNot Nothing)
            assert(r IsNot Nothing)
            Return compare(l.second, r.second)
        End Function

    Public Shared ReadOnly second_first_comparer As Func(Of first_const_pair(Of FT, ST), first_const_pair(Of FT, ST), Int32) =
        Function(ByVal l As first_const_pair(Of FT, ST),
                 ByVal r As first_const_pair(Of FT, ST)) As Int32
            assert(l IsNot Nothing)
            assert(r IsNot Nothing)
            Dim cmp As Int32 = 0
            cmp = compare(l.second, r.second)
            If cmp <> 0 Then
                Return cmp
            End If
            Return compare(l.first, r.first)
        End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return New first_const_pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](ByVal first As FT) As first_const_pair(Of FT, ST)
        Return [of](first, Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](ByVal second As ST) As first_const_pair(Of FT, ST)
        Return [of](Nothing, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of]() As first_const_pair(Of FT, ST)
        Return [of](Nothing, Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(ByVal first As FT, ByVal second As ST) As first_const_pair(Of FT, ST)
        Return New first_const_pair(Of FT, ST)(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(ByVal first As FT) As first_const_pair(Of FT, ST)
        Return emplace_of(first, Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(ByVal second As ST) As first_const_pair(Of FT, ST)
        Return emplace_of(Nothing, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of() As first_const_pair(Of FT, ST)
        Return emplace_of(Nothing, Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](ByVal i As KeyValuePair(Of FT, ST)) As first_const_pair(Of FT, ST)
        Return [of](i.Key(), i.Value())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(ByVal i As KeyValuePair(Of FT, ST)) As first_const_pair(Of FT, ST)
        Return emplace_of(i.Key(), i.Value())
    End Function

#If Not IS_CONST AndAlso Not IS_FIRST_CONST Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function move(ByVal that As first_const_pair(Of FT, ST)) As first_const_pair(Of FT, ST)
        If that Is Nothing Then
            Return Nothing
        End If
        Dim r As first_const_pair(Of FT, ST) = Nothing
        r = New first_const_pair(Of FT, ST)()
        r.first = that.first
        r.second = that.second
        that.first = Nothing
        that.second = Nothing
        Return r
    End Function
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal other As first_const_pair(Of FT, ST)) As Int32 _
                             Implements IComparable(Of first_const_pair(Of FT, ST)).CompareTo
        If other Is Nothing Then
            Return 1
        End If
        Dim c As Int32 = 0
        c = compare(first, other.first)
        If c = 0 Then
            Return compare(second, other.second)
        End If
        Return c
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of first_const_pair(Of FT, ST))().from(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As first_const_pair(Of FT, ST), ByVal that As first_const_pair(Of FT, ST)) As Boolean
        Return compare(this, that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As first_const_pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As first_const_pair(Of FT, ST), ByVal that As first_const_pair(Of FT, ST)) As Boolean
        Return compare(this, that) <> 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As first_const_pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) <> 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As first_const_pair(Of FT, ST) Implements ICloneable(Of first_const_pair(Of FT, ST)).Clone
        Return New first_const_pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Overrides Function ToString() As String
        Return "{"c + Convert.ToString(first) + ", " + Convert.ToString(second) + "}"c
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return (If(first Is Nothing, 0, first.GetHashCode()) Xor If(second Is Nothing, 0, second.GetHashCode()))
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return compare(Me, obj) = 0
    End Function

#If Not IS_CONST Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function to_const_pair() As const_pair(Of FT, ST)
        Return const_pair.of(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace_to_const_pair() As const_pair(Of FT, ST)
        Return const_pair.emplace_of(first, second)
    End Function
#End If
#If Not IS_FIRST_CONST Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function to_first_const_pair() As first_const_pair(Of FT, ST)
        Return first_const_pair.of(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace_to_first_const_pair() As first_const_pair(Of FT, ST)
        Return first_const_pair.emplace_of(first, second)
    End Function
#End If
#If IS_CONST OrElse IS_FIRST_CONST Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function to_pair() As pair(Of FT, ST)
        Return pair.of(first, second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace_to_pair() As pair(Of FT, ST)
        Return pair.emplace_of(first, second)
    End Function
#End If
End Class

Public Module _first_const_pair
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function first_or_null(Of FT, ST)(ByVal i As first_const_pair(Of FT, ST)) As FT
        Return If(i Is Nothing, Nothing, i.first)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function second_or_null(Of FT, ST)(ByVal i As first_const_pair(Of FT, ST)) As ST
        Return If(i Is Nothing, Nothing, i.second)
    End Function

    <Extension()> Public Function to_array(Of T)(ByVal i() As first_const_pair(Of T, T)) As T(,)
        If isemptyarray(i) Then
            Return Nothing
        End If
        Dim r(,) As T = Nothing
        ReDim r(CInt(array_size(i) - uint32_1), 2 - 1)
        For j As Int32 = 0 To CInt(array_size(i) - uint32_1)
            If i(j) Is Nothing Then
                Continue For
            End If
            r(j, 0) = i(j).first
            r(j, 1) = i(j).second
        Next
        Return r
    End Function

    <Extension()> Public Function to_two_dimensional_array(Of T)(ByVal i() As first_const_pair(Of T, T)) As T()()
        If isemptyarray(i) Then
            Return Nothing
        End If
        Dim r()() As T = Nothing
        ReDim r(CInt(array_size(i) - uint32_1))
        For j As Int32 = 0 To CInt(array_size(i) - uint32_1)
            ReDim r(j)(2 - 1)
            r(j)(0) = i(j).first
            r(j)(1) = i(j).second
        Next
        Return r
    End Function
End Module
'finish pair.vbp --------
'finish first_const_pair.vbp --------
