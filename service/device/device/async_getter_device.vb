
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.selector

' Converts a combination of async_getter(Of T) and T to an idevice(Of T)
Public MustInherit Class async_getter_device(Of T, ASG As {async_getter(Of T), T})
    Inherits async_getter_device(Of T)
    Implements idevice(Of T)

    Private ReadOnly c As ASG

    Public Sub New(ByVal c As ASG)
        MyBase.New(c)
        Me.c = c
    End Sub

    Public Overloads Function [get]() As T Implements idevice(Of T).get
        Return c
    End Function
End Class

' Converts an async_getter(Of T) to an idevice(Of async_getter(Of T))
Public MustInherit Class async_getter_device(Of T)
    Inherits device(Of async_getter(Of T))

    Public Sub New(ByVal c As async_getter(Of T))
        MyBase.New(c)
    End Sub

    Public Function to_device(ByRef o As idevice(Of T)) As Boolean
        Dim v As T = Nothing
        If [get]().get(v) Then
            o = v.make_device(AddressOf validate, AddressOf close, AddressOf identity, AddressOf check)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function to_device() As idevice(Of T)
        Dim o As idevice(Of T) = Nothing
        assert(to_device(o))
        Return o
    End Function

    Protected MustOverride Overloads Sub check(ByVal c As T)
    Protected MustOverride Overloads Function identity(ByVal c As T) As String
    Protected MustOverride Overloads Sub close(ByVal c As T)
    Protected MustOverride Overloads Function validate(ByVal c As T) As Boolean

    Protected NotOverridable Overrides Sub check(ByVal c As async_getter(Of T))
        assert(Not c Is Nothing)
        Dim r As T = Nothing
        If c.get(r) Then
            check(r)
        End If
    End Sub

    Protected NotOverridable Overrides Function identity(ByVal c As async_getter(Of T)) As String
        assert(Not c Is Nothing)
        Dim r As T = Nothing
        If c.get(r) Then
            Return identity(r)
        Else
            Return Convert.ToString(c)
        End If
    End Function

    Protected NotOverridable Overrides Sub close(ByVal c As async_getter(Of T))
        assert(Not c Is Nothing)
        If c.initialized() Then
            Dim r As T = Nothing
            'assert(Not in_restricted_threadpool_thread())
            If c.get(r) Then
                close(r)
            End If
        Else
            Dim p As ref(Of T) = Nothing
            assert_begin(New event_comb(Function() As Boolean
                                            p = New ref(Of T)()
                                            Return waitfor(c.get(p)) AndAlso
                                                   goto_next()
                                        End Function,
                                        Function() As Boolean
                                            If Not p.empty() Then
                                                close(+p)
                                            End If
                                            Return goto_end()
                                        End Function))
        End If
    End Sub

    Protected NotOverridable Overrides Function validate(ByVal c As async_getter(Of T)) As Boolean
        assert(Not c Is Nothing)
        Dim r As T = Nothing
        Return c.not_initialized() OrElse (c.get(r) AndAlso validate(r))
    End Function
End Class
