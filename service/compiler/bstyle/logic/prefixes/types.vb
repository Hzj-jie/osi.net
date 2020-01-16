
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports logic_builder = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class types
        Implements statement

        Public Const bigint As String = "bigint"
        Public Const biguint As String = "biguint"
        Public Const uint As String = "uint"
        Public Const int As String = "int"
        Public Const bool As String = "bool"
        Public Const [byte] As String = "byte"
        Public Const [string] As String = "string"
        Public Const float As String = "float"
        Public Const void As String = "void"

        Private Shared ReadOnly v As vector(Of pair(Of String, Int32))
        Private Shared ReadOnly type_0_s As vector(Of String)
        Private Shared ReadOnly type_asterisk_s As vector(Of String)

        Shared Sub New()
            v = vector.of(
                pair.emplace_of(uint, 4),
                pair.emplace_of(int, 4),
                pair.emplace_of(bool, 1),
                pair.emplace_of([byte], 1),
                pair.emplace_of(float, 8)
            )
            type_0_s = vector.of(void)
            type_asterisk_s = vector.of(bigint, biguint, [string])
        End Sub

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_rule_wrapper)
            assert(Not p Is Nothing)
            assert(Not l Is Nothing)
            p.register(New types(l.type_alias))
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                assert(v(i).second >= 0)
                logic_builder.of_type(v(i).first, CUInt(v(i).second)).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New(ByVal ta As type_alias)
            assert(Not ta Is Nothing)
            Dim i As UInt32 = 0
            While i < type_0_s.size()
                assert(ta.define(type_0_s(i), "type0"))
                i += uint32_1
            End While
            i = 0
            While i < type_asterisk_s.size()
                assert(ta.define(type_asterisk_s(i), "type*"))
                i += uint32_1
            End While
        End Sub
    End Class
End Class
