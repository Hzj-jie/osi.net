
Public Class fixed_pool(Of T As Class)
    Inherits pool(Of T)

    Public Sub New()
        MyBase.New(True)
    End Sub

    Public Sub New(ByVal max_size As UInt32)
        MyBase.New(max_size, True)
    End Sub
End Class
