
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector

' Consumers should only use this class to create a new interface, say, async_getter_herald.
<type_attribute()>
Public Class async_getter_dev_T(Of T)
    Inherits async_getter_dev_T(Of T, dev_T(Of T))

    Private Sub New(ByVal p As pair(Of async_getter(Of dev_T(Of T)), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function [New](Of IT, DT As dev_T(Of T)) _
                                        (ByVal i As async_getter(Of IT),
                                         ByVal c As Func(Of IT, DT)) As async_getter_dev_T(Of T)
        Return New async_getter_dev_T(Of T, dev_T(Of T))(async_getter_adapter(Of dev_T(Of T)).convert(i, c))
    End Function

    Public Shared Shadows Function [New](Of IT)(ByVal i As async_getter(Of IT),
                                                ByVal c As Func(Of IT, dev_T(Of T))) As async_getter_dev_T(Of T)
        Return New async_getter_dev_T(Of T, dev_T(Of T))(async_getter_adapter(Of dev_T(Of T)).convert(i, c))
    End Function

    Public Shared Shadows Function [New](ByVal i As async_getter(Of dev_T(Of T))) As async_getter_dev_T(Of T)
        Return New async_getter_dev_T(Of T)(async_getter_adapter(Of dev_T(Of T)).convert(i))
    End Function
End Class

Public Class async_getter_dev_T(Of T, DT As dev_T(Of T))
    Inherits async_getter_adapter(Of DT)
    Implements dev_T(Of T)

    Protected Sub New(ByVal p As pair(Of async_getter(Of DT), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function [New](Of IT, DDT As DT) _
                                         (ByVal i As async_getter(Of IT),
                                          ByVal c As Func(Of IT, DDT)) As async_getter_dev_T(Of T, DT)
        Return New async_getter_dev_T(Of T, DT)(async_getter_adapter(Of DT).convert(i, c))
    End Function

    Public Shared Shadows Function [New](Of IT)(ByVal i As async_getter(Of IT),
                                                 ByVal c As Func(Of IT, DT)) As async_getter_dev_T(Of T, DT)
        Return New async_getter_dev_T(Of T, DT)(async_getter_adapter(Of DT).convert(i, c))
    End Function

    Public Shared Shadows Function [New](ByVal i As async_getter(Of DT)) As async_getter_dev_T(Of T, DT)
        Return New async_getter_dev_T(Of T, DT)(async_getter_adapter(Of DT).convert(i))
    End Function

    Public Function send(ByVal i As T) As event_comb Implements T_injector(Of T).send
        Return _do(Function(x) x.send(i))
    End Function

    Public Function receive(ByVal result As pointer(Of T)) As event_comb Implements T_pump(Of T).receive
        Return _do(Function(x) x.receive(result))
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return _do(Function(x) x.sense(pending, timeout_ms))
    End Function
End Class
