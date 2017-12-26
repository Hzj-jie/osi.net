
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.service.argument

Public NotInheritable Class wrapper
    Private Shared Function convert(Of T)(ByVal f As Func(Of var, T, T)) As _do_val_val_ref(Of var, T, T, Boolean)
        If f Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
                       o = f(v, i)
                       Return True
                   End Function
        End If
    End Function

    Public Shared Function register(Of T)(ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                          Optional ByRef index As UInt32 = Nothing) As Boolean
        Return wrapper(Of T).register(v, index)
    End Function

    Public Shared Function register(Of T)(ByVal v As Func(Of var, T, T),
                                          Optional ByRef index As UInt32 = Nothing) As Boolean
        Return wrapper(Of T).register(convert(v), index)
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal v As _do_val_val_ref(Of var, T, T, Boolean),
                                          Optional ByRef index As UInt32 = Nothing) As Boolean
        Return wrapper(Of T).register(type, v, index)
    End Function

    Public Shared Function register(Of T)(ByVal type As String,
                                          ByVal v As Func(Of var, T, T),
                                          Optional ByRef index As UInt32 = Nothing) As Boolean
        Return wrapper(Of T).register(type, convert(v), index)
    End Function

    Public Shared Function wrap(Of T)(ByVal v As var, ByVal i As T, ByRef o As T) As Boolean
        Return wrapper(Of T).wrap(v, i, o)
    End Function

    Private Sub New()
    End Sub
End Class
