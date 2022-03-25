
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class includes_t
        Private ReadOnly _included As New unordered_set(Of String)()

        Public Function should_include(ByVal f As String) As Boolean
            Return _included.emplace(f).second()
        End Function
    End Class
End Class
