
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class start_scope
        Implements exportable

        Private ReadOnly p As paragraph

        Public Sub New(ByVal p As paragraph)
            assert(Not p Is Nothing)
            Me.p = p
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Return p.export(o)
        End Function
    End Class
End Namespace
