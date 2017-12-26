
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.constructor

Partial Public Class wrapper_test
    <test>
    Public NotInheritable Class wrapper_register_in_multiple_threads_test
        Private Const type_base As String = "register_in_multiple_threads_wrapper"
        Private Const repeat_count As UInt64 = 1000
        Private Const thread_count As UInt32 = 16

        Private Class test_class
            Inherits test_class(Of wrapper_register_in_multiple_threads_test)

            Public Sub New()
                MyBase.New()
            End Sub

            Public Sub New(ByVal t As test_class)
                MyBase.New(t)
            End Sub
        End Class

        <test>
        <repeat(repeat_count)>
        <multi_threading(thread_count)>
        Private Shared Sub run()
            Dim type As String = Nothing
            type = strcat(type_base, rnd_uint(0, thread_count))
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
            If repeat_case_wrapper.current_round() = repeat_count - 1 Then
                type = strcat(type_base, multithreading_case_wrapper.thread_id())
                assert_true(wrapper.register(Function(ByVal v As var, ByVal i As test_class) As test_class
                                                 Return New test_class(i)
                                             End Function,
                                             index))
                assert_true(wrapper.register(type,
                                             Function(ByVal v As var, ByVal i As test_class) As test_class
                                                 Return New test_class(i)
                                             End Function,
                                             index))
            End If
        End Sub

        <finish>
        Private Shared Sub verify()
            Dim t As test_class = Nothing
            assert_true(wrapper.wrap(New var(), New test_class(), t))
            t.assert_wrap(thread_count)

            For i As UInt32 = 0 To thread_count - uint32_1
                assert_true(wrapper.wrap(New var(strcat("--wrapper=", type_base, i)), New test_class(), t))
                t.assert_wrap(CInt(thread_count) + 1)
                assert_true(wrapper(Of test_class).erase(strcat(type_base, i)))
            Next
            assert_true(wrapper(Of test_class).erase())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
