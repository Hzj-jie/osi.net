
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.commander
Imports osi.service.storage.constants.remote

Public Class remote_fs
    Implements ifs

    Private ReadOnly q As target_questioner
    Private ReadOnly target As String

    Shared Sub New()
        bytes_serializer(Of action).forward_registration.from(Of SByte)()
        bytes_serializer(Of parameter).forward_registration.from(Of SByte)()
    End Sub

    Private Sub New(ByVal target As String, ByVal q As questioner)
        assert(Not String.IsNullOrEmpty(target))
        Me.target = target
        Me.q = target_questioner.ctor(target, q)
    End Sub

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner,
                                ByRef o As remote_fs) As Boolean
        Return Not String.IsNullOrEmpty(target) AndAlso
               q IsNot Nothing AndAlso
               eva(o, New remote_fs(target, q))
    End Function

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner) As remote_fs
        Dim o As remote_fs = Nothing
        assert(ctor(target, q, o))
        Return o
    End Function

    Private Function create_command(ByVal path As String, ByVal wait_ms As Int64) As command
        Return command.[New]().attach(action.ifs_create) _
                              .attach(parameter.path, path) _
                              .attach(parameter.wait_ms, wait_ms)
    End Function

    Public Function create(ByVal path As String,
                           ByVal r As ref(Of inode),
                           Optional ByVal wait_ms As Int64 = npos) As event_comb Implements ifs.create
        Return q(create_command(path, wait_ms),
                 Function() As Boolean
                     Return eva(r, remote_node.ctor(target, +q, path))
                 End Function)
    End Function

    Private Function exist_command(ByVal path As String) As command
        Return command.[New]().attach(action.ifs_exist) _
                              .attach(parameter.path, path)
    End Function

    Public Function exist(ByVal path As String, ByVal r As ref(Of Boolean)) As event_comb Implements ifs.exist
        Return q(exist_command(path),
                 Function(c As command) As Boolean
                     Return c IsNot Nothing AndAlso
                            c.parameter(Of parameter, Boolean)(parameter.result, r)
                 End Function)
    End Function

    Private Function open_command(ByVal path As String) As command
        Return command.[New]().attach(action.ifs_open) _
                              .attach(parameter.path, path)
    End Function

    Public Function open(ByVal path As String, ByVal o As ref(Of inode)) As event_comb Implements ifs.open
        Return q(open_command(path),
                 Function() As Boolean
                     Return eva(o, remote_node.ctor(target, +q, path))
                 End Function)
    End Function
End Class
