
Option Explicit On
Option Infer Off
Option Strict On

' In general, the return value of at(Of T)() should be type-based cached.
Public MustInherit Class typed_func(Of RT)
    Public Overridable Function at(Of T)() As RT
        Return Nothing
    End Function
End Class
