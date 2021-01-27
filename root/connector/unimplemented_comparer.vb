
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class unimplemented_comparer(Of T1, T2)
    Inherits _comparer(Of T1, T2)

    Public Overrides Function at(ByRef i As T1, ByRef j As T2) As Int32
        assert(False)
        Return 0
    End Function
End Class

Public NotInheritable Class unimplemented_comparer(Of T)
    Inherits _comparer(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Int32
        assert(False)
        Return 0
    End Function
End Class
