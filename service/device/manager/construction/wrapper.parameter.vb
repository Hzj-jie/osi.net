
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.service.argument

Partial Public Class wrapper
    Public Shared Function parameter(Of T)(ByVal p As String,
                                           ByVal f As _do_val_val_val_ref(Of String, var, T, T, Boolean)) _
                                          As _do_val_val_ref(Of var, T, T, Boolean)
        assert(Not p.null_or_empty())
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(v As var, i As T, ByRef o As T) As Boolean
                       Dim s As String = Nothing
                       If v Is Nothing Then
                           Return False
                       ElseIf v.value(p, s) Then
                           Return f(s, v, i, o)
                       Else
                           o = i
                           Return True
                       End If
                   End Function
        End If
    End Function

    Public Shared Function parameter(Of T)(ByVal p As String,
                                           ByVal f As _do_val_val_ref(Of String, var, T, T)) _
                                          As _do_val_val_ref(Of var, T, T, Boolean)
        If f Is Nothing Then
            Return Nothing
        Else
            Return parameter(p, Function(s As String, v As var, i As T, ByRef o As T) As Boolean
                                    o = f(s, v, i)
                                    Return True
                                End Function)
        End If
    End Function
End Class
