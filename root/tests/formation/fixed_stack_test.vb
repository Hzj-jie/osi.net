
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports SIZE_T = osi.root.template._4096

Public Class fixed_stack_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New fixed_stack_case(), If(isdebugbuild(), 1, 8) * 16 * 1024 * 1024)
    End Sub

    Private Class fixed_stack_case
        Inherits random_run_case

        Private Shared ReadOnly size As Int64
        Private q As fixed_stack(Of Int64, SIZE_T)
        Private c As Int64

        Shared Sub New()
            size = +(alloc(Of SIZE_T)())
        End Sub

        Public Sub New()
            insert_call(0.4, AddressOf read)
            insert_call(0.59, AddressOf write)
            insert_call(0.01, AddressOf clear)
        End Sub

        Public Overrides Function finish() As Boolean
            q.clear()
            c = 0
            Return MyBase.finish()
        End Function

        Private Sub clear()
            q.clear()
            c = 0
            assertion.is_true(q.empty())
        End Sub

        Private Sub read()
            If c = 0 Then
                assertion.is_true(q.empty())
                write()
                assertion.is_false(q.empty())
            End If
            assertion.more(c, 0)
            assertion.equal(c, q.back() + 1)
            q.pop()
            c -= 1
        End Sub

        Private Sub write()
            If c = size Then
                assertion.is_true(q.full())
                read()
                assertion.is_false(q.full())
            End If
            q.push(c)
            c += 1
        End Sub
    End Class
End Class
