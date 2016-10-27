
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.selector
Imports osi.service.transmitter

<global_init(global_init_level.services)>
Public NotInheritable Class async_device_device_converter
    Public Shared Function [New](Of T)(ByVal adapter As Func(Of async_getter(Of T), T)) _
                                      As async_device_device_converter(Of T)
        Return New async_device_device_converter(Of T)(adapter)
    End Function

    Public Shared Function register(Of T)(ByVal v As Func(Of async_getter(Of T), T),
                                          Optional ByVal overwrite As Boolean = False) As Boolean
        Return async_device_device_converter(Of T).register(v, overwrite)
    End Function

    Public Shared Function adapt(Of T) _
                                (ByVal i As idevice(Of async_getter(Of T)),
                                 ByRef o As idevice(Of T),
                                 Optional ByVal adapter As Func(Of async_getter(Of T), T) = Nothing) As Boolean
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

' If a conversion failed, this class should return an invalid device.
Public Class async_device_device_converter(Of T)
    Private ReadOnly c As Func(Of async_getter(Of T), T)

    Private Shared Sub set_global(ByVal v As Func(Of async_getter(Of T), T))
        binder(Of Func(Of async_getter(Of T), T), async_device_device_converter).set_global(v)
    End Sub

    Public Shared Function registered() As Boolean
        Return binder(Of Func(Of async_getter(Of T), T), async_device_device_converter).has_global_value()
    End Function

    Public Shared Function [default]() As Func(Of async_getter(Of T), T)
        Return binder(Of Func(Of async_getter(Of T), T), async_device_device_converter).global()
    End Function

    Public Shared Function register(ByVal v As Func(Of async_getter(Of T), T),
                                    Optional ByVal overwrite As Boolean = False) As Boolean
        If v Is Nothing Then
            Return False
        ElseIf Not overwrite AndAlso registered() Then
            ' TODO: Is the PROTECTOR (async_device_device_converter) really useful?
            Return False
        Else
            set_global(v)
            Return True
        End If
    End Function

    Public Shared Function unregister() As Boolean
        If registered() Then
            set_global(Nothing)
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub New(Optional ByVal converter As Func(Of async_getter(Of T), T) = Nothing)
        If converter Is Nothing Then
            Me.c = binder(Of Func(Of async_getter(Of T), T), async_device_device_converter).global()
        Else
            Me.c = converter
        End If
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
                assert(Not c Is Nothing)
                o = device_adapter.[New](d, c(ag))
                Return True
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
