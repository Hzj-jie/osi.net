
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module _transceive_timeout
    <Extension()> Public Function send_timeout_ms_or_npos(ByVal this As transceive_timeout,
                                                          ByVal count As UInt32) As Int64
        If this Is Nothing Then
            Return npos
        Else
            Return this.send_timeout_ms(count)
        End If
    End Function

    <Extension()> Public Function receive_timeout_ms_or_npos(ByVal this As transceive_timeout,
                                                             ByVal count As UInt32) As Int64
        If this Is Nothing Then
            Return npos
        Else
            Return this.receive_timeout_ms(count)
        End If
    End Function
End Module

Public Class transceive_timeout
    Implements ICloneable, ICloneable(Of transceive_timeout)

    Private ReadOnly st As rate_timeout
    Private ReadOnly rt As rate_timeout

    Public Sub New(ByVal send_rate_timeout As rate_timeout, ByVal receive_rate_timeout As rate_timeout)
        assert(send_rate_timeout IsNot Nothing)
        assert(receive_rate_timeout IsNot Nothing)
        Me.st = send_rate_timeout
        Me.rt = receive_rate_timeout
    End Sub

    Public Sub New(ByVal send_rate_sec As UInt32,
                   ByVal receive_rate_sec As UInt32,
                   Optional ByVal overhead As UInt32 = uint32_0)
        Me.New(New rate_timeout(send_rate_sec, overhead), New rate_timeout(receive_rate_sec, overhead))
    End Sub

    Public Function send_timeout() As Boolean
        Return st.timeout()
    End Function

    Public Function receive_timeout() As Boolean
        Return rt.timeout()
    End Function

    Public Sub update_send(ByVal size As UInt32)
        st.update(size)
    End Sub

    Public Sub update_receive(ByVal size As UInt32)
        rt.update(size)
    End Sub

    Public Function send_timeout_ms(ByVal size As UInt32) As Int64
        Return st.timeout_ms(size)
    End Function

    Public Function receive_timeout_ms(ByVal size As UInt32) As Int64
        Return rt.timeout_ms(size)
    End Function

    Public Function clone() As transceive_timeout
        Return New transceive_timeout(st.clone(), rt.clone())
    End Function

    Public Function IClonable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function

    Public Function ICloneableT_Clone() As transceive_timeout Implements ICloneable(Of transceive_timeout).Clone
        Return clone()
    End Function
End Class
