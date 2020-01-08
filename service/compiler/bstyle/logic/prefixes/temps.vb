
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class temps
        Implements statement

        Public Shared ReadOnly bigint As String = unique_name("bigint")
        Public Shared ReadOnly biguint As String = unique_name("biguint")

        Private Shared ReadOnly v As vector(Of pair(Of String, String))

        Shared Sub New()
            v = vector.of(
                emplace_make_pair(bigint, types.bigint),
                emplace_make_pair(biguint, types.biguint)
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@temps@" + name
        End Function

        Public Shared Sub register(ByVal p As statements)
            assert(Not p Is Nothing)
            p.register(New temps())
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                builders.of_define(v(i).first, v(i).second).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
