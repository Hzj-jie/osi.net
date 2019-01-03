
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports SIZE_T = osi.root.template._4096

Public Class fixed_queue_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New fixed_queue_case(), If(isdebugbuild(), 1, 8) * 16 * 1024 * 1024)
    End Sub

    Private Class fixed_queue_case
        Inherits random_run_case

        Private Shared ReadOnly size As Int64
        Private q As fixed_queue(Of Int64, SIZE_T)
        Private f As Int64
        Private l As Int64

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
            f = 0
            l = 0
            Return MyBase.finish()
        End Function

        Private Sub clear()
            q.clear()
            f = l
            assertion.is_true(q.empty())
        End Sub

        Private Sub read()
            assert(f <= l)
            If f = l Then
                assertion.is_true(q.empty())
                write()
                assertion.is_false(q.empty())
            End If
            assertion.less(f, l)
            assertion.equal(f, q.pop())
            f += 1
        End Sub

        Private Sub write()
            If l - f = size Then
                assertion.is_true(q.full())
                read()
                assertion.is_false(q.full())
            End If
            q.push(l)
            l += 1
        End Sub
    End Class
End Class
