
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
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
            s = New simulator(New interrupts(io))
            assertion.is_true(s.import(sim3.as_text()))
            Using out As TextWriter = New StringWriter(),
                  err As TextWriter = New StringWriter()
                io.redirect_output(out)
                io.redirect_error(err)
                s.execute()
                assertion.is_false(s.halt())
                assertion.is_true(s.errors().empty())
                assertion.equal(Convert.ToString(out),
                             "hello world" + character.newline + "hello world" + character.newline)
                assertion.equal(Convert.ToString(err),
                             "hello world" + character.newline + "hello world" + character.newline)
            End Using
            Return True
        End Function

        Private Shared Function io_case() As Boolean
            Dim s As simulator = Nothing
            Dim io As console_io = Nothing
            io = New console_io()
            s = New simulator(New interrupts(io))
            assertion.is_true(s.import(sim4.as_text()))
            Dim input As String = Nothing
            input = rnd_en_chars(rnd_int(100, 1000))
            Using out As TextWriter = New StringWriter(),
                  err As TextWriter = New StringWriter(),
                  [in] As TextReader = New StringReader(input)
                io.redirect_input([in])
                io.redirect_output(out)
                io.redirect_error(err)
                s.execute()
                assertion.is_false(s.halt())
                assertion.is_true(s.errors().empty())
                assertion.equal(Convert.ToString(out), strcat(input, input))
                assertion.equal(Convert.ToString(err), strcat(input, input))
            End Using
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return output_case() AndAlso
                   io_case()
        End Function
    End Class
End Namespace
