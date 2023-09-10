
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class hash_distributor
    Implements istrkeyvt

    Private ReadOnly container As istrkeyvt_container

    Public Sub New(ByVal container As istrkeyvt_container)
        assert(Not container Is Nothing)
        Me.container = container
    End Sub

    Public Sub New(ByVal ParamArray names() As String)
        Me.New(istrkeyvt_container.ctor(names))
    End Sub

    Private Function [select](ByVal key As String) As istrkeyvt
        'use another signing index to avoid hurt fces
        Return container(signing(key, 0) Mod container.size())
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return [select](key).append(key, value, ts, result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return sum_capacity(container, result)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return [select](key).delete(key, result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return and_empty(container, result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return or_full(container, result)
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return all_heartbeat(container)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return sum_keycount(container, result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return merge_list(container, result)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return [select](key).modify(key, value, ts, result)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return [select](key).read(key, result, ts)
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return all_retire(container)
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return [select](key).seek(key, result)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return [select](key).sizeof(key, result)
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return event_comb.succeeded()
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return [select](key).unique_write(key, value, ts, result)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return sum_valuesize(container, result)
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As hash_distributor) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Dim c As istrkeyvt_container = Nothing
            If istrkeyvt_container.create(v, c) Then
                o = New hash_distributor(c)
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of istrkeyvt)("hash-distributor", AddressOf create))
    End Sub
End Class
