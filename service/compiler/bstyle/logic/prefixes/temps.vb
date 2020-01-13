
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
        Private ReadOnly l As logic_gens

        Shared Sub New()
            v = vector.of(
                emplace_make_pair(bigint, types.bigint),
                emplace_make_pair(biguint, types.biguint)
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@temps@" + name
        End Function

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_gens)
            assert(Not p Is Nothing)
            p.register(New temps(l))
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                l.define_variable(v(i).first, v(i).second, o)
                i += uint32_1
            End While
        End Sub

        Private Sub New(ByVal l As logic_gens)
            assert(Not l Is Nothing)
            Me.l = l
        End Sub
    End Class
End Class
