
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_udec
    Public Function str_match(ByVal s As String, Optional ByVal str_base As Byte = constants.str_base) As UInt32
        Dim i As UInt32 = 0
        Dim n As big_uint = Nothing
        n = Me.n.CloneT()
        While True
            Dim r As big_uint = Nothing
            n.assert_divide(d, r)
            Dim c As String = Nothing
            c = n.str(str_base)
            If Not s.strsame(i, c, 0, c.strlen()) Then
                Return i
            End If
            i += c.strlen()
            If i = c.strlen() Then
                If Not s.strsame(i, character.dot, 0, character.dot.ToString().strlen()) Then
                    Return i
                End If
                i += character.dot.ToString().strlen()
            End If
            n = r
            n.multiply(str_base)
        End While
        assert(False)
        Return 0
    End Function
End Class
