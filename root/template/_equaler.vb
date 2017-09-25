
Option Explicit On
Option Infer Off
Option Strict On

Public MustInherit Class _equaler(Of T1, T2)
    Inherits __do(Of T1, T2, Boolean)
End Class

Public MustInherit Class _equaler(Of T)
    Inherits __do(Of T, T, Boolean)
End Class