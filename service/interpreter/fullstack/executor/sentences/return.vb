
Imports osi.root.connector

Namespace fullstack.executor
    Public Class [return]
        Implements sentence

        Private ReadOnly location As location
        Private ReadOnly level As UInt32

        Public Sub New(ByVal location As location,
                       ByVal level As UInt32)
            assert(Not location Is Nothing)
            Me.location = location
            Me.level = level
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            domain.ancestor(level).replace_last(location(domain))
            Throw break_exception.instance
        End Sub
    End Class
End Namespace
