
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.commander
Imports osi.service.device
Imports osi.service.storage.constants.remote

<global_init(global_init_level.server_services)>
Partial Public Class istrkeyvt_questioner
    Implements istrkeyvt

    Private ReadOnly q As target_questioner

    Private Sub New(ByVal q As target_questioner)
        assert(Not q Is Nothing)
        Me.q = q
    End Sub

    Private Sub New(ByVal name As String, ByVal q As questioner)
        Me.New(target_questioner.ctor(name, q))
    End Sub

    Public Shared Function ctor(ByVal q As target_questioner, ByRef o As istrkeyvt_questioner) As Boolean
        Return Not q Is Nothing AndAlso eva(o, New istrkeyvt_questioner(q))
    End Function

    Public Shared Function ctor(ByVal q As target_questioner, ByRef o As istrkeyvt) As Boolean
        Dim x As istrkeyvt_questioner = Nothing
        Return ctor(q, x) AndAlso eva(o, x)
    End Function

    Public Shared Function ctor(ByVal name As String,
                                ByVal q As questioner,
                                ByRef o As istrkeyvt_questioner) As Boolean
        Return Not String.IsNullOrEmpty(name) AndAlso
               Not q Is Nothing AndAlso
               eva(o, New istrkeyvt_questioner(name, q))
    End Function

    Public Shared Function ctor(ByVal q As target_questioner) As istrkeyvt_questioner
        Dim o As istrkeyvt_questioner = Nothing
        assert(ctor(q, o))
        Return o
    End Function

    Public Shared Function ctor(ByVal name As String, ByVal q As questioner) As istrkeyvt_questioner
        Dim o As istrkeyvt_questioner = Nothing
        assert(ctor(name, q, o))
        Return o
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return q(request(action.istrkeyvt_append, key, value, ts),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return q(request(action.istrkeyvt_capacity),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return q(request(action.istrkeyvt_delete, key),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return q(request(action.istrkeyvt_empty),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return q(request(action.istrkeyvt_full),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return q(request(action.istrkeyvt_heartbeat),
                 Function(c As command) As Boolean
                     Return True
                 End Function)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return q(request(action.istrkeyvt_keycount),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        ' Special handle for list, it should always return a valid vector.
        Return q(request(action.istrkeyvt_list),
                 Function(c As command) As Boolean
                     If c Is Nothing Then
                         Return False
                     End If
                     Return (c.has_parameter(parameter.keys) AndAlso
                             c.parameter(parameter.keys, result)) OrElse
                            eva(result, New vector(Of String)())
                 End Function)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return q(request(action.istrkeyvt_modify, key, value, ts),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return q(request(action.istrkeyvt_read, key),
                 Function(c As command) As Boolean
                     Return Not c Is Nothing AndAlso
                            c.parameter(Of parameter)(parameter.buff, result) AndAlso
                            c.parameter(Of parameter, Int64)(parameter.timestamp, ts)
                 End Function)
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return q(request(action.istrkeyvt_retire),
                 Function() As Boolean
                     Return True
                 End Function)
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return q(request(action.istrkeyvt_seek, key),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return q(request(action.istrkeyvt_sizeof, key),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return q(request(action.istrkeyvt_stop))
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return q(request(action.istrkeyvt_unique_write, key, value, ts),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return q(request(action.istrkeyvt_valuesize),
                 Function(c As command) As Boolean
                     Return response(c, result)
                 End Function)
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As istrkeyvt_questioner) As Boolean
        Return secondary_resolve(v,
                                 Function(i As target_questioner, ByRef r As istrkeyvt_questioner) As Boolean
                                     Return ctor(i, r)
                                 End Function,
                                 o)
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As istrkeyvt) As Boolean
        Return secondary_resolve(v,
                                 Function(i As target_questioner, ByRef r As istrkeyvt) As Boolean
                                     Return ctor(i, r)
                                 End Function,
                                 o)
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of istrkeyvt_questioner)(AddressOf create))
        assert(constructor.register(Of istrkeyvt)("remote", AddressOf istrkeyvt_questioner.create))
    End Sub
End Class
