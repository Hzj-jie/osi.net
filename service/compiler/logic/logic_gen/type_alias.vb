
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Namespace logic
    Public NotInheritable Class type_alias
        Private ReadOnly m As New unordered_map(Of String, String)()

        Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
            If m.emplace([alias], canonical).second Then
                Return True
            End If
            errors.type_alias_redefined([alias], canonical, m([alias]))
            Return False
        End Function

        Default Public ReadOnly Property _D(ByVal [alias] As String) As String
            Get
                Return m.find_or([alias], [alias])
            End Get
        End Property
    End Class
End Namespace
