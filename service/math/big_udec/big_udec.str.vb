
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Function str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return as_str().with_str_base(str_base).ToString()
    End Function

    Public Function fractional_str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return strcat(n.str(str_base), " / ", d.str(str_base))
    End Function

    Public Structure str_option
        Private ReadOnly this As big_udec
        Private str_base As Byte
        Private upure_numerator_size_multiply As UInt32
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

        Public Function with_upure_numerator_size_multiply(ByVal v As UInt32) As str_option
            assert(v > 0)
            Me.upure_numerator_size_multiply = v
            Return Me
        End Function

        Public Function with_upure_length(ByVal v As UInt32) As str_option
            assert(v > 0)
            Me.upure_len = v
            Return Me
        End Function

        Private Function upure_part() As String
            Dim n As big_uint = Nothing
            n = this.dec_part().n
            If n.is_zero() Then
                Return empty_string
            End If
            Dim base As big_uint = Nothing
            base = New big_uint(str_base)
            Dim l As UInt32 = 0
            l = (this.d.uint32_size() * upure_numerator_size_multiply - n.uint32_size()) *
                CUInt(System.Math.Log(upure_len, str_base) + 1)
            n.multiply(base.power(l))
            n.divide(this.d)
            Dim r As String = Nothing
            r = n.str(str_base)
            assert(l >= strlen(r))
            Return strcat(strncat(character.dot, character.zero, l - strlen(r)), r).
                       strleft(upure_len).
                       TrimEnd(character.dot, character.zero)
        End Function

        Public Overrides Function ToString() As String
            Return strcat(this.int_part().str(str_base), upure_part())
        End Function

        Public Shared Widening Operator CType(ByVal this As str_option) As String
            Return this.ToString()
        End Operator
    End Structure

    Public Function as_str() As str_option
        Return New str_option(Me).
                with_str_base(constants.str_base).
                with_upure_numerator_size_multiply(constants.str_upure_numerator_size_multiply).
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
        o = New big_udec(n, d)
        Return True
    End Function

    Public Shared Function parse(ByVal s As String, Optional ByVal base As Byte = constants.str_base) As big_udec
        Dim r As big_udec = Nothing
        assert(parse(s, r, base))
        Return r
    End Function
End Class
