
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Namespace logic
    ' Shared between copy_array and define_array.
    Public NotInheritable Class heaps
        Public Shared Function name_of(ByVal name As String) As String
            Return strcat("@", name, "[]")
        End Function

        Public Shared Function original_name_of(ByVal name As String) As String
            assert(Not name.null_or_whitespace())
            assert(name.StartsWith("@"))
            assert(name.EndsWith("[]"))
            Return name.strmid(uint32_1, CUInt(name.Length() - 3))
        End Function

        ' TODO: If a dedicated type is better.
        Public Shared ReadOnly ptr_type As String = types.variable_type

        Private Sub New()
        End Sub
    End Class
End Namespace
