
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.device

Public Class manual_device_exporter_test
    Inherits multithreading_case_wrapper

    Private Const test_size As Int32 = 1024 * 64
    Private Const thread_count As Int32 = 8
    Private Const device_id_space As Int32 = test_size * thread_count

    Public Sub New()
        MyBase.New(repeat(New run_case(), test_size), thread_count)
    End Sub

    Private Class run_case
        Inherits [case]

        Private ReadOnly m As imanual_device_exporter(Of Int32)
        Private ReadOnly i As atomic_int
        Private ReadOnly c As atomic_int
        Private ReadOnly b As bit_array

        Public Sub New()
            m = New manual_device_exporter(Of Int32)()
            i = New atomic_int()
            c = New atomic_int()
            b = New bit_array()
            AddHandler m.new_device_exported, Sub(d As idevice(Of Int32), ByRef export_result As Boolean)
                                                  If assertion.is_not_null(d) Then
                                                      c.increment()
                                                      assertion.less_or_equal(+c, +i)
                                                      assertion.is_false(b(d.get()))
                                                      b(d.get()) = True
                                                  End If
                                                  export_result = True
                                              End Sub
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                b.resize(device_id_space)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            assertion.is_true(m.inject((i.increment() - 1).make_device()))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(+c, device_id_space)
            assert(+i, device_id_space)
            b.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
