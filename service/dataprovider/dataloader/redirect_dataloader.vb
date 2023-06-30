
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Class redirect_dataloader(Of T)
    Implements idataloader(Of T)

    Private ReadOnly l As idataloader(Of T)
    Private ReadOnly target As String

    Public Sub New(ByVal l As idataloader(Of T), ByVal target As String)
        assert(Not l Is Nothing)
        assert(Not target.null_or_empty())
        Me.l = l
        Me.target = target
    End Sub

    Public Function load(ByVal localfile As String, ByVal result As ref(Of T)) As event_comb _
                        Implements idataloader(Of T).load
        Return l.load(target, result)
    End Function
End Class
