
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class once_action_test
    Private Const thread_count As UInt32 = 16
    Private Const repeat_size As UInt32 = 1000

    Private ReadOnly vs As vector(Of once_action)
    Private ReadOnly c As atomic_int

    Private Sub New()
        vs = New vector(Of once_action)()
        c = New atomic_int()
    End Sub

    <prepare>
    Private Sub clear()
        c.set(0)
        For i As UInt32 = 0 To repeat_size - uint32_1
            vs.emplace_back(New once_action(Sub()
                                                c.increment()
                                            End Sub))
        Next
    End Sub

    <test>
    <multi_threading(thread_count)>
    <repeat(repeat_size)>
    Private Sub run()
        Dim i As UInt32 = 0
        i = rnd_uint(0, vs.size() - uint32_1)
        If Not vs(i) Then
            assert_false(vs(i).run())
        End If
    End Sub

    <finish>
    Private Sub verify()
        For i As UInt32 = 0 To vs.size() - uint32_1
            vs(i).run()
        Next
        assert_equal(CUInt(+c), vs.size())
    End Sub
End Class
