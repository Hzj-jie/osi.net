
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Private NotInheritable Class _start_scope
        Implements instruction_gen

        Private ReadOnly p As paragraph

        Public Sub New(ByVal p As paragraph)
            assert(Not p Is Nothing)
            Me.p = p
        End Sub

        Private Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Return p.build(o)
        End Function
    End Class
End Class
