
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.lock

Public Class domain_unhandled_exception_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New domain_unhandled_exception_case())
    End Sub

    Private Class domain_unhandled_exception_case
        Inherits [case]

        Private Class an_exception
            Inherits Exception
        End Class

        Public Overrides Function run() As Boolean
            enable_domain_unhandled_exception_handler(True)
            Dim thrown As Boolean = False
            Dim called As Boolean = False
            Dim throw_id As Int32 = 0
            AddHandler domain_unhandled_exception,
                       Sub(ex As Exception)
                           assert_true(TypeOf ex Is an_exception)
                           assert_equal(throw_id, current_thread_id())
                           called = True
                       End Sub
            'make sure the following logic is running in another not delegate wrapped thread
            Dim th As Thread = Nothing
            th = New Thread(Sub()
                                thrown = True
                                throw_id = current_thread_id()
                                Throw New an_exception()
                            End Sub)
            th.Start()
            assert_true(timeslice_sleep_wait_until(Function() thrown, seconds_to_milliseconds(1)))
            assert_true(timeslice_sleep_wait_until(Function() called, seconds_to_milliseconds(1)))
            Return True
        End Function
    End Class
End Class
