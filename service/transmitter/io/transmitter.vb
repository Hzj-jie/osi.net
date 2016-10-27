
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _transmitter
    Public Function transmit_mode(Of T)(ByVal x As T) As transmitter.mode_t
        Return transmitter.from_type(Of T)(x).transmit_mode()
    End Function

    Public Function transmit_mode(Of T)() As transmitter.mode_t
        Return transmitter.from_type(Of T)().transmit_mode()
    End Function

    Public Function concurrent_operation(Of T)(ByVal x As T) As Boolean
        Return transmitter.from_type(Of T)(x).concurrent_operation()
    End Function

    Public Function concurrent_operation(Of T)() As Boolean
        Return transmitter.from_type(Of T)().concurrent_operation()
    End Function

    Public Function one_off(Of T)(ByVal x As T) As Boolean
        Return transmitter.from_type(Of T)(x).one_off()
    End Function

    Public Function one_off(Of T)() As Boolean
        Return transmitter.from_type(Of T)().one_off()
    End Function

    Public Function packet_size(Of T)(ByVal x As T) As UInt32
        Return datagram_transmitter.from_type(Of T)(x).packet_size()
    End Function

    Public Function packet_size(Of T)() As UInt32
        Return datagram_transmitter.from_type(Of T)().packet_size()
    End Function
End Module

' TODO: Rename
Public Class transmitter
    Implements ICloneable

    Public Enum mode_t
        send_receive
        receive_send
        duplex
    End Enum

    ' Whether the device is able to send and receive at the same time, without conflict, i.e. several senders and
    ' receivers can send and receive data from this device without any specific handling. This requires not only thread-
    ' safty, but also an inner sequence number or multiple connections. Usually this setting should be false.
    Private concurrent As Boolean
    Private mode As mode_t
    Private oneoff As Boolean

    Protected Sub New()
        concurrent = False
        mode = mode_t.send_receive
        oneoff = False
    End Sub

    Public Shared Function [New]() As transmitter
        Return New transmitter()
    End Function

    Public Function concurrent_operation() As Boolean
        Return concurrent
    End Function

    Public Function with_concurrent_operation(ByVal c As Boolean) As transmitter
        concurrent = c
        Return Me
    End Function

    Public Function transmit_mode() As mode_t
        Return mode
    End Function

    Public Function with_transmit_mode(ByVal mode As mode_t) As transmitter
        Me.mode = mode
        Return Me
    End Function

    Public Function one_off() As Boolean
        Return oneoff
    End Function

    Public Function with_one_off(ByVal c As Boolean) As transmitter
        Me.oneoff = c
        Return Me
    End Function

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        Return transmitter.[New]().
                   with_concurrent_operation(concurrent_operation()).
                   with_transmit_mode(transmit_mode()).
                   with_one_off(one_off())
    End Function

    Public Shared Function from_type(Of T)() As transmitter
        Return type_attribute.of(Of T).get(Of transmitter)()
    End Function

    Public Shared Function from_type(Of T)(ByVal this As T) As transmitter
        Return type_attribute.of(this).get(Of transmitter)()
    End Function
End Class

Public Class datagram_transmitter
    Inherits transmitter

    Private pack_size As UInt32

    Protected Sub New()
        MyBase.New()
        pack_size = uint32_1
    End Sub

    Public Shared Shadows Function [New]() As datagram_transmitter
        Return New datagram_transmitter()
    End Function

    Public Shared Shadows Function [New](ByVal t As transmitter) As datagram_transmitter
        assert(Not t Is Nothing)
        Return datagram_transmitter.[New](). _
                   with_concurrent_operation(t.concurrent_operation()). _
                   with_transmit_mode(t.transmit_mode()). _
                   with_one_off(t.one_off())
    End Function

    Public Function packet_size() As UInt32
        Return pack_size
    End Function

    Public Function with_packet_size(ByVal size As UInt32) As datagram_transmitter
        assert(size > uint32_0)
        Me.pack_size = size
        Return Me
    End Function

    Public Overrides Function Clone() As Object
        Return [New](Me).with_packet_size(packet_size())
    End Function

    Public Shared Shadows Function from_type(Of T)() As datagram_transmitter
        Return type_attribute.of(Of T)().get(Of datagram_transmitter)()
    End Function

    Public Shared Shadows Function from_type(Of T)(ByVal this As T) As datagram_transmitter
        Return type_attribute.of(this).get(Of datagram_transmitter)()
    End Function
End Class
