
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading

Public NotInheritable Class typed_once_action(Of T)
    Private Const _true As Int32 = 1
    Private Const _false As Int32 = 0
    Private Shared v As Int32 = _false

    Public Shared Function first() As Boolean
        Return Interlocked.CompareExchange(v, _true, _false) = _false
    End Function

    Public Shared Sub [do](ByVal i As Action)
        assert(Not i Is Nothing)
        If first() Then
            i()
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
