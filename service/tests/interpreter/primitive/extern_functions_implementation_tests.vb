
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class console_io_test
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim s As String = Nothing
            Using out As TextWriter = New StringWriter()
                console_io.redirect_output(out)
                s = strcat(guid_str(), newline.incode())
                assert_array_equal(extern_functions.stdout(str_bytes(s)), Nothing)
                assert_equal(Convert.ToString(out), s)
                console_io.redirect_output()
            End Using
            Using out As TextWriter = New StringWriter()
                console_io.redirect_error(out)
                s = strcat(guid_str(), newline.incode())
                assert_array_equal(extern_functions.stderr(str_bytes(s)), Nothing)
                assert_equal(Convert.ToString(out), s)
                console_io.redirect_error()
            End Using
            s = guid_str()
            Using [in] As TextReader = New StringReader(strcat(s, newline.incode()))
                console_io.redirect_input([in])
                assert_equal(s, bytes_str(extern_functions.stdin(Nothing)))
                console_io.redirect_input()
            End Using
            Return True
        End Function
    End Class
End Namespace
