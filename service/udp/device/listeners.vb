
Imports osi.root.template

Public NotInheritable Class listeners
    Inherits _46_collection(Of listener, _New)

    Public NotInheritable Class _New
        Inherits __do(Of powerpoint, listener, Boolean)

        Public Overrides Function at(ByRef i As powerpoint, ByRef j As listener) As Boolean
            Return listener.[New](i, j)
        End Function
    End Class

    Private Sub New()
        MyBase.New()
    End Sub
End Class
