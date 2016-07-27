
Imports osi.root.connector

Namespace fullstack.executor
    Public Class location
        Private ReadOnly level As UInt32
        Private ReadOnly offset As UInt32

        Public Sub New(ByVal level As UInt32,
                       ByVal offset As UInt32)
            Me.level = level
            Me.offset = offset
        End Sub

        Default Public ReadOnly Property v(ByVal domain As domain) As variable
            Get
                Return [get](domain)
            End Get
        End Property

        Public Function [get](ByVal domain As domain) As variable
            assert(Not domain Is Nothing)
            Return domain.variable(level, offset)
        End Function

        Public Sub replace(ByVal domain As domain, ByVal v As variable)
            assert(Not domain Is Nothing)
            domain.replace(level, offset, v)
        End Sub
    End Class
End Namespace
