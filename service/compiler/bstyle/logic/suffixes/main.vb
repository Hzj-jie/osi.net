
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports statement = osi.service.compiler.statement(Of osi.service.compiler.logic.logic_writer)
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic.logic_writer)

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class main
        Implements statement

        Public Shared Sub register(ByVal p As statements)
            assert(Not p Is Nothing)
            p.register(New main())
        End Sub

        Public Sub export(ByVal o As logic_writer) Implements statement.export
            assert(builders.of_caller("main", vector.of(Of String)()).to(o))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
