
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Public NotInheritable Class collection
    Private Shared ReadOnly d As dataprovider_collection
    Private Shared ReadOnly w As unique_weak_map(Of String, idatawatcher)
    Private Shared ReadOnly f As unique_weak_map(Of String, idatafetcher)

    Shared Sub New()
        d = New dataprovider_collection()
        w = New unique_weak_map(Of String, idatawatcher)()
        f = New unique_weak_map(Of String, idatafetcher)()
    End Sub

    Public Shared Function generate(ByVal name As String, ByVal ctor As Func(Of idataprovider)) As idataprovider
        Return d.generate(name, ctor)
    End Function

    Public Shared Function generate(ByVal name As String, ByVal ctor As Func(Of idatawatcher)) As idatawatcher
        Return w.generate(name, ctor)
    End Function

    Public Shared Function generate(ByVal name As String, ByVal ctor As Func(Of idatafetcher)) As idatafetcher
        Return f.generate(name, ctor)
    End Function

    Public Shared Function dataprovider_count() As UInt32
        Return d.size()
    End Function

    Public Shared Sub start_auto_cleanup(ByVal lifetime_ms As Int64)
        d.start_auto_cleanup(lifetime_ms)
    End Sub

    Public Shared Sub stop_auto_cleanup()
        d.stop_auto_cleanup()
    End Sub

    Public Shared Sub waitfor_auto_cleanup_stop()
        d.waitfor_auto_cleanup_stop()
    End Sub

    Private Sub New()
    End Sub
End Class
