
Imports osi.root.formation
Imports osi.root.connector

Namespace fullstack.executor
    Public Class variables
        Private ReadOnly values As vector(Of variable)

        Public Sub New()
            values = New vector(Of variable)()
        End Sub

        Public Sub push(ByVal v As variable)
            assert(Not v Is Nothing)
            values.emplace_back(v)
        End Sub

        Public Sub pop()
            assert(Not empty())
            values.pop_back()
        End Sub

        Public Sub replace(ByVal i As UInt32, ByVal v As variable)
            assert(Not v Is Nothing)
            assert(i < size())
            assert(v.is_type(values(i).type))
            values(i) = v
        End Sub

        Default Public ReadOnly Property v(ByVal i As UInt32) As variable
            Get
                assert(i < size())
                Return values(i)
            End Get
        End Property

        Public Function size() As UInt32
            Return values.size()
        End Function

        Public Function empty() As Boolean
            Return values.empty()
        End Function
    End Class
End Namespace
