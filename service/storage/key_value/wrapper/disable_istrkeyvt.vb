
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class disable_istrkeyvt
    Implements istrkeyvt

    Private ReadOnly disable_append As Boolean
    Private ReadOnly disable_capacity As Boolean
    Private ReadOnly disable_delete As Boolean
    Private ReadOnly disable_empty As Boolean
    Private ReadOnly disable_full As Boolean
    Private ReadOnly disable_heartbeat As Boolean
    Private ReadOnly disable_keycount As Boolean
    Private ReadOnly disable_list As Boolean
    Private ReadOnly disable_modify As Boolean
    Private ReadOnly disable_read As Boolean
    Private ReadOnly disable_retire As Boolean
    Private ReadOnly disable_seek As Boolean
    Private ReadOnly disable_sizeof As Boolean
    Private ReadOnly disable_stop As Boolean
    Private ReadOnly disable_unique_write As Boolean
    Private ReadOnly disable_valuesize As Boolean
    Private ReadOnly impl As istrkeyvt

    Public Sub New(ByVal v As var, ByVal impl As istrkeyvt)
        assert(Not v Is Nothing)
        assert(Not impl Is Nothing)
        Const append As String = "append"
        Const capacity As String = "capacity"
        Const delete As String = "delete"
        Const empty As String = "empty"
        Const full As String = "full"
        Const heartbeat As String = "heartbeat"
        Const keycount As String = "keycount"
        Const list As String = "list"
        Const modify As String = "modify"
        Const read As String = "read"
        Const retire As String = "retire"
        Const seek As String = "seek"
        Const sizeof As String = "sizeof"
        Const [stop] As String = "stop"
        Const unique_write As String = "unique-write"
        Const valuesize As String = "valuesize"
        v.bind(append,
               capacity,
               delete,
               empty,
               full,
               heartbeat,
               keycount,
               list,
               modify,
               read,
               retire,
               seek,
               sizeof,
               [stop],
               unique_write,
               valuesize)
        disable_append = v.reverse_switch(append)
        disable_capacity = v.reverse_switch(capacity)
        disable_delete = v.reverse_switch(delete)
        disable_empty = v.reverse_switch(empty)
        disable_full = v.reverse_switch(full)
        disable_heartbeat = v.reverse_switch(heartbeat)
        disable_keycount = v.reverse_switch(keycount)
        disable_list = v.reverse_switch(list)
        disable_modify = v.reverse_switch(modify)
        disable_read = v.reverse_switch(read)
        disable_retire = v.reverse_switch(retire)
        disable_seek = v.reverse_switch(seek)
        disable_sizeof = v.reverse_switch(sizeof)
        disable_stop = v.reverse_switch([stop])
        disable_unique_write = v.reverse_switch(unique_write)
        disable_valuesize = v.reverse_switch(valuesize)
        Me.impl = impl
    End Sub

    Public Sub New(ByVal disable_append As Boolean,
                   ByVal disable_capacity As Boolean,
                   ByVal disable_delete As Boolean,
                   ByVal disable_empty As Boolean,
                   ByVal disable_full As Boolean,
                   ByVal disable_heartbeat As Boolean,
                   ByVal disable_keycount As Boolean,
                   ByVal disable_list As Boolean,
                   ByVal disable_modify As Boolean,
                   ByVal disable_read As Boolean,
                   ByVal disable_retire As Boolean,
                   ByVal disable_seek As Boolean,
                   ByVal disable_sizeof As Boolean,
                   ByVal disable_stop As Boolean,
                   ByVal disable_unique_write As Boolean,
                   ByVal disable_valuesize As Boolean,
                   ByVal impl As istrkeyvt)
        assert(Not impl Is Nothing)
        Me.disable_append = disable_append
        Me.disable_capacity = disable_capacity
        Me.disable_delete = disable_delete
        Me.disable_empty = disable_empty
        Me.disable_full = disable_full
        Me.disable_heartbeat = disable_heartbeat
        Me.disable_keycount = disable_keycount
        Me.disable_list = disable_list
        Me.disable_modify = disable_modify
        Me.disable_read = disable_read
        Me.disable_retire = disable_retire
        Me.disable_seek = disable_seek
        Me.disable_sizeof = disable_sizeof
        Me.disable_stop = disable_stop
        Me.disable_unique_write = disable_unique_write
        Me.disable_valuesize = disable_valuesize
        Me.impl = impl
    End Sub

    Private Function work(ByVal d As Boolean,
                          ByVal w As Func(Of event_comb)) As event_comb
        assert(Not w Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If d Then
                                      Return False
                                  Else
                                      ec = w()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function take_effect() As Boolean
        Return disable_append OrElse
               disable_capacity OrElse
               disable_delete OrElse
               disable_empty OrElse
               disable_full OrElse
               disable_heartbeat OrElse
               disable_keycount OrElse
               disable_list OrElse
               disable_modify OrElse
               disable_read OrElse
               disable_retire OrElse
               disable_seek OrElse
               disable_sizeof OrElse
               disable_stop OrElse
               disable_unique_write OrElse
               disable_valuesize
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return work(disable_append,
                    Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return work(disable_capacity,
                    Function() impl.capacity(result))
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return work(disable_delete,
                    Function() impl.delete(key, result))
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return work(disable_empty,
                    Function() impl.empty(result))
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return work(disable_full,
                    Function() impl.full(result))
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return work(disable_heartbeat,
                    Function() impl.heartbeat())
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return work(disable_keycount,
                    Function() impl.keycount(result))
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return work(disable_list,
                    Function() impl.list(result))
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return work(disable_modify,
                    Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As pointer(Of Byte()),
                         ByVal ts As pointer(Of Int64)) As event_comb Implements istrkeyvt.read
        Return work(disable_read,
                    Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return work(disable_retire,
                    Function() impl.retire())
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return work(disable_seek,
                    Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return work(disable_sizeof,
                    Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return work(disable_stop,
                    Function() impl.stop())
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return work(disable_unique_write,
                    Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return work(disable_valuesize,
                    Function() impl.valuesize(result))
    End Function

    Private Shared Sub init()
        assert(wrapper.register("disable",
                                Function(v As var,
                                         i As istrkeyvt,
                                         ByRef o As istrkeyvt) As Boolean
                                    If v Is Nothing Then
                                        Return False
                                    Else
                                        Dim c As disable_istrkeyvt = Nothing
                                        c = New disable_istrkeyvt(v, i)
                                        If c.take_effect() Then
                                            o = c
                                        Else
                                            o = i
                                        End If
                                        Return True
                                    End If
                                End Function))
    End Sub
End Class
