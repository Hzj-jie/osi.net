
Imports osi.root.connector

Namespace turing.executor
    Public Class pop
        Implements instruction

        Private ReadOnly location As location

        Public Sub New(ByVal location As location)
            assert(Not location Is Nothing)
            Me.location = location
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            If processor.space.empty() Then
                Return False
            Else
                Dim var As variable = Nothing
                If processor.variable(location, var) AndAlso
                   var.set_value(processor.space.back()) Then
                    processor.space.pop_back()
                    Return True
                Else
                    Return False
                End If
            End If
        End Function
    End Class
End Namespace
