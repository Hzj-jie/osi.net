
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class bit_array_thread_safe_test
    Inherits multithreading_case_wrapper

    Private Const round As Int32 = 1024 * 1024 * 32

    Public Sub New()
        MyBase.New(repeat(New bit_array_thread_safe_case(max(Environment.ProcessorCount(), 4), round),
                          round),
                   max(Environment.ProcessorCount(), 4))
    End Sub

    Private Class bit_array_thread_safe_case
        Inherits [case]

        Private ReadOnly write_thread_count As Int32
        Private ReadOnly size As UInt32
        Private ReadOnly b As bit_array_thread_safe
        Private i As Int32
        Private all_true_before As Int32

        Public Sub New(ByVal thread_count As Int32, ByVal round As Int32)
            Me.write_thread_count = thread_count - 2
            Me.size = write_thread_count * round
            Me.b = New bit_array_thread_safe()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                b.resize(size)
                i = 0
                all_true_before = 0
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < write_thread_count Then
                b(Interlocked.Increment(i) - 1) = True
            ElseIf i > write_thread_count Then
                Dim current As Int32 = 0
                current = i
                Dim fc As Int32 = 0
                For j As Int32 = all_true_before To current - 1
                    If Not b(j) Then
                        fc += 1
                    End If
                Next
                assert_less_or_equal(fc, write_thread_count)
                If fc = 0 AndAlso current > all_true_before Then
                    all_true_before = current
                End If
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
