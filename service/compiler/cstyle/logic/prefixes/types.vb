
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class types
        Implements prefix

        Public Const bigint As String = "bigint"
        Public Const uint As String = "uint"
        Public Const int As String = "int"
        Public Const bool As String = "bool"
        Public Const [byte] As String = "byte"

        Private Shared ReadOnly v As vector(Of pair(Of String, Int32))

        Shared Sub New()
            v = vector.of(
                emplace_make_pair(bigint, 0),
                emplace_make_pair(uint, 4),
                emplace_make_pair(int, 4),
                emplace_make_pair(bool, 1),
                emplace_make_pair([byte], 1)
            )
        End Sub

        Public Shared Sub register()
            prefixes.register(New types())
        End Sub

        Public Sub export(ByVal o As writer) Implements prefix.export
            Dim i As UInt32 = 0
            While i < v.size()
                o.append("type", v(i).first, v(i).second)
                i += uint32_1
            End While
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
