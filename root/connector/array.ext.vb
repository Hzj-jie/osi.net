
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _array_ext
    <Extension()> Public Function reverse(Of T)(ByVal i() As T) As T()
        If isemptyarray(i) Then
            Return i
        End If
        Dim r() As T = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            copy(r(j), i(array_size_i(i) - j - 1))
        Next
        Return r
    End Function

    <Extension()> Public Function emplace_reverse(Of T)(ByVal i() As T) As T()
        If isemptyarray(i) Then
            Return i
        End If
        Dim r() As T = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = i(array_size_i(i) - j - 1)
        Next
        Return r
    End Function

    <Extension()> Public Sub in_place_reverse(Of T)(ByVal i() As T)
        If array_size(i) <= 1 Then
            Return
        End If
        For j As Int32 = 0 To (array_size_i(i) >> 1) - 1
            swap(i(j), i(array_size_i(i) - j - 1))
        Next
    End Sub

    <Extension()> Public Sub gc_keepalive(Of T)(ByVal v() As T)
        For i As Int32 = 0 To array_size_i(v) - 1
            GC.KeepAlive(v(i))
        Next
        GC.KeepAlive(v)
    End Sub

    <Extension()> Public Function has(Of T)(ByVal i() As T, ByVal j As T) As Boolean
        Return Not isemptyarray(i) AndAlso Array.IndexOf(i, j) >= 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function index_of(Of T)(ByVal i() As T, ByVal j As T) As Int32
        If isemptyarray(i) Then
            Return npos
        End If
        Return Array.IndexOf(i, j)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function append(Of T)(ByRef this() As T, ByVal ParamArray that() As T) As T()
        this = this.concat(that)
        Return this
    End Function

    <Extension()> Public Function hash(Of T)(ByVal this() As T) As Int32
        'WTF, the Array.GetHashCode returns some random number in .net 3.5
        Return this.hash(this.array_size())
    End Function

    <Extension()> Public Function hash(Of T)(ByVal this() As T, ByVal size As UInt32) As Int32
        assert(size <= this.array_size())
        Dim r As UInt32 = 0
        Dim i As UInt32 = 0
        While i < size
            r = fast_to_uint32(Of T).on(this(CInt(i)))
            i += uint32_1
        End While
        Return uint32_int32(r)
    End Function

    <Extension()> Public Function to_string(Of T)(ByVal this() As T,
                                                  Optional ByVal limited_length As UInt32 = 8,
                                                  Optional ByVal separator As String = ", ",
                                                  Optional ByVal ellipsis As String = " ...") As String
        If isemptyarray(this) OrElse limited_length = 0 Then
            Return Nothing
        End If
        Dim s As New StringBuilder()
        Dim i As Int32 = 0
        For i = 0 To min(array_size_i(this), CInt(limited_length)) - 1
            If i > 0 AndAlso Not separator.null_or_empty() Then
                s.Append(separator)
            End If
            s.Append(Convert.ToString(this(i)))
        Next
        If i < array_size(this) AndAlso Not ellipsis.null_or_empty() Then
            s.Append(ellipsis)
        End If
        Return Convert.ToString(s)
    End Function

    Public Function array_concat(Of T)(ByVal ParamArray i()() As T) As T()
        If isemptyarray(i) Then
            Return Nothing
        End If
        If array_size(i) = uint32_1 Then
            Return i(0)
        End If
        Dim l As UInt32 = 0
        For j As Int32 = 0 To array_size_i(i) - 1
            l += array_size(i(j))
        Next
        Dim r() As T = Nothing
        If l = 0 Then
            ReDim r(-1)
        Else
            ReDim r(CInt(l - uint32_1))
            l = 0
            For j As Int32 = 0 To array_size_i(i) - 1
                arrays.copy(r, l, i(j), uint32_0, array_size(i(j)))
                l += array_size(i(j))
            Next
        End If
        Return r
    End Function

    <Extension()> Public Function concat(Of T)(ByVal i() As T, ByVal ParamArray j()() As T) As T()
        Dim r()() As T = Nothing
        ReDim r(array_size_i(j))
        r(0) = i
        arrays.copy(r, uint32_1, j)
        Return array_concat(r)
    End Function

    Public Function uint32_array_byte_array(ByVal i() As UInt32) As Byte()
        Dim o() As Byte = Nothing
        If isemptyarray(i) Then
            ReDim o(-1)
        Else
            ReDim o(CInt(array_size(i) * sizeof_uint32 - uint32_1))
            For j As UInt32 = 0 To array_size(i) - uint32_1
                assert(uint32_bytes(i(CInt(j)), o, j * sizeof_uint32))
            Next
        End If
        Return o
    End Function

    Public Function byte_array_uint32_array(ByVal i() As Byte, ByRef o() As UInt32) As Boolean
        If isemptyarray(i) Then
            ReDim o(-1)
            Return True
        End If
        If array_size(i) Mod sizeof_uint32 = 0 Then
            ReDim o(array_size_i(i) \ CInt(sizeof_uint32) - 1)
            For j As UInt32 = 0 To array_size(i) - uint32_1 Step sizeof_uint32
                assert(bytes_uint32(i, j, sizeof_uint32, o(CInt(j \ sizeof_uint32))))
            Next
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function byte_array_uint32_array(ByVal i() As Byte) As UInt32()
        Dim r() As UInt32 = Nothing
        assert(byte_array_uint32_array(i, r))
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function byte_array(ByVal i() As UInt32) As Byte()
        Return uint32_array_byte_array(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function uint32_array(ByVal i() As Byte, ByRef o() As UInt32) As Boolean
        Return byte_array_uint32_array(i, o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function uint32_array(ByVal i() As Byte) As UInt32()
        Return byte_array_uint32_array(i)
    End Function

    <Extension()> Public Function deep_clone(Of T)(ByVal i() As T) As T()
        If i Is Nothing Then
            Return Nothing
        End If
        Dim r() As T = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = copy_no_error(i(j))
        Next
        Return r
    End Function

    <Extension()> Public Function shallow_clone(Of T)(ByVal i() As T) As T()
        If i Is Nothing Then
            Return Nothing
        End If

        Return direct_cast(Of T())(i.Clone())
    End Function

    <Extension()> Public Function deep_compare(Of T, T2)(ByVal i() As T,
                                                         ByVal j() As T2,
                                                         ByVal count As UInt32) As Int32
        assert(count <= max_int32)
        assert(array_size(i) >= count)
        assert(array_size(j) >= count)

        For k As Int32 = 0 To CInt(count) - 1
            Dim c As Int32 = 0
            c = compare(i(k), j(k))
            If c <> 0 Then
                Return c
            End If
        Next
        Return 0
    End Function

    <Extension()> Public Function deep_compare(Of T, T2)(ByVal i() As T, ByVal j() As T2) As Int32
        If array_size(i) < array_size(j) Then
            Return -1
        End If
        If array_size(i) > array_size(j) Then
            Return 1
        End If
        Return deep_compare(i, j, array_size(i))
    End Function

    <Extension()> Public Function resize(Of T)(ByRef i() As T,
                                               ByVal size As UInt64,
                                               ByVal preserve As Boolean) As Boolean
        If size > max_int32 Then
            Return False
        End If
        If size = uint32_0 Then
            ReDim i(-1)
        ElseIf preserve Then
            ReDim Preserve i(CInt(size) - 1)
        Else
            ReDim i(CInt(size) - 1)
        End If
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function resize(Of T)(ByRef i() As T, ByVal size As UInt64) As Boolean
        Return resize(i, size, False)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function preserve(Of T)(ByRef i() As T, ByVal size As UInt64) As Boolean
        Return resize(i, size, True)
    End Function

    ' Unlike determined types, T may not have compariable implementations and cause compare to fail. But equal() always
    ' returns a result.
    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mem_equal(Of T)(ByVal first() As T,
                                    ByVal first_start As UInt32,
                                    ByVal second() As T,
                                    ByVal second_start As UInt32,
                                    ByVal len As UInt32) As Boolean
        assert(array_size(first) >= first_start + len)
        assert(array_size(second) >= second_start + len)
        Dim fs As Int32 = 0
        fs = CInt(first_start)
        Dim ss As Int32 = 0
        ss = CInt(second_start)
        For i As Int32 = 0 To CInt(len) - 1
            If Not equal(first(i + fs), second(i + ss)) Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mem_equal(Of T)(ByVal first() As T,
                                    ByVal second() As T,
                                    ByVal len As UInt32) As Boolean
        Return mem_equal(first, uint32_0, second, uint32_0, len)
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mem_equal(Of T)(ByVal first() As T, ByVal second() As T) As Boolean
        Dim ll As UInt32 = 0
        Dim rl As UInt32 = 0
        ll = array_size(first)
        rl = array_size(second)
        If ll <> rl Then
            Return False
        End If
        Return mem_equal(first, second, ll)
    End Function
End Module
