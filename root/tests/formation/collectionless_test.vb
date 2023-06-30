
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class collectionless_test
    Inherits multithreading_case_wrapper

    Private Const size As UInt32 = 1024 * 32
    Private Const thread_count As UInt32 = 64

    Public Sub New()
        MyBase.New(repeat(New collectionless_case(), size), thread_count)
    End Sub

    Private NotInheritable Class collectionless_case
        Inherits [case]

        Private ReadOnly c As New collectionless(Of UInt32)()

        Public Overrides Function run() As Boolean
            Dim r As UInt32 = rnd_uint()
            Dim p As UInt32 = c.emplace(r)
            assertion.is_false(c.empty())
            assertion.equal(c(p), r)
            If rnd_bool() Then
                c.erase(p)
                c.empty()
            Else
                assertion.is_false(c.empty())
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.less_or_equal(c.free_pool_size(), thread_count)
            assertion.less_or_equal(c.pool_size(), size * thread_count / 2 * 1.1)
            c.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
