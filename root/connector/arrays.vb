
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class arrays
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function type_erasure(Of T, IT As T)(ByVal i() As IT) As T()
        If i Is Nothing Then
            Return Nothing
        End If
        Dim o() As T = Nothing
        ReDim o(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            o(j) = i(j)
        Next
        Return o
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub copy(Of T)(ByVal dest() As T,
                                 ByVal deststart As UInt32,
                                 ByVal src() As T,
                                 ByVal srcstart As UInt32,
                                 ByVal count As UInt32)
        If count = 0 Then
            Return
        End If
        Array.Copy(src, srcstart, dest, deststart, count)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub copy(Of T)(ByVal dest() As T, ByVal src() As T)
        copy(dest, uint32_0, src, uint32_0, array_size(src))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub copy(Of T)(ByVal dest() As T, ByVal deststart As UInt32, ByVal src() As T)
        copy(dest, deststart, src, uint32_0, array_size(src))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub copy(Of T)(ByVal dest() As T, ByVal src() As T, ByVal count As UInt32)
        copy(dest, uint32_0, src, uint32_0, count)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub fill(Of T)(ByVal dest() As T,
                                 ByVal start As UInt32,
                                 ByVal count As UInt32,
                                 ByVal src As T)
        If count = 0 Then
            Return
        End If
        assert(start + count <= max_int32)
        For i As UInt32 = start To start + count - uint32_1
            connector.copy(dest(CInt(i)), src)
        Next
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub fill(Of T)(ByVal dest() As T, ByVal src As T)
        fill(dest, uint32_0, array_size(dest), src)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub clear(Of T)(ByVal dest() As T, ByVal start As UInt32, ByVal count As UInt32)
        If count = uint32_0 Then
            Return
        End If
        Array.Clear(dest, CInt(start), CInt(count))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub clear(Of T)(ByVal dest() As T)
        clear(dest, uint32_0, array_size(dest))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub memcpy(Of T1, T2)(ByVal dest() As T1, ByVal src() As T2)
        memcpy(src, dest, array_size(src) * uint32_sizeof(Of T2)())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub memcpy(Of T1, T2)(ByVal dest() As T1, ByVal src() As T2, ByVal byte_count As UInt32)
        Buffer.BlockCopy(src, 0, dest, 0, CInt(byte_count))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](ByVal ParamArray v() As Object) As Object()
        Return v
    End Function

    Private Sub New()
    End Sub
End Class
