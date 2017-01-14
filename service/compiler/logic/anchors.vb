
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class anchors
        Private ReadOnly begins As map(Of String, UInt32)
        Private ReadOnly ends As map(Of String, UInt32)

        Public Sub New()
            begins = New map(Of String, UInt32)()
            ends = New map(Of String, UInt32)()
        End Sub

        Public Function define_begin(ByVal name As String, ByVal o As vector(Of String)) As Boolean
            assert(Not o Is Nothing)
            If begins.find(name) = begins.end() Then
                begins(name) = o.size()
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub define_end(ByVal name As String, ByVal o As vector(Of String))
            assert(Not o Is Nothing)
            assert(ends.find(name) = ends.end())
            ends(name) = o.size()
        End Sub

        Public Function retrieve_begin(ByVal name As String, ByRef pos As UInt32) As Boolean
            Return begins.find(name, pos)
        End Function

        Public Function begin(ByVal name As String) As UInt32
            Dim o As UInt32 = 0
            assert(retrieve_begin(name, o))
            Return o
        End Function

        Public Function retrieve_end(ByVal name As String, ByRef pos As UInt32) As Boolean
            Return ends.find(name, pos)
        End Function

        Public Function [end](ByVal name As String) As UInt32
            Dim o As UInt32 = 0
            assert(retrieve_end(name, o))
            Return o
        End Function
    End Class
End Namespace
