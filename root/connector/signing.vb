
Imports System.Security.Cryptography
Imports System.Text
Imports osi.root.constants

Public Module _signing
    Private NotInheritable Class _md5_calculator_holder
        <ThreadStatic> Public Shared c As MD5

        Private Sub New()
        End Sub
    End Class

    Public Function md5calculator() As MD5
        If _md5_calculator_holder.c Is Nothing Then
            _md5_calculator_holder.c = Security.Cryptography.MD5.Create()
        End If
        Return _md5_calculator_holder.c
    End Function

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

    Private ReadOnly signing8MaxMoveBits As Int32 = sizeof(Of UInt32)() * 8
    Private ReadOnly signing8MaxMoveBits_1 As Int32 = signing8MaxMoveBits - 1
    Private ReadOnly signing8MaxMoveBits_m_2 As Int32 = signing8MaxMoveBits * 2
    Private ReadOnly signing8MaxMoveBits_m_2_1 As Int32 = signing8MaxMoveBits_m_2 - 1
    Private ReadOnly signing8MaxMoveBits_m_3 As Int32 = signing8MaxMoveBits * 3
    Private ReadOnly signing8MaxMoveBits_m_3_1 As Int32 = signing8MaxMoveBits_m_3 - 1
    Private ReadOnly signing8MaxMoveBits_m_4 As Int32 = signing8MaxMoveBits * 4
    Private ReadOnly signing8MaxMoveBits_m_4_1 As Int32 = signing8MaxMoveBits_m_4 - 1
    Public ReadOnly maxSigningIndex As Int32 = signing8MaxMoveBits << 2

    Private Function signing_from_hashcode(ByVal v As Int32, ByVal signingIndex As Int32) As UInt32
        Select Case signingIndex
            Case 0 To signing8MaxMoveBits - 1
                Return signing8(v, signingIndex)
            Case signing8MaxMoveBits To signing8MaxMoveBits_m_2_1
                Return signing9(v, signingIndex - signing8MaxMoveBits)
            Case signing8MaxMoveBits_m_2 To signing8MaxMoveBits_m_3_1
                Return signing10(v, signingIndex - signing8MaxMoveBits_m_2)
            Case signing8MaxMoveBits_m_3 To signing8MaxMoveBits_m_4_1
                Return signing11(v, signingIndex - signing8MaxMoveBits_m_3)
            Case Else
                Return signing11(v, signingIndex Mod signing8MaxMoveBits)
        End Select
    End Function

    Public Function signing(ByVal v As Object, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Dim bi As Int32 = 0
        If Not v Is Nothing Then
            bi = v.GetHashCode()
        End If
        Return signing_from_hashcode(bi, signingIndex)
    End Function

    Public Function signing(Of t As Structure)(ByVal v As t, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As Byte, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As SByte, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As Int16, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As UInt16, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As Int32, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As UInt32, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As Int64, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Public Function signing(ByVal v As UInt64, Optional ByVal signingIndex As Int32 = 16) As UInt32
        Return signing_from_hashcode(v.GetHashCode(), signingIndex)
    End Function

    Private Function signing2(ByVal v As Int32) As UInt32
        Return Convert.ToString(v).reverse().GetHashCode()
    End Function

    Private Function signing3(ByVal v As Int32) As UInt32
        Dim v2 As String = Nothing
        v2 = Convert.ToString(v)
        Return (v2 + v2).reverse().GetHashCode()
    End Function

    Private Function signing4(ByVal v As Int32) As UInt32
        Return shortmd5signing(Convert.ToString(v))
    End Function

    Private Function signing5(ByVal v As Int32) As UInt32
        Dim v2 As UInt32 = 0
        v2 = signing8(v)
        Return Convert.ToString(v2).GetHashCode()
    End Function

    Private Function signing6(ByVal v As Int32) As UInt32
        Return Convert.ToBase64String(Encoding.UTF8().GetBytes(Convert.ToString(v.GetHashCode))).GetHashCode
    End Function

    Private Function signing7(ByVal v As Int32) As UInt32
        Return Convert.ToString(v.GetHashCode()).GetHashCode
    End Function

    Private Function signing8(ByVal v As Int32, Optional ByVal movebits As Int32 = 16) As UInt32
        Dim v2 As UInt32 = 0
        v2 = CLng(v.GetHashCode()) - min_int32
        While movebits < 0
            movebits += signing8MaxMoveBits
        End While
        While movebits >= signing8MaxMoveBits
            movebits -= signing8MaxMoveBits
        End While
        right_shift(v2, movebits)
        Return v2
    End Function

    Private Function signing9(ByVal v As Int32, Optional ByVal movebits As Int32 = 16) As UInt32
        Return Not signing8(v, movebits)
    End Function

    Private Function signing10(ByVal v As Int32, Optional ByVal movebits As Int32 = 16) As UInt32
        Return signing8(v, signing8MaxMoveBits - movebits)
    End Function

    Private Function signing11(ByVal v As Int32, Optional ByVal movebits As Int32 = 16) As UInt32
        Return signing9(v, signing8MaxMoveBits - movebits)
    End Function
End Module
