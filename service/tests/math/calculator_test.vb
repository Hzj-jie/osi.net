
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.math

Public Class calculator_test
    Inherits [case]

    Private Shared Function run_cases(ByVal e As int_basic_expression,
                                      ByVal cases() As pair(Of String(), String),
                                      ByVal has_error As Boolean,
                                      ByVal has_result As Boolean) As Boolean
        assert(Not e Is Nothing)
        assert(Not isemptyarray(cases))
        For i As Int32 = 0 To array_size(cases) - 1
            assert(Not cases(i) Is Nothing)
            Dim r As expression_result(Of Int32) = Nothing
            r = e.execute(cases(i).first)
            assertion.equal(r.has_error(), has_error, strcat(cases(i).first))
            assertion.equal(r.has_result(), has_result, strcat(cases(i).first))
            assertion.equal(r.str(), cases(i).second, strcat(cases(i).first))
        Next
        Return True
    End Function

    Private Shared Function run_cases(ByVal e As int_basic_expression,
                                      ByVal cases(,) As String,
                                      ByVal has_error As Boolean,
                                      ByVal has_result As Boolean) As Boolean
        assert(Not e Is Nothing)
        assert(Not isemptyarray(cases))
        For i As Int32 = 0 To array_size(cases) - 1
            Dim r As expression_result(Of Int32) = Nothing
            r = e.execute(cases(i, 0))
            assertion.equal(r.has_error(), has_error, cases(i, 0))
            assertion.equal(r.has_result(), has_result, cases(i, 0))
            assertion.equal(r.str(), cases(i, 1), cases(i, 0))
        Next
        Return True
    End Function

    Private Shared Function run_success_cases(ByVal e As int_basic_expression, ByVal cases(,) As String) As Boolean
        Return run_cases(e, cases, False, True)
    End Function

    Private Shared Function run_failure_cases(ByVal e As int_basic_expression, ByVal cases(,) As String) As Boolean
        Return run_cases(e, cases, True, False)
    End Function

    Private Shared Function run_failure_cases(ByVal e As int_basic_expression,
                                              ByVal cases() As pair(Of String(), String)) As Boolean
        Return run_cases(e, cases, True, False)
    End Function

    Private Shared Function predefined_case(ByVal e As int_basic_expression) As Boolean
        Return run_success_cases(e, math_expression.predefined_cases)
    End Function

    Private Shared Function failure_case(ByVal e As int_basic_expression) As Boolean
        Return run_failure_cases(e, math_expression.failure_cases) AndAlso
               run_failure_cases(e, math_expression.failure_cases2)
    End Function

    Public Overrides Function run() As Boolean
        Dim e As int_basic_expression = Nothing
        e = New int_basic_expression()
        Return predefined_case(e) AndAlso
               failure_case(e)
    End Function
End Class
