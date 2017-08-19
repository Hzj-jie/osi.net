
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class ref_ptr_test
    Inherits multithreading_case_wrapper

    Private Const repeat_count As Int64 = 32
    Private Const thread_count As Int16 = 64

    Public Sub New()
        MyBase.New(repeat(New ref_ptr_case(), repeat_count), thread_count)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    Private Class ref_ptr_case
        Inherits [case]

        Private i As ref_ptr(Of atomic_int)

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                i = New ref_ptr(Of atomic_int)(New atomic_int(),
                                               disposer:=Sub(x As atomic_int)
                                                             If assert_not_nothing(x) AndAlso
                                                                assert_equal(object_compare(x, i.get()), 0) Then
                                                                 x.increment()
                                                             End If
                                                         End Sub)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If rnd_bool() Then
                sleep(rnd_int(0, 100))
            End If
            i.ref()
            If rnd_bool() Then
                sleep(rnd_int(0, 100))
            End If
            i.unref()
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_equal(i.ref(), uint32_2)
            assert_equal(i.unref(), uint32_1)
            assert_equal(i.unref(), uint32_0)
            assert_equal(++i, 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
