
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates

Public NotInheritable Class scope_arguments
    Public Shared remove_unused_functions As argument(Of Boolean)
    Public Shared do_not_include_twice As argument(Of Boolean)

    Private Sub New()
    End Sub
End Class
