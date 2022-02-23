
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports osi.root.constants

Public Module _signing
    Private NotInheritable Class _md5_calculator_holder
        <ThreadStatic> Public Shared c As MD5

        Private Sub New()
        End Sub
    End Class

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function md5calculator() As MD5
        If _md5_calculator_holder.c Is Nothing Then
            _md5_calculator_holder.c = Security.Cryptography.MD5.Create()
        End If
        Return _md5_calculator_holder.c
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function shortmd5signing(ByVal s As String, Optional ByVal e As Encoding = Nothing) As UInt32
        Dim rtn As UInt32 = 0
        Dim b As Byte() = Nothing

        If e Is Nothing Then
            e = default_encoding
        End If
        b = md5calculator().ComputeHash(e.GetBytes(s))

        For i As Int32 = 0 To b.Length() - 1 Step b.Length() >> 2
            rtn <<= 8
            rtn += CUInt(b(i))
        Next

        Return rtn
    End Function

    Private ReadOnly signing8_max_move_bits As Int32 = sizeof(Of UInt32)() * 8
    Private ReadOnly signing8_max_move_bits_1 As Int32 = signing8_max_move_bits - 1
    Private ReadOnly signing8_max_move_bits_m_2 As Int32 = signing8_max_move_bits * 2
    Private ReadOnly signing8_max_move_bits_m_2_1 As Int32 = signing8_max_move_bits_m_2 - 1
    Private ReadOnly signing8_max_move_bits_m_3 As Int32 = signing8_max_move_bits * 3
    Private ReadOnly signing8_max_move_bits_m_3_1 As Int32 = signing8_max_move_bits_m_3 - 1
    Private ReadOnly signing8_max_move_bits_m_4 As Int32 = signing8_max_move_bits * 4
    Private ReadOnly signing8_max_move_bits_m_4_1 As Int32 = signing8_max_move_bits_m_4 - 1
    Public ReadOnly maxSigningIndex As Int32 = signing8_max_move_bits << 2

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing_from_hashcode(ByVal v As Int32, ByVal signing_index As Int32) As UInt32
        Select Case signing_index
            Case 0 To signing8_max_move_bits - 1
                Return signing8(v, signing_index)
            Case signing8_max_move_bits To signing8_max_move_bits_m_2_1
                Return signing9(v, signing_index - signing8_max_move_bits)
            Case signing8_max_move_bits_m_2 To signing8_max_move_bits_m_3_1
                Return signing10(v, signing_index - signing8_max_move_bits_m_2)
            Case signing8_max_move_bits_m_3 To signing8_max_move_bits_m_4_1
                Return signing11(v, signing_index - signing8_max_move_bits_m_3)
            Case Else
                Return signing11(v, signing_index Mod signing8_max_move_bits)
        End Select
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As Object, Optional ByVal signing_index As Int32 = 16) As UInt32
        Dim bi As Int32 = 0
        If v IsNot Nothing Then
            bi = v.GetHashCode()
        End If
        Return signing_from_hashcode(bi, signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(Of t As Structure)(ByVal v As t, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As Byte, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As SByte, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As Int16, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As UInt16, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As Int32, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As UInt32, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As Int64, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function signing(ByVal v As UInt64, Optional ByVal signing_index As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signing_index)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing2(ByVal v As Int32) As UInt32
        Return int32_uint32(Convert.ToString(v).reverse().GetHashCode())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing3(ByVal v As Int32) As UInt32
        Dim v2 As String = Nothing
        v2 = Convert.ToString(v)
        Return int32_uint32((v2 + v2).reverse().GetHashCode())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing4(ByVal v As Int32) As UInt32
        Return shortmd5signing(Convert.ToString(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing5(ByVal v As Int32) As UInt32
        Dim v2 As UInt32 = 0
        v2 = signing8(v)
        Return int32_uint32(Convert.ToString(v2).GetHashCode())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing6(ByVal v As Int32) As UInt32
        Return int32_uint32(Convert.ToBase64String(
                   Encoding.UTF8().GetBytes(Convert.ToString(v.GetHashCode))).GetHashCode)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing7(ByVal v As Int32) As UInt32
        Return int32_uint32(Convert.ToString(v.GetHashCode()).GetHashCode)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing8(ByVal v As Int32, Optional ByVal move_bits As Int32 = 16) As UInt32
        Dim v2 As UInt32 = 0
        v2 = CUInt(CLng(v) - min_int32)
        While move_bits < 0
            move_bits += signing8_max_move_bits
        End While
        While move_bits >= signing8_max_move_bits
            move_bits -= signing8_max_move_bits
        End While
        v2.right_shift(CByte(move_bits))
        Return v2
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing9(ByVal v As Int32, Optional ByVal move_bits As Int32 = 16) As UInt32
        Return Not signing8(v, move_bits)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing10(ByVal v As Int32, Optional ByVal move_bits As Int32 = 16) As UInt32
        Return signing8(v, signing8_max_move_bits - move_bits)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function signing11(ByVal v As Int32, Optional ByVal move_bits As Int32 = 16) As UInt32
        Return signing9(v, signing8_max_move_bits - move_bits)
    End Function
End Module
