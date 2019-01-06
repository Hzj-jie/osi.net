
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class bit_array_thread_safe_test
    Inherits multithreading_case_wrapper

    Private Const round As Int32 = 1024 * 1024 * 32

    Public Sub New()
        MyBase.New(repeat(New bit_array_thread_safe_case(CUInt(max(Environment.ProcessorCount(), 4)), round),
                          round),
                   max(Environment.ProcessorCount(), 4))
    End Sub

    Private Class bit_array_thread_safe_case
        Inherits [case]

        Private ReadOnly write_thread_count As UInt32
        Private ReadOnly size As UInt32
        Private ReadOnly b As bit_array_thread_safe
        Private i As Int32
        Private all_true_before As Int32

        Public Sub New(ByVal thread_count As UInt32, ByVal round As UInt32)
            Me.write_thread_count = thread_count - uint32_2
            Me.size = write_thread_count * round
            Me.b = New bit_array_thread_safe()
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            b.resize(size)
            i = 0
            all_true_before = 0
            Return True
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < write_thread_count Then
                b(CUInt(Interlocked.Increment(i) - 1)) = True
                Return True
            End If
            If i <= write_thread_count Then
                Return True
            End If
            Dim current As Int32 = 0
            current = i
            Dim fc As Int32 = 0
            For j As Int32 = all_true_before To current - 1
                If Not b(CUInt(j)) Then
                    fc += 1
                End If
            Next
            assertion.less_or_equal(CUInt(fc), write_thread_count)
            If fc = 0 AndAlso current > all_true_before Then
                all_true_before = current
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            For i As UInt32 = 0 To b.size() - uint32_1
                assertion.is_true(b(i))
            Next
            b.resize(0)
            Return MyBase.finish()
        End Function
    End Class
End Class
