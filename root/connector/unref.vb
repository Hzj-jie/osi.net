
Option Explicit On
Option Infer Off
Option Strict On

Public Module _unref
    ' Convert a ByRef parameter to a ByVal one.
    ' Say,
    ' Sub do_something(ByRef i As Int32)
    ' End Sub
    '
    ' Sub caller()
    '     Dim changeable As Int32 = 0
    '     Dim unchangeable As Int32 = 1
    '     do_something(changeable)
    '     do_something(unref(unchangeable))
    ' End
    Public Function unref(Of T)(ByVal a As T) As T
        Return a
    End Function
End Module
