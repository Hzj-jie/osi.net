
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class configuration
    Private Shared ReadOnly pdc As New reserved_disk_capacity()

    Public Shared Property reserved_disk_capacity(ByVal path As String) As UInt64
        Friend Get
            Return pdc(path)
        End Get
        Set(ByVal value As UInt64)
            pdc(path) = value
        End Set
    End Property
End Class
