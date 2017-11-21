
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.selector

Public NotInheritable Class wrapper_test
    Private Class test_class
        Public ReadOnly t As test_class

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal t As test_class)
            Me.t = t
        End Sub

        Public Sub assert_wrap(ByVal count As Int32)
            If count > 0 Then
                assert_not_nothing(t)
                t.assert_wrap(count - 1)
            End If
        End Sub
    End Class

    <test>
    Public NotInheritable Class wrapper_register_in_multiple_threads
        Private Const thread_count As UInt32 = 16

        <test>
        <repeat(1000)>
        <multi_threading(thread_count)>
        Private Shared Sub run()
            Dim type As String = Nothing
            type = strcat("wrapper", multithreading_case_wrapper.thread_id())
            Dim index As UInt32 = 0
            assert_true(wrapper.register(Function(ByVal v As var, ByVal i As test_class) As test_class
                                             Return New test_class(i)
                                         End Function,
                                     index))
            assert_true(wrapper(Of test_class).erase(index))
            assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class(i)
                                     End Function,
                                     index))
            assert_true(wrapper(Of test_class).erase(type, index))
            assert_true(wrapper.register(Function(ByVal v As var, ByVal i As test_class) As test_class
                                             Return New test_class(i)
                                         End Function,
                                     index))
            assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class(i)
                                     End Function,
                                     index))
        End Sub

        <finish>
        Private Shared Sub finish()
            Dim t As test_class = Nothing
            assert_true(wrapper.wrap(New var(), New test_class(), t))
            t.assert_wrap(4)

            For i As UInt32 = 0 To thread_count - uint32_1
                assert_true(wrapper.wrap(New var(strcat("--wrapper=wrapper", i)), New test_class(), t))
                t.assert_wrap(5)
            Next
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
