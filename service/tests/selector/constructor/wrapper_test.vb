
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.selector

<test>
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
            Dim this As test_class = Nothing
            this = Me
            While count > 0
                assert_not_nothing(this.t)
                this = this.t
                count -= 1
            End While
            assert_nothing(this.t)
        End Sub
    End Class

    <test>
    Private Shared Sub multiple_registers()
        Const wrapper_count As UInt32 = 10
        Const type As String = "multiple_register_wrapper"
        For i As UInt32 = 0 To wrapper_count - uint32_1
            wrapper.register(type,
                             Function(ByVal v As var, ByVal j As test_class) As test_class
                                 Return New test_class(j)
                             End Function)
        Next
        Dim r As test_class = Nothing
        assert_true(wrapper.wrap(New var("--wrapper=multiple_register_wrapper"), New test_class(), r))
        r.assert_wrap(wrapper_count)
    End Sub

    <test>
    Public NotInheritable Class wrapper_register_in_multiple_threads
        Private Const type_base As String = "register_in_multiple_threads_wrapper"
        Private Const repeat_count As UInt64 = 1000
        Private Const thread_count As UInt32 = 16

        <test>
        <repeat(repeat_count)>
        <multi_threading(thread_count)>
        Private Shared Sub run()
            Dim type As String = Nothing
            type = strcat(type_base, multithreading_case_wrapper.thread_id())
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
        Private Shared Sub finish()
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

    Private Sub New()
    End Sub
End Class
