
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class condition_test
        Inherits executor_case

        Private Shared ReadOnly value1 As String
        Private Shared ReadOnly value2 As String
        Private ReadOnly io As console_io
        Private ReadOnly ext As extern_functions
        Private ReadOnly out As disposer(Of StringWriter)

        Shared Sub New()
            value1 = random_value()
            value2 = random_value()
        End Sub

        Public Sub New()
            MyBase.New(
                New define("a-bool", types.variable_type),
                New define("value1", types.variable_type),
                New copy_const(types.empty, "value1", unique_ptr.[New](New data_block(value1))),
                New define("value2", types.variable_type),
                New copy_const(types.empty, "value2", unique_ptr.[New](New data_block(value2))),
                New define("value", types.variable_type),
                New copy_const(types.empty, "a-bool", unique_ptr.[New](New data_block(True))),
                create_condition(),
                New copy_const(types.empty, "a-bool", unique_ptr.[New](New data_block(False))),
                create_condition(),
                New [stop]()
            )
            out = make_disposer(New StringWriter())
            io = New console_io()
            io.redirect_output(+out)
            ext = New extern_functions(io)
        End Sub

        Private Shared Function random_value() As String
            Return strcat(rnd_utf8_chars(rnd_int(100, 200)), newline.incode())
        End Function

        Private Shared Function create_condition() As condition
            Return New condition("a-bool",
                unique_ptr.[New](New paragraph(
                    New extern_function(types.empty, New extern_functions(), "stdout", "value1", "value")
                )),
                unique_ptr.[New](New paragraph(
                    New extern_function(types.empty, New extern_functions(), "stdout", "value2", "value")
                ))
            )
        End Function

        Protected Overrides Function extern_functions() As extern_functions
            Return ext
        End Function

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assert_equal(Convert.ToString(io.output()), strcat(value1, value2))
        End Sub
    End Class
End Namespace
