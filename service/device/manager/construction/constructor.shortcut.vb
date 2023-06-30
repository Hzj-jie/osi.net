
Imports osi.root.delegates
Imports osi.service.argument
Imports osi.service.selector

Partial Public Class constructor(Of T)
    Public Shared Function register(ByVal i As _do_val_ref(Of var, T, Boolean)) As Boolean
        Dim a As allocator(Of T, var) = Nothing
        Return make_allocator(i, a) AndAlso register(a)
    End Function

    Public Shared Function register(ByVal i As Func(Of var, T)) As Boolean
        Dim a As allocator(Of T, var) = Nothing
        Return make_allocator(i, a) AndAlso register(a)
    End Function

    Public Shared Function register(ByVal type As String, ByVal i As _do_val_ref(Of var, T, Boolean)) As Boolean
        Dim a As allocator(Of T, var) = Nothing
        Return make_allocator(i, a) AndAlso register(type, a)
    End Function

    Public Shared Function register(ByVal type As String, ByVal i As Func(Of var, T)) As Boolean
        Dim a As allocator(Of T, var) = Nothing
        Return make_allocator(i, a) AndAlso register(type, a)
    End Function

    Public Shared Function resolve(Of DT As T)(ByVal v As var, ByRef o As T) As Boolean
        Dim x As DT = Nothing
        If constructor(Of DT).resolve(v, x) Then
            o = x
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function resolve(Of DT As T)(ByVal type_key As String, ByVal v As var, ByRef o As T) As Boolean
        Dim x As DT = Nothing
        If constructor(Of DT).resolve(type_key, v, x) Then
            o = x
            Return True
        Else
            Return False
        End If
    End Function
End Class
