
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.connector

Public Class cast_perf
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(performance(multithreading(repeat(New cast_test(), 1024 * If(isdebugbuild(), 1, 8)),
                                              Environment.ProcessorCount() << 1)))
    End Sub

    Public Overrides Function run() As Boolean
        Using regional_atomic_bool(suppress.compare_error)
            Return MyBase.run()
        End Using
    End Function

    'to avoid the impact from suppress.compare_error
    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
