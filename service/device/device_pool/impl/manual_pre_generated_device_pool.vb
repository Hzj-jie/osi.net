
Imports osi.root.constants
Imports osi.root.connector

Public NotInheritable Class manual_pre_generated_device_pool
    Public Shared Function [New](Of T)(ByVal i As imanual_device_exporter(Of T),
                                       ByVal max_count As UInt32,
                                       ByVal identity As String) As manual_pre_generated_device_pool(Of T)
        Return New manual_pre_generated_device_pool(Of T)(i, max_count, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As imanual_device_exporter(Of T),
                                       ByVal max_count As UInt32) As manual_pre_generated_device_pool(Of T)
        Return New manual_pre_generated_device_pool(Of T)(i, max_count)
    End Function

    Public Shared Function [New](Of T)(ByVal i As imanual_device_exporter(Of T),
                                       ByVal identity As String) As manual_pre_generated_device_pool(Of T)
        Return New manual_pre_generated_device_pool(Of T)(i, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As imanual_device_exporter(Of T)) As manual_pre_generated_device_pool(Of T)
        Return New manual_pre_generated_device_pool(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class manual_pre_generated_device_pool(Of T)
    Inherits pre_generated_device_pool(Of T)

    Private ReadOnly i As imanual_device_exporter(Of T)

    Public Sub New(ByVal i As imanual_device_exporter(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(i, max_count, identity)
        assert(i IsNot Nothing)
        Me.i = i
    End Sub

    Public Sub New(ByVal i As imanual_device_exporter(Of T), ByVal max_count As UInt32)
        Me.New(i, max_count, Nothing)
    End Sub

    Public Sub New(ByVal i As imanual_device_exporter(Of T), ByVal identity As String)
        Me.New(i, uint32_0, identity)
    End Sub

    Public Sub New(ByVal i As imanual_device_exporter(Of T))
        Me.New(i, uint32_0, Nothing)
    End Sub

    Protected Overrides Function manual_device_exporter() As imanual_device_exporter(Of T)
        Return i
    End Function
End Class
