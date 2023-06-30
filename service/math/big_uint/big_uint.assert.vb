
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_uint
    Public Shared Function support_base(ByVal b As Byte) As Boolean
        Return b > 1 AndAlso b <= support_str_base
    End Function

    ' TODO: This method should be used somewhere everywhere.
    Private Sub assert_no_extra_blank_position()
        assert(v.empty() OrElse v.back() > 0)
    End Sub

    Private Shared Sub assert_support_base(ByVal b As Byte)
        assert(support_base(b))
    End Sub
End Class
