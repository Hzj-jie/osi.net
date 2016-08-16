
Imports osi.root.template

Public NotInheritable Class speakers
    Inherits _46_collection(Of speaker, _New)

    Public NotInheritable Class _New
        Inherits __do(Of powerpoint, speaker, Boolean)

        Public Overrides Function at(ByRef i As powerpoint, ByRef j As speaker) As Boolean
            Return speaker.[New](i, j)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
