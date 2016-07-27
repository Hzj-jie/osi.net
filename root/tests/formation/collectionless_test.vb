
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class collectionless_test
    Inherits multithreading_case_wrapper

    Private Const size As UInt32 = 1024 * 32
    Private Const thread_count As UInt32 = 64

    Public Sub New()
        MyBase.New(repeat(New collectionless_case(), size), thread_count)
    End Sub

    Private Class collectionless_case
        Inherits [case]

        Private ReadOnly c As collectionless(Of UInt32)

        Public Sub New()
            c = New collectionless(Of UInt32)()
        End Sub

        Public Overrides Function run() As Boolean
            Dim r As UInt32 = 0
            r = rnd_uint()
            Dim p As UInt32 = 0
            p = c.emplace(r)
            assert_false(c.empty())
            assert_equal(c(p), r)
            If rnd_bool() Then
                c.erase(p)
                c.empty()
            Else
                assert_false(c.empty())
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_less_or_equal(c.free_pool_size(), thread_count)
            assert_less_or_equal(c.pool_size(), size * thread_count / 2 * 1.1)
            c.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
