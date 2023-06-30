
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class type_resolver(Of T, PROTECTOR)
    Public Shared ReadOnly r As type_resolver(Of T) = New type_resolver(Of T)()

    Public Shared Function registered(ByVal type As Type) As Boolean
        Return r.registered(type)
    End Function

    Public Shared Sub register(ByVal type As Type, ByVal i As T)
        r.register(type, i)
    End Sub

    Public Shared Sub assert_first_register(ByVal type As Type, ByVal i As T)
        r.assert_first_register(type, i)
    End Sub

    Public Shared Sub unregister(ByVal type As Type)
        r.unregister(type)
    End Sub

    Public Shared Sub assert_unregister(ByVal type As Type)
        r.assert_unregister(type)
    End Sub

    Public Shared Function from_type(ByVal type As Type, ByRef o As T) As Boolean
        Return r.from_type(type, o)
    End Function

    Public Shared Function from_type(ByVal type As Type) As T
        Return r.from_type(type)
    End Function

    Public Shared Function from_type_or_base(ByVal type As Type, ByRef o As T) As Boolean
        Return r.from_type_or_base(type, o)
    End Function

    Public Shared Function from_type_or_base(ByVal type As Type) As T
        Return r.from_type_or_base(type)
    End Function

    Public Shared Function from_type_or_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        Return r.from_type_or_interfaces(type, o)
    End Function

    Public Shared Function from_type_or_interfaces(ByVal type As Type) As T
        Return r.from_type_or_interfaces(type)
    End Function

    Public Shared Function from_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        Return r.from_interfaces(type, o)
    End Function

    Public Shared Function from_interfaces(ByVal type As Type) As T
        Return r.from_interfaces(type)
    End Function

    Private Sub New()
    End Sub
End Class