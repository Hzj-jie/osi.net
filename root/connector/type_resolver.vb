
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Concurrent
Imports System.Collections.Generic

Public NotInheritable Class type_resolver(Of T)
    Public Shared ReadOnly [default] As type_resolver(Of T) = New type_resolver(Of T)()
    Private ReadOnly d As ConcurrentDictionary(Of Type, T)

    Public Sub New()
        d = New ConcurrentDictionary(Of Type, T)()
    End Sub

    Public Function registered(ByVal type As Type) As Boolean
        Return d.ContainsKey(type)
    End Function

    Public Sub register(ByVal type As Type, ByVal i As T)
        d(type) = i
    End Sub

    Public Sub assert_first_register(ByVal type As Type, ByVal i As T)
        assert(d.TryAdd(type, i))
    End Sub

    Public Sub unregister(ByVal type As Type)
        d.TryRemove(type, Nothing)
    End Sub

    Public Sub assert_unregister(ByVal type As Type)
        assert(d.TryRemove(type, Nothing))
    End Sub

    Public Function from_type(ByVal type As Type, ByRef o As T) As Boolean
        If type Is Nothing Then
            Return False
        End If
        Return d.TryGetValue(type, o)
    End Function

    Public Function from_type(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type(type, o))
        Return o
    End Function

    Public Function from_types(ByVal types As ICollection(Of Type), ByRef o As T) As Boolean
        assert(Not types Is Nothing)
        If types.Count() = 0 Then
            Return False
        End If
        For Each type As Type In types
            If from_type(type, o) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function from_type_or_base(ByVal type As Type, ByRef o As T) As Boolean
        Dim types As List(Of Type) = Nothing
        types = New List(Of Type)()
        While Not type Is Nothing
            types.Add(type)
            type = type.BaseType()
        End While
        Return from_types(types, o)
    End Function

    Public Function from_type_or_base(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type_or_base(type, o))
        Return o
    End Function

    Public Function from_type_or_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        Dim types As List(Of Type) = Nothing
        types = New List(Of Type)()
        types.Add(type)
        types.AddRange(type.GetInterfaces())
        Return from_types(types, o)
    End Function

    Public Function from_type_or_interfaces(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type_or_interfaces(type, o))
        Return o
    End Function

    Public Function from_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        Return from_types(New List(Of Type)(type.GetInterfaces()), o)
    End Function

    Public Function from_interfaces(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_interfaces(type, o))
        Return o
    End Function
End Class
