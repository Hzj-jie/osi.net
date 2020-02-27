
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class udec
    Public Function str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return strcat(n.str(str_base), "/", d.str(str_base))
    End Function

    Public Structure upure_str_option
        Private ReadOnly this As udec
        Private str_base As Byte
        Private str_numerator_size_multiply As UInt32
        Private str_len As UInt32
        Private no_leading_zero As Boolean

        Public Sub New(ByVal this As udec)
            assert(Not this Is Nothing)
            Me.this = this
        End Sub

        Public Function with_str_base(ByVal b As Byte) As upure_str_option
            assert(big_uint.support_base(b))
            Me.str_base = b
            Return Me
        End Function

        Public Function with_str_numerator_size_multiply(ByVal v As UInt32) As upure_str_option
            assert(v > 0)
            Me.str_numerator_size_multiply = v
            Return Me
        End Function

        Public Function without_leading_zero() As upure_str_option
            Me.no_leading_zero = True
            Return Me
        End Function

        Public Function with_leading_zero() As upure_str_option
            Me.no_leading_zero = False
            Return Me
        End Function

        Public Function with_length(ByVal v As UInt32) As upure_str_option
            assert(v > 0)
            Me.str_len = v
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Dim base As big_uint = Nothing
            base = New big_uint(str_base)
            Dim l As UInt32 = 0
            Dim n As big_uint = Nothing
            n = New big_uint(this.n)
            While n.uint32_size() <= (this.d.uint32_size() * str_numerator_size_multiply)
                l += uint32_1
                n *= base
            End While
            n.divide(this.d)
            Dim r As String = Nothing
            r = n.str(str_base)
            assert(l >= strlen(r))
            r = strcat(strncat(character.dot, character.zero, l - strlen(r)), r).
                    TrimEnd(character.zero).
                    strleft(str_len)
            If no_leading_zero Then
                Return r
            End If
            Return strcat(character.zero, r)
        End Function

        Public Shared Widening Operator CType(ByVal this As upure_str_option) As String
            Return this.ToString()
        End Operator
    End Structure

    Public Function upure_dec_str() As upure_str_option
        Return New upure_str_option(Me.upure_dec_part()).
                with_str_base(constants.str_base).
                with_str_numerator_size_multiply(constants.str_numerator_size_multiply).
                with_length(constants.str_len).
                with_leading_zero()
    End Function
End Class
