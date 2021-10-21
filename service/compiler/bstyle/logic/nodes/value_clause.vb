
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_clause
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_clause)()
        End Sub

        Public Function build(ByVal name As typed_node, ByVal value As typed_node, ByVal o As writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(name.leaf())
            If Not l.of(value).build(o) Then
                Return False
            End If
            Dim type As String = Nothing
            assert(scope.current().variables().resolve(name.word().str(), type))
            If scope.current().structs().defined(type) Then
                Using r As read_scoped(Of vector(Of String)).ref = code_gen_of(Of value)().read_target()
                    Return struct.move(+r, name.word().str(), o)
                End Using
            Else
                Using r As read_scoped(Of vector(Of String)).ref(Of String) =
                        code_gen_of(Of value)().read_target_internal_typed()
                    builders.of_move(name.word().str(), +r).to(o)
                End Using
                Return True
            End If
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            assert(n.child(0).leaf())
            Return build(n.child(0), n.child(2), o)
        End Function
    End Class
End Class
