
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class wrapper(Of T)
    Private Shared ReadOnly wt As unique_strong_map(Of String,
                                                       collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))
    Private Shared ReadOnly w As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean))

    Shared Sub New()
        wt = New unique_strong_map(Of String, collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))()
        w = New collectionless(Of _do_val_val_ref(Of var, T, T, Boolean))()
    End Sub

    Private Shared Function convert(ByVal f As Func(Of var, T, T)) As _do_val_val_ref(Of var, T, T, Boolean)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal v As var, ByVal t As T, ByRef o As T) As Boolean
                       o = f(v, t)
                       Return True
                   End Function
        End If
    End Function

    Public Shared Function register(ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                    Optional ByRef index As UInt32 = Nothing) As Boolean
        If v Is Nothing Then
            Return False
        Else
            index = w.emplace(v)
            Return True
        End If
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                    Optional ByRef index As UInt32 = Nothing) As Boolean
        If type.null_or_empty() OrElse
           v Is Nothing Then
            Return False
        Else
            index = wt.generate(Of collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)))(type).emplace(v)
            Return True
        End If
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

    Public Shared Function [erase](ByVal type As String) As Boolean
        Return wt.erase(type)
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
        w.clear()
        wt.clear()
    End Sub

    Private Shared Function wrap(ByVal vs As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)),
                                 ByVal v As var,
                                 ByVal i As T,
                                 ByRef o As T) As Boolean
        assert(Not vs Is Nothing)
        o = i
        Dim j As UInt32 = uint32_0
        While j < vs.pool_size()
            Dim x As [optional](Of _do_val_val_ref(Of var, T, T, Boolean)) = vs.optional(j)
            If x Then
                If Not (+x)(v, i, o) Then
                    Return False
                End If
                i = o
            End If
            j += uint32_1
        End While
        Return True
    End Function

    Public Shared Function wrap(ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        If v Is Nothing Then
            Return False
        End If
        Dim ss As vector(Of String) = Nothing
        If ss.split_from(v("wrapper")) Then
            assert(Not ss.null_or_empty())
            Dim j As UInt32 = uint32_0
            While j < ss.size()
                Dim vs As collectionless(Of _do_val_val_ref(Of var, T, T, Boolean)) = Nothing
                If wt.get(ss(j), vs) AndAlso wrap(vs, v, i, o) Then
                    i = o
                Else
                    Return False
                End If
                j += uint32_1
            End While
        End If
        Return wrap(w, v, i, o)
    End Function

    Private Sub New()
    End Sub
End Class
