
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.configuration
Imports osi.service.selector

Partial Public Class constructor(Of T)
    Private Shared ReadOnly lt As lazier(Of T, String, var)
    Private Shared l As lazier(Of T, var)

    Shared Sub New()
        lt = New lazier(Of T, String, var)()
        static_constructor(Of registry(Of T)).execute()
    End Sub

    Protected Sub New()
    End Sub

    Public Shared Function empty() As Boolean
        Return lt.empty() AndAlso l Is Nothing
    End Function

    Public Shared Function register(ByVal allocator As allocator(Of T, var)) As Boolean
        If allocator Is Nothing OrElse l IsNot Nothing Then
            Return False
        Else
            l = New lazier(Of T, var)(allocator)
            Return True
        End If
    End Function

    Public Shared Function [erase]() As Boolean
        If l Is Nothing Then
            Return False
        Else
            l = Nothing
            Return True
        End If
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal allocator As allocator(Of T, var)) As Boolean
        Return lt.register(type, allocator)
    End Function

    Public Shared Function [erase](ByVal type As String) As Boolean
        Return lt.erase(type)
    End Function

    Public Shared Sub clear()
        [erase]()
        lt.clear()
    End Sub

    Private Shared Function resolve(ByVal type_key As String,
                                    ByVal type As String,
                                    ByVal v As var,
                                    ByRef o As T) As Boolean
        If lt.registered(type) Then
            If Not lt.select(type, v, o) Then
                Return False
            End If
        ElseIf l IsNot Nothing Then
            If Not l.select(v, o) Then
                Return False
            End If
        Else
            Return False
        End If
        Return wrapper(Of T).wrap(type_key, v, o, o)
    End Function

    Public Shared Function resolve(ByVal type_key As String, ByVal v As var, ByRef o As T) As Boolean
        Return resolve(type_key, v(type_key), v, o)
    End Function

    Public Shared Function resolve(ByVal v As var, ByRef o As T) As Boolean
        Return resolve(default_string, v("type"), v, o)
    End Function

    Public Shared Function resolve(ByVal type_key As String, ByVal v As section, ByRef o As T) As Boolean
        Return resolve(type_key, New var(v.values_or_null()), o)
    End Function

    Public Shared Function resolve(ByVal v As section, ByRef o As T) As Boolean
        Return resolve(New var(v.values_or_null()), o)
    End Function
End Class
