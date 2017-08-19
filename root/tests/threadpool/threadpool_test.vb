
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.lock

Public Class threadpool_test
    Inherits [case]

    Private Shared ReadOnly size As Int64 = 100000
    Private ReadOnly w As threadpool_case

    Shared Sub New()
        size *= If(isreleasebuild(), 100, 1)
    End Sub

    Public Sub New()
        Dim c As atomic_int = Nothing
        w = New threadpool_case(size,
                                Sub()
                                    c.increment()
                                End Sub,
                                Sub()
                                    c = New atomic_int()
                                End Sub,
                                Sub()
                                    assert_equal(+c, size)
                                End Sub)
    End Sub

    Public Overrides Function run() As Boolean
        Return w.run()
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
