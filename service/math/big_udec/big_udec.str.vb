
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Function str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return as_str().with_str_base(str_base).ToString()
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
            Dim base As big_uint = Nothing
            base = New big_uint(str_base)
            Dim l As UInt32 = 0
            Dim n As big_uint = Nothing
            n = New big_uint(this.upure_dec_part().n)
            While n.uint32_size() <= (this.d.uint32_size() * upure_numerator_size_multiply)
                l += uint32_1
                n *= base
            End While
            n.divide(this.d)
            Dim r As String = Nothing
            r = n.str(str_base)
            assert(l >= strlen(r))
            Return strcat(strncat(character.dot, character.zero, l - strlen(r)), r).
                       TrimEnd(character.zero).
                       strleft(upure_len)
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
End Class
