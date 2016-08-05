
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class bit_array_thread_safe_test
    Inherits multithreading_case_wrapper

    Private Const round As Int32 = 1024 * 1024 * 32

    Public Sub New()
        MyBase.New(repeat(New bit_array_thread_safe_case(Environment.ProcessorCount(), round),
                          round),
                   Environment.ProcessorCount())
    End Sub

    Private Class bit_array_thread_safe_case
        Inherits [case]

        Private ReadOnly write_thread_count As Int32
        Private ReadOnly size As UInt32
        Private ReadOnly b As bit_array_thread_safe
        Private i As Int32

        Public Sub New(ByVal thread_count As Int32, ByVal round As Int32)
            Me.write_thread_count = (thread_count >> 1)
            Me.size = write_thread_count * round
            Me.b = New bit_array_thread_safe()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                b.resize(size)
                i = 0
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < write_thread_count Then
                b(Interlocked.Increment(i) - 1) = True
            ElseIf i > write_thread_count Then
                assert_true(b(rnd_int(0, i - write_thread_count)))
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            For i As Int32 = 0 To b.size() - 1
                assert_true(b(i))
            Next
            b.resize(0)
            Return MyBase.finish()
        End Function
    End Class
End Class
