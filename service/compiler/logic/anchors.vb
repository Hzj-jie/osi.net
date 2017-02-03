
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class anchors
        Private ReadOnly begins As map(Of String, UInt32)

        Public Sub New()
            begins = New map(Of String, UInt32)()
        End Sub

        Public Sub clear()
            begins.clear()
        End Sub

        Public Function define(ByVal name As String, ByVal o As vector(Of String)) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            assert(Not o Is Nothing)
            If begins.find(name) = begins.end() Then
                begins(name) = o.size()
                Return True
            Else
                Return False
            End If
        End Function

        Public Function retrieve(ByVal name As String, ByRef pos As UInt32) As Boolean
            Return begins.find(name, pos)
        End Function

        Default Public ReadOnly Property D(ByVal name As String) As UInt32
            Get
                Dim o As UInt32 = 0
                assert(retrieve(name, o))
                Return o
            End Get
        End Property
    End Class
End Namespace
