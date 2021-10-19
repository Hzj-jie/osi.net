
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class read_scoped(Of T)
        Private ReadOnly s As stack(Of T)
        Private pending_dispose As UInt32

        Public Sub New()
            s = New stack(Of T)()
            pending_dispose = 0
        End Sub

        Public Sub push(ByVal v As T)
            s.emplace(v)
        End Sub

        Public Class ref(Of RT)
            Implements IDisposable

            Private ReadOnly r As read_scoped(Of T)
            Private ReadOnly v As RT

            Public Sub New(ByVal r As read_scoped(Of T), ByVal f As Func(Of T, RT))
                assert(Not r Is Nothing)
                assert(Not r.s.empty())
                assert(Not f Is Nothing)
                Me.r = r
                Me.v = f(r.s.back())
                r.pending_dispose += uint32_1
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                r.pending_dispose -= uint32_1
                r.s.pop()
            End Sub

            Public Shared Operator +(ByVal this As ref(Of RT)) As RT
                assert(Not this Is Nothing)
                Return this.v
            End Operator
        End Class

        Public NotInheritable Class ref
            Inherits ref(Of T)

            Public Sub New(ByVal r As read_scoped(Of T))
                MyBase.New(r, Function(ByVal x As T) As T
                                  Return x
                              End Function)
            End Sub
        End Class

        Public Function pop() As ref
            assert(size() > pending_dispose)
            Return New ref(Me)
        End Function

        Public Function pop(Of RT)(ByVal f As Func(Of T, RT)) As ref(Of RT)
            assert(size() > pending_dispose)
            Return New ref(Of RT)(Me, f)
        End Function

        Public Function size() As UInt32
            Return s.size()
        End Function
    End Class
End Namespace
