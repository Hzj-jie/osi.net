
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_udec
    Private NotInheritable Class constants
        Public Const replace_by_dec_max_shift As UInt32 = 50
        Public Const str_base As Byte = 10
        Public Const str_upure_numerator_size_multiply As UInt32 = 2
        Public Const str_upure_len As UInt32 = 64
        Public Const extract_power_base As UInt32 = 10

        Private Sub New()
        End Sub
    End Class
End Class
