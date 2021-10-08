
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class typedef
        Implements logic_gen

        Private ReadOnly ta As type_alias

        Private Sub New(ByVal p As parameters_t)
            assert(Not p Is Nothing)
            ta = p.type_alias
            assert(Not ta Is Nothing)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal p As parameters_t)
            assert(Not b Is Nothing)
            b.register(New typedef(p))
        End Sub

        Private Shared Function get_type(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            If n.child().type_name.Equals("string") Then
                Return n.child().word().str().Trim(character.quote).c_unescape()
            End If
            If n.child().type_name.Equals("name") Then
                Return n.child().word().str()
            End If
            assert(False)
            Return Nothing
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Return ta.define(get_type(n.child(2)), get_type(n.child(1)))
        End Function
    End Class

    Public NotInheritable Class typedef_with_semi_colon
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of typedef_with_semi_colon)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return l.of(n.child(0)).build(o)
        End Function
    End Class
End Class
