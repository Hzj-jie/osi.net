
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class convertor
    Private Shared Function convert(Of T, T2)(ByVal f As Func(Of T, pointer(Of T2), event_comb)) _
                                             As Func(Of var, T, pointer(Of T2), event_comb)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal v As var, ByVal i As T, ByVal o As pointer(Of T2)) As event_comb
                       Return f(i, o)
                   End Function
        End If
    End Function

    Private Shared Function convert(Of T, T2)(ByVal f As _do_val_val_ref(Of var, T, T2, Boolean)) _
                                             As Func(Of var, T, pointer(Of T2), event_comb)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal v As var, ByVal i As T, ByVal o As pointer(Of T2)) As event_comb
                       Return sync_async(Function() As Boolean
                                             Dim r As T2 = Nothing
                                             Return f(v, i, r) AndAlso
                                                    eva(o, r)
                                         End Function)
                   End Function
        End If
    End Function

    Private Shared Function convert(Of T, T2)(ByVal f As _do_val_ref(Of T, T2, Boolean)) _
                                             As Func(Of var, T, pointer(Of T2), event_comb)
        If f Is Nothing Then
            Return Nothing
        Else
            Return convert(Function(ByVal v As var, ByVal i As T, ByRef o As T2) As Boolean
                               Return f(i, o)
                           End Function)
        End If
    End Function

    Private Shared Function convert(Of T, T2)(ByVal f As Func(Of var, T, T2)) _
                                             As Func(Of var, T, pointer(Of T2), event_comb)

        If f Is Nothing Then
            Return Nothing
        Else
            Return convert(Function(ByVal v As var, ByVal i As T, ByRef o As T2) As Boolean
                               o = f(v, i)
                               Return True
                           End Function)
        End If
    End Function

    Private Shared Function convert(Of T, T2)(ByVal f As Func(Of T, T2)) _
                                             As Func(Of var, T, pointer(Of T2), event_comb)
        If f Is Nothing Then
            Return Nothing
        Else
            Return convert(Function(ByVal v As var, ByVal i As T) As T2
                               Return f(i)
                           End Function)
        End If
    End Function

    Private Shared Function allocate(Of T, T2)(ByVal f As Func(Of var, T, pointer(Of T2), event_comb),
                                               ByVal T_type_filter As String) _
                                              As Func(Of var, pointer(Of T2), event_comb)
        assert(Not f Is Nothing)
        T_type_filter = strcat(T_type_filter, character.dot)
        Return Function(ByVal v As var, ByVal o As pointer(Of T2)) As event_comb
                   Dim ec As event_comb = Nothing
                   Dim p As pointer(Of T) = Nothing
                   Return New event_comb(Function() As Boolean
                                             p = New pointer(Of T)()
                                             ec = constructor.resolve(filtered_var.[New](v, T_type_filter), p)
                                             Return waitfor(ec) AndAlso
                                                    goto_next()
                                         End Function,
                                         Function() As Boolean
                                             If ec.end_result() Then
                                                 ec = f(v, +p, o)
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             Else
                                                 Return False
                                             End If
                                         End Function,
                                         Function() As Boolean
                                             Return ec.end_result() AndAlso
                                                    goto_end()
                                         End Function)
               End Function
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As Func(Of var, T, pointer(Of T2), event_comb),
                                              ByVal T_type_filter As String) As Boolean
        If f Is Nothing Then
            Return False
        Else
            Return constructor.register(allocate(f, T_type_filter))
        End If
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As Func(Of T, pointer(Of T2), event_comb),
                                              ByVal T_type_filter As String) As Boolean
        Return register(convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As _do_val_val_ref(Of var, T, T2, Boolean),
                                              ByVal T_type_filter As String) As Boolean
        Return register(convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As _do_val_ref(Of T, T2, Boolean),
                                              ByVal T_type_filter As String) As Boolean
        Return register(convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As Func(Of var, T, T2),
                                              ByVal T_type_filter As String) As Boolean
        Return register(convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As Func(Of T, T2),
                                              ByVal T_type_filter As String) As Boolean
        Return register(convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal f As Func(Of var, T, pointer(Of T2), event_comb),
                                              ByVal T_type_filter As String) As Boolean
        If f Is Nothing Then
            Return False
        Else
            Return constructor.register(type, allocate(f, T_type_filter))
        End If
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal f As Func(Of T, pointer(Of T2), event_comb),
                                              ByVal T_type_filter As String) As Boolean
        Return register(type, convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal f As _do_val_val_ref(Of var, T, T2, Boolean),
                                              ByVal T_type_filter As String) As Boolean
        Return register(type, convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal f As _do_val_ref(Of T, T2, Boolean),
                                              ByVal T_type_filter As String) As Boolean
        Return register(type, convert(f), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal v As Func(Of var, T, T2),
                                              ByVal T_type_filter As String) As Boolean
        Return register(type, convert(v), T_type_filter)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal v As Func(Of T, T2),
                                              ByVal T_type_filter As String) As Boolean
        Return register(type, convert(v), T_type_filter)
    End Function

    Private Sub New()
    End Sub
End Class
