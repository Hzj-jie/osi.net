
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure

Public NotInheritable Class isolate_case_wrapper_test
    Inherits [case]

    Private Shared ReadOnly suc_cases() As [case] = {New success_case(), New success_event_comb_case()}
    Private Shared ReadOnly fail_cases() As [case] = {
        New failure_case(),
        New failure_case2(),
        New failure_event_comb_case(),
        New failure_event_comb_case2()
    }
    Private Shared ReadOnly dead_cases() As isolate_case_wrapper = {
        New assert_failed_case()
    }

    Private Shared Function exec_case(ByVal c As [case], ByVal exp As Boolean) As Boolean
        Dim cc As commandline_specified_case_wrapper = cast(Of commandline_specified_case_wrapper).from(c)
        Dim i As [case] = isolated(cc)
        assertion.is_true(i.prepare())
        assertion.is_true(i.run())
        assertion.equal(i.finish(), exp)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To suc_cases.array_size_i() - 1
            If Not exec_case(suc_cases(i), True) Then
                Return False
            End If
        Next
        For i As Int32 = 0 To fail_cases.array_size_i() - 1
            If Not exec_case(fail_cases(i), False) Then
                Return False
            End If
        Next
        For i As Int32 = 0 To dead_cases.array_size_i() - 1
            Dim c As isolate_case_wrapper = dead_cases(i)
            assertion.is_true(c.prepare())
            assertion.is_true(c.run())
            assertion.is_true(c.finish())
            assertion.is_true(c.assert_death_msg().Contains(type_info(Of assert_failed_case).name))
        Next

        Return True
    End Function

    Public NotInheritable Class success_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_case(Sub()
                                         End Sub))
        End Sub
    End Class

    Public NotInheritable Class failure_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_case(Sub()
                                             assertion.is_true(False)
                                         End Sub))
        End Sub
    End Class

    Public NotInheritable Class failure_case2
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_case(Function() As Boolean
                                             Return False
                                         End Function))
        End Sub
    End Class

    Public NotInheritable Class success_event_comb_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_event_comb_case(AddressOf event_comb.succeeded))
        End Sub
    End Class

    Public NotInheritable Class failure_event_comb_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_event_comb_case(Function() As event_comb
                                                        Return New event_comb(Function() As Boolean
                                                                                  assertion.is_true(False)
                                                                                  Return goto_end()
                                                                              End Function)
                                                    End Function))
        End Sub
    End Class

    Public NotInheritable Class failure_event_comb_case2
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New delegate_event_comb_case(AddressOf event_comb.failed))
        End Sub
    End Class

    Public NotInheritable Class assert_failed_case
        Inherits isolate_case_wrapper

        Public Sub New()
            MyBase.New(New assert_failed_case_executor())
        End Sub

        Protected Overrides Function expected_return() As Int32
            Return constants.exit_code.assertion_failure
        End Function

        Protected Overrides Function check_results() As Boolean
            Return assertion.is_true(case_started()) AndAlso
                   assertion.is_false(case_finished()) AndAlso
                   assertion.equal(assertion_failures(), uint32_0)
        End Function

        Public NotInheritable Class assert_failed_case_executor
            Inherits commandline_specified_case_wrapper

            Public Sub New()
                MyBase.New(New delegate_case(Sub()
                                                 assert(False)
                                             End Sub))
            End Sub
        End Class
    End Class
End Class
