
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.commander
Imports osi.service.storage.constants.remote

Public Class remote_node
    Implements inode

    Private ReadOnly q As target_questioner
    Private ReadOnly target As String
    Private ReadOnly p As String

    Private Sub New(ByVal target As String, ByVal q As questioner(Of _true), ByVal path As String)
        assert(Not String.IsNullOrEmpty(target))
        Me.target = target
        Me.q = target_questioner.ctor(target, q)
        Me.p = path
    End Sub

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner(Of _true),
                                ByVal path As String,
                                ByRef o As remote_node) As Boolean
        Return Not String.IsNullOrEmpty(target) AndAlso
               q IsNot Nothing AndAlso
               eva(o, New remote_node(target, q, path))
    End Function

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner(Of _true),
                                ByVal path As String) As remote_node
        Dim o As remote_node = Nothing
        assert(ctor(target, q, path, o))
        Return o
    End Function

    Private Function new_command() As command
        Return command.[New]().attach(parameter.path, path())
    End Function

    Private Function create_command(ByVal name As String, ByVal wait_ms As Int64) As command
        Return new_command().attach(action.inode_create) _
                            .attach(parameter.name, name) _
                            .attach(parameter.wait_ms, wait_ms)
    End Function

    Public Function create(ByVal name As String,
                           ByVal o As ref(Of iproperty),
                           Optional ByVal wait_ms As Int64 = npos) As event_comb Implements inode.create
        Return q(create_command(name, wait_ms),
                 Function() As Boolean
                     Return eva(o, remote_property.ctor(target, +q, path(), name))
                 End Function)
    End Function

    Private Function open_command(ByVal name As String) As command
        Return new_command().attach(action.inode_open) _
                            .attach(parameter.name, name)
    End Function

    Public Function open(ByVal name As String,
                         ByVal o As ref(Of iproperty)) As event_comb Implements inode.open
        Return q(open_command(name),
                 Function() As Boolean
                     Return eva(o, remote_property.ctor(target, +q, path(), name))
                 End Function)
    End Function

    Public Function path() As String Implements inode.path
        Return p
    End Function

    Private Function properties_command() As command
        Return new_command().attach(action.inode_properties)
    End Function

    Public Function properties(ByVal r As ref(Of vector(Of String))) As event_comb Implements inode.properties
        Return q(properties_command(),
                 Function(c As command) As Boolean
                     Return c IsNot Nothing AndAlso
                            c.parameter(Of parameter, vector(Of String))(parameter.keys, r)
                 End Function)
    End Function

    Private Function subnodes_command() As command
        Return new_command().attach(action.inode_subnodes)
    End Function

    Public Function subnodes(ByVal r As ref(Of vector(Of String))) As event_comb Implements inode.subnodes
        Return q(subnodes_command(),
                 Function(c As command) As Boolean
                     Return c IsNot Nothing AndAlso
                            c.parameter(Of parameter, vector(Of String))(parameter.keys, r)
                 End Function)
    End Function
End Class
