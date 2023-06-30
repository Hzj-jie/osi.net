
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.service.argument

Public Class wrapper
    Private Sub New()
    End Sub

    Public Shared Function register(Of T)(ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                          Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return wrapper(Of T).register(v, index)
    End Function

    Public Shared Function register(Of T)(ByVal v As Func(Of var, T, T),
                                          Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return wrapper(Of T).register(v, index)
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                          Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return wrapper(Of T).register(type, v, index)
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal v As Func(Of var, T, T),
                                          Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return wrapper(Of T).register(type, v, index)
    End Function

    Public Shared Function [erase](Of T)(ByVal type As String) As Boolean
        Return wrapper(Of T).erase(type)
    End Function

    Public Shared Function [erase](Of T)(ByVal type As String, ByVal index As UInt32) As Boolean
        Return wrapper(Of T).erase(type, index)
    End Function

    Public Shared Function [erase](Of T)(ByVal index As UInt32) As Boolean
        Return wrapper(Of T).erase(index)
    End Function

    Public Shared Function [erase](Of T)() As Boolean
        Return wrapper(Of T).erase()
    End Function

    Public Shared Function empty(Of T)() As Boolean
        Return wrapper(Of T).empty()
    End Function

    Public Shared Sub clear(Of T)()
        wrapper(Of T).clear()
    End Sub

    Public Shared Function wrap(Of T)(ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        Return wrapper(Of T).wrap(v, i, o)
    End Function

    Public Shared Function wrap(Of T)(ByVal type_key As String, ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        Return wrapper(Of T).wrap(type_key, v, i, o)
    End Function

    Public Shared Function bind(Of T)(ByVal v As var,
                                      ByRef o As _do_val_val_ref(Of String, T, T, Boolean)) As Boolean
        Return wrapper(Of T).bind(v, o)
    End Function

    Public Shared Function bind(Of T)(ByVal v As var,
                                      ByRef o As _do_val_ref(Of T, T, Boolean)) As Boolean
        Return wrapper(Of T).bind(v, o)
    End Function
End Class
