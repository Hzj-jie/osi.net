
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.math

Namespace primitive
    Public NotInheritable Class loaded_methods
        Public Shared Function current_ms(ByVal i() As Byte) As Byte()
            Return int64_bytes(nowadays.milliseconds())
        End Function

        Public Shared Function big_uint_to_str(ByVal i() As Byte) As Byte()
            Return str_bytes(New big_uint(i).str())
        End Function

        Public Shared Function big_udec_to_str(ByVal i() As Byte) As Byte()
            Dim u As big_udec = Nothing
            u = New big_udec()
            If Not u.replace_by(i) Then
                raise_error(error_type.user, "Failed to parse ", i, " into big_udec.")
                executor_stop_error.throw(executor.error_type.interrupt_implementation_failure)
            End If
            Return str_bytes(u.str())
        End Function

        Public Shared Function big_uint_to_big_udec(ByVal i() As Byte) As Byte()
            Dim u As big_uint = Nothing
            u = New big_uint()
            u.replace_by(i)
            Dim d As big_udec = Nothing
            d = big_udec.fraction(u, big_uint.one())
            Return d.as_bytes()
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
