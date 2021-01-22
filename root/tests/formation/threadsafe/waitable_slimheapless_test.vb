
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with waitable_slimheapless_test.vbp ----------
'so change waitable_slimheapless_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with waitable_slimqless2_test.vbp ----------
'so change waitable_slimqless2_test.vbp instead of this file


Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class waitable_slimheapless_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(New waitable_slimheapless_case(), waitable_slimheapless_case.push_threads << 1)
    End Sub

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
    Private NotInheritable Class waitable_slimheapless_case
        Inherits [case]

        Public Const push_threads As Int32 = 2
        Private Const test_size As Int32 = 1000000
        Private ReadOnly q As waitable_slimheapless(Of Int32)
        Private ReadOnly passed As atomic_int
        Private ReadOnly finished As atomic_int

        Public Sub New()
            q = New waitable_slimheapless(Of Int32)()
            passed = New atomic_int()
            finished = New atomic_int()
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            q.clear()
            passed.set(0)
            finished.set(0)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() < push_threads Then
                For i As Int32 = 0 To test_size - 1
                    Dim c As Int32 = 0
                    c = +passed
                    q.emplace(i)
                    assertion.buzy_happening(Function() As Boolean
                                                 Return +passed > c
                                             End Function)
                Next
                finished.increment()
                Return True
            End If

            While +finished < push_threads
                assertion.buzy_happening(Function() As Boolean
                                             Return q.wait(seconds_to_milliseconds(1)) OrElse
                                                    +finished = push_threads
                                         End Function)
                If +finished = push_threads Then
                    While q.pop(Nothing)
                        passed.increment()
                    End While
                Else
                    passed.increment()
                    assertion.is_true(q.pop(Nothing))
                End If
            End While
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(+passed, test_size * push_threads)
            q.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
'finish waitable_slimqless2_test.vbp --------
'finish waitable_slimheapless_test.vbp --------
