
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.math
Imports osi.service.resource

Namespace logic
    Public NotInheritable Class import_executor_test1
        Inherits import_executor_case

        Public Sub New()
            MyBase.New(_import_executor_cases.case1.as_text())
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assertion.equal((+e).stack_size(), CUInt(8))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(0))),
                                  str_bytes(strcat("hello world", character.newline)))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(1))),
                                  str_bytes(strcat(character.newline, "dlrow olleh")))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(2))),
                                  str_bytes(strcat(character.newline, "dlrow olleh")))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(3))), uint32_bytes(0))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(4))), (New big_uint(1)).as_bytes())
            assertion.array_equal(+((+e).access_stack(data_ref.abs(5))), uint32_bytes(12))
            assertion.array_equal(+((+e).access_stack(data_ref.abs(6))), (New big_uint(0)).as_bytes())
            assertion.array_equal(+((+e).access_stack(data_ref.abs(7))), bool_bytes(False))
        End Sub
    End Class

    Public NotInheritable Class import_executor_test2
        Inherits import_executor_case

        Public Sub New()
            MyBase.New(_import_executor_cases.case2.as_text())
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            Dim b() As Byte = Nothing
            Try
                b = +((+e).access_stack(data_ref.abs(0)))
            Catch ex As executor_stop_error
                assertion.is_true(False, ex)
                MyBase.check_result(e)
                Return
            End Try

            Dim o As vector(Of Byte()) = Nothing
            If Not assertion.is_true(chunks.parse(b, o)) Then
                MyBase.check_result(e)
                Return
            End If

            If assertion.is_not_null(o) AndAlso
               assertion.equal(o.size(), CUInt(100)) Then
                For i As UInt32 = 0 To o.size() - uint32_1
                    assertion.array_equal(o(i), (New big_uint(i + uint32_1)).factorial().as_bytes())
                Next
            End If
            MyBase.check_result(e)
        End Sub
    End Class

    Public NotInheritable Class import_executor_test3
        Inherits import_executor_case

        Public Sub New()
            MyBase.New(_import_executor_cases.case3.as_text())
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            Dim b() As Byte = Nothing
            Try
                b = +((+e).access_stack(data_ref.abs(0)))
            Catch ex As executor_stop_error
                assertion.is_true(False, ex)
                MyBase.check_result(e)
                Return
            End Try

            Dim o As vector(Of Byte()) = Nothing
            If Not assertion.is_true(chunks.parse(b, o)) Then
                MyBase.check_result(e)
                Return
            End If

            If assertion.is_not_null(o) AndAlso
               assertion.equal(o.size(), CUInt(100)) Then
                For i As UInt32 = 0 To o.size() - uint32_1
                    assertion.array_equal(o(i), uint32_bytes(i + uint32_1))
                Next
            End If
            MyBase.check_result(e)
        End Sub
    End Class

    Public NotInheritable Class import_executor_test4
        Inherits import_executor_case

        Private Shared ReadOnly io As New console_io.test_wrapper()

        Public Sub New()
            MyBase.New(case4.as_text(), New interrupts(+io))
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            MyBase.check_result(e)
            assertion.equal(io.output(), "hello world")
            assertion.equal(io.error(), "hello world")
        End Sub
    End Class

    Public NotInheritable Class import_executor_heap
        Inherits import_executor_case

        Private Shared ReadOnly io As New console_io.test_wrapper()

        Public Sub New()
            MyBase.New(heap.as_text(), New interrupts(+io))
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            MyBase.check_result(e)
            assertion.equal(io.output(), Convert.ToChar(99))
            assertion.array_equal(assertion.catch_thrown(Of executor_stop_error) _
                                                        (Sub()
                                                             e.get().access_heap(heap_ref.of_high(0))
                                                         End Sub).error_types,
                                  {executor.error_type.heap_access_out_of_boundary})
        End Sub
    End Class
End Namespace
