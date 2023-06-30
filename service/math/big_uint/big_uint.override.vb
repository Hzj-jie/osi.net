
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_uint)().from(obj, False))
    End Function

    Public Overrides Function GetHashCode() As Int32
        If is_zero() Then
            Return 0
        End If
        If is_one() Then
            Return 1
        End If
        Dim h As Int32 = 0
        Dim i As UInt32 = 0
        While i < v.size()
            h = h Xor v.get(i).GetHashCode()
            i += uint32_1
        End While
        Return h
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
