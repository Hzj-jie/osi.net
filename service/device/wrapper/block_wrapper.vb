
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.service.argument
Imports osi.service.transmitter

Public Class block_wrapper
    Implements block

    Public ReadOnly inner As block
    Public ReadOnly wrapped As block

    Private Sub New(ByVal inner As block, ByVal wrapped As block)
        assert(Not inner Is Nothing)
        assert(Not wrapped Is Nothing)
        Me.inner = inner
        Me.wrapped = wrapped
    End Sub

    Public Shared Function bind_wrap(ByVal v As var, ByRef o As _do_val_ref(Of block, block, Boolean)) As Boolean
        Dim f As _do_val_ref(Of block, block, Boolean) = Nothing
        If wrapper.bind(v, f) AndAlso
           assert(Not f Is Nothing) Then
            o = Function(i As block, ByRef r As block) As Boolean
                    Dim wrapped As block = Nothing
                    If f(i, wrapped) Then
                        r = New block_wrapper(i, wrapped)
                        Return True
                    Else
                        Return False
                    End If
                End Function
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function unwrap(ByVal i As block, ByRef o As block) As Boolean
        Dim w As block_wrapper = Nothing
        If cast(Of block_wrapper)(i, w) AndAlso
           assert(Not w Is Nothing) AndAlso
           assert(Not w.inner Is Nothing) Then
            o = w.inner
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function create(ByVal v As var, ByVal inner As block, ByRef o As block) As Boolean
        Dim r As block = Nothing
        If wrapper.wrap(v, inner, r) Then
            o = New block_wrapper(inner, r)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb Implements block_injector.send
        Return wrapped.send(buff, offset, count)
    End Function

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return wrapped.receive(result)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return wrapped.sense(pending, timeout_ms)
    End Function
End Class
