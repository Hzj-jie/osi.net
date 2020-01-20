
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class predicates
    Public Shared Function is_null(Of T)() As Func(Of T, Boolean)
        Return Function(ByVal i As T) As Boolean
                   Return i Is Nothing
               End Function
    End Function

    Public Shared Function is_not_null(Of T)() As Func(Of T, Boolean)
        Return Function(ByVal i As T) As Boolean
                   Return Not i Is Nothing
               End Function
    End Function

    Public Shared Function is_null(Of T)(ByVal i As T) As Func(Of Boolean)
        Return Function() As Boolean
                   Return i Is Nothing
               End Function
    End Function

    Public Shared Function is_not_null(Of T)(ByVal i As T) As Func(Of Boolean)
        Return Function() As Boolean
                   Return Not i Is Nothing
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
