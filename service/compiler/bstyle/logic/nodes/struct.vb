
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct
        Inherits logic_gen_wrapper
        Implements logic_gen

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of struct)()
        End Sub

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Function pack(ByVal source As String, ByVal target As String, ByVal o As writer) As Boolean
            Dim vs As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(source, vs) Then
                Return False
            End If
            assert(Not vs Is Nothing)
            vs.stream().foreach(Sub(ByVal p As builders.parameter)
                                    assert(Not p Is Nothing)
                                    builders.of_append_slice(p.name, target).to(o)
                                End Sub)
            Return True
        End Function

        Public Function unpack(ByVal source As String, ByVal target As String, ByVal o As writer) As Boolean
            Dim vs As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(target, vs) Then
                Return False
            End If
            assert(Not vs Is Nothing)
            Using New scope_wrapper()
                Dim index As String = strcat(target, "@index")
                assert(code_gen_of(Of value_declaration).
                           declare_internal_typed_variable(New builders.parameter(code_types.int, index), o))
                vs.stream().foreach(Sub(ByVal p As builders.parameter)
                                        assert(Not p Is Nothing)
                                        builders.of_cut_slice(source, index, p.name).to(o)
                                        builders.of_add(index, index, "@@prefixes@constants@int_1").to(o)
                                    End Sub)
            End Using
            Return True
        End Function

        Public Function export(ByVal n As typed_node, ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            If n.child_count() > 2 Then
                ' TODO: Support value-definition
                Return False
            End If
            assert(n.child_count() = 2)
            Dim v As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(n.child(0).word().str(), n.child(1).word().str(), v) Then
                Return False
            End If
            If Not scope.current().variables().define(n.child(0).word().str(), n.child(1).word().str()) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return v.stream().
                     map(Function(ByVal m As builders.parameter) As Boolean
                             Return code_gen_of(Of value_declaration)().declare_internal_typed_variable(m, o)
                         End Function).
                     aggregate(bool_stream.aggregators.all_true)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Return scope.current().
                         structs().
                         define(n.child(1).word().str(),
                                streams.range_closed(CUInt(3), n.child_count() - CUInt(3)).
                                        map(Function(ByVal index As Int32) As builders.parameter
                                                ' TODO: Support value_definition.
                                                Dim c As typed_node = n.child(CUInt(index))
                                                assert(Not c Is Nothing)
                                                assert(c.child_count() = 2)
                                                assert(c.child(0).child_count() = 2)
                                                Return New builders.parameter(c.child(0).child(0).word().str(),
                                                                              c.child(0).child(1).word().str())
                                            End Function).
                                        collect(Of vector(Of builders.parameter))())
        End Function
    End Class
End Class
