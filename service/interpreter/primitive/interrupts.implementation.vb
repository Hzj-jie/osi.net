
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace primitive
    ' Visible for tests.
    Partial Public NotInheritable Class interrupts
        Public Function stdout(ByVal i() As Byte) As Byte()
            io.output().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Function stderr(ByVal i() As Byte) As Byte()
            io.error().Write(bytes_str(i))
            Return Nothing
        End Function

        Public Function stdin(ByVal i() As Byte) As Byte()
            Return str_bytes(io.input().ReadToEnd())
        End Function

        Public Function current_ms(ByVal i() As Byte) As Byte()
            Return int64_bytes(nowadays.milliseconds())
        End Function

        Public Function load_method(ByVal i() As Byte) As Byte()
            lm.load(i)
            Return Nothing
        End Function

        Public Function execute_loaded_method(ByVal i() As Byte) As Byte()
            Return lm.execute(i)
        End Function

        Public Function getchar(ByVal i() As Byte) As Byte()
            Return int32_bytes(io.input().Read())
        End Function

        Public Function putchar(ByVal i() As Byte) As Byte()
            io.output().Write(Convert.ToChar(bytes_int32(i)))
            Return Nothing
        End Function
    End Class
End Namespace
