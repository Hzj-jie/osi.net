
Option Explicit On
Option Infer Off
Option Strict On

Public MustInherit Class _comparer(Of T1, T2)
    Inherits __do(Of T1, T2, Int32)
End Class

Public MustInherit Class _comparer(Of T)
    Inherits __do(Of T, T, Int32)
End Class
