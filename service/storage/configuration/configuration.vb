
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class configuration
    Private Shared ReadOnly pdc As reserved_disk_capacity

    Public Shared Property reserved_disk_capacity(ByVal path As String) As UInt64
        Friend Get
            Return pdc(path)
        End Get
        Set(value As UInt64)
            pdc(path) = value
        End Set
    End Property

    Shared Sub New()
        pdc = New reserved_disk_capacity()
    End Sub
End Class
