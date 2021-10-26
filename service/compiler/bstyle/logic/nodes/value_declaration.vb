
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
            Return build(n.child(0), n.child(1), o)
        End Function

        Public Function build(ByVal type As typed_node, ByVal name As typed_node, ByVal o As writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            assert(type.leaf())
            assert(name.leaf())
            Dim t As String = type.word().str()
            Dim n As String = name.word().str()
            Return l.typed_code_gen(Of struct).define_in_stack(t, n, o) OrElse
                   declare_single_data_slot(t, n, o)
        End Function

        Public Shared Function declare_single_data_slot(ByVal type As String,
                                                      ByVal name As String,
                                                      ByVal o As writer) As Boolean
            If scope.current().structs().defined(type) Then
                Return False
            End If
            assert(Not o Is Nothing)
            If Not scope.current().variables().define(type, name) Then
                Return False
            End If
            builders.of_define(name,
                               scope.current().type_alias()(type)).
                     to(o)
            Return True
        End Function
    End Class
End Class
