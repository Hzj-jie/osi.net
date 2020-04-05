'this file is generated by /osi/root/codegen/adaptive_array/adaptive_array.exe
'so change /osi/root/codegen/adaptive_array/adaptive_array.cs instead of this file
'usually you do not need to use this codegen and the code generated unless it's in a very strict performance related code
'use vector is a better way, while the implementation of vector is also using the code generated by this codegen
'p.s. this file needs to work with osi.root.connector project

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Imports osi.root
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class big_uint
Private Class adaptive_array_uint32
    Implements ICloneable, ICloneable(Of adaptive_array_uint32), IComparable(Of adaptive_array_uint32), IComparable

    Private Shared Function expected_capacity(ByVal n As UInt32) As UInt32
        assert(n <= max_array_size)
        If n = max_array_size Then
            root.connector.throws.out_of_memory("adaptive_array size ", n, " exceeds limitation.")
        End If
        If n <= 2 Then
            Return 4
        End If
        n <<= 1
        Return If(n > max_array_size, max_array_size, n)
    End Function

    Private d() As UInt32
    Private s As UInt32

    Public Sub New()
    End Sub

    Public Sub New(ByVal n As UInt32)
        reserve(n)
    End Sub

    Public Shared Function move(ByVal that As adaptive_array_uint32) As adaptive_array_uint32
        If that Is Nothing Then
            Return Nothing
        End If

        Dim r As adaptive_array_uint32 = Nothing
        r = New adaptive_array_uint32()
        r.d = that.d
        r.s = that.s
        that.d = Nothing
        that.s = 0
        Return r
    End Function

    Public Shared Function swap(ByVal this As adaptive_array_uint32, ByVal that As adaptive_array_uint32) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        connector.swap(this.d, that.d)
        connector.swap(this.s, that.s)
        Return True
    End Function

    Public Function replace_by(ByVal d() As UInt32, ByVal s As UInt32) As Boolean
        If array_size(d) >= s Then
            Me.d = d
            Me.s = s
            Return True
        End If
        Return False
    End Function

    Public Sub replace_by(ByVal d() As UInt32)
        assert(replace_by(d, array_size(d)))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function max_size() As UInt32
        Return max_array_size
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function data() As UInt32()
        Return d
    End Function

' Property access is expensive.
#If 0 Then
    Default Public Property at(ByVal p As UInt32) As UInt32
        Get
            Return [get](p)
        End Get
        Set(ByVal value As UInt32)
            [set](p, value)
        End Set
    End Property
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get](ByVal p As UInt32) As UInt32
        Return d(CInt(p))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [set](ByVal p As UInt32, ByVal v As UInt32)
        d(CInt(p)) = v
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return s
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function capacity() As UInt32
        Return array_size(d)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function back() As UInt32
        Return d(CInt(size() - uint32_1))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        s = uint32_0
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push_back(ByVal v As UInt32)
        reserve(size() + uint32_1)
        d(CInt(size())) = v
        s += uint32_1
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub pop_back()
        s -= uint32_1
    End Sub

    Public Sub reserve(ByVal n As UInt32)
        If capacity() >= n Then
            Return
        End If
        Dim ec As UInt32 = 0
        ec = expected_capacity(n)
        assert(ec >= uint32_1)
        If empty() Then
            ReDim d(CInt(ec - uint32_1))
        Else
            ReDim Preserve d(CInt(ec - uint32_1))
        End If
    End Sub

    Public Sub resize(ByVal n As UInt32)
        reserve(n)
        If size() < n Then
            memclr(d, size(), n - size())
        End If
        s = n
    End Sub

    Public Sub resize(ByVal n As UInt32, ByVal v As UInt32)
        reserve(n)
        If size() < n Then
            memset(d, size(), n - size(), v)
        End If
        s = n
    End Sub

    Public Sub shrink_to_fit()
        If empty() Then
            ReDim d(-1)
        ElseIf capacity() > size() Then
            assert(size() >= uint32_1)
            ReDim Preserve d(CInt(size() - uint32_1))
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As adaptive_array_uint32 Implements ICloneable(Of adaptive_array_uint32).Clone
        Dim r As adaptive_array_uint32 = Nothing
        r = New adaptive_array_uint32()
        r.d = deep_clone(d)
        r.s = s
        Return r
    End Function

    Public Shared Function compare(ByVal this As adaptive_array_uint32, ByVal that As adaptive_array_uint32) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        If this.size() < that.size() Then
            Return -1
        ElseIf this.size() > that.size() Then
            Return 1
        Else
            Return deep_compare(this.d, that.d, this.size())
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of adaptive_array_uint32)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As adaptive_array_uint32) As Int32 Implements IComparable(Of adaptive_array_uint32).CompareTo
        Return compare(Me, other)
    End Function

End Class
End Class
