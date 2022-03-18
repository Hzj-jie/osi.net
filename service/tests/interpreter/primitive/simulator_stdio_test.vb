
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public Class simulator_stdio_test
        Inherits [case]

        Private Shared Function output_case() As Boolean
            Dim io As New console_io()
            Dim s As New simulator(New interrupts(io))
            assertion.is_true(s.import(sim3.as_text()))
            Using out As TextWriter = New StringWriter(),
                  err As TextWriter = New StringWriter()
                io.redirect_output(out)
                io.redirect_error(err)
                s.execute()
                assertion.is_false(s.halt(), lazier.of(AddressOf s.halt_error))
                assertion.equal(Convert.ToString(out),
                             "hello world" + character.newline + "hello world" + character.newline)
                assertion.equal(Convert.ToString(err),
                             "hello world" + character.newline + "hello world" + character.newline)
            End Using
            Return True
        End Function

        Private Shared Function io_case() As Boolean
            Dim io As New console_io()
            Dim s As New simulator(New interrupts(io))
            assertion.is_true(s.import(sim4.as_text()))
            Dim input As String = rnd_en_chars(rnd_int(100, 1000))
            Using out As TextWriter = New StringWriter(),
                  err As TextWriter = New StringWriter(),
                  [in] As TextReader = New StringReader(input)
                io.redirect_input([in])
                io.redirect_output(out)
                io.redirect_error(err)
                s.execute()
                assertion.is_false(s.halt(), lazier.of(AddressOf s.halt_error))
                assertion.equal(Convert.ToString(out), strcat(input, input))
                assertion.equal(Convert.ToString(err), strcat(input, input))
            End Using
            Return True
        End Function

        Private Shared Function case8() As Boolean
            Dim io As New console_io.test_wrapper()
            Dim s As New simulator(New interrupts(+io))
            assertion.is_true(s.import(sim8.as_text()))
            s.execute()
            assertion.is_false(s.halt(), lazier.of(AddressOf s.halt_error))
            assertion.equal(io.output().Length(), 4)
            assertion.equal(Convert.ToInt32(io.output()(0)), 1)
            assertion.equal(Convert.ToInt32(io.output()(1)), 2)
            assertion.equal(Convert.ToInt32(io.output()(2)), 3)
            assertion.equal(Convert.ToInt32(io.output()(3)), 4)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return output_case() AndAlso
                   io_case() AndAlso
                   case8()
        End Function
    End Class
End Namespace
