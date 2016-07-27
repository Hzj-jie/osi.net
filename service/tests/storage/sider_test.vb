
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.storage
Imports osi.service.device

'--herald=tcp --host=localhost --port=11000 --target=fces --token=token
'--herald=http --host=localhost --port=11001 --target=fces
Public Class sider_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New sider_case())
    End Sub

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(New sider_case(i))
    End Sub

    Private Class sider_case
        Inherits istrkeyvt_case

        Public Sub New(ByVal i As iistrkeyvt_case)
            MyBase.New(i)
        End Sub

        Public Sub New()
            Me.New(New default_istrkeyvt_case())
        End Sub

        Protected Overrides Function create_istrkeyvt(ByVal p As pointer(Of istrkeyvt)) As event_comb
            Return sync_async(Function() As Boolean
                                  Dim r As istrkeyvt_questioner = Nothing
                                  Dim v As var = Nothing
                                  v = New var(Console.ReadLine())
                                  Return constructor.resolve(v, r) AndAlso
                                         assert_not_nothing(r) AndAlso
                                         eva(p, r) AndAlso
                                         If(strsame(v("herald"), "tcp"), waitfor(milliseconds_to_seconds(5)), True)
                                  ' wait for TCP connections to be generated
                              End Function)
        End Function
    End Class
End Class

Public Class sider_case3_test
    Inherits sider_test

    Public Sub New()
        MyBase.New(New default_istrkeyvt_case3())
    End Sub
End Class

Public Class sider_small_data_perf_test
    Inherits sider_test

    Private Class sider_small_data_perf_case
        Inherits istrkeyvt_perf_case(Of _128, _2, _4, _8, _1024, _NPOS, _false)
    End Class

    Public Sub New()
        MyBase.New(New sider_small_data_perf_case())
    End Sub
End Class

Public Class sider_normal_data_perf_test
    Inherits sider_test

    Private Class sider_normal_data_perf_case
        Inherits istrkeyvt_perf_case(Of _128, _32, _256, _512, _4096, _NPOS, _false)
    End Class

    Public Sub New()
        MyBase.New(New sider_normal_data_perf_case())
    End Sub
End Class

Public Class sider_large_data_perf_test
    Inherits sider_test

    Private Class sider_large_data_perf_case
        Inherits istrkeyvt_perf_case(Of _128, _1024, _4096, _4096, _max_uint16, _NPOS, _false)
    End Class

    Public Sub New()
        MyBase.New(New sider_large_data_perf_case())
    End Sub
End Class
