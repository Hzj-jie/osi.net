
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic
Imports osi.service.math

Namespace logic
    Public Class importer_test
        Inherits [case]

        Private Shared ReadOnly importable_cases() As String
        Private Shared ReadOnly not_importable_cases() As String
        Private Shared ReadOnly cases() As pair(Of String, Byte()())

        Private Shared Function make_case(ByVal ParamArray lines() As String) As String
            assert(Not isemptyarray(lines))
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            assert(v.emplace_back(lines))
            Return v.ToString(newline.incode())
        End Function

        Private Shared Function make_case(ByVal result()() As Byte,
                                          ByVal ParamArray lines() As String) As pair(Of String, Byte()())
            Return emplace_make_pair(make_case(lines), result)
        End Function

        Shared Sub New()
            importable_cases = {
                make_case("define v *"),
                make_case("type int 4",
                          "define v int",
                          "define s int",
                          "sizeof s v")
            }

            not_importable_cases = {
                make_case("define v *",
                          "define v *"),
                make_case("unknown"),
                make_case("define v unknown_type",
                          "add v v v")
            }

            cases = {
                make_case({(New big_uint(1000)).as_bytes(),
                           (New big_uint(1)).as_bytes(),
                           (New big_uint(1000)).as_bytes(),
                           (New big_uint(500500)).as_bytes(),
                           bool_bytes(False)},
                          "type int 0",
                          "type bool 1",
                          "define upper_bound int",
                          "copy_const upper_bound i1000",
                          "define 1 int",
                          "copy_const 1 i1",
                          "define i int",
                          "define result int",
                          "copy_const i i0",
                          "define continue bool",
                          "less continue i upper_bound",
                          "while_then continue { ",
                          "add i i 1",
                          "add result result i",
                          "less continue i upper_bound",
                          "}")
            }
        End Sub

        Private Shared Function import_empty() As Boolean
            Dim e As executor = Nothing
            e = importer.[New]().import("")
            assertion.is_null(e)
            e = importer.[New]().import(Nothing)
            assertion.is_null(e)
            Return True
        End Function

        Private Shared Function importable() As Boolean
            For i As UInt32 = 0 To array_size(importable_cases) - uint32_1
                Dim e As executor = Nothing
                e = importer.[New]().import(importable_cases(CInt(i)))
                assertion.is_not_null(e)
            Next
            Return True
        End Function

        Private Shared Function not_importable() As Boolean
            For i As UInt32 = 0 To array_size(not_importable_cases) - uint32_1
                Dim e As executor = Nothing
                e = importer.[New]().import(not_importable_cases(CInt(i)))
                assertion.is_null(e)
            Next
            Return True
        End Function

        Private Shared Function execute_cases() As Boolean
            For i As UInt32 = 0 To array_size(cases) - uint32_1
                Dim e As executor = Nothing
                e = importer.[New]().import(cases(CInt(i)).first)
                If assertion.is_not_null(e) Then
                    e.execute()
                    assertion.is_false(e.halt())
                    assertion.equal(e.errors().size(), uint32_0)
                    For j As UInt32 = 0 To array_size(cases(CInt(i)).second) - uint32_1
                        Dim x() As Byte = Nothing
                        Try
                            x = +e.access_stack(data_ref.abs(j))
                        Catch ex As executor_stop_error
                            assertion.is_true(False, ex)
                            Continue For
                        End Try
                        assertion.array_equal(x, cases(CInt(i)).second(CInt(j)))
                    Next
                End If
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return import_empty() AndAlso
                   importable() AndAlso
                   not_importable() AndAlso
                   execute_cases()
        End Function
    End Class
End Namespace
