
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class param
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly rs As New read_scoped(Of vector(Of builders.parameter))

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of param)()
        End Sub

        Public Function current_target() As read_scoped(Of vector(Of builders.parameter)).ref
            Return rs.pop()
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Dim params As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(n.child(0).word().str(), n.child(1).word().str(), params) Then
                params = vector.of(New builders.parameter(n.child(0).word().str(), n.child(1).word().str()))
            End If
            rs.push(params)
            ' No parameter nesting expected, use read_scoped to reduce the cost of maintaining the state only.
            assert(rs.size() = 1)
            Return True
        End Function
    End Class
End Class
