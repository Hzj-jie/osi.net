
Imports osi.root.connector

Namespace fullstack.executor
    Public Class definition
        Implements sentence

        Private ReadOnly type As type

        Public Sub New(ByVal type As type)
            assert(Not type Is Nothing)
            Me.type = type
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            domain.define(New variable(type))
        End Sub
    End Class
End Namespace
