
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with array_pointer.vbp ----------
'so change array_pointer.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with pointer.vbp ----------
'so change pointer.vbp instead of this file



Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _array_pointer
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function renew(Of T)(ByRef i As array_pointer(Of T)) As array_pointer(Of T)
        If i Is Nothing Then
            i = New array_pointer(Of T)()
        Else
            i.clear()
        End If
        Return i
    End Function
End Module

Public NotInheritable Class array_pointers
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal ParamArray i As T()()) As array_pointer(Of T)()
        Dim r() As array_pointer(Of T) = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = array_pointer.[of](i(j))
        Next
        Return r
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class array_pointer
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal i As T()) As array_pointer(Of T)
        Return New array_pointer(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

<SuppressMessage("Microsoft.Design", "BC42333")>
Public Class array_pointer(Of T)
    Implements IComparable, IComparable(Of array_pointer(Of T)), IComparable(Of T()),
               ICloneable, ICloneable(Of array_pointer(Of T))

    Shared Sub New()
        bytes_serializer(Of array_pointer(Of T)).forward_registration.from(Of T())()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function move(ByVal that As array_pointer(Of T)) As array_pointer(Of T)
        If that Is Nothing Then
            Return Nothing
        End If
        Dim r As array_pointer(Of T) = Nothing
        r = New array_pointer(Of T)(that)
        that.clear()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        clear()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal i As T())
        [set](i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal i As array_pointer(Of T))
        If i Is Nothing Then
            clear()
        Else
            [set](+i)
        End If
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with array_pointer.override.vbp ----------
'so change array_pointer.override.vbp instead of this file


    Private Shared Function compare(ByVal i As T(), ByVal j As T()) As Int32
        Return memcmp(i, j)
    End Function

    Private Shared Function hash(ByVal i As T()) As Int32
        'WTF, the Array.GetHashCode returns some random number in .net 3.5
        Dim r As Int32 = 0
        For j As Int32 = 0 To min(32, array_size_i(i)) - 1
            If i(j) Is Nothing Then
                r = r Xor 0
            Else
                r = r Xor i(j).GetHashCode()
            End If
        Next
        Return r
    End Function
    

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with strong_pointer_override.vbp ----------
'so change strong_pointer_override.vbp instead of this file


    Public p As T()

    Public Sub clear()
        p = Nothing
    End Sub

    Public Function empty() As Boolean
        Return p Is Nothing
    End Function

    Public Function [get]() As T()
        Return p
    End Function

    Public Sub [set](ByVal i As T())
        p = i
    End Sub
'finish strong_pointer_override.vbp --------
'finish array_pointer.override.vbp --------

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function release() As T()
        Dim r As T() = Nothing
        r = [get]()
        clear()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of array_pointer(Of T))(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal that As array_pointer(Of T)) As Int32 _
                             Implements IComparable(Of array_pointer(Of T)).CompareTo
        Return CompareTo(+that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal that As T()) As Int32 _
                             Implements IComparable(Of T()).CompareTo
        Return compare([get](), that)
    End Function

#If Not (PocketPC OrElse Smartphone) Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator ^(ByVal p As array_pointer(Of T), ByVal ji As Decimal) As Object
        On Error GoTo finish
        Dim p2 As Object = Nothing
        p2 = p
        Dim jumps As Int64 = 0
        jumps = CLng(Math.Truncate(ji))
        While jumps > 0
            p2 = +(cast(Of array_pointer(Of T))(p2, False))
            jumps -= 1
        End While
finish:
        Return p2
    End Operator
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As array_pointer(Of T) Implements ICloneable(Of array_pointer(Of T)).Clone
        Return New array_pointer(Of T)(Me)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal p As array_pointer(Of T)) As T()
        Return If(p Is Nothing, Nothing, p.get())
    End Operator

    'special treatment for pointer, it compares reference equaling, instead of internal object
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal that As array_pointer(Of T)) As Boolean
        If that Is Nothing OrElse that.get() Is Nothing Then
            Return this Is Nothing OrElse this.get() Is Nothing
        End If
        Return this = that.get()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal that As T) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return that Is Nothing
        End If
        Return object_compare(this.get(), that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As T, ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As T, ByVal that As array_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal obj As Object) As Boolean
        Dim that As array_pointer(Of T) = Nothing
        If cast(Of array_pointer(Of T))(obj, that) Then
            Return this = that
        End If
        Return this = cast(Of T)(obj, False)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal obj As Object) As Boolean
        Return Not this = obj
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As Object, ByVal that As array_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As Object, ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <(ByVal this As array_pointer(Of T), ByVal that As T()) As Boolean
        If this Is Nothing Then
            Return False
        End If
        this.set(that)
        Return True
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator >(ByVal this As array_pointer(Of T), ByVal that As T()) As Boolean
        Return assert(False)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <(ByVal this As T(), ByVal that As array_pointer(Of T)) As Boolean
        Return that > this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator >(ByVal this As T(), ByVal that As array_pointer(Of T)) As Boolean
        Return that < this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As array_pointer(Of T)) As Boolean
        Return Not this Is Nothing AndAlso Not this.empty()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Narrowing Operator CType(ByVal i As T()) As array_pointer(Of T)
        Return New array_pointer(Of T)(i)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Narrowing Operator CType(ByVal i As array_pointer(Of T)) As T()
        Return +i
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator Not(ByVal this As array_pointer(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Operator

    Public NotOverridable Overrides Function Equals(ByVal that As Object) As Boolean
        Return Me = that
    End Function

    'open for array_pointer
    Public NotOverridable Overrides Function GetHashCode() As Int32
        Dim i As T() = Nothing
        i = [get]()
        Return If(i Is Nothing, 0, hash(i))
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return Convert.ToString([get]())
    End Function
End Class
'finish pointer.vbp --------
'finish array_pointer.vbp --------
