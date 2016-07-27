
Imports osi.root.formation
Imports osi.root.connector

Namespace fullstack.parser
    Public Class types
        Private ReadOnly m As map(Of String, type)

        Public Sub New()
            m = New map(Of String, type)()
        End Sub

        Public Function define(ByVal name As String, ByVal type As type) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            assert(Not type Is Nothing)
            If m.find(name) = m.end() Then
                m(name) = type
                Return True
            Else
                Return False
            End If
        End Function

        Public Function resolve(ByVal name As String, ByRef type As type) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            Dim it As map(Of String, type).iterator = Nothing
            it = m.find(name)
            If it = m.end() Then
                Return False
            Else
                type = (+it).second
                assert(Not type Is Nothing)
                Return True
            End If
        End Function
    End Class
End Namespace
