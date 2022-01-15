
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports primitive = osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class interrupt_test
        Inherits executor_case

        Private ReadOnly text As String
        Private ReadOnly input As disposer(Of StringReader)
        Private ReadOnly out As disposer(Of StringWriter)
        Private ReadOnly err As disposer(Of StringWriter)
        Private ReadOnly _interrupts As interrupts

        Public Sub New()
            MyBase.New(
                New define("1", scope.type_t.variable_type),
                New copy_const("1", New data_block(1)),
                New define("i", scope.type_t.variable_type),
                New define("input", scope.type_t.variable_type),
                New interrupt("stdin", "i", "input"),
                New interrupt("stdout", "input", "i"),
                New interrupt("stderr", "input", "i"),
                New define("len", scope.type_t.variable_type),
                New sizeof("len", "input"),
                New define("i-less-then-len", scope.type_t.variable_type),
                New do_while("i-less-then-len", New paragraph(
                    New define("char", scope.type_t.variable_type),
                    New define("result", scope.type_t.variable_type),
                    New cut_len("char", "input", "i", "1"),
                    New interrupt("stdout", "char", "result"),
                    New interrupt("stderr", "char", "result"),
                    New add("i", "i", "1"),
                    New less("i-less-then-len", "i", "len")
                ))
            )
            text = rnd_en_chars(rnd_int(1000, 2000))
            input = make_disposer(New StringReader(text))
            out = make_disposer(New StringWriter())
            err = make_disposer(New StringWriter())
            Dim io As console_io = Nothing
            io = New console_io()
            io.redirect_input(+input)
            io.redirect_output(+out)
            io.redirect_error(+err)
            _interrupts = New interrupts(io)
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assertion.equal(Convert.ToString(+out), strcat(text, text))
            assertion.equal(Convert.ToString(+err), strcat(text, text))
        End Sub

        Protected Overrides Function interrupts() As interrupts
            Return _interrupts
        End Function
    End Class
End Namespace
