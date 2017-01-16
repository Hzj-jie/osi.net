
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.formation
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic
Imports osi.service.math

Namespace logic
    Public Class while_then_perf_500000
        Inherits while_then_perf(Of _500000)
    End Class

    Public Class while_then_perf_1000000
        Inherits while_then_perf(Of _1000000)
    End Class

    Public Class while_then_perf_5000000
        Inherits while_then_perf(Of _5000000)
    End Class

    Public Class while_then_perf_10000000
        Inherits while_then_perf(Of _10000000)
    End Class

    Public Class while_then_perf_50000000
        Inherits while_then_perf(Of _50000000)
    End Class

    Public Class while_then_perf(Of _UPPER_BOUND As _int64)
        Inherits commandline_specific_case_wrapper

        Public Sub New()
            MyBase.New(New while_then_perf_case())
        End Sub

        Private Class while_then_perf_case
            Inherits performance_comparison_case_wrapper

            Private Shared ReadOnly upper_bound As Int64

            Shared Sub New()
                upper_bound = +alloc(Of _UPPER_BOUND)()
                assert(upper_bound >= 0)
            End Sub

            Public Sub New()
                MyBase.New(New while_then_case(), New internal_case())
            End Sub

            Private Class while_then_case
                Inherits executor_case

                Public Sub New()
                    MyBase.New({
                New define("state", types.variable_type),
                New define("result", types.variable_type),
                New define("i", types.variable_type),
                New define("1", types.variable_type),
                New define("UPPER_BOUND", types.variable_type),
                New move_const(types.empty, "state", unique_ptr.[New](New data_block(True))),
                New move_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                New move_const(types.empty, "UPPER_BOUND", unique_ptr.[New](New data_block(upper_bound))),
                New while_then("state", unique_ptr.[New](New paragraph(
                    New add(types.empty, "i", "i", "1"),
                    New add(types.empty, "result", "result", "i"),
                    New less_or_equal(types.empty, "state", "i", "UPPER_BOUND")
                )))
            })
                End Sub

                Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
                    assert_equal(e.get().access_stack_as_bool(data_ref.abs(0)), False)
                    assert_equal(e.get().access_stack_as_uint64(data_ref.abs(1)),
                             CULng((upper_bound + 1) * (upper_bound + 2) / 2))
                    assert_equal(e.get().access_stack_as_uint32(data_ref.abs(2)), CUInt(upper_bound + 1))
                    assert_equal(e.get().access_stack_as_uint32(data_ref.abs(3)), CUInt(1))
                    assert_equal(e.get().access_stack_as_uint32(data_ref.abs(4)), CUInt(upper_bound))
                End Sub
            End Class

            Private Class internal_case
                Inherits [case]

                Public Overrides Function run() As Boolean
                    Dim r As big_uint = Nothing
                    r = New big_uint()
                    Dim i As Int32 = 0
                    While i <= upper_bound
                        i += 1
                        r += New big_uint(CUInt(i))
                    End While
                    assert_equal(r, CULng((upper_bound + 1) * (upper_bound + 2) / 2))
                    Return True
                End Function
            End Class
        End Class
    End Class
End Namespace
