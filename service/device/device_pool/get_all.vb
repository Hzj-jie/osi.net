
Imports System.Runtime.CompilerServices
Imports osi.root.lock
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector

Public Module _get_all
    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Action(Of idevice(Of T), idevice_pool(Of T)),
                                          Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Return this.get_all(Sub(c As idevice(Of T))
                                    v(c, this)
                                End Sub,
                                stopping)
        End If
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Action(Of idevice(Of T), idevice_pool(Of T)),
                                          ByVal stopping As ref(Of singleentry)) As Boolean
        Return this.get_all(v, AddressOf stopping.not_null_and_in_use)
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Action(Of idevice(Of T)),
                                          Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        If this Is Nothing OrElse
           v Is Nothing Then
            Return False
        Else
            Dim h As idevice_pool(Of T).new_device_insertedEventHandler =
                Sub(c As idevice(Of T))
                    If stopping.null_or_false() Then
                        While this.[get](c)
                            v(c)
                        End While
                    Else
                        RemoveHandler this.new_device_inserted, h
                    End If
                End Sub
            AddHandler this.closing,
                       Sub()
                           RemoveHandler this.new_device_inserted, h
                       End Sub
            AddHandler this.new_device_inserted, h
            h(Nothing)
            Return True
        End If
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Action(Of idevice(Of T)),
                                          ByVal stopping As ref(Of singleentry)) As Boolean
        Return this.get_all(v, AddressOf stopping.not_null_and_in_use)
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Func(Of idevice(Of T),
                                                             idevice_pool(Of T),
                                                             event_comb),
                                          Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Return this.get_all(Function(c As idevice(Of T)) As event_comb
                                    Return v(c, this)
                                End Function,
                                stopping)
        End If
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Func(Of idevice(Of T),
                                                             idevice_pool(Of T),
                                                             event_comb),
                                          ByVal stopping As ref(Of singleentry)) As Boolean
        Return this.get_all(v, AddressOf stopping.not_null_and_in_use)
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Func(Of idevice(Of T), event_comb),
                                          Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Return this.get_all(Sub(c As idevice(Of T))
                                    Dim ec As event_comb = Nothing
                                    assert_begin(New event_comb(Function() As Boolean
                                                                    ec = v(c)
                                                                    Return waitfor(ec) AndAlso
                                                                           goto_next()
                                                                End Function,
                                                                Function() As Boolean
                                                                    Return this.release(c) AndAlso
                                                                           goto_end()
                                                                End Function))
                                End Sub,
                                stopping)
        End If
    End Function

    <Extension()> Public Function get_all(Of T) _
                                         (ByVal this As idevice_pool(Of T),
                                          ByVal v As Func(Of idevice(Of T), event_comb),
                                          ByVal stopping As ref(Of singleentry)) As Boolean
        Return this.get_all(v, AddressOf stopping.not_null_and_in_use)
    End Function
End Module
