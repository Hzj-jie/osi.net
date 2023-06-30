
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports _sizeof = osi.service.compiler.logic._sizeof

Namespace logic
    ' TODO: Avoid using constructors directly.
    Public NotInheritable Class interrupt_test
        Inherits executor_case

        Private ReadOnly text As String
        Private ReadOnly input As disposer(Of StringReader)
        Private ReadOnly out As disposer(Of StringWriter)
        Private ReadOnly err As disposer(Of StringWriter)
        Private ReadOnly _interrupts As interrupts

        Public Sub New()
            MyBase.New(
                New _define("1", scope.type_t.variable_type),
                New _copy_const("1", New data_block(1)),
                New _define("i", scope.type_t.variable_type),
                New _define("input", scope.type_t.variable_type),
                New _interrupt("stdin", "i", "input"),
                New _interrupt("stdout", "input", "i"),
                New _interrupt("stderr", "input", "i"),
                New _define("len", scope.type_t.variable_type),
                New _sizeof("len", "input"),
                New _define("i-less-then-len", scope.type_t.variable_type),
                New _do_while("i-less-then-len", New paragraph(
                    New _define("char", scope.type_t.variable_type),
                    New _define("result", scope.type_t.variable_type),
                    New _cut_len("char", "input", "i", "1"),
                    New _interrupt("stdout", "char", "result"),
                    New _interrupt("stderr", "char", "result"),
                    New _add("i", "i", "1"),
                    New _less("i-less-then-len", "i", "len")
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
