
Imports osi.root.utt
Imports osi.root.connector

Friend Class threadpool_perf
    Inherits [case]

    Private ReadOnly w As threadpool_case

    Public Sub New(ByVal base_size As Int64, ByVal max_fake_ticks As Int64)
        w = New threadpool_case(base_size * If(isreleasebuild(), 100, 1),
                                Sub()
                                    fake_processor_ticks_work(rnd_int(0, max_fake_ticks))
                                End Sub)
    End Sub

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Return w.run()
    End Function
End Class

Public Class threadpool_perf_10
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(62500, 10))
    End Sub
End Class

Public Class threadpool_perf_100
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(25000, 100))
    End Sub
End Class

Public Class threadpool_perf_1000
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(10000, 1000))
    End Sub
End Class

Public Class threadpool_perf_10000
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(2000, 10000))
    End Sub
End Class

Public Class threadpool_perf_100000
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(400, 100000))
    End Sub
End Class

Public Class threadpool_perf_1000000
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New threadpool_perf(80, 1000000))
    End Sub
End Class
