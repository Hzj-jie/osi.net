
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class convertor
    Private Shared Function convert(Of T, T2)(ByVal f As Func(Of var, T, T2)) As _do_val_val_ref(Of var, T, T2, Boolean)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal v As var, ByVal i As T, ByRef o As T2) As Boolean
                       o = f(v, i)
                       Return True
                   End Function
        End If
    End Function

    Private Shared Function allocate(Of T, T2)(ByVal f As _do_val_val_ref(Of var, T, T2, Boolean),
                                               ByVal T_type_prefix As String) _
                                    As Func(Of var, pointer(Of T2), event_comb)
        assert(Not f Is Nothing)
        Return Function(ByVal v As var, ByVal o As pointer(Of T2)) As event_comb
                   Dim ec As event_comb = Nothing
                   Dim p As pointer(Of T) = Nothing
                   Return New event_comb(Function() As Boolean
                                             p = New pointer(Of T)()
                                             ec = constructor.resolve(filtered_var.[New](v, T_type_prefix), p)
                                             Return waitfor(ec) AndAlso
                                                    goto_next()
                                         End Function,
                                         Function() As Boolean
                                             Dim r As T2 = Nothing
                                             Return ec.end_result() AndAlso
                                                    Not p.empty() AndAlso
                                                    f(v, +p, r) AndAlso
                                                    eva(o, r) AndAlso
                                                    goto_end()
                                         End Function)
               End Function
    End Function

    Public Shared Function register(Of T, T2)(ByVal f As _do_val_val_ref(Of var, T, T2, Boolean),
                                              ByVal T_type_prefix As String) As Boolean
        If f Is Nothing Then
            Return False
        Else
            Return constructor.register(allocate(f, T_type_prefix))
        End If
    End Function

    Public Shared Function register(Of T, T2)(ByVal v As Func(Of var, T, T2),
                                              ByVal T_type_prefix As String) As Boolean
        Return register(convert(v), T_type_prefix)
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal f As _do_val_val_ref(Of var, T, T2, Boolean),
                                              ByVal T_type_prefix As String) As Boolean
        If f Is Nothing Then
            Return False
        Else
            Return constructor.register(type, allocate(f, T_type_prefix))
        End If
    End Function

    Public Shared Function register(Of T, T2)(ByVal type As String,
                                              ByVal v As Func(Of var, T, T2),
                                              ByVal T_type_prefix As String) As Boolean
        Return register(type, convert(v), T_type_prefix)
    End Function

    Private Sub New()
    End Sub
End Class
