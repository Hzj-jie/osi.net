
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class class_t
            Private ReadOnly m As unordered_map(Of String, definition)

            Private NotInheritable Class definition
            End Class

        End Class

        Public Structure class_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub
        End Structure

        Public Function classes() As class_proxy
            Return New class_proxy(Me)
        End Function
    End Class
End Class
