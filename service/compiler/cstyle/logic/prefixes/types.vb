
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports logic_builder = osi.service.compiler.logic.builders

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class types
        Implements prefix

        Public Const bigint As String = "bigint"
        Public Const biguint As String = "biguint"
        Public Const uint As String = "uint"
        Public Const int As String = "int"
        Public Const bool As String = "bool"
        Public Const [byte] As String = "byte"

        Private Shared ReadOnly v As vector(Of pair(Of String, Int32))

        Shared Sub New()
            v = vector.of(
                emplace_make_pair(uint, 4),
                emplace_make_pair(int, 4),
                emplace_make_pair(bool, 1),
                emplace_make_pair([byte], 1),
                emplace_make_pair(bigint, 0),
                emplace_make_pair(biguint, 0)
            )
        End Sub

        Public Shared Sub register(ByVal p As prefixes)
            assert(Not p Is Nothing)
            p.register(New types())
        End Sub

        Public Sub export(ByVal o As writer) Implements prefix.export
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
