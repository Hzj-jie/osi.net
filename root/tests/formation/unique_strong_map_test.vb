
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation

Public Class unique_strong_map_heavy_read_perf_test
    Inherits unique_strong_map_perf_test

    Public Sub New()
        MyBase.New(Function() New unique_strong_map_heavy_read_test())
    End Sub
End Class

Public Class unique_strong_map_perf_test
    Inherits performance_case_wrapper

    Protected Sub New(ByVal n As Func(Of [case]))
        MyBase.New(repeat(n(), 4))
    End Sub

    Public Sub New()
        Me.New(Function() New unique_strong_map_test())
    End Sub
End Class

Public Class unique_strong_map_heavy_read_test
    Inherits unique_strong_map_test

    Public Sub New()
        MyBase.New(0, 0.2, 0.3, 0.2, 0.2, 0.1)
    End Sub
End Class

Public Class unique_strong_map_test
    Inherits multithreading_case_wrapper

    Private Sub New(ByVal n As Func(Of unique_strong_map_case))
        MyBase.New(repeat(n(), 65536 * If(isdebugbuild(), 1, 4)), 4 * If(isdebugbuild(), 1, 2))
    End Sub

    Protected Sub New(ByVal erase_perc As Double,
                      ByVal set_perc As Double,
                      ByVal generate_perc As Double,
                      ByVal get_perc As Double,
                      ByVal exist_perc As Double,
                      ByVal replace_perc As Double)
        Me.New(Function() New unique_strong_map_case(erase_perc,
                                                     set_perc,
                                                     generate_perc,
                                                     get_perc,
                                                     exist_perc,
                                                     replace_perc))
    End Sub

    Public Sub New()
        Me.New(Function() New unique_strong_map_case())
    End Sub

    Private Class unique_strong_map_case
        Inherits random_run_case

        Private Shared Function rnd_key() As Int64
            Return rnd_int64(0, 128)
        End Function

        Private Shared Function value(ByVal key As Int64) As Int64
            Return -key
        End Function

        Private Function [erase]() As Boolean
            m.erase(rnd_key())
            Return True
        End Function

        Private Function [set]() As Boolean
            Dim k As Int64 = 0
            k = rnd_key()
            m.set(k, value(k))
            Return True
        End Function

        Private Function [get]() As Boolean
            Dim k As Int64 = 0
            k = rnd_key()
            Dim v As Int64 = 0
            If m.get(k, v) Then
                assertion.equal(v, value(k))
            End If
            Return True
        End Function

        Private Function replace() As Boolean
            Dim k As Int64 = 0
            k = rnd_key()
            m.replace(k, value(k))
            Return True
        End Function

        Private Function exist() As Boolean
            m.exist(rnd_key())
            Return True
        End Function

        Private Function generate() As Boolean
            Dim k As Int64 = 0
            k = rnd_key()
            Dim v As Int64 = 0
            v = value(k)
            assertion.equal(m.generate(k, Function() As Int64
                                              Return v
                                          End Function), v)
            Return True
        End Function

        Private ReadOnly m As unique_strong_map(Of Int64, Int64)

        Public Sub New(ByVal erase_perc As Double,
                       ByVal set_perc As Double,
                       ByVal generate_perc As Double,
                       ByVal get_perc As Double,
                       ByVal exist_perc As Double,
                       ByVal replace_perc As Double)
            MyBase.New()
            m = New unique_strong_map(Of Int64, Int64)()
            insert_call(erase_perc, AddressOf [erase])
            insert_call(set_perc, AddressOf [set])
            insert_call(generate_perc, AddressOf generate)
            insert_call(get_perc, AddressOf [get])
            insert_call(exist_perc, AddressOf exist)
            insert_call(replace_perc, AddressOf replace)
        End Sub

        Public Sub New()
            Me.New(0.4, 0.2, 0.2, 0.1, 0.05, 0.05)
        End Sub
    End Class
End Class
