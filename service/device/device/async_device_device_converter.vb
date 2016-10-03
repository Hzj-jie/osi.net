
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.selector

<global_init(global_init_level.services)>
Public NotInheritable Class async_device_device_converter
    Public Shared Function [New](Of T)(ByVal adapter As _do_val_ref(Of async_getter(Of T), T, Boolean)) _
                                      As async_device_device_converter(Of T)
        Return New async_device_device_converter(Of T)(adapter)
    End Function

    Public Shared Function [New](Of T)(ByVal adapter As Func(Of async_getter(Of T), T)) _
                                      As async_device_device_converter(Of T)
        Return New async_device_device_converter(Of T)(adapter)
    End Function

    Public Shared Function register(Of T)(ByVal v As Func(Of async_getter(Of T), T),
                                          Optional ByVal overwrite As Boolean = False) As Boolean
        Return async_device_device_converter(Of T).register(v, overwrite)
    End Function

    Public Shared Function register(Of T)(ByVal v As _do_val_ref(Of async_getter(Of T), T, Boolean),
                                          Optional ByVal overwrite As Boolean = False) As Boolean
        Return async_device_device_converter(Of T).register(v, overwrite)
    End Function

    Public Shared Function adapt(Of T) _
                                (ByVal i As idevice(Of async_getter(Of T)),
                                 ByRef o As idevice(Of T),
                                 Optional ByVal adapter As _do_val_ref(Of async_getter(Of T), T, Boolean) = Nothing) _
                                As Boolean
        Return [New](Of T)(adapter).adapt(i, o)
    End Function

    Public Shared Function adapt(Of T) _
                                (ByVal i As idevice(Of async_getter(Of T)),
                                 ByRef o As idevice(Of T),
                                 ByVal adapter As Func(Of async_getter(Of T), T)) As Boolean
        Return [New](Of T)(adapter).adapt(i, o)
    End Function

    Shared Sub New()
        assert(register(Function(i As async_getter(Of block)) As block
                            Return async_getter_block.[New](i)
                        End Function))
        assert(register(Function(i As async_getter(Of datagram)) As datagram
                            Return async_getter_datagram.[New](i)
                        End Function))
        assert(register(Function(i As async_getter(Of flow)) As flow
                            Return async_getter_flow.[New](i)
                        End Function))
        assert(register(Function(i As async_getter(Of piece_dev)) As piece_dev
                            Return async_getter_piece_dev.[New](i)
                        End Function))
        assert(register(Function(i As async_getter(Of stream_text)) As stream_text
                            Return async_getter_stream_text.[New](i)
                        End Function))
        assert(register(Function(i As async_getter(Of text)) As text
                            Return async_getter_text.[New](i)
                        End Function))
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class

Public Class async_device_device_converter(Of T)
    Private ReadOnly c As _do_val_ref(Of async_getter(Of T), T, Boolean)

    Private Shared Function convert(ByVal i As Func(Of async_getter(Of T), T)) _
                                   As _do_val_ref(Of async_getter(Of T), T, Boolean)
        If i Is Nothing Then
            Return Nothing
        Else
            Return Function(a As async_getter(Of T), ByRef o As T) As Boolean
                       o = i(a)
                       Return True
                   End Function
        End If
    End Function

    Public Shared Function register(ByVal v As Func(Of async_getter(Of T), T),
                                    Optional ByVal overwrite As Boolean = False) As Boolean
        Return register(convert(v), overwrite)
    End Function

    Public Shared Function register(ByVal v As _do_val_ref(Of async_getter(Of T), T, Boolean),
                                    Optional ByVal overwrite As Boolean = False) As Boolean
        If v Is Nothing Then
            Return False
        ElseIf Not overwrite AndAlso
              binder(Of _do_val_ref(Of async_getter(Of T), T, Boolean), 
                        async_device_device_converter).has_global_value() Then
            Return False
        Else
            binder(Of _do_val_ref(Of async_getter(Of T), T, Boolean), async_device_device_converter).set_global(v)
            Return True
        End If
    End Function

    Public Shared Function unregister() As Boolean
        If binder(Of _do_val_ref(Of async_getter(Of T), T, Boolean), 
                     async_device_device_converter).has_global_value() Then
            binder(Of _do_val_ref(Of async_getter(Of T), T, Boolean), 
                      async_device_device_converter).set_global(Nothing)
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub New(Optional ByVal converter As _do_val_ref(Of async_getter(Of T), T, Boolean) = Nothing)
        If converter Is Nothing Then
            Me.c = binder(Of _do_val_ref(Of async_getter(Of T), T, Boolean), 
                             async_device_device_converter).global()
        Else
            Me.c = converter
        End If
    End Sub

    Public Sub New(ByVal converter As Func(Of async_getter(Of T), T))
        Me.New(convert(converter))
    End Sub

    Public Function adapt(ByVal d As idevice(Of async_getter(Of T)), ByRef o As idevice(Of T)) As Boolean
        assert(Not d Is Nothing)
        Dim ad As async_getter_device(Of T) = Nothing
        ad = TryCast(d, async_getter_device(Of T))
        If ad Is Nothing OrElse Not ad.get().initialized() Then
            Dim ag As async_getter(Of T) = Nothing
            ag = d.get()
            If ag Is Nothing Then
                Return False
            Else
                Dim v As T = Nothing
                assert(Not c Is Nothing)
                If c(ag, v) Then
                    o = device_adapter.[New](d, v)
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            ' The initialization may fail, so we still cannot get the device.
            If Not ad.to_device(o) Then
                o = device(Of T).empty()
            End If
            Return True
        End If
    End Function
End Class
