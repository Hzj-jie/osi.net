
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.constants
Imports osi.service.selector

Public Class selector_test
    Inherits multithreading_case_wrapper

    Private Shadows Const thread_count As Int32 = 1024

    Public Sub New()
        MyBase.New(repeat(New singleton_case(),
                          64 * If(isdebugbuild(), 1, 4)),
                   thread_count * If(isdebugbuild(), 1, 4))
    End Sub

    Private Class singleton_case
        Inherits [case]

        Private Class test_class
            Inherits cd_object(Of test_class)

            Private ReadOnly inited As ref(Of singleentry)
            Private ReadOnly p As Int32

            Public Sub New(ByVal p As Int32)
                MyBase.New()
                Me.p = p
                Me.inited = New ref(Of singleentry)()
            End Sub

            Public Shared Operator +(ByVal i As test_class) As Int32
                If i Is Nothing Then
                    Return 0
                Else
                    Return i.p
                End If
            End Operator

            Public Function initialized() As Boolean
                Return inited.in_use()
            End Function

            Public Shared Function create(ByVal p As Int32, ByRef o As test_class) As Boolean
                o = New test_class(p)
                sleep(rnd_int(0, seconds_to_milliseconds(10)))
                assert(o.inited.mark_in_use())
                Return True
            End Function
        End Class

        Private Const max_key As Int32 = (thread_count >> 1)
        Private Const min_key As Int32 = 0
        Private ReadOnly s As selector(Of test_class, Int32)

        Public Sub New()
            s = make_selector(make_allocator(Of test_class, Int32)(AddressOf test_class.create))
        End Sub

        Public Overrides Function run() As Boolean
            Dim i As Int32 = 0
            Dim p As test_class = Nothing
            i = rnd_int(min_key, max_key)
            assert_true(s.select(i, p))
            If assert_not_nothing(p) Then
                assert_true(p.initialized())
                assert_equal(+p, i)
            End If
            assert_less_or_equal(test_class.constructed(), CUInt(max_key - min_key))
            assert_equal(test_class.destructed(), uint32_0)
            Return True
        End Function
    End Class
End Class
