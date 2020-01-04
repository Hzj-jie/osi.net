
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class write_scoped(Of T)
    Private ReadOnly s As stack(Of T)

    Public Sub New()
        s = New stack(Of T)()
    End Sub

    Public NotInheritable Class ref
        Implements IDisposable

        Private ReadOnly r As write_scoped(Of T)
        Private ReadOnly v As T

        Public Sub New(ByVal r As write_scoped(Of T))
            assert(Not r Is Nothing)
            assert(Not r.s.empty())
            Me.r = r
            Me.v = r.s.back()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            r.s.pop()
        End Sub

        Public Shared Operator +(ByVal this As ref) As T
            assert(Not this Is Nothing)
            Return this.v
        End Operator
    End Class

    Public Function push(ByVal v As T) As ref
        s.emplace(v)
        Return New ref(Me)
    End Function

    Public Function current() As T
        assert(Not s.empty())
        Return s.back()
    End Function
End Class
