
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.service.selector

Public NotInheritable Class manual_device_exporter(Of T)
    Inherits device_exporter(Of T)
    Implements imanual_device_exporter(Of T)

    Private ReadOnly sc As atomic_int

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub New(ByVal id As String)
        MyBase.New(id)
        sc = New atomic_int()
    End Sub

    Public Overrides Function start() As Boolean
        If sc.increment() = 1 Then
            Return assert(MyBase.start())
        Else
            Return False
        End If
    End Function

    Public Overrides Function started() As Boolean
        Return sc.get() > 0
    End Function

    Public Overrides Function [stop]() As Boolean
        If sc.decrement() = 0 Then
            Return assert(MyBase.[stop]())
        Else
            Return False
        End If
    End Function

    Public Overrides Function stopped() As Boolean
        Return sc.get() = 0
    End Function

    Public Function inject(ByVal d As idevice(Of T)) As Boolean Implements imanual_device_exporter(Of T).inject
        Return device_exported(d)
    End Function

    Public Function inject(ByVal d As idevice(Of async_getter(Of T))) As event_comb _
                          Implements imanual_device_exporter(Of T).inject
        Dim ec As event_comb = Nothing
        Dim p As ref(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim a As async_getter(Of T) = Nothing
                                  If d Is Nothing Then
                                      Return False
                                  Else
                                      a = d.get()
                                      If a Is Nothing Then
                                          Return False
                                      Else
                                          p = New ref(Of T)()
                                          ec = a.get(p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not p.empty() Then
                                      Return inject(device_adapter.[New](d, +p)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
