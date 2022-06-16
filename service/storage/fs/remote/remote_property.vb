
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

Public Class remote_property
    Implements iproperty

    Private ReadOnly q As target_questioner
    Private ReadOnly n As String
    Private ReadOnly p As String

    Private Sub New(ByVal target As String,
                    ByVal q As questioner(Of _true),
                    ByVal path As String,
                    ByVal name As String)
        assert(Not target.null_or_empty())
        Me.q = target_questioner.ctor(target, q)
        Me.p = path
        Me.n = name
    End Sub

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner(Of _true),
                                ByVal path As String,
                                ByVal name As String,
                                ByRef o As remote_property) As Boolean
        Return Not target.null_or_empty() AndAlso
               Not q Is Nothing AndAlso
               eva(o, New remote_property(target, q, name, path))
    End Function

    Public Shared Function ctor(ByVal target As String,
                                ByVal q As questioner(Of _true),
                                ByVal path As String,
                                ByVal name As String) As remote_property
        Dim o As remote_property = Nothing
        assert(ctor(target, q, name, path, o))
        Return o
    End Function

    Private Function new_command() As command
        Return command.[New]().attach(parameter.path, path()) _
                              .attach(parameter.name, name())
    End Function

    Private Function append_command(ByVal v() As Byte) As command
        Return new_command.attach(action.iproperty_append) _
                          .attach(parameter.buff, v)
    End Function

    Public Function append(ByVal v() As Byte) As event_comb Implements iproperty.append
        Return q(append_command(v))
    End Function

    Private Function get_command() As command
        Return new_command().attach(action.iproperty_get)
    End Function

    Public Function [get](ByVal i As ref(Of Byte())) As event_comb Implements iproperty.get
        Return q(get_command(),
                 Function(c As command) As Boolean
                     Return Not c Is Nothing AndAlso
                            c.parameter(Of parameter)(parameter.buff, i)
                 End Function)
    End Function

    Private Function lock_command(ByVal wait_ms As Int64) As command
        Return new_command().attach(action.iproperty_lock) _
                            .attach(parameter.wait_ms, wait_ms)
    End Function

    Public Function lock(Optional ByVal wait_ms As Int64 = npos) As event_comb Implements iproperty.lock
        Return q(lock_command(wait_ms))
    End Function

    Public Function name() As String Implements iproperty.name
        Return n
    End Function

    Public Function path() As String Implements iproperty.path
        Return p
    End Function

    Private Function release_command() As command
        Return new_command().attach(action.iproperty_release)
    End Function

    Public Function release() As event_comb Implements iproperty.release
        Return q(release_command())
    End Function

    Private Function set_command(ByVal v() As Byte) As command
        Return new_command().attach(action.iproperty_set) _
                            .attach(parameter.buff, v)
    End Function

    Public Function [set](ByVal v() As Byte) As event_comb Implements iproperty.set
        Return q(set_command(v))
    End Function
End Class
