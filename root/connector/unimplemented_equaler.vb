
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class unimplemented_equaler(Of T1, T2)
    Inherits _equaler(Of T1, T2)

    Public Overrides Function at(ByRef i As T1, ByRef j As T2) As Boolean
        assert(False)
        Return False
    End Function
End Class

Public NotInheritable Class unimplemented_equaler(Of T)
    Inherits _equaler(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Boolean
        assert(False)
        Return False
    End Function
End Class