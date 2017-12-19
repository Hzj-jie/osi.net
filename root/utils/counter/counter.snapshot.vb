
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Namespace counter
    Public NotInheritable Class snapshot
        Public ReadOnly name As String
        Public ReadOnly count As constant(Of Int64)
        Public ReadOnly average As constant(Of Int64)
        Public ReadOnly last_average As constant(Of Int64)
        Public ReadOnly rate As constant(Of Int64)
        Public ReadOnly last_rate As constant(Of Int64)

        Public Sub New(ByVal name As String,
                       ByVal count As constant(Of Int64),
                       ByVal average As constant(Of Int64),
                       ByVal last_average As constant(Of Int64),
                       ByVal rate As constant(Of Int64),
                       ByVal last_rate As constant(Of Int64))
            Me.name = name
            Me.count = count
            Me.average = average
            Me.last_average = last_average
            Me.rate = rate
            Me.last_rate = last_rate
        End Sub

        Public Shared Function [New](ByVal index As Int64) As snapshot
            Return _counter_collection.snapshot(index)
        End Function
    End Class
End Namespace
