
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class group
        Private ReadOnly m As matcher
        Private ReadOnly u As matcher

        Public Sub New(ByVal m As matcher, ByVal u As matcher)
            assert(Not m Is Nothing)
            assert(Not u Is Nothing)
            Me.m = m
            Me.u = u
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32)
            Dim mr As [optional](Of UInt32) = Nothing
            mr = m.match(i, pos)
            assert(Not mr Is Nothing)
            If Not mr Then
                Return [optional].of(Of UInt32)()
            End If
            If u.match(i, pos) Then
                Return [optional].of(Of UInt32)()
            End If
            Return mr
        End Function

        ' Process a,b,c|d,e,f
        Public Shared Function [of](ByVal s As String) As group

        End Function
    End Class
End Class
