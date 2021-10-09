
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports logic_builder = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    ' Must supported types used in logic/nodes code generation.
    Public NotInheritable Class code_types
        Implements statement

        Public Const int As String = "Integer"
        Public Const biguint As String = "BigUnsignedInteger"
        Public Const bool As String = "Boolean"
        Public Const [string] As String = "String"
        Public Const ufloat As String = "BigUnsignedFloat"

        Private Shared ReadOnly v As vector(Of pair(Of String, UInt32)) = vector.of(
            type_of(int, 4),
            type_of(bool, 1),
            type_of(biguint, max_uint32 - 1),
            type_of(ufloat, max_uint32 - 2),
            type_of([string], max_uint32 - 3)
        )
        Private Shared ReadOnly type_asterisk_s As vector(Of String) = vector.of(Of String)()

        Private Shared Function type_of(ByVal name As String, ByVal size As UInt32) As pair(Of String, UInt32)
            Return pair.emplace_of(name, size)
        End Function

        Private Shared Function type_of(ByVal name As String, ByVal size As Int32) As pair(Of String, UInt32)
            Return type_of(name, assert_which.of(size).can_cast_to_uint32())
        End Function

        Private Shared Function type_of(ByVal name As String, ByVal size As Int64) As pair(Of String, UInt32)
            Return type_of(name, assert_which.of(size).can_cast_to_uint32())
        End Function

        Public Shared Sub register(ByVal p As statements, ByVal l As parameters_t)
            assert(Not p Is Nothing)
            assert(Not l Is Nothing)
            p.register(New code_types(l.type_alias))
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
            While i < type_asterisk_s.size()
                assert(ta.define(type_asterisk_s(i), "type*"))
                i += uint32_1
            End While
        End Sub
    End Class
End Class
