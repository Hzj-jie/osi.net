
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class heap_clause
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of heap_clause)()
        End Sub

        Public Function copy(ByVal index As typed_node,
                             ByVal f As Func(Of String, Boolean),
                             ByVal o As writer) As Boolean
            Return l.typed_code_gen(Of heap_name).build(index, o, f)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            assert(n.child(0).child_count() = 4)
            Dim name As typed_node = n.child(0).child(0)
            Dim index As typed_node = n.child(0).child(2)
            Return l.typed_code_gen(Of value_clause).build(
                       name,
                       n.child(2),
                       Function(ByVal r As vector(Of String)) As Boolean
                           Return l.typed_code_gen(Of struct).copy_heap(r, name.word().str(), index, o)
                       End Function,
                       Function(ByVal r As String) As Boolean
                           Return copy(index,
                                       Function(ByVal indexstr As String) As Boolean
                                           ' Return builders.of_copy_heap_in(name.word().str(), indexstr, r).to(o)
                                       End Function,
                                       o)
                       End Function,
                       o)
        End Function
    End Class
End Class
