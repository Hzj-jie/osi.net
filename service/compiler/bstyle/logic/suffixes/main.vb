
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class main
        Implements statement

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_gens)
            assert(Not p Is Nothing)
            p.register(New main())
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            builders.of_caller("main", vector.of(Of String)()).to(o)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
