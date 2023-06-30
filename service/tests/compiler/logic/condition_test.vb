
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
    Public NotInheritable Class condition_test
        Inherits executor_case

        Private Shared ReadOnly value1 As String
        Private Shared ReadOnly value2 As String
        Private ReadOnly io As console_io
        Private ReadOnly ext As interrupts
        Private ReadOnly out As disposer(Of StringWriter)

        Shared Sub New()
            value1 = random_value()
            value2 = random_value()
        End Sub

        Public Sub New()
            MyBase.New(
                New _define("a-bool", scope.type_t.variable_type),
                New _define("value1", scope.type_t.variable_type),
                New _copy_const("value1", New data_block(value1)),
                New _define("value2", scope.type_t.variable_type),
                New _copy_const("value2", New data_block(value2)),
                New _define("value", scope.type_t.variable_type),
                New _copy_const("a-bool", New data_block(True)),
                create_condition(),
                New _copy_const("a-bool", New data_block(False)),
                create_condition(),
                New _stop()
            )
            out = make_disposer(New StringWriter())
            io = New console_io()
            io.redirect_output(+out)
            ext = New interrupts(io)
        End Sub

        Private Shared Function random_value() As String
            Return strcat(rnd_utf8_chars(rnd_int(100, 200)), newline.incode())
        End Function

        Private Shared Function create_condition() As _if
            Return New _if("a-bool",
                New paragraph(
                    New _interrupt("stdout", "value1", "value")
                ),
                New paragraph(
                    New _interrupt("stdout", "value2", "value")
                )
            )
        End Function

        Protected Overrides Function interrupts() As interrupts
            Return ext
        End Function

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assertion.equal(Convert.ToString(io.output()), strcat(value1, value2))
        End Sub
    End Class
End Namespace
