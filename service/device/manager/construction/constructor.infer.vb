
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.argument
Imports osi.service.configuration
Imports osi.service.selector

Partial Public Class constructor
    Private Sub New()
    End Sub

    Public Shared Function empty(Of T)() As Boolean
        Return constructor(Of T).empty()
    End Function

    Public Shared Function [erase](Of T)() As Boolean
        Return constructor(Of T).erase()
    End Function

    Public Shared Function [erase](Of T)(ByVal type As String) As Boolean
        Return constructor(Of T).erase(type)
    End Function

    Public Shared Sub clear(Of T)()
        constructor(Of T).clear()
    End Sub

    Public Shared Function register(Of T)(ByVal allocator As allocator(Of T, var)) As Boolean
        Return constructor(Of T).register(allocator)
    End Function

    Public Shared Function register(Of T)(ByVal type As String, ByVal allocator As allocator(Of T, var)) As Boolean
        Return constructor(Of T).register(type, allocator)
    End Function

    Public Shared Function resolve(Of T)(ByVal type_key As String, ByVal v As var, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(type_key, v, o)
    End Function

    Public Shared Function resolve(Of T)(ByVal v As var, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(v, o)
    End Function

    Public Shared Function resolve(Of T)(ByVal type_key As String, ByVal v As section, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(type_key, v, o)
    End Function

    Public Shared Function resolve(Of T)(ByVal v As section, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(v, o)
    End Function
End Class
