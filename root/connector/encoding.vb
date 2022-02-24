
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _encoding
    Public Function try_get_encoding(ByVal name As String, ByRef o As Encoding) As Boolean
        If String.IsNullOrEmpty(name) Then
            Return False
        End If
        Try
            o = Encoding.GetEncoding(name)
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Function try_get_encoding(ByVal codepage As Int32, ByRef o As Encoding) As Boolean
        Try
            o = Encoding.GetEncoding(codepage)
            Return True
        Catch
            Return False
        End Try
    End Function

    <Extension()> Public Function try_get_string(ByVal e As Encoding,
                                                 ByVal buff() As Byte,
                                                 ByVal offset As Int32,
                                                 ByVal count As Int32,
                                                 ByRef o As String) As Boolean
        If e Is Nothing OrElse
           buff Is Nothing OrElse
           offset < 0 OrElse
           count < 0 OrElse
           array_size(buff) < offset + count Then
            Return False
        End If
        Try
            o = e.GetString(buff, offset, count)
            Return True
        Catch
            Return False
        End Try
    End Function

    <Extension()> Public Function try_get_string(ByVal e As Encoding,
                                                 ByVal buff() As Byte,
                                                 ByRef o As String) As Boolean
        If e Is Nothing OrElse
           buff Is Nothing Then
            Return False
        End If
        Try
            o = e.GetString(buff)
            Return True
        Catch
            Return False
        End Try
    End Function

    <Extension()> Public Function GetBytes(ByVal e As Encoding,
                                           ByVal chars() As Char,
                                           ByVal count As Int32) As Byte()
        assert(Not e Is Nothing)
        Return e.GetBytes(chars, 0, count)
    End Function

    <Extension()> Public Function GetBytes(ByVal e As Encoding,
                                           ByVal s As String,
                                           ByVal index As Int32,
                                           ByVal length As Int32) As Byte()
        assert(Not e Is Nothing)
        assert(Not s Is Nothing)
        Return e.GetBytes(s.ToCharArray(index, length))
    End Function

    <Extension()> Public Function GetBytes(ByVal e As Encoding,
                                           ByVal s As String,
                                           ByVal length As Int32) As Byte()
        Return GetBytes(e, s, 0, length)
    End Function

    <Extension()> Public Function GetByteCount(ByVal e As Encoding,
                                               ByVal chars() As Char,
                                               ByVal count As Int32) As Int32
        assert(Not e Is Nothing)
        Return e.GetByteCount(chars, 0, count)
    End Function

    <Extension()> Public Function GetByteCount(ByVal e As Encoding,
                                               ByVal s As String,
                                               ByVal index As Int32,
                                               ByVal length As Int32) As Int32
        assert(Not e Is Nothing)
        assert(Not s Is Nothing)
        Return e.GetByteCount(s.ToCharArray(index, length))
    End Function

    <Extension()> Public Function GetByteCount(ByVal e As Encoding,
                                               ByVal s As String,
                                               ByVal length As Int32) As Int32
        Return GetByteCount(e, s, 0, length)
    End Function

    <Extension()> Public Function encoding_possibility(ByVal this As Stream, ByVal e As Encoding) As Double
        assert(Not this Is Nothing)
        assert(Not e Is Nothing)
        If this.Length() < e.GetPreamble().array_size_i() Then
            Return 0
        End If

        Dim p As Int64 = this.Position()
        Try
            Dim m(CInt(min(5120, this.Length())) - 1) As Byte
            assert(this.Read(m, 0, m.array_size_i()) = m.array_size_i())
            Dim s As String = Nothing
            If e.GetPreamble().isemptyarray() Then
                s = e.GetString(m)
            Else
                s = e.GetString(m, e.GetPreamble().array_size_i(), m.array_size_i() - e.GetPreamble().array_size_i())
            End If
            Dim d() As Byte = e.GetPreamble().concat(e.GetBytes(s))
            Dim equals As UInt32 = 0
            For i As Int32 = 0 To min(m.array_size_i(), d.array_size_i()) - 1
                If m(i) = d(i) Then
                    equals += uint32_1
                End If
            Next
            Return equals / min(m.array_size_i(), d.array_size_i())
        Finally
            this.Position() = p
        End Try
    End Function

    <Extension()> Public Function guess_encoding(ByVal this As Stream,
                                                 ByRef max_possibility As Double,
                                                 ParamArray ByVal candidates() As Encoding) As Encoding
        assert(Not this Is Nothing)
        assert(Not candidates.isemptyarray())
        max_possibility = 0
        Dim max_encoding As Encoding = Encoding.Default
        For i As Int32 = 0 To candidates.array_size_i() - 1
            If Array.IndexOf(candidates, candidates(i)) < i Then
                Continue For
            End If
            Dim c As Double = this.encoding_possibility(candidates(i))
            If c > max_possibility Then
                max_possibility = c
                max_encoding = candidates(i)
            End If
        Next
        Return max_encoding
    End Function

    <Extension()> Public Function guess_encoding(ByVal this As Stream, ByRef max_possibility As Double) As Encoding
        Return guess_encoding(this,
                              max_possibility,
                              Encoding.UTF8,
                              encodings.utf8_nobom,
                              Encoding.Unicode,
                              Encoding.BigEndianUnicode,
                              encodings.gbk_or_default)
    End Function

    <Extension()> Public Function guess_encoding(ByVal this As Stream) As Encoding
        Return guess_encoding(this, 0)
    End Function

    <Extension()> Public Function guess_encoding(ByVal this As Stream,
                                                 ParamArray ByVal candidates() As Encoding) As Encoding
        Return guess_encoding(this, 0, candidates)
    End Function
End Module
