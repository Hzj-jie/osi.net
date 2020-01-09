
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class read_scoped(Of T)
    Private ReadOnly s As stack(Of T)

    Public Sub New()
        s = New stack(Of T)()
    End Sub

    Public Sub push(ByVal v As T)
        s.emplace(v)
    End Sub

    Public NotInheritable Class ref
        Implements IDisposable

        Private Shared c As UInt32 = 0
        Private ReadOnly r As read_scoped(Of T)
        Private ReadOnly v As T

        Public Sub New(ByVal r As read_scoped(Of T))
            c += uint32_1
            assert(Not r Is Nothing)
            assert(Not r.s.empty())
            Me.r = r
            Me.v = r.s.back()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            assert(c > uint32_0)
            c -= uint32_1
            r.s.pop()
        End Sub

        Public Shared Function pending_dispose() As UInt32
            Return c
        End Function

        Public Shared Operator +(ByVal this As ref) As T
            assert(Not this Is Nothing)
            Return this.v
        End Operator
    End Class

    Public Function pop() As ref
        assert(size() > ref.pending_dispose)
        Return New ref(Me)
    End Function

    Public Function size() As UInt32
        Return s.size()
    End Function
End Class
