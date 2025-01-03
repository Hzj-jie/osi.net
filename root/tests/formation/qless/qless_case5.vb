
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_case5.vbp ----------
'so change qless_case5.vbp instead of this file



Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Friend Class qless_case5
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(New case5(max(Environment.ProcessorCount(), 32)),
                   max(Environment.ProcessorCount(), 32))
    End Sub

    Private Class case5
        Inherits [case]

        Private Const round As UInt32 = 4 * 1024 * 1024
        Private ReadOnly thread_count As Int32
        Private ReadOnly write_thread_count As UInt32
        Private ReadOnly q As qless(Of UInt32)
        Private ReadOnly b As bit_array_thread_safe
        Private i As Int32

        Public Sub New(ByVal thread_count As Int32)
            Me.thread_count = thread_count
            Me.write_thread_count = CUInt(thread_count >> 1)
            assert(Me.write_thread_count > 1)
            Me.q = New qless(Of UInt32)()
            Me.b = New bit_array_thread_safe()
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            i = 0
            b.resize(write_thread_count * round)
            Return True
        End Function

        Private Sub write()
#If False Then
            If Not q.push(CUInt(Interlocked.Increment(i) - 1)) Then
                Interlocked.Decrement(i)
            End If
#Else
            q.push(CUInt(Interlocked.Increment(i) - 1))
#End If
        End Sub

        Private Sub read()
            Dim v As UInt32 = 0
            If q.pop(v) Then
                b(v) = True
            Else
                yield()
            End If
        End Sub

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < write_thread_count Then
                For i As Int32 = 0 To round - 1
                    write()
                Next
            Else
                ' Write threads are still running
                While multithreading_case_wrapper.running_thread_count() > thread_count - write_thread_count
                    read()
                End While
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
#If False Then
            While i < b.size()
                write()
            End While
#End If
            While Not q.empty()
                read()
            End While
            For j As UInt32 = 0 To b.size() - uint32_1
                assertion.is_true(b(j))
            Next
            Return MyBase.finish()
        End Function
    End Class
End Class
'finish qless_case5.vbp --------
