
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
Public Class constructor_register_in_multiple_threads_test
    Private Const repeat_count As UInt32 = 1000
    Private Const thread_count As UInt32 = 16

    Private Class test_class
        Public ReadOnly i As UInt32

        Public Sub New(ByVal i As UInt32)
            Me.i = i
        End Sub
    End Class

    <test>
    <repeat(repeat_count)>
    <multi_threading(thread_count)>
    Private Shared Sub run()
        Dim type As String = Nothing
        type = Convert.ToString(rnd_uint(0, thread_count))
        Dim index As UInt32 = 0
        constructor.register(Function(ByVal v As var) As test_class
                                 Return New test_class(rnd_uint(thread_count, max_uint32))
                             End Function)
        constructor(Of test_class).erase()
        constructor.register(type,
                             Function(ByVal v As var) As test_class
                                 Return New test_class(rnd_uint(thread_count, max_uint32))
                             End Function)
        constructor(Of test_class).erase(type)
        If repeat_case_wrapper.current_round() = repeat_count - 1 Then
            For i As UInt32 = 0 To thread_count - uint32_1
                Dim thread_id As UInt32 = 0
                thread_id = i
                type = Convert.ToString(thread_id)
                constructor.register(Function(ByVal v As var) As test_class
                                         Return New test_class(thread_id)
                                     End Function)
                constructor.register(type,
                                 Function(ByVal v As var) As test_class
                                     Return New test_class(thread_id)
                                 End Function)
            Next
        End If
    End Sub

    <finish>
    Private Shared Sub verify()
        Dim r As test_class = Nothing
        assert_true(constructor.sync_resolve(New var(), r))
        assert_not_nothing(r)
        assert_more_or_equal_and_less(r.i, uint32_0, thread_count)

        For i As UInt32 = 0 To thread_count - uint32_1
            r = Nothing
            assert_true(constructor.sync_resolve(New var(strcat("--type=", i)), r))
            assert_not_nothing(r)
            assert_equal(r.i, i)
        Next
    End Sub
End Class
