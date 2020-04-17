
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.dataprovider

Public NotInheritable Class dataprovider_collection_test
    Inherits [case]

    Private Shared ReadOnly lifetime_ms As Int64 = seconds_to_milliseconds(1)

    Private NotInheritable Class fake_datawatcher
        Inherits cd_object(Of fake_datawatcher)
        Implements idatawatcher

        Private Sub New()
            MyBase.New()
        End Sub

        Public Shared Function generate() As idatawatcher
            Return collection.generate("fake-datawatcher://", Function() New fake_datawatcher())
        End Function

        Public Function watch(ByVal exp As expiration_controller) As event_comb Implements idatawatcher.watch
            Dim first As Boolean = True
            Return sync_async(Sub()
                                  If first Then
                                      first = False
                                  Else
                                      assert_waitfor(two_timeslice_length_ms)
                                  End If
                              End Sub)
        End Function
    End Class

    Private NotInheritable Class fake_datafetcher
        Inherits cd_object(Of fake_datafetcher)
        Implements idatafetcher

        Private Sub New()
            MyBase.New()
        End Sub

        Public Shared Function generate() As idatafetcher
            Return collection.generate("fake-datafetcher://", Function() New fake_datafetcher())
        End Function

        Public Function fetch(ByVal localfile As String) As event_comb Implements idatafetcher.fetch
            Return event_comb.succeeded()
        End Function
    End Class

    Private NotInheritable Class fake_dataloader
        Implements idataloader(Of Byte)

        Public Shared ReadOnly instance As fake_dataloader

        Private Sub New()
        End Sub

        Shared Sub New()
            instance = New fake_dataloader()
        End Sub

        Public Function load(ByVal localfile As String, ByVal result As pointer(Of Byte)) As event_comb _
                            Implements idataloader(Of Byte).load
            Return sync_async(Function() eva(result, 0))
        End Function
    End Class

    Private NotInheritable Class fake_dataprovider
        Inherits dataprovider(Of Byte)

        Private Sub New()
            MyBase.New(fake_datawatcher.generate(), fake_datafetcher.generate(), fake_dataloader.instance)
        End Sub

        Public Shared Function generate() As idataprovider
            Return collection.generate("fake-dataprovider://", Function() New fake_dataprovider())
        End Function
    End Class

    Public Overrides Function reserved_processors() As Int16
        Return CShort(Environment.ProcessorCount())
    End Function

    Public Overrides Function run() As Boolean
        Using garbage_collector.force_aggressive_collecting
            collection.start_auto_cleanup(lifetime_ms)
            timeslice_sleep_wait_until(Function() collection.dataprovider_count() = 0, lifetime_ms << 1)
            assertion.equal(collection.dataprovider_count(), uint32_0)

            Dim i As idataprovider = Nothing
            i = fake_dataprovider.generate()
            assertion.equal(collection.dataprovider_count(), uint32_1)
            timeslice_sleep_wait_until(Function() i.valid(), lifetime_ms >> 2)
            assertion.is_true(i.valid())
            assertion.equal(fake_datawatcher.constructed(), uint32_1)
            assertion.equal(fake_datafetcher.constructed(), uint32_1)
            assertion.equal(fake_datawatcher.destructed(), uint32_0)
            assertion.equal(fake_datafetcher.destructed(), uint32_0)
            GC.KeepAlive(i)
            i = Nothing
            sleep(lifetime_ms << 1)
            assertion.equal(collection.dataprovider_count(), uint32_0)
            assertion.equal(fake_datawatcher.constructed(), uint32_1)
            assertion.equal(fake_datafetcher.constructed(), uint32_1)
            expectation.happening(Function() As Boolean
                                      garbage_collector.repeat_collect()
                                      Return fake_datawatcher.destructed() = uint32_1
                                  End Function)
            expectation.happening(Function() As Boolean
                                      garbage_collector.repeat_collect()
                                      Return fake_datafetcher.destructed() = uint32_1
                                  End Function)

            i = fake_dataprovider.generate()
            assertion.equal(collection.dataprovider_count(), uint32_1)
            timeslice_sleep_wait_until(Function() i.valid(), lifetime_ms >> 2)
            assertion.is_true(i.valid())
            expectation.equal(fake_datawatcher.constructed(), uint32_2)
            expectation.equal(fake_datafetcher.constructed(), uint32_2)
            expectation.equal(fake_datawatcher.destructed(), uint32_1)
            expectation.equal(fake_datafetcher.destructed(), uint32_1)
            GC.KeepAlive(i)
            i = Nothing
            sleep(0)
            garbage_collector.repeat_collect()
            assertion.equal(collection.dataprovider_count(), uint32_1)
            expectation.equal(fake_datawatcher.constructed(), uint32_2)
            expectation.equal(fake_datafetcher.constructed(), uint32_2)
            expectation.equal(fake_datawatcher.destructed(), uint32_1)
            expectation.equal(fake_datafetcher.destructed(), uint32_1)

            sleep(lifetime_ms << 1)
            garbage_collector.repeat_collect()
            assertion.equal(collection.dataprovider_count(), uint32_0)
            expectation.equal(fake_datawatcher.constructed(), uint32_2)
            expectation.equal(fake_datafetcher.constructed(), uint32_2)
            expectation.happening(Function() As Boolean
                                      garbage_collector.repeat_collect()
                                      Return fake_datawatcher.destructed() = uint32_2
                                  End Function)
            expectation.happening(Function() As Boolean
                                      garbage_collector.repeat_collect()
                                      Return fake_datafetcher.destructed() = uint32_2
                                  End Function)

            collection.stop_auto_cleanup()
            collection.waitfor_auto_cleanup_stop()
            Return True
        End Using
    End Function
End Class
