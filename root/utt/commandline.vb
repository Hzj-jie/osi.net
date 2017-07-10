
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Friend Class commandline
    Private Shared ReadOnly s As [set](Of String)
    Private Shared ReadOnly v As vector(Of String)

    Shared Sub New()
        s = New [set](Of String)()
        v = New vector(Of String)()
    End Sub

    Public Shared Sub initialize(ByVal args() As String)
        For i As Int32 = 0 To array_size(args) - 1
            v.emplace_back(args(i))
            assert(s.insert(args(i)).first <> s.end())
        Next
    End Sub

    Public Shared Function args() As vector(Of String)
        Return v
    End Function

    Public Shared Function has_specific_selections() As Boolean
        assert(v.empty() = s.empty())
        Return Not v.empty()
    End Function

    Private Shared Function specific(ByVal c As String) As Boolean
        Return s.find(c) <> s.end()
    End Function

    Public Shared Function specific(ByVal c As [case]) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return specific(c.full_name) OrElse
                   specific(c.assembly_qualified_name) OrElse
                   specific(c.name)
        End If
    End Function
End Class
