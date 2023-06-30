
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class boolean_cache(Of b As _boolean)
    Public Shared ReadOnly v As Boolean = +(alloc(Of b)())

    Private Sub New()
    End Sub
End Class
