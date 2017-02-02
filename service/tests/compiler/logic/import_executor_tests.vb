
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.math
Imports osi.service.resource

Namespace logic
    Public Class import_executor_test1
        Inherits import_executor_case

        Public Sub New()
            MyBase.New(case1.as_text())
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assert_equal((+e).stack_size(), CUInt(8))
            assert_array_equal(+((+e).access_stack(data_ref.abs(0))),
                               str_bytes(strcat("hello world", character.newline)))
            assert_array_equal(+((+e).access_stack(data_ref.abs(1))),
                               str_bytes(strcat(character.newline, "dlrow olleh")))
            assert_array_equal(+((+e).access_stack(data_ref.abs(2))),
                               str_bytes(strcat(character.newline, "dlrow olleh")))
            assert_array_equal(+((+e).access_stack(data_ref.abs(3))), uint32_bytes(0))
            assert_array_equal(+((+e).access_stack(data_ref.abs(4))), (New big_uint(1)).as_bytes())
            assert_array_equal(+((+e).access_stack(data_ref.abs(5))), uint32_bytes(12))
            assert_array_equal(+((+e).access_stack(data_ref.abs(6))), (New big_uint(0)).as_bytes())
            assert_array_equal(+((+e).access_stack(data_ref.abs(7))), bool_bytes(False))
        End Sub
    End Class
End Namespace
