
Imports osi.root.connector

Namespace turing.executor
    Public Class goto_if
        Implements instruction

        Private ReadOnly var_loc As location
        Private ReadOnly true_loc As location
        Private ReadOnly false_loc As location

        Public Sub New(ByVal var_loc As location,
                       ByVal true_loc As location,
                       ByVal false_loc As location)
            assert(Not var_loc Is Nothing)
            assert(Not true_loc Is Nothing)
            assert(Not false_loc Is Nothing)
            Me.var_loc = var_loc
            Me.true_loc = true_loc
            Me.false_loc = false_loc
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            Dim var As variable = Nothing
            Return processor.variable(var_loc, var) AndAlso
                   If(var.true(), processor.move(true_loc), processor.move(false_loc))
        End Function
    End Class
End Namespace
