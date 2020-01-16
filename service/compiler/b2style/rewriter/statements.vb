
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.compiler.rewriters
Imports osi.service.resource

Partial Public NotInheritable Class b2style
    Public NotInheritable Class prefix
        Implements statement

        Private Shared ReadOnly instance As prefix

        Shared Sub New()
            instance = New prefix()
        End Sub

        Public Shared Sub register(ByVal p As statements)
            assert(Not p Is Nothing)
            p.register(instance)
        End Sub

        Public Sub export(ByVal o As typed_node_writer) Implements statement(Of typed_node_writer).export
            assert(Not o Is Nothing)
            o.append(b2style_statements.prefix.as_text())
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class suffix
        Implements statement

        Private Shared ReadOnly instance As suffix

        Shared Sub New()
            instance = New suffix()
        End Sub

        Public Shared Sub register(ByVal p As statements)
            assert(Not p Is Nothing)
            p.register(instance)
        End Sub

        Public Sub export(ByVal o As typed_node_writer) Implements statement(Of typed_node_writer).export
            assert(Not o Is Nothing)
            o.append(b2style_statements.suffix.as_text())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
