
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic
Imports osi.service.math

Namespace logic
    <test>
    Public NotInheritable Class importer_test
        Private Shared Function make_case(ByVal ParamArray lines() As String) As String
            assert(Not isemptyarray(lines))
            Dim v As New vector(Of String)()
            assert(v.emplace_back(lines))
            Return v.str(newline.incode())
        End Function

        Private Shared Function make_case(ByVal result()() As Byte,
                                          ByVal ParamArray lines() As String) As pair(Of String, Byte()())
            Return pair.emplace_of(make_case(lines), result)
        End Function

        <test>
        Private Shared Function import_empty() As Boolean
            Dim e As executor = Nothing
            assertion.is_false(New importer().import("", e))
            assertion.is_null(e)
            assertion.is_false(New importer().import(Nothing, e))
            assertion.is_null(e)
            Return True
        End Function

        Private Shared ReadOnly importable_cases() As String = {
                make_case("define v type*"),
                make_case("type int 4",
                          "define v int",
                          "define s int",
                          "sizeof s v")
            }

        <test>
        Private Shared Function importable() As Boolean
            For i As UInt32 = 0 To array_size(importable_cases) - uint32_1
                Dim e As executor = Nothing
                assertion.is_true(New importer().import(importable_cases(CInt(i)), e))
                assertion.is_not_null(e)
            Next
            Return True
        End Function

        Private Shared ReadOnly not_importable_cases() As String = {
                make_case("define v type*",
                          "define v type*"),
                make_case("unknown"),
                make_case("define v unknown_type",
                          "add v v v")
            }

        <test>
        Private Shared Function not_importable() As Boolean
            For i As UInt32 = 0 To array_size(not_importable_cases) - uint32_1
                Dim e As executor = Nothing
                assertion.is_false(New importer().import(not_importable_cases(CInt(i)), e))
                assertion.is_null(e)
            Next
            Return True
        End Function

        Private Shared ReadOnly cases() As pair(Of String, Byte()()) = {
                make_case({(New big_uint(1000)).as_bytes(),
                           (New big_uint(1)).as_bytes(),
                           (New big_uint(1000)).as_bytes(),
                           (New big_uint(500500)).as_bytes(),
                           bool_bytes(False)},
                          "type int 4",
                          "type bool 1",
                          "define upper_bound int",
                          "copy_const upper_bound i1000",
                          "define 1 int",
                          "copy_const 1 i1",
                          "define i type*",
                          "define result type*",
                          "copy_const i i0",
                          "define continue bool",
                          "less continue i upper_bound",
                          "while_then continue { ",
                          "add i i 1",
                          "add result result i",
                          "less continue i upper_bound",
                          "}")
            }

        <test>
        Private Shared Function execute_cases() As Boolean
            For i As UInt32 = 0 To array_size(cases) - uint32_1
                Dim e As executor = Nothing
                If Not assertion.is_true(New importer().import(cases(CInt(i)).first, e)) OrElse
                   Not assertion.is_not_null(e) Then
                    Return False
                End If
                e.execute()
                assertion.is_false(e.halt(), lazier.of(AddressOf e.halt_error))
                For j As UInt32 = 0 To array_size(cases(CInt(i)).second) - uint32_1
                    Dim x() As Byte = Nothing
                    Try
                        x = +e.access(data_ref.abs(j))
                    Catch ex As executor_stop_error
                        assertion.is_true(False, ex)
                        Continue For
                    End Try
                    assertion.array_equal(x, cases(CInt(i)).second(CInt(j)))
                Next
            Next
            Return True
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
