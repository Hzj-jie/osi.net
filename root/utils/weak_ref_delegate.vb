
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

' A delegate without binding the target object.
Public NotInheritable Class weak_ref_delegate
    Public Shared Function bind(Of T, R)(ByVal i As T,
                                         ByVal f As Func(Of T, R),
                                         Optional ByVal default_value As R = Nothing) As Func(Of R)
        assert(i IsNot Nothing)
        assert(f IsNot Nothing)
        Dim o As weak_ref(Of T) = Nothing
        o = weak_ref.of(i)
        Return Function() As R
                   Dim x As T = Nothing
                   x = o.get()
                   If x IsNot Nothing Then
                       Return f(x)
                   End If
                   Return Nothing
               End Function
    End Function

    Public Shared Function bind(Of T)(ByVal i As T, ByVal f As Action(Of T)) As Action
        assert(i IsNot Nothing)
        assert(f IsNot Nothing)
        Dim o As weak_ref(Of T) = Nothing
        o = weak_ref.of(i)
        Return Sub()
                   Dim x As T = Nothing
                   x = o.get()
                   If x IsNot Nothing Then
                       f(x)
                   End If
               End Sub
    End Function

    Private Sub New()
    End Sub
End Class
