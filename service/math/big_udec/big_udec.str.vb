
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Function str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return as_str().with_str_base(str_base).ToString()
    End Function

    Public Function fractional_str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return strcat(n.str(str_base), " ", character.left_slash, " ", d.str(str_base))
    End Function

    Public Structure str_option
        Private ReadOnly this As big_udec
        Private str_base As Byte
        Private upure_len As UInt32

        Public Sub New(ByVal this As big_udec)
            assert(Not this Is Nothing)
            Me.this = this
        End Sub

        Public Function with_str_base(ByVal b As Byte) As str_option
            assert(big_uint.support_base(b))
            Me.str_base = b
            Return Me
        End Function

        Public Function with_upure_length(ByVal v As UInt32) As str_option
            assert(v > 0)
            Me.upure_len = v
            Return Me
        End Function

        Public Function increase_upure_length(ByVal v As UInt32) As str_option
            assert(v > 0)
            Me.upure_len += v
            Return Me
        End Function

        Public Function double_upure_length() As str_option
            assert(Me.upure_len > 0)
            Me.upure_len <<= 1
            Return Me
        End Function

        Public Overrides Function ToString() As String
            If this.is_zero() Then
                Return character._0
            End If
            If this.is_one() Then
                Return character._1
            End If

            Dim o As StringBuilder = Nothing
            o = New StringBuilder()
            Dim n As big_uint = Nothing
            n = this.n.CloneT()
            For i As UInt32 = 0 To upure_len
                Dim r As big_uint = Nothing
                n.assert_divide(this.d, r)
                o.Append(n.str(str_base))
                If i = 0 Then
                    o.Append(character.dot)
                End If
                If r.is_zero() Then
                    Exit For
                End If
                n = r
                n.multiply(str_base)
            Next
            o.trim_end(character.zero)
            o.trim_end(character.dot)
            If o.Length() = 0 Then
                Return character._0
            End If
            Return o.ToString()
        End Function

        Public Shared Widening Operator CType(ByVal this As str_option) As String
            Return this.ToString()
        End Operator
    End Structure

    Public Function as_str() As str_option
        Return New str_option(Me).
                with_str_base(constants.str_base).
                with_upure_length(constants.str_upure_len)
    End Function

    Public Shared Function support_base(ByVal base As Byte) As Boolean
        Return big_uint.support_base(base)
    End Function

    Public Shared Sub assert_support_base(ByVal base As Byte)
        assert(support_base(base))
    End Sub

    Public Shared Function parse(ByVal s As String,
                                 ByRef o As big_udec,
                                 Optional ByVal base As Byte = constants.str_base) As Boolean
        If Not support_base(base) Then
            Return False
        End If
        If String.IsNullOrEmpty(s) Then
            o = big_udec.zero()
            Return True
        End If
        s = s.Trim()
        Dim i As Int32 = 0
        i = s.IndexOf(character.dot)
        If i = strlen(s) - uint32_1 Then
            Return False
        End If
        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        If i = npos Then
            If Not big_uint.parse(s, n, base) Then
                Return False
            End If
            d = big_uint.one()
        Else
            Dim zero_count As UInt32 = 0
            zero_count = strlen(s) - CUInt(i) - uint32_1
            assert(zero_count > 0)
            s = s.Remove(i, 1)
            If Not big_uint.parse(s, n, base) Then
                Return False
            End If
            assert(big_uint.parse(strncat("1", "0", zero_count), d, base))
        End If
        o = New big_udec(n, d).fast_reduce_fraction()
        Return True
    End Function

    Public Shared Function parse(ByVal s As String, Optional ByVal base As Byte = constants.str_base) As big_udec
        Dim r As big_udec = Nothing
        assert(parse(s, r, base))
        Return r
    End Function

    Public Shared Function parse_fraction(ByVal s As String,
                                          ByRef o As big_udec,
                                          Optional ByVal base As Byte = constants.str_base) As Boolean
        If Not support_base(base) Then
            Return False
        End If
        If String.IsNullOrEmpty(s) Then
            o = big_udec.zero()
            Return True
        End If
        s = s.Trim()
        If s.StartsWith(character.left_slash) OrElse s.EndsWith(character.left_slash) Then
            Return False
        End If
        Dim i As Int32 = 0
        i = s.IndexOf(character.left_slash)
        assert(i > 0 OrElse i < s.Length() - 1 OrElse i = npos)
        Dim n As big_uint = Nothing
        Dim d As big_uint = Nothing
        If i = npos Then
            If Not big_uint.parse(s, n, base) Then
                Return False
            End If
            d = big_uint.one()
        Else
            If Not big_uint.parse(s.Substring(0, i - 1), n, base) Then
                Return False
            End If
            If Not big_uint.parse(s.Substring(i + 1), d, base) Then
                Return False
            End If
        End If
        assert(Not n Is Nothing)
        assert(Not d Is Nothing)
        If d.is_zero() Then
            Return False
        End If
        o = New big_udec(n, d).fast_reduce_fraction()
        Return True
    End Function

    Public Shared Function parse_fraction(ByVal s As String,
                                          Optional ByVal base As Byte = constants.str_base) As big_udec
        Dim r As big_udec = Nothing
        assert(parse_fraction(s, r, base))
        Return r
    End Function
End Class
