
Imports osi.root.constants
Imports osi.root.connector

Public NotInheritable Class auto_pre_generated_device_pool
    Private Sub New()
    End Sub

    Public Shared Function [New](Of T)(ByVal e As iauto_device_exporter(Of T),
                                       ByVal max_count As UInt32,
                                       ByVal identity As String,
                                       ByRef o As auto_pre_generated_device_pool(Of T)) As Boolean
        If max_count = uint32_0 Then
            Return False
        Else
            o = New auto_pre_generated_device_pool(Of T)(e, max_count, identity)
            Return True
        End If
    End Function

    Public Shared Function [New](Of T)(ByVal e As iauto_device_exporter(Of T),
                                       ByVal max_count As UInt32,
                                       ByVal identity As String) As auto_pre_generated_device_pool(Of T)
        Dim o As auto_pre_generated_device_pool(Of T) = Nothing
        assert([New](e, max_count, identity, o))
        Return o
    End Function

    Public Shared Function [New](Of T)(ByVal e As iauto_device_exporter(Of T),
                                       ByVal max_count As UInt32,
                                       ByRef o As auto_pre_generated_device_pool(Of T)) As Boolean
        Return [New](e, max_count, Nothing, o)
    End Function

    Public Shared Function [New](Of T)(ByVal e As iauto_device_exporter(Of T),
                                       ByVal max_count As UInt32) As auto_pre_generated_device_pool(Of T)
        Return [New](e, max_count, "")
    End Function
End Class

Public NotInheritable Class auto_pre_generated_device_pool(Of T)
    Inherits pre_generated_device_pool(Of T)

    Private ReadOnly e As iauto_device_exporter(Of T)

    Public Sub New(ByVal e As iauto_device_exporter(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(e, max_count, identity)
        assert(e.starting() OrElse e.started())
        Me.e = e
        assert(limited_max_count())
        AddHandler device_removed, Sub()
                                       e.require()
                                   End Sub
        e.require(max_count)
    End Sub

    Public Sub New(ByVal e As iauto_device_exporter(Of T), ByVal max_count As UInt32)
        Me.New(e, max_count, Nothing)
    End Sub

    Protected Overrides Function auto_device_exporter() As iauto_device_exporter(Of T)
        Return e
    End Function

    Public Function stopped() As Boolean
        Return e.stopped()
    End Function
End Class
