
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector

Public Module collection
    Private ReadOnly d As dataprovider_collection
    Private ReadOnly w As unique_weak_map(Of String, idatawatcher)
    Private ReadOnly f As unique_weak_map(Of String, idatafetcher)

    Sub New()
        d = New dataprovider_collection()
        w = New unique_weak_map(Of String, idatawatcher)()
        f = New unique_weak_map(Of String, idatafetcher)()
    End Sub

    Public Function generate(ByVal name As String, ByVal ctor As Func(Of idataprovider)) As idataprovider
        Return d.generate(name, ctor)
    End Function

    Public Function generate(ByVal name As String, ByVal ctor As Func(Of idatawatcher)) As idatawatcher
        Return w.generate(name, ctor)
    End Function

    Public Function generate(ByVal name As String, ByVal ctor As Func(Of idatafetcher)) As idatafetcher
        Return f.generate(name, ctor)
    End Function

    Public Function dataprovider_count() As UInt32
        Return d.size()
    End Function

    Public Sub start_auto_cleanup(ByVal lifetime_ms As Int64)
        d.start_auto_cleanup(lifetime_ms)
    End Sub

    Public Sub stop_auto_cleanup()
        d.stop_auto_cleanup()
    End Sub

    Public Sub waitfor_auto_cleanup_stop()
        d.waitfor_auto_cleanup_stop()
    End Sub
End Module
