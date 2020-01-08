
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

        Shared Sub New()
            v = vector.of(
                emplace_make_pair(uint, 4),
                emplace_make_pair(int, 4),
                emplace_make_pair(bool, 1),
                emplace_make_pair([byte], 1),
                emplace_make_pair(bigint, 0),
                emplace_make_pair(biguint, 0),
                emplace_make_pair([string], 0),
                emplace_make_pair(float, 8),
                emplace_make_pair(void, 0)
            )
        End Sub

        Public Shared Sub register(ByVal p As statements)
            assert(Not p Is Nothing)
            p.register(New types())
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                assert(v(i).second >= 0)
                logic_builder.of_type(v(i).first, CUInt(v(i).second)).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
