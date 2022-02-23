
' TODO: Remove
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.argument

Public Class wrapper(Of T)
    Private Shared ReadOnly wt As unique_strong_map(Of String, 
                                                       collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))
    Private Shared ReadOnly w As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean))

    Shared Sub New()
        wt = New unique_strong_map(Of String, collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))()
        w = New collectionless(Of _do_val_val_ref(Of var, T, T, Boolean))()
        static_constructor(Of registry(Of T)).execute()
    End Sub

    Private Shared Function conv(ByVal f As Func(Of var, T, T)) As _do_val_val_ref(Of var, T, T, Boolean)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(v As var, i As T, ByRef o As T) As Boolean
                       o = f(v, i)
                       Return True
                   End Function
        End If
    End Function

    Public Shared Function register(ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                    Optional ByRef index As UInt32 = uint32_0) As Boolean
        If v Is Nothing Then
            Return False
        Else
            index = w.emplace(v)
            Return True
        End If
    End Function

    Public Shared Function register(ByVal f As Func(Of var, T, T),
                                    Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return register(conv(f), index)
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                    Optional ByRef index As UInt32 = uint32_0) As Boolean
        If String.IsNullOrEmpty(type) OrElse
           v Is Nothing Then
            Return False
        Else
            index = wt.generate(Of collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))(type).emplace(v)
            Return True
        End If
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal f As Func(Of var, T, T),
                                    Optional ByRef index As UInt32 = uint32_0) As Boolean
        Return register(type, conv(f), index)
    End Function

    Public Shared Function [erase](ByVal type As String) As Boolean
        Return wt.erase(type)
    End Function

    Public Shared Function [erase](ByVal type As String, ByVal index As UInt32) As Boolean
        Dim x As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)) = Nothing
        If wt.get(type, x) Then
            x.erase(index)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [erase](ByVal index As UInt32) As Boolean
        If w.empty() Then
            Return False
        Else
            w.erase(index)
            Return True
        End If
    End Function

    Public Shared Function [erase]() As Boolean
        If w.empty() Then
            Return False
        Else
            w.clear()
            Return True
        End If
    End Function

    Public Shared Function empty() As Boolean
        Return w.empty() AndAlso
               wt.empty()
    End Function

    Public Shared Sub clear()
        wt.clear()
        w.clear()
    End Sub

    Public Shared Function bind(ByVal v As var,
                                ByRef o As _do_val_val_ref(Of String, T, T, Boolean)) As Boolean
        If v Is Nothing Then
            Return False
        Else
            o = Function(type_key As String, i As T, ByRef r As T) As Boolean
                    Return wrap(type_key, v, i, r)
                End Function
            Return True
        End If
    End Function

    Public Shared Function bind(ByVal v As var,
                                ByRef o As _do_val_ref(Of T, T, Boolean)) As Boolean
        If v Is Nothing Then
            Return False
        Else
            o = Function(i As T, ByRef r As T) As Boolean
                    Return wrap(v, i, r)
                End Function
            Return True
        End If
    End Function

    Private Shared Function wrap(ByVal vs As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)),
                                 ByVal v As var,
                                 ByVal i As T,
                                 ByRef o As T) As Boolean
        assert(vs IsNot Nothing)
        o = i
        Dim j As UInt32 = uint32_0
        While j < vs.pool_size()
            Dim x As _do_val_val_ref(Of var, T, T, Boolean) = Nothing
            x = vs(j)
            If x IsNot Nothing Then
                If x(v, i, o) Then
                    i = o
                Else
                    Return False
                End If
            End If
            j += uint32_1
        End While
        Return True
    End Function

    Public Shared Function wrap(ByVal type_key As String, ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        If v Is Nothing Then
            Return False
        Else
            If Not registry(Of T).bypass Then
                If type_key IsNot Nothing Then
                    type_key = strcat(type_key, "_")
                End If
                type_key = strcat(type_key, "wrapper")
                Dim ss As vector(Of String) = Nothing
                If ss.split_from(v(type_key)) Then
                    assert(Not ss.null_or_empty())
                    Dim j As UInt32 = uint32_0
                    While j < ss.size()
                        Dim vs As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)) = Nothing
                        If wt.get(ss(j), vs) Then
                            If wrap(vs, v, i, o) Then
                                i = o
                            End If
                        Else
                            Return False
                        End If
                        j += uint32_1
                    End While
                End If
            End If
            Return wrap(w, v, i, o)
        End If
    End Function

    Public Shared Function wrap(ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        Return wrap(default_string, v, i, o)
    End Function
End Class
