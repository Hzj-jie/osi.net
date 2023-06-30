
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.commander
Imports osi.service.storage.constants.remote

Public Class istrkeyvt_dispatcher
    Private ReadOnly dispatcher As target_dispatcher(Of istrkeyvt)

    Public Sub New(ByVal dispatcher As target_dispatcher(Of istrkeyvt))
        assert(Not dispatcher Is Nothing)
        Me.dispatcher = dispatcher
        assert(listen(Me.dispatcher))
    End Sub

    Public Sub New(ByVal dispatcher As dispatcher)
        Me.New(New target_dispatcher(Of istrkeyvt)(dispatcher))
    End Sub

    Public Sub New()
        Me.New(New target_dispatcher(Of istrkeyvt)())
    End Sub

    Public Shared Operator +(ByVal this As istrkeyvt_dispatcher) As target_dispatcher(Of istrkeyvt)
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.dispatcher
        End If
    End Operator

    Public Shared Function listen(ByVal dispatcher As target_dispatcher(Of istrkeyvt)) As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.register(action.istrkeyvt_append, AddressOf append) AndAlso
               dispatcher.register(action.istrkeyvt_capacity, AddressOf capacity) AndAlso
               dispatcher.register(action.istrkeyvt_delete, AddressOf delete) AndAlso
               dispatcher.register(action.istrkeyvt_empty, AddressOf empty) AndAlso
               dispatcher.register(action.istrkeyvt_full, AddressOf full) AndAlso
               dispatcher.register(action.istrkeyvt_heartbeat, AddressOf heartbeat) AndAlso
               dispatcher.register(action.istrkeyvt_keycount, AddressOf keycount) AndAlso
               dispatcher.register(action.istrkeyvt_list, AddressOf list) AndAlso
               dispatcher.register(action.istrkeyvt_modify, AddressOf modify) AndAlso
               dispatcher.register(action.istrkeyvt_read, AddressOf read) AndAlso
               dispatcher.register(action.istrkeyvt_retire, AddressOf retire) AndAlso
               dispatcher.register(action.istrkeyvt_seek, AddressOf seek) AndAlso
               dispatcher.register(action.istrkeyvt_sizeof, AddressOf sizeof) AndAlso
               dispatcher.register(action.istrkeyvt_stop, AddressOf [stop]) AndAlso
               dispatcher.register(action.istrkeyvt_unique_write, AddressOf unique_write) AndAlso
               dispatcher.register(action.istrkeyvt_valuesize, AddressOf valuesize)
    End Function

    Public Shared Function ignore(ByVal dispatcher As target_dispatcher(Of istrkeyvt)) As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.erase(action.istrkeyvt_append) AndAlso
               dispatcher.erase(action.istrkeyvt_capacity) AndAlso
               dispatcher.erase(action.istrkeyvt_delete) AndAlso
               dispatcher.erase(action.istrkeyvt_empty) AndAlso
               dispatcher.erase(action.istrkeyvt_full) AndAlso
               dispatcher.erase(action.istrkeyvt_heartbeat) AndAlso
               dispatcher.erase(action.istrkeyvt_keycount) AndAlso
               dispatcher.erase(action.istrkeyvt_list) AndAlso
               dispatcher.erase(action.istrkeyvt_modify) AndAlso
               dispatcher.erase(action.istrkeyvt_read) AndAlso
               dispatcher.erase(action.istrkeyvt_retire) AndAlso
               dispatcher.erase(action.istrkeyvt_seek) AndAlso
               dispatcher.erase(action.istrkeyvt_sizeof) AndAlso
               dispatcher.erase(action.istrkeyvt_stop) AndAlso
               dispatcher.erase(action.istrkeyvt_unique_write) AndAlso
               dispatcher.erase(action.istrkeyvt_valuesize)
    End Function

    Public Function ignore() As Boolean
        Return ignore(dispatcher)
    End Function

    Private Shared Function read(ByVal strkeyvt As istrkeyvt,
                                 ByVal i As command,
                                 ByVal o As command) As event_comb
        Dim key As String = Nothing
        Dim buff As ref(Of Byte()) = Nothing
        Dim ts As ref(Of Int64) = Nothing
        Return handle(i,
                      o,
                      Function() As Boolean
                          Return i.parameter(parameter.key, key)
                      End Function,
                      Function() As event_comb
                          buff = New ref(Of Byte())()
                          ts = New ref(Of Int64)()
                          Return a_strkeyvt(strkeyvt).read(key, buff, ts)
                      End Function,
                      Function() As Boolean
                          o.attach(parameter.buff, +buff) _
                           .attach(parameter.timestamp, +ts)
                          Return True
                      End Function)
    End Function

    Private Shared Function append(ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Return amuw(i, o, Function(a, b, c, d) a_strkeyvt(strkeyvt).append(a, b, c, d))
    End Function

    Private Shared Function delete(ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Return key_result_handle(i, o, Function(a, b) a_strkeyvt(strkeyvt).delete(a, b))
    End Function

    Private Shared Function seek(ByVal strkeyvt As istrkeyvt,
                                 ByVal i As command,
                                 ByVal o As command) As event_comb
        Return key_result_handle(i, o, Function(a, b) a_strkeyvt(strkeyvt).seek(a, b))
    End Function

    Private Shared Function list(ByVal strkeyvt As istrkeyvt,
                                 ByVal i As command,
                                 ByVal o As command) As event_comb
        Dim r As ref(Of vector(Of String)) = Nothing
        Return handle(i,
                      o,
                      Nothing,
                      Function() As event_comb
                          r = New ref(Of vector(Of String))()
                          Return a_strkeyvt(strkeyvt).list(r)
                      End Function,
                      Function() As Boolean
                          If Not (+r) Is Nothing Then
                              o.attach(parameter.keys, +r)
                          End If
                          Return True
                      End Function)
    End Function

    Private Shared Function modify(ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Return amuw(i, o, Function(a, b, c, d) a_strkeyvt(strkeyvt).modify(a, b, c, d))
    End Function

    Private Shared Function sizeof(ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Dim key As String = Nothing
        Return size_handle(i,
                           o,
                           Function() As Boolean
                               Return i.parameter(parameter.key, key)
                           End Function,
                           Function(result As ref(Of Int64)) As event_comb
                               Return a_strkeyvt(strkeyvt).sizeof(key, result)
                           End Function)
    End Function

    Private Shared Function full(ByVal strkeyvt As istrkeyvt,
                                 ByVal i As command,
                                 ByVal o As command) As event_comb
        Return result_handle(i,
                             o,
                             Nothing,
                             Function(result As ref(Of Boolean)) As event_comb
                                 Return a_strkeyvt(strkeyvt).full(result)
                             End Function)
    End Function

    Private Shared Function empty(ByVal strkeyvt As istrkeyvt,
                                  ByVal i As command,
                                  ByVal o As command) As event_comb
        Return result_handle(i,
                             o,
                             Nothing,
                             Function(result As ref(Of Boolean)) As event_comb
                                 Return a_strkeyvt(strkeyvt).empty(result)
                             End Function)
    End Function

    Private Shared Function retire(ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Return handle(i,
                      o,
                      Nothing,
                      Function() As event_comb
                          Return a_strkeyvt(strkeyvt).retire()
                      End Function,
                      Nothing)
    End Function

    Private Shared Function capacity(ByVal strkeyvt As istrkeyvt,
                                     ByVal i As command,
                                     ByVal o As command) As event_comb
        Return size_handle(i,
                           o,
                           Nothing,
                           Function(result As ref(Of Int64)) As event_comb
                               Return a_strkeyvt(strkeyvt).capacity(result)
                           End Function)
    End Function

    Private Shared Function valuesize(ByVal strkeyvt As istrkeyvt,
                                      ByVal i As command,
                                      ByVal o As command) As event_comb
        Return size_handle(i,
                           o,
                           Nothing,
                           Function(result As ref(Of Int64)) As event_comb
                               Return a_strkeyvt(strkeyvt).valuesize(result)
                           End Function)
    End Function

    Private Shared Function keycount(ByVal strkeyvt As istrkeyvt,
                                     ByVal i As command,
                                     ByVal o As command) As event_comb
        Return size_handle(i,
                           o,
                           Nothing,
                           Function(result As ref(Of Int64)) As event_comb
                               Return a_strkeyvt(strkeyvt).keycount(result)
                           End Function)
    End Function

    Private Shared Function heartbeat(ByVal strkeyvt As istrkeyvt,
                                      ByVal i As command,
                                      ByVal o As command) As event_comb
        Return handle(i,
                      o,
                      Nothing,
                      Function() As event_comb
                          Return a_strkeyvt(strkeyvt).heartbeat()
                      End Function,
                      Nothing)
    End Function

    Private Shared Function [stop](ByVal strkeyvt As istrkeyvt,
                                   ByVal i As command,
                                   ByVal o As command) As event_comb
        Return handle(i,
                      o,
                      Nothing,
                      Function() As event_comb
                          Return a_strkeyvt(strkeyvt).stop()
                      End Function,
                      Nothing)
    End Function

    Private Shared Function unique_write(ByVal strkeyvt As istrkeyvt,
                                         ByVal i As command,
                                         ByVal o As command) As event_comb
        Return amuw(i, o, Function(a, b, c, d) a_strkeyvt(strkeyvt).unique_write(a, b, c, d))
    End Function
End Class
