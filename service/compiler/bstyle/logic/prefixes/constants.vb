
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class constants
        Implements statement

        Public Shared ReadOnly int_0 As String = unique_name("int_0")
        Public Shared ReadOnly int_1 As String = unique_name("int_1")
        Public Shared ReadOnly size_of_int As String = unique_name("size_of_int")
        Public Shared ReadOnly size_of_long As String = unique_name("size_of_long")
        Public Shared ReadOnly size_of_bool As String = unique_name("size_of_bool")
        Public Shared ReadOnly size_of_byte As String = unique_name("size_of_byte")
        Public Shared ReadOnly size_of_float As String = unique_name("size_of_float")
        Public Shared ReadOnly size_of_char As String = unique_name("size_of_char")

        Private Shared ReadOnly v As vector(Of def)
        Private ReadOnly ta As type_alias

        Shared Sub New()
            v = vector.of(
                New def(types.int, int_0, New data_block(0)),
                New def(types.int, int_1, New data_block(1)),
                New def(types.int, size_of_int, New data_block(4)),
                New def(types.int, size_of_long, New data_block(8)),
                New def(types.int, size_of_bool, New data_block(1)),
                New def(types.int, size_of_byte, New data_block(1)),
                New def(types.int, size_of_float, New data_block(8)),
                New def(types.int, size_of_char, New data_block(2))
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@constants@" + name
        End Function

        Private NotInheritable Class def
            Public ReadOnly type As String
            Public ReadOnly name As String
            Public ReadOnly value As data_block

            Public Sub New(ByVal type As String, ByVal name As String, ByVal value As data_block)
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                assert(Not value Is Nothing)
                Me.type = type
                Me.name = name
                Me.value = value
            End Sub
        End Class

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_rule_wrapper)
            assert(Not p Is Nothing)
            assert(Not l Is Nothing)
            p.register(New constants(l.type_alias))
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                builders.of_define(ta, v(i).name, v(i).type).to(o)
                builders.of_copy_const(v(i).name, v(i).value).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New(ByVal ta As type_alias)
            assert(Not ta Is Nothing)
            Me.ta = ta
        End Sub
    End Class
End Class
