
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
                New define(anchors.empty, "1", types.variable_type),
                New copy_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                New define(anchors.empty, "i", types.variable_type),
                New define(anchors.empty, "input", types.variable_type),
                New interrupt(types.empty, primitive.interrupts.default, "stdin", "i", "input"),
                New interrupt(types.empty, primitive.interrupts.default, "stdout", "input", "i"),
                New interrupt(types.empty, primitive.interrupts.default, "stderr", "input", "i"),
                New define(anchors.empty, "len", types.variable_type),
                New sizeof(types.empty, "len", "input"),
                New define(anchors.empty, "i-less-then-len", types.variable_type),
                New do_while("i-less-then-len", unique_ptr.[New](New paragraph(
                    New define(anchors.empty, "char", types.variable_type),
                    New define(anchors.empty, "result", types.variable_type),
                    New cut_slice(types.empty, "char", "input", "i", "1"),
                    New interrupt(types.empty, primitive.interrupts.default, "stdout", "char", "result"),
                    New interrupt(types.empty, primitive.interrupts.default, "stderr", "char", "result"),
                    New add(types.empty, "i", "i", "1"),
                    New less(types.empty, "i-less-then-len", "i", "len")
                )))
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
