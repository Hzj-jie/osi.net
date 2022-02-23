
Imports osi.root.connector
Imports osi.root.utils

Public Class T_adapter(Of T)
    Implements type_attribute.signal

    Public ReadOnly underlying_device As T
    Private ReadOnly attr As type_attribute

    Public Sub New(ByVal d As T, ByVal attr As type_attribute)
        assert(d IsNot Nothing)
        assert(attr IsNot Nothing)
        Me.underlying_device = d
        Me.attr = attr
    End Sub

    Public Sub New(ByVal d As T)
        Me.New(d, type_attribute.of(d))
    End Sub

    Public Function attribute() As type_attribute Implements type_attribute.signal.attribute
        Return attr
    End Function
End Class
