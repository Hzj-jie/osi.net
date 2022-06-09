
' TODO: Move to service.devicepool

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports counter = osi.root.utils.counter

Public Class device_pool
    Implements idevice_pool
    Private ReadOnly mc As UInt32
    Private ReadOnly id As String
    Private ReadOnly count As atomic_int32
    Private ReadOnly TOTAL_COUNT_COUNTER As Int64
    Private ReadOnly FREE_COUNT_COUNTER As Int64
    Private exp As singleentry

    Private Sub New(ByVal max_count As UInt32, ByVal identity As String, ByVal register_counter As Boolean)
        assert(Not identity.null_or_empty())
        Me.mc = max_count
        Me.count = New atomic_int32()
        Me.id = identity
        strtoupper(identity)
        If register_counter Then
            Me.TOTAL_COUNT_COUNTER = counter.register_average_and_last_average(strcat(identity, "_DEVICE_TOTAL_COUNT"),
                                                                               last_average_length:=256,
                                                                               sample_rate:=1)
            Me.FREE_COUNT_COUNTER = counter.register_average_and_last_average(strcat(identity, "_DEVICE_FREE_COUNT"),
                                                                              last_average_length:=256,
                                                                              sample_rate:=1)
        End If
        assert(exp.not_in_use())
    End Sub

    Protected Sub New(ByVal max_count As UInt32, ByVal identity As String)
        Me.New(max_count, identity, True)
    End Sub

    Public Shared Function new_for_test() As device_pool
        Return New device_pool(rnd_uint(), guid_str(), False)
    End Function

    Private Function assert_decrease_total_count() As UInt32
        Dim r As Int32 = 0
        r = count.decrement()
        assert(r >= 0)
        Return r
    End Function

    Protected Function increase_total_count() As Boolean
        If limited_max_count() Then
            Dim r As Int32 = 0
            r = count.increment()
            If r > mc Then
                assert_decrease_total_count()
                Return False
            Else
                counter.increase(TOTAL_COUNT_COUNTER, r)
                Return True
            End If
        Else
            counter.increase(TOTAL_COUNT_COUNTER, count.increment())
            Return True
        End If
    End Function

    Protected Sub zero_total_count()
        count.set(0)
        counter.increase(TOTAL_COUNT_COUNTER, 0)
    End Sub

    Protected Sub decrease_total_count()
        Dim r As Int32 = 0
        r = assert_decrease_total_count()
        counter.increase(TOTAL_COUNT_COUNTER, r)
    End Sub

    Public NotOverridable Overrides Function ToString() As String
        Return strcat(identity(), "[", free_count(), " - ", total_count(), "]")
    End Function

    Protected Overrides Sub Finalize()
        close()
        MyBase.Finalize()
    End Sub
End Class
