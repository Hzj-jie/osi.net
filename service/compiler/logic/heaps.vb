

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    ' Shared between copy_array and define_array.
    Public NotInheritable Class heaps
        Public Shared Function name_of(ByVal name As String) As String
            Return strcat("@", name, "[]")
        End Function

        ' TODO: If a dedicated type is better.
        Public Shared ReadOnly type As String = types.variable_type

        Private Sub New()
        End Sub
    End Class
End Namespace
