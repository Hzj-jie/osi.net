
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class paramlist
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of paramlist)()
        End Sub

        Public Function build(ByVal n As typed_node) As vector(Of builders.parameter)
            assert(Not n Is Nothing)
            Dim v As New vector(Of builders.parameter)()
            Dim i As UInt32 = 0
            While i < n.child_count()
                v.emplace_back(l.typed_code_gen(Of param)().build(n.child(i)))
                i += uint32_1
            End While
            Return v
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(False)
            Return False
        End Function
    End Class
End Class
