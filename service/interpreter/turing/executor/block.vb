
Imports osi.root.formation
Imports osi.root.utils

Namespace executor
    Public Class block
        Private ReadOnly instructions As vector(Of instruction)

        Public Sub New(ByVal instructions As vector(Of instruction))
            assert(Not instructions Is Nothing)
            Me.instructions = instructions
        End Sub

        Public Function variable(ByVal i As UInt32, ByRef var As variable) As Boolean
            If i < instructions.size() AndAlso
               TypeOf instructions(i) Is variable Then
                var = DirectCast(instructions(i), variable)
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
