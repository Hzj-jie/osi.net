
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

        Public Shared ReadOnly biguint As String = unique_name("biguint")
        Public Shared ReadOnly [string] As String = unique_name("string")

        Private Shared ReadOnly v As vector(Of pair(Of String, String))
        Private ReadOnly ta As type_alias

        Shared Sub New()
            v = vector.of(
                pair.emplace_of(biguint, types.biguint),
                pair.emplace_of([string], types.string)
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@temps@" + name
        End Function

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_rule_wrapper)
            assert(Not p Is Nothing)
            assert(Not l Is Nothing)
            p.register(New temps(l.type_alias))
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                builders.of_define(ta, v(i).first, v(i).second).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New(ByVal ta As type_alias)
            assert(Not ta Is Nothing)
            Me.ta = ta
        End Sub
    End Class
End Class
