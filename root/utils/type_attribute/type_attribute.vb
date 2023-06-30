
Imports osi.root.connector

Partial Public Class type_attribute
    Inherits Attribute

    Public Enum init_mode
        exactly_once  ' must be set once and only once before get
        once          ' can be unset or set once
        must_be_set   ' must be set before get, but can be set for several times
        any           ' no restriction
    End Enum

    Public Enum forward_mode
        [get]         ' forward get only
        [set]         ' forward set only
        any           ' forward get and set
    End Enum

    Private Const default_init_mode As init_mode = init_mode.exactly_once
    Private Const default_forward_mode As forward_mode = forward_mode.get
    Private ReadOnly init As init_mode
    Private ReadOnly fwd As forward_mode
    Private s As store

    Public Interface signal
        Function attribute() As type_attribute
    End Interface

    Public Sub [set](ByVal i As Object)
        assert(Not s Is Nothing)
        s.set(i, init, fwd)
    End Sub

    Public Function [get]() As Object
        assert(Not s Is Nothing)
        Return s.get(init, fwd)
    End Function

    Public Function [get](Of T)() As T
        assert(Not s Is Nothing)
        Return s.get(Of T)(init, fwd)
    End Function

    Public Sub New(ByVal forward_mode As forward_mode, ByVal init_mode As init_mode)
        Me.New(init_mode, forward_mode)
    End Sub

    Public Sub New(ByVal forward_mode As forward_mode)
        Me.New(default_init_mode, forward_mode)
    End Sub

    Public Sub New(Optional ByVal init_mode As init_mode = default_init_mode,
                   Optional ByVal forward_mode As forward_mode = default_forward_mode)
        Me.init = init_mode
        Me.fwd = forward_mode
    End Sub

    Public Function with_store() As type_attribute
        s = New store()
        Return Me
    End Function
End Class
