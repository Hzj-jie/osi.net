
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _array_ext
    <Extension()> Public Function reverse(Of T)(ByVal i() As T) As T()
        If isemptyarray(i) Then
            Return i
        Else
            Dim r() As T = Nothing
            ReDim r(array_size(i) - uint32_1)
            For j As UInt32 = 0 To array_size(i) - uint32_1
                copy(r(j), i(array_size(i) - j - uint32_1))
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function emplace_reverse(Of T)(ByVal i() As T) As T()
        If isemptyarray(i) Then
            Return i
        Else
            Dim r() As T = Nothing
            ReDim r(array_size(i) - uint32_1)
            For j As UInt32 = 0 To array_size(i) - uint32_1
                r(j) = i(array_size(i) - j - uint32_1)
            Next
            Return r
        End If
    End Function

    <Extension()> Public Sub in_place_reverse(Of T)(ByVal i() As T)
        If array_size(i) > 1 Then
            For j As UInt32 = 0 To array_size(i) \ 2 - uint32_1
                swap(i(j), i(array_size(i) - j - uint32_1))
            Next
        End If
    End Sub

    <Extension()> Public Sub gc_keepalive(Of T)(ByVal v() As T)
        For i As UInt32 = 0 To array_size(v) - 1
            GC.KeepAlive(v(i))
        Next
        GC.KeepAlive(v)
    End Sub

    <Extension()> Public Function has(Of T)(ByVal i() As T, ByVal j As T) As Boolean
        Return Not isemptyarray(i) AndAlso Array.IndexOf(i, j) >= 0
    End Function

    Public Sub memcpy(Of T)(ByVal dest() As T,
                            ByVal deststart As UInt32,
                            ByVal src() As T,
                            ByVal srcstart As UInt32,
                            ByVal count As UInt32)
        memmove(dest, deststart, src, srcstart, count)
    End Sub

    Public Sub memcpy(Of T)(ByVal dest() As T, ByVal src() As T)
        memcpy(dest, uint32_0, src, uint32_0, array_size(src))
    End Sub

    Public Sub memcpy(Of T)(ByVal dest() As T, ByVal deststart As UInt32, ByVal src() As T)
        memcpy(dest, deststart, src, uint32_0, array_size(src))
    End Sub

    Public Sub memcpy(Of T)(ByVal dest() As T, ByVal src() As T, ByVal count As UInt32)
        memcpy(dest, uint32_0, src, uint32_0, count)
    End Sub

    Public Sub memmove(Of T)(ByVal dest() As T,
                             ByVal deststart As UInt32,
                             ByVal src() As T,
                             ByVal srcstart As UInt32,
                             ByVal count As UInt32)
        If count > 0 Then
            Array.Copy(src, srcstart, dest, deststart, count)
        End If
    End Sub

    Public Sub memset(Of T)(ByVal dest() As T,
                            ByVal start As UInt32,
                            ByVal count As UInt32,
                            ByVal src As T)
        If count > 0 Then
            For i As UInt32 = start To start + count - 1
                copy(dest(i), src)
            Next
        End If
    End Sub

    Public Sub memset(Of T)(ByVal dest() As T, ByVal src As T)
        memset(dest, uint32_0, array_size(dest), src)
    End Sub

    Public Sub memclr(Of T)(ByVal dest() As T, ByVal start As UInt32, ByVal count As UInt32)
        If count > uint32_0 Then
            Array.Clear(dest, start, count)
        End If
    End Sub

    Public Sub memclr(Of T)(ByVal dest() As T)
        memclr(dest, uint32_0, array_size(dest))
    End Sub

    <Extension()> Public Function append(Of T)(ByRef this() As T, ByVal ParamArray that() As T) As T()
        this = this.concat(that)
        Return this
    End Function

    <Extension()> Public Function to_string(Of T)(ByVal this() As T,
                                                  Optional ByVal limited_length As UInt32 = 8,
                                                  Optional ByVal separator As String = ", ",
                                                  Optional ByVal ellipsis As String = " ...") As String
        If isemptyarray(this) OrElse limited_length = 0 Then
            Return Nothing
        Else
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            Dim i As Int32 = 0
            For i = 0 To min(array_size(this), limited_length) - 1
                If i > 0 AndAlso Not String.IsNullOrEmpty(separator) Then
                    s.Append(separator)
                End If
                s.Append(Convert.ToString(this(i)))
            Next
            If i < array_size(this) AndAlso Not String.IsNullOrEmpty(ellipsis) Then
                s.Append(ellipsis)
            End If
            Return Convert.ToString(s)
        End If
    End Function

    Public Function array_concat(Of T)(ByVal ParamArray i()() As T) As T()
        If isemptyarray(i) Then
            Return Nothing
        ElseIf array_size(i) = uint32_1 Then
            Return i(0)
        Else
            Dim l As UInt32 = 0
            For j As UInt32 = 0 To array_size(i) - uint32_1
                l += array_size(i(j))
            Next
            Dim r() As T = Nothing
            If l = Nothing Then
                ReDim r(-1)
            Else
                ReDim r(l - uint32_1)
                l = 0
                For j As UInt32 = 0 To array_size(i) - uint32_1
                    memcpy(r, l, i(j), uint32_0, array_size(i(j)))
                    l += array_size(i(j))
                Next
            End If
            Return r
        End If
    End Function

    <Extension()> Public Function concat(Of T)(ByVal i() As T, ByVal ParamArray j()() As T) As T()
        Dim r()() As T = Nothing
        ReDim r(array_size(j))
        r(0) = i
        memcpy(r, uint32_1, j)
        Return array_concat(r)
    End Function

    Public Function uint32_array_byte_array(ByVal i() As UInt32) As Byte()
        Dim o() As Byte = Nothing
        If isemptyarray(i) Then
            ReDim o(-1)
        Else
            ReDim o(array_size(i) * sizeof_uint32 - uint32_1)
            For j As Int32 = 0 To array_size_i(i) - 1
                assert(uint32_bytes(i(j), o, j * sizeof_uint32))
            Next
        End If
        Return o
    End Function

    Public Function byte_array_uint32_array(ByVal i() As Byte, ByRef o() As UInt32) As Boolean
        If isemptyarray(i) Then
            ReDim o(-1)
            Return True
        ElseIf array_size(i) Mod sizeof_uint32 = 0 Then
            ReDim o(array_size(i) \ sizeof_uint32 - uint32_1)
            For j As Int32 = 0 To array_size_i(i) - 1 Step sizeof_uint32
                assert(bytes_uint32(i, j, sizeof_uint32, o(j \ sizeof_uint32)))
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Public Function byte_array_uint32_array(ByVal i() As Byte) As UInt32()
        Dim r() As UInt32 = Nothing
        assert(byte_array_uint32_array(i, r))
        Return r
    End Function

    <Extension()> Public Function byte_array(ByVal i() As UInt32) As Byte()
        Return uint32_array_byte_array(i)
    End Function

    <Extension()> Public Function uint32_array(ByVal i() As Byte, ByRef o() As UInt32) As Boolean
        Return byte_array_uint32_array(i, o)
    End Function

    <Extension()> Public Function uint32_array(ByVal i() As Byte) As UInt32()
        Return byte_array_uint32_array(i)
    End Function
End Module
