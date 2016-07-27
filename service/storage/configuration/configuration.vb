
Public Class configuration
    Private Shared ReadOnly pdc As preserved_disk_capacity

    Public Shared Property preserved_disk_capacity(ByVal path As String) As UInt64
        Friend Get
            Return pdc(path)
        End Get
        Set(value As UInt64)
            pdc(path) = value
        End Set
    End Property

    Shared Sub New()
        pdc = New preserved_disk_capacity()
    End Sub
End Class
