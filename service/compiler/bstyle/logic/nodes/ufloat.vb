
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive
Imports osi.service.math

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class ufloat
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of ufloat)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim s As String = Nothing
            s = n.word().str()
            s = strmid(s, 0, strlen(s) - uint32_1)
            Dim i As big_udec = Nothing
            If Not big_udec.parse(s, i) Then
                raise_error(error_type.user, "Cannot parse data to bigufloat ", n.trace_back_str())
                Return False
            End If
            builders.of_copy_const(code_gen_of(Of value)().with_temp_target(types.ufloat, n, o),
                                   New data_block(i.as_bytes())).to(o)
            Return True
        End Function
    End Class
End Class
