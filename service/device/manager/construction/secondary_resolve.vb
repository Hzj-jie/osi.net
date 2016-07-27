
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.service.argument

' Client registers an allocator as a function to generate the target. Such as,
' register(Func(v As var) As target_type ... )
' But it can use some input parameters (currently only support one), which is also resolved from the same var object.
' If type_name is defined, resolver will use the v(type_name) as the type-name of some_parameter, otherwise, use default
' string, i.e. no type-name defined for some_parameter.
' Client can resolve the target still by calling resolve(v (As var), (ByRef) output (As target_type)) (As Boolean)
Public Module _secondary_resolve
    Public Function secondary_resolve_cast(Of T, PT, RT As T) _
                                          (ByVal v As var,
                                           ByVal type_name As String,
                                           ByVal f As _do_val_ref(Of PT, RT, Boolean),
                                           ByRef o As T) As Boolean
        assert(Not f Is Nothing)
        If v Is Nothing Then
            Return False
        End If
        Dim p As PT = Nothing
        If Not type_name Is Nothing Then
            v.bind(type_name)
        End If
        Return constructor(Of PT).resolve(type_name, v, p) AndAlso
               f(p, o) AndAlso
               wrapper(Of T).wrap(type_name, v, o, o)
    End Function

    Public Function secondary_resolve_cast(Of T, PT, RT As T) _
                                          (ByVal v As var,
                                           ByVal type_name As String,
                                           ByVal f As Func(Of PT, RT),
                                           ByRef o As T) As Boolean
        Return secondary_resolve_cast(v,
                                      type_name,
                                      Function(i As PT, ByRef r As RT) As Boolean
                                          r = f(i)
                                          Return True
                                      End Function,
                                      o)
    End Function

    Public Function secondary_resolve_cast(Of T, PT, RT As T) _
                                          (ByVal v As var,
                                           ByVal f As _do_val_ref(Of PT, RT, Boolean),
                                           ByRef o As T) As Boolean
        Return secondary_resolve_cast(v, Nothing, f, o)
    End Function

    Public Function secondary_resolve_cast(Of T, PT, RT As T) _
                                          (ByVal v As var,
                                           ByVal f As Func(Of PT, RT),
                                           ByRef o As T) As Boolean
        Return secondary_resolve_cast(v, Nothing, f, o)
    End Function

    Public Function secondary_resolve(Of T, PT) _
                                     (ByVal v As var,
                                      ByVal type_name As String,
                                      ByVal f As _do_val_ref(Of PT, T, Boolean),
                                      ByRef o As T) As Boolean
        Return secondary_resolve_cast(Of T, PT, T)(v, type_name, f, o)
    End Function

    Public Function secondary_resolve(Of T, PT) _
                                     (ByVal v As var,
                                      ByVal type_name As String,
                                      ByVal f As Func(Of PT, T),
                                      ByRef o As T) As Boolean
        Return secondary_resolve_cast(Of T, PT, T)(v, type_name, f, o)
    End Function

    Public Function secondary_resolve(Of T, PT) _
                                     (ByVal v As var,
                                      ByVal f As _do_val_ref(Of PT, T, Boolean),
                                      ByRef o As T) As Boolean
        Return secondary_resolve_cast(Of T, PT, T)(v, f, o)
    End Function

    Public Function secondary_resolve(Of T, PT) _
                                     (ByVal v As var,
                                      ByVal f As Func(Of PT, T),
                                      ByRef o As T) As Boolean
        Return secondary_resolve_cast(Of T, PT, T)(v, f, o)
    End Function
End Module
