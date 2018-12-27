
' TODO: Remove, use service.constructor
Imports osi.root.delegates
Imports osi.service.argument

Partial Public Class constructor
    Public Shared Function register(Of T)(ByVal i As _do_val_ref(Of var, T, Boolean)) As Boolean
        Return constructor(Of T).register(i)
    End Function

    Public Shared Function register(Of T)(ByVal i As Func(Of var, T)) As Boolean
        Return constructor(Of T).register(i)
    End Function

    Public Shared Function register(Of T)(ByVal type As String, ByVal i As _do_val_ref(Of var, T, Boolean)) As Boolean
        Return constructor(Of T).register(type, i)
    End Function

    Public Shared Function register(Of T)(ByVal type As String, ByVal i As Func(Of var, T)) As Boolean
        Return constructor(Of T).register(type, i)
    End Function

    Public Shared Function resolve(Of T, DT As T)(ByVal v As var, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(Of DT)(v, o)
    End Function

    Public Shared Function resolve(Of T, DT As T)(ByVal type_key As String, ByVal v As var, ByRef o As T) As Boolean
        Return constructor(Of T).resolve(Of DT)(type_key, v, o)
    End Function
End Class
