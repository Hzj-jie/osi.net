
Imports osi.root.template

Public NotInheritable Class default_new(Of T)
    Inherits _new(Of T)

    Protected Overrides Function at() As T
        Return alloc(Of T)()
    End Function
End Class

Public NotInheritable Class newable_new(Of T As New)
    Inherits _new(Of T)

    Protected Overrides Function at() As T
        Return New T()
    End Function
End Class