
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Namespace logic
    Public NotInheritable Class writer
        Inherits object_list_writer(Of debug_dump)

        Public NotInheritable Class debug_dump
            Inherits __void(Of String)

            Public Overrides Sub at(ByRef s As String)
                If builders.debug_dump Then
                    raise_error(error_type.user, "Debug dump of logic ", s)
                End If
            End Sub
        End Class
    End Class
End Namespace
