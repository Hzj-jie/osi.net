
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure

Public Class isolate_case_wrapper_test
    Inherits [case]

    Private Shared ReadOnly suc_cases() As [case]
    Private Shared ReadOnly fail_cases() As [case]

    Shared Sub New()
        suc_cases = {New success_case(), New success_event_comb_case()}
        fail_cases = {New failure_case(),
                      New failure_case2(),
                      New failure_event_comb_case(),
                      New failure_event_comb_case2()}
    End Sub

    Private Shared Function exec_case(ByVal c As [case], ByVal exp As Boolean) As Boolean
        Dim i As [case] = Nothing
        Dim cc As commandline_specified_case_wrapper = Nothing
        If cast(c, cc) AndAlso i Is Nothing Then
            i = isolated(cc)
        End If
        Dim ce As commandline_specified_case_wrapper = Nothing
        If cast(c, ce) AndAlso i Is Nothing Then
            i = isolated(ce)
        End If
        assert(Not i Is Nothing)
        assert_true(i.prepare())
        assert_true(i.run())
        assert_equal(i.finish(), exp)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim c As isolate_case_wrapper = Nothing
        For i As Int32 = 0 To array_size_i(suc_cases) - 1
            If Not exec_case(suc_cases(i), True) Then
                Return False
            End If
        Next
        For i As Int32 = 0 To array_size_i(fail_cases) - 1
            If Not exec_case(fail_cases(i), False) Then
                Return False
            End If
        Next

        Return True
    End Function

    Public Class success_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits [case]

            Public Overrides Function run() As Boolean
                Return True
            End Function
        End Class
    End Class

    Public Class failure_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits [case]

            Public Overrides Function run() As Boolean
                assert_true(False)
                Return True
            End Function
        End Class
    End Class

    Public Class failure_case2
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits [case]

            Public Overrides Function run() As Boolean
                Return False
            End Function
        End Class
    End Class

    Public Class success_event_comb_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits event_comb_case

            Public Overrides Function create() As event_comb
                Return event_comb.succeeded()
            End Function
        End Class
    End Class

    Public Class failure_event_comb_case
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits event_comb_case

            Public Overrides Function create() As event_comb
                Return New event_comb(Function() As Boolean
                                          assert_true(False)
                                          Return goto_end()
                                      End Function)
            End Function
        End Class
    End Class

    Public Class failure_event_comb_case2
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New c())
        End Sub

        Private Class c
            Inherits event_comb_case

            Public Overrides Function create() As event_comb
                Return event_comb.failed()
            End Function
        End Class
    End Class
End Class
