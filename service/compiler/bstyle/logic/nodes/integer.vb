
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [integer]
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [integer])()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim i As Int32 = 0
            If Not Int32.TryParse(n.word().str(), i) Then
                raise_error(error_type.user, "Cannot parse data to int ", n.trace_back_str())
                Return False
            End If
            builders.of_copy_const(l.typed_code_gen(Of value)().
                     with_internal_typed_temp_target(code_types.int, n, o), New data_block(i)).
                     to(o)
            Return True
        End Function
    End Class
End Class
