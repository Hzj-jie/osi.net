
Imports osi.root.connector

Namespace turing.executor
    Public Class [set]
        Implements instruction

        Private ReadOnly destination As location
        Private ReadOnly source As location

        Public Sub New(ByVal destination As location, ByVal source As location)
            assert(Not destination Is Nothing)
            assert(Not source Is Nothing)
            Me.destination = destination
            Me.source = source
        End Sub

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            assert(Not processor Is Nothing)
            Dim source_var As variable = Nothing
            Dim destination_var As variable = Nothing
            Return processor.variable(source, source_var) AndAlso
                   processor.variable(destination, destination_var) AndAlso
                   destination_var.set_value(source_var) AndAlso
                   processor.move_next()
        End Function
    End Class
End Namespace
