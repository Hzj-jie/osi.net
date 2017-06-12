
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public Class default_comparer(Of T1, T2)
    Inherits _comparer(Of T1, T2)

    Public Overrides Function at(ByRef i As T1, ByRef j As T2) As Int32
        Return compare(i, j)
    End Function
End Class

Public Class default_comparer(Of T)
    Inherits _comparer(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Int32
        Return compare(i, j)
    End Function
End Class
