
Imports osi.service.transmitter

Public Class flow_herald_adapter
    Public Shared Function [New](ByVal i As flow) As herald
        Return New block_herald_adapter(New flow_block_adapter(i))
    End Function

    Private Sub New()
    End Sub
End Class
