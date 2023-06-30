
Imports System.Runtime.CompilerServices
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.connector

Public Interface sync_indicator
    Function indicate(ByRef pending As Boolean) As Boolean
End Interface

Public Class sync_indicator_adapter
    Implements sync_indicator

    Private ReadOnly f As _do(Of Boolean, Boolean)

    Public Sub New(ByVal f As _do(Of Boolean, Boolean))
        assert(Not f Is Nothing)
        Me.f = f
    End Sub

    Public Sub New(ByVal f As Func(Of Boolean))
        Me.New(Function(ByRef o As Boolean) As Boolean
                   o = f()
                   Return True
               End Function)
    End Sub

    Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
        Return f(pending)
    End Function
End Class

Public Interface indicator
    Function indicate(ByVal pending As ref(Of Boolean)) As event_comb
End Interface

Public Class indicator_adapter
    Implements indicator

    Private ReadOnly f As Func(Of ref(Of Boolean), event_comb)

    Public Sub New(ByVal f As Func(Of ref(Of Boolean), event_comb))
        assert(Not f Is Nothing)
        Me.f = f
    End Sub

    Public Function indicate(ByVal pending As ref(Of Boolean)) As event_comb Implements indicator.indicate
        Return f(pending)
    End Function
End Class

Public Interface sensor
    'pending shows whether there are pending data to read
    'if timeout_ms < 0, the procedure will not return until there is data arrived, i.e. pending = true
    Function sense(ByVal pending As ref(Of Boolean),
                   ByVal timeout_ms As Int64) As event_comb
End Interface

Public Class sensor_adapter
    Implements sensor

    Private ReadOnly f As Func(Of ref(Of Boolean), Int64, event_comb)

    Public Sub New(ByVal f As Func(Of ref(Of Boolean), Int64, event_comb))
        assert(Not f Is Nothing)
        Me.f = f
    End Sub

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return f(pending, timeout_ms)
    End Function
End Class

Public Module _sensor
    <Extension()> Public Function as_sensor(ByVal f As Func(Of Boolean)) As sensor
        Return New indicator_sensor_adapter(New sync_indicator_adapter(f))
    End Function

    <Extension()> Public Function as_sensor(ByVal f As _do(Of Boolean, Boolean)) As sensor
        Return New indicator_sensor_adapter(New sync_indicator_adapter(f))
    End Function

    <Extension()> Public Function as_sensor(ByVal f As Func(Of ref(Of Boolean), event_comb)) As sensor
        Return New indicator_sensor_adapter(New indicator_adapter(f))
    End Function

    <Extension()> Public Function sense(ByVal this As sensor,
                                        ByVal timeout_ms As Int64) As event_comb
        assert(Not this Is Nothing)
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of Boolean)()
                                  ec = this.sense(p, timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+p) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function sense(ByVal this As sensor) As event_comb
        assert(Not this Is Nothing)
#If DEBUG Then
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of Boolean)()
                                  ec = this.sense(p, npos)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(+p) AndAlso
                                         goto_end()
                              End Function)
#Else
        Return this.sense(Nothing, npos)
#End If
    End Function
End Module
