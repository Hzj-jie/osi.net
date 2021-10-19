
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_declaration
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_declaration)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return code_gen_of(Of struct).export(n, o) OrElse
                   declare_internal_typed_variable(n, o)
        End Function

        Private Function declare_internal_typed_variable(ByVal type As String,
                                                         ByVal name As String,
                                                         ByVal o As writer) As Boolean
            assert(Not o Is Nothing)
            If Not scope.current().variables().define(type, name) Then
                Return False
            End If
            builders.of_define(name,
                               scope.current().type_alias()(type)).
                     to(o)
            Return True
        End Function

        Public Function declare_internal_typed_variable(ByVal n As typed_node, ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() >= 2)
            Return declare_internal_typed_variable(n.child(0).word().str(), n.child(1).word().str(), o)
        End Function

        Public Function declare_internal_typed_variable(ByVal p As builders.parameter, ByVal o As writer) As Boolean
            assert(Not p Is Nothing)
            Return declare_internal_typed_variable(p.type, p.name, o)
        End Function
    End Class
End Class
