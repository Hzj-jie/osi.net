
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public MustInherit Class raw_value(Of _CODE_TYPE As func_t(Of String),
                                          _TEMP_TARGET As func_t(Of String, logic_writer, String))
        Implements code_gen(Of logic_writer)

        Private Shared ReadOnly code_type As String = assert_which.of(alloc(Of _CODE_TYPE)().run()).not_null_or_empty()
        Private Shared ReadOnly temp_target As Func(Of String, logic_writer, String) =
                AddressOf alloc(Of _TEMP_TARGET)().run

        Protected MustOverride Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean

        Protected Shared Function build(ByVal i As data_block, ByVal o As logic_writer) As Boolean
            assert(Not i Is Nothing)
            assert(Not o Is Nothing)
            Return compiler.logic.builders.of_copy_const(temp_target(code_type, o), i).to(o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim d As data_block = Nothing
            If Not parse(n, d) Then
                raise_error(error_type.user, "Cannot parse data to ", code_type, " ", n.trace_back_str())
                Return False
            End If
            Return build(d, o)
        End Function
    End Class
End Class
