
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

<global_init(global_init_level.other)>
Friend NotInheritable Class commandline
    Private Shared ReadOnly s As New [set](Of String)()
    Private Shared others As argument(Of vector(Of String))

    Private Shared Sub init()
        With (+others)
            .stream().foreach(Sub(ByVal arg As String)
                                  assert(s.insert(arg).first <> s.end())
                              End Sub)
        End With
    End Sub

    Public Shared Function args() As vector(Of String)
        Return +others
    End Function

    Public Shared Function has_specified_selections() As Boolean
        assert((+others).empty() = s.empty())
        Return Not (+others).empty()
    End Function

    Private Shared Function specified(ByVal c As String) As Boolean
        Return s.find(c) <> s.end()
    End Function

    Public Shared Function specified(ByVal c As [case]) As Boolean
        If c Is Nothing Then
            Return False
        End If
        Return specified(c.full_name) OrElse
               specified(c.assembly_qualified_name) OrElse
               specified(c.name)
    End Function

    Private Sub New()
    End Sub
End Class
