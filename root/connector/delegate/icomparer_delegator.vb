
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic

Public NotInheritable Class icomparer_delegator(Of T)
    Implements IComparer(Of T)

    Private ReadOnly f As Func(Of T, T, Int32)

    Public Sub New(ByVal f As Func(Of T, T, Int32))
        assert(f IsNot Nothing)
        Me.f = f
    End Sub

    Public Function Compare(ByVal x As T, ByVal y As T) As Int32 Implements IComparer(Of T).Compare
        Return f(x, y)
    End Function
End Class

Public NotInheritable Class icomparer_delegator
    Public Shared Function [of](Of T)(ByVal f As Func(Of T, T, Int32)) As icomparer_delegator(Of T)
        Return New icomparer_delegator(Of T)(f)
    End Function
    Private Sub New()
    End Sub
End Class
