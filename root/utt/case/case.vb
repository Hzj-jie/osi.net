
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

Public MustInherit Class [case]
    Public ReadOnly full_name As String
    Public ReadOnly assembly_qualified_name As String
    Public ReadOnly name As String

    Public Sub New()
        full_name = Me.GetType().FullName()
        assembly_qualified_name = Me.GetType().AssemblyQualifiedName()
        name = Me.GetType().Name()
    End Sub

    Protected Sub New(ByVal full_name As String,
                      ByVal assembly_qualified_name As String,
                      ByVal name As String)
        assert(Not full_name.null_or_empty())
        assert(Not assembly_qualified_name.null_or_empty())
        assert(Not name.null_or_empty())
        Me.full_name = full_name
        Me.assembly_qualified_name = assembly_qualified_name
        Me.name = name
    End Sub

    Public Overridable Function reserved_processors() As Int16
        Return 1
    End Function

    Public MustOverride Function run() As Boolean

    Public Overridable Function prepare() As Boolean
        Return True
    End Function

    Public Overridable Function finish() As Boolean
        Return True
    End Function

    Protected Function commandline_specified() As Boolean
        Return commandline.specified(Me)
    End Function

    Public Shared Function exceed_memory_limit(ByVal expected_memory_usage As Int64) As Boolean
        Return max_physical_memory_usage < expected_memory_usage OrElse
               max_virtual_memory_usage < expected_memory_usage
    End Function
End Class
