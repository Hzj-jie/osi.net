
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Public NotInheritable Class type_alias
    Private ReadOnly m As map(Of String, String)

    Public Sub New()
        m = New map(Of String, String)()
    End Sub

    Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
        Return m.emplace([alias], canonical).second
    End Function

    Public Function canonical_type(ByVal [alias] As String) As String
        Return m.find_or([alias], [alias])
    End Function

    Default Public ReadOnly Property _D(ByVal [alias] As String) As String
        Get
            Return canonical_type([alias])
        End Get
    End Property
End Class
