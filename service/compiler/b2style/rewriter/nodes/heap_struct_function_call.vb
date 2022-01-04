
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class heap_struct_function_call
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of heap_struct_function_call)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            Return l.typed_code_gen(Of function_call).build(heap_struct_name.bstyle_function(n.child(0)), n, o)
        End Function
    End Class
End Class
