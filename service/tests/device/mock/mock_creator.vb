
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.device

Public MustInherit Class mock_creator(Of T)
    Private ReadOnly random_fail As Boolean
    Private ReadOnly fake_cpu_ms As Int32

    Public Sub New(ByVal random_fail As Boolean, ByVal fake_cpu_ms As Int32)
        Me.random_fail = random_fail
        Me.fake_cpu_ms = fake_cpu_ms
    End Sub

    Protected MustOverride Function create() As idevice(Of T)

    Protected Function create(ByRef o As idevice(Of T)) As Boolean
        If random_fail AndAlso rnd_bool_trues(3) Then
            Return False
        Else
            fake_processor_work(fake_cpu_ms)
            o = create()
            Return True
        End If
    End Function
End Class
