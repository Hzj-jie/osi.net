
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.dataprovider.constants.compare_datawatcher
Imports osi.service.dataprovider.constants.trigger_datawatcher

Public Class compare_datawatcher
    Inherits trigger_datawatcher

    Private ReadOnly fields() As Int64

    Public Sub New(ByVal field_count As Int32, Optional ByVal interval_ms As Int64 = default_interval_ms)
        MyBase.new(interval_ms)
        assert(field_count <= max_field_count)
        ReDim fields(field_count - 1)
    End Sub

    Public Sub New(Optional ByVal interval_ms As Int64 = default_interval_ms)
        Me.New(default_field_count, interval_ms)
    End Sub

    Public Sub field(ByVal i As Int32, ByVal v As Int64)
        assert(i >= 0 AndAlso i < array_size(fields))
        If fields(i) <> v Then
            fields(i) = v
            trigger()
        End If
    End Sub
End Class
