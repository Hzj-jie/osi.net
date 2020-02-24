
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_ufloat
    Public Function str(Optional ByVal str_base As Byte = constants.str_base) As String
        Return as_str().with_str_base(str_base).ToString()
    End Function

    Public Structure str_option
        Private ReadOnly i As big_uint
        Private ReadOnly upure_str_option As upure_dec.str_option
        Private str_base As Byte

        Public Sub New(ByVal this As big_ufloat)
            assert(Not this Is Nothing)
            Me.i = this.i
            Me.upure_str_option = this.d.as_str().without_leading_zero()
        End Sub

        Public Function with_str_base(ByVal b As Byte) As str_option
            assert(big_uint.support_base(b))
            Me.str_base = b
            Me.upure_str_option.with_str_base(b)
            Return Me
        End Function

        Public Function with_str_numerator_size_multiply(ByVal v As UInt32) As str_option
            Me.upure_str_option.with_str_numerator_size_multiply(v)
            Return Me
        End Function

        Public Function with_dec_part_length(ByVal v As UInt32) As str_option
            Me.upure_str_option.with_length(v)
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Return strcat(Me.i.str(Me.str_base), Me.upure_str_option)
        End Function

        Public Shared Widening Operator CType(ByVal this As str_option) As String
            Return this.ToString()
        End Operator
    End Structure

    Public Function as_str() As str_option
        Return New str_option(Me).with_str_base(constants.str_base)
    End Function
End Class
