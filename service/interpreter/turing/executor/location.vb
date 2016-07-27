
Namespace turing.executor
    Public Class location
        Public Enum def As Int32
            relative
            absolute
        End Enum

        Public ReadOnly type As def
        Public ReadOnly offset As Int32

        Public Sub New(ByVal type As def, ByVal offset As Int32)
            Me.type = type
            Me.offset = offset
        End Sub
    End Class
End Namespace
