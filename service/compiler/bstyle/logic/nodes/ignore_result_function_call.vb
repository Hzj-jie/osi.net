
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.automata

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class ignore_result_function_call(Of BUILDER As func_t(Of String, logic_writer, Boolean),
                                                               CODE_GENS As func_t(Of code_gens(Of logic_writer)),
                                                               T As scope(Of logic_writer, BUILDER, CODE_GENS, T))
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return function_call(Of BUILDER, CODE_GENS, T).without_return(n.child(), o)
        End Function
    End Class
End Class
