
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class console_io_test
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim io As console_io = Nothing
            io = New console_io()
            Dim f As interrupts = Nothing
            f = New interrupts(io)
            Dim s As String = Nothing
            Using out As TextWriter = New StringWriter()
                io.redirect_output(out)
                s = strcat(guid_str(), newline.incode())
                assert_array_equal(f.stdout(str_bytes(s)), Nothing)
                assert_equal(Convert.ToString(out), s)
                io.redirect_output()
            End Using
            Using out As TextWriter = New StringWriter()
                io.redirect_error(out)
                s = strcat(guid_str(), newline.incode())
                assert_array_equal(f.stderr(str_bytes(s)), Nothing)
                assert_equal(Convert.ToString(out), s)
                io.redirect_error()
            End Using
            s = guid_str()
            Using [in] As TextReader = New StringReader(s)
                io.redirect_input([in])
                assert_equal(s, bytes_str(f.stdin(Nothing)))
                io.redirect_input()
            End Using
            Return True
        End Function
    End Class
End Namespace
