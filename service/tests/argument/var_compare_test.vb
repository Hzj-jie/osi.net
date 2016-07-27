
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.argument

Public Class var_compare_test
    Inherits [case]

    Private Shared Function run_case(ByVal left As String,
                                     ByVal right As String,
                                     ByVal exp As Int32) As Boolean
        assert_compare(New var(left), New var(right), exp)
        Return True
    End Function

    Private Shared Function run_case(ByVal v As String) As Boolean
        Return run_case(v, v, 0)
    End Function

    Private Shared Function equal_cases() As Boolean
        Return run_case("--a=b -cde ~fgh") AndAlso
               run_case("--a=b --c=d --e=f g h i") AndAlso
               run_case("--a=b -cde ~fgh", "-c ~fgh -d -e --a=b", 0)
    End Function

    Private Shared Function unequal_cases() As Boolean
        Return run_case("--a=b", "--A=B", -1) AndAlso
               run_case("--a=A", "--a=a", 1) AndAlso
               run_case("AAA", "aaa", 1) AndAlso
               run_case("--a=a", "--a=a -b", -1) AndAlso
               run_case("--a=a ab", "ab --a=ab", -1)
    End Function

    Public Overrides Function run() As Boolean
        Return equal_cases() AndAlso
               unequal_cases()
    End Function
End Class
