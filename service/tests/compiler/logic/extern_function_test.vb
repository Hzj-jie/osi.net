
Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports primitive = osi.service.interpreter.primitive

Namespace logic
    Public Class extern_function_test
        Inherits executor_case

        Private ReadOnly text As String
        Private ReadOnly input As disposer(Of StringReader)
        Private ReadOnly out As disposer(Of StringWriter)
        Private ReadOnly err As disposer(Of StringWriter)
        Private ReadOnly _extern_functions As extern_functions

        Public Sub New()
            MyBase.New({
                New define("1", types.variable_type),
                New move_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                New define("i", types.variable_type),
                New define("input", types.variable_type),
                New extern_function(types.empty, primitive.extern_functions.default, "stdin", "i", "input"),
                New extern_function(types.empty, primitive.extern_functions.default, "stdout", "input", "i"),
                New extern_function(types.empty, primitive.extern_functions.default, "stderr", "input", "i"),
                New define("len", types.variable_type),
                New sizeof(types.empty, "len", "input"),
                New define("i-less-then-len", types.variable_type),
                New do_while("i-less-then-len", unique_ptr.[New](New paragraph({
                    New define("char", types.variable_type),
                    New define("result", types.variable_type),
                    New cut_slice(types.empty, "char", "input", "i", "1"),
                    New extern_function(types.empty, primitive.extern_functions.default, "stdout", "char", "result"),
                    New extern_function(types.empty, primitive.extern_functions.default, "stderr", "char", "result"),
                    New add(types.empty, "i", "i", "1"),
                    New less(types.empty, "i-less-then-len", "i", "len")
                })))
            })
            text = rnd_en_chars(rnd_int(1000, 2000))
            input = make_disposer(New StringReader(text))
            out = make_disposer(New StringWriter())
            err = make_disposer(New StringWriter())
            Dim io As console_io = Nothing
            io = New console_io()
            io.redirect_input(+input)
            io.redirect_output(+out)
            io.redirect_error(+err)
            _extern_functions = New extern_functions(io)
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assert_equal(Convert.ToString(+out), strcat(text, text))
            assert_equal(Convert.ToString(+err), strcat(text, text))
        End Sub

        Protected Overrides Function extern_functions() As extern_functions
            Return _extern_functions
        End Function
    End Class
End Namespace
