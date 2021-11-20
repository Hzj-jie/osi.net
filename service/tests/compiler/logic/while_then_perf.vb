
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.formation
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic
Imports osi.service.math

Namespace logic
    Public NotInheritable Class while_then_perf_500000
        Inherits while_then_perf(Of _500000)
    End Class

    Public NotInheritable Class while_then_perf_1000000
        Inherits while_then_perf(Of _1000000)
    End Class

    Public NotInheritable Class while_then_perf_5000000
        Inherits while_then_perf(Of _5000000)
    End Class

    Public NotInheritable Class while_then_perf_10000000
        Inherits while_then_perf(Of _10000000)
    End Class

    Public NotInheritable Class while_then_perf_50000000
        Inherits while_then_perf(Of _50000000)
    End Class

    Public Class while_then_perf(Of _UPPER_BOUND As _int64)
        Inherits commandline_specified_case_wrapper

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

            Private NotInheritable Class while_then_case
                Inherits executor_case

                Public Sub New()
                    MyBase.New(
                        New define(types.empty, "state", types.variable_type),
                        New define(types.empty, "result", types.variable_type),
                        New define(types.empty, "i", types.variable_type),
                        New define(types.empty, "1", types.variable_type),
                        New define(types.empty, "UPPER_BOUND", types.variable_type),
                        New copy_const(types.empty, "state", unique_ptr.[New](New data_block(True))),
                        New copy_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                        New copy_const(types.empty, "UPPER_BOUND", unique_ptr.[New](New data_block(upper_bound))),
                        New while_then("state", unique_ptr.[New](New paragraph(
                            New add(types.empty, "i", "i", "1"),
                            New add(types.empty, "result", "result", "i"),
                            New less_or_equal(types.empty, "state", "i", "UPPER_BOUND")
                        )))
                    )
                End Sub

                Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
                    assertion.equal(e.get().access_as_bool(data_ref.abs(0)), False)
                    assertion.equal(e.get().access_as_uint64(data_ref.abs(1)),
                             CULng((upper_bound + 1) * (upper_bound + 2) / 2))
                    assertion.equal(e.get().access_as_uint32(data_ref.abs(2)), CUInt(upper_bound + 1))
                    assertion.equal(e.get().access_as_uint32(data_ref.abs(3)), CUInt(1))
                    assertion.equal(e.get().access_as_uint32(data_ref.abs(4)), CUInt(upper_bound))
                End Sub
            End Class

            Private NotInheritable Class internal_case
                Inherits [case]

                Public Overrides Function run() As Boolean
                    Dim r As big_uint = Nothing
                    r = New big_uint()
                    Dim i As Int32 = 0
                    While i <= upper_bound
                        i += 1
                        r += New big_uint(CUInt(i))
                    End While
                    assertion.equal(r, CULng((upper_bound + 1) * (upper_bound + 2) / 2))
                    Return True
                End Function
            End Class
        End Class
    End Class

    Public NotInheritable Class while_then_perf
        Inherits performance_case_wrapper

        Public Sub New()
            MyBase.New(New while_then_case())
        End Sub

        Private NotInheritable Class while_then_case
            Inherits executor_case

            Private Const upper_bound As Int32 = 10000000

            Public Sub New()
                MyBase.New({
                New define(types.empty, "state", types.variable_type),
                New define(types.empty, "result", types.variable_type),
                New define(types.empty, "i", types.variable_type),
                New define(types.empty, "1", types.variable_type),
                New define(types.empty, "UPPER_BOUND", types.variable_type),
                New copy_const(types.empty, "state", unique_ptr.[New](New data_block(True))),
                New copy_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                New copy_const(types.empty, "UPPER_BOUND", unique_ptr.[New](New data_block(upper_bound))),
                New while_then("state", unique_ptr.[New](New paragraph(
                    New add(types.empty, "i", "i", "1"),
                    New add(types.empty, "result", "result", "i"),
                    New less_or_equal(types.empty, "state", "i", "UPPER_BOUND")
                )))
            })
            End Sub
        End Class
    End Class
End Namespace
