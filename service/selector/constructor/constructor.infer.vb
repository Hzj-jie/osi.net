
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class constructor
    Private Shared Function convert(Of T)(ByVal f As _do_val_ref(Of var, T, Boolean)) _
                                         As Func(Of var, pointer(Of T), event_comb)
        assert(Not f Is Nothing)
        Return Function(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
                   Return sync_async(Function() As Boolean
                                         Dim r As T = Nothing
                                         Return f(v, r) AndAlso
                                                eva(o, r)
                                     End Function)
               End Function
    End Function

    Private Shared Function convert(Of T)(ByVal f As Func(Of var, T)) As Func(Of var, pointer(Of T), event_comb)
        assert(Not f Is Nothing)
        Return Function(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
                   Return sync_async(Sub()
                                         eva(o, f(v))
                                     End Sub)
               End Function
    End Function

    Public Shared Function register(Of T)(ByVal allocator As Func(Of var, pointer(Of T), event_comb)) As Boolean
        Return constructor(Of T).register(allocator)
    End Function

    Public Shared Function register(Of T)(ByVal allocator As _do_val_ref(Of var, T, Boolean)) As Boolean
        Return register(convert(allocator))
    End Function

    Public Shared Function register(Of T)(ByVal allocator As Func(Of var, T)) As Boolean
        Return register(convert(allocator))
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal allocator As Func(Of var, pointer(Of T), event_comb)) As Boolean
        Return constructor(Of T).register(type, allocator)
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal allocator As _do_val_ref(Of var, T, Boolean)) As Boolean
        Return register(type, convert(allocator))
    End Function

    Public Shared Function register(Of T)(ByVal type As String, ByVal allocator As Func(Of var, T)) As Boolean
        Return register(type, convert(allocator))
    End Function

    Public Shared Function resolve(Of T)(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
        Return constructor(Of T).resolve(v, o)
    End Function

    Public Shared Function resolve(Of T, DT As T)(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
        Return constructor(Of T).resolve(Of DT)(v, o)
    End Function

    Public Shared Function resolve(Of T, RT As T)(ByVal v As var, ByVal o As pointer(Of RT)) As event_comb
        Return constructor(Of T).resolve(Of RT)(v, o)
    End Function

    ' For test purpose: this function should not be used explicitly.
    Public Shared Function sync_resolve(Of T)(ByVal v As var, ByRef o As T) As Boolean
        Dim r As pointer(Of T) = Nothing
        r = New pointer(Of T)()
        Return async_sync(resolve(v, r)) AndAlso
               eva(o, +r)
    End Function

    ' For test purpose: this function should not be used explicitly.
    Public Shared Function sync_resolve(Of T, DT As T)(ByVal v As var, ByRef o As T) As Boolean
        Dim r As pointer(Of T) = Nothing
        r = New pointer(Of T)()
        Return async_sync(resolve(Of T, DT)(v, r)) AndAlso
               eva(o, +r)
    End Function

    ' For test purpose: this function should not be used explicitly.
    Public Shared Function sync_resolve(Of T, RT As T)(ByVal v As var, ByRef o As RT) As Boolean
        Dim r As pointer(Of RT) = Nothing
        r = New pointer(Of RT)()
        Return async_sync(resolve(Of T, RT)(v, r)) AndAlso
               eva(o, +r)
    End Function

    Private Sub New()
    End Sub
End Class
