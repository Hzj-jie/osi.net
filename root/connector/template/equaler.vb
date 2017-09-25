
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public Class default_equaler(Of T1, T2)
    Inherits _equaler(Of T1, T2)

    Public Overrides Function at(ByRef i As T1, ByRef j As T2) As Boolean
        Return equal(i, j)
    End Function
End Class

Public Class default_equaler(Of T)
    Inherits _equaler(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Boolean
        Return equal(i, j)
    End Function
End Class
