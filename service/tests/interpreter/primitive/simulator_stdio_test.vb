
Imports System.IO
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public Class simulator_stdio_test
        Inherits [case]

        Private Shared Function output_case() As Boolean
            Dim s As simulator = Nothing
            Dim io As console_io = Nothing
            io = New console_io()
            s = New simulator(New extern_functions(io))
            s.import(sim3.as_text())
            Using out As TextWriter = New StringWriter()
                io.redirect_output(out)
                s.execute()
                assert_false(s.halt())
                assert_true(s.errors().empty())
                assert_equal(Convert.ToString(out),
                             "hello world" + character.newline + "hello world" + character.newline)
            End Using
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return output_case()
        End Function
    End Class
End Namespace
