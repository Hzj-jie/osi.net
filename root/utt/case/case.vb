
Public MustInherit Class [case]
    Public ReadOnly full_name As String
    Public ReadOnly assembly_qualified_name As String
    Public ReadOnly name As String

    Public Sub New()
        full_name = Me.GetType().FullName()
        assembly_qualified_name = Me.GetType().AssemblyQualifiedName()
        name = Me.GetType().Name()
    End Sub

    Public Overridable Function preserved_processors() As Int16
        Return 1
    End Function

    Public MustOverride Function run() As Boolean

    Public Overridable Function prepare() As Boolean
        Return True
    End Function

    Public Overridable Function finish() As Boolean
        Return True
    End Function

    Protected Function commandline_specific() As Boolean
        Return commandline.specific(Me)
    End Function
End Class
