
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders
Imports variable = osi.service.compiler.logic.variable

Partial Public NotInheritable Class bstyle
    ' TODO: Avoid publicizing struct to b2style.
    Public NotInheritable Class struct
        Implements code_gen(Of logic_writer)

        Private Shared Function copy(ByVal sources As vector(Of String),
                                     ByVal target As String,
                                     ByVal target_naming As Func(Of String, String),
                                     ByVal o As logic_writer) As Boolean
            assert(Not sources Is Nothing)
            assert(Not target.null_or_whitespace())
            assert(Not target_naming Is Nothing)
            Dim vs As scope.struct_def = Nothing
            If Not scope.current().structs().variables().resolve(target, vs) Then
                Return False
            End If
            assert(Not vs Is Nothing)
            If vs.primitive_count() <> sources.size() Then
                raise_error(error_type.user,
                            "Sources ",
                            sources,
                            " are not consistent with the type of ",
                            target,
                            ", ",
                            vs)
                Return False
            End If
            Return vs.primitives().
                      with_index().
                      map(Function(ByVal t As tuple(Of UInt32, builders.parameter)) As Boolean
                              Return builders.of_copy(target_naming(t.second().name), sources(t.first())).to(o)
                          End Function).
                      aggregate(bool_stream.aggregators.all_true)
        End Function

        Public Shared Function copy(ByVal sources As vector(Of String),
                                    ByVal target As String,
                                    ByVal o As logic_writer) As Boolean
            Return copy(sources,
                        target,
                        Function(ByVal n As String) As String
                            Return n
                        End Function,
                        o)
        End Function

        Public Shared Function copy(ByVal sources As vector(Of String),
                                    ByVal target As String,
                                    ByVal indexstr As String,
                                    ByVal o As logic_writer) As Boolean
            Return copy(sources,
                        target,
                        Function(ByVal n As String) As String
                            Return variable.name_of(n, indexstr)
                        End Function,
                        o)
        End Function

        Public Shared Function pack(ByVal sources As vector(Of String),
                                    ByVal target As String,
                                    ByVal o As logic_writer) As Boolean
            assert(Not sources Is Nothing)
            assert(Not target.null_or_whitespace())
            If sources.empty() Then
                Return True
            End If
            Return sources.stream().
                           map(Function(ByVal source As String) As Boolean
                                   assert(Not source.null_or_whitespace())
                                   Return builders.of_append_slice(target, source).to(o)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function

        Public Shared Function unpack(ByVal source As String,
                                      ByVal targets As vector(Of String),
                                      ByVal o As logic_writer) As Boolean
            assert(Not source.null_or_whitespace())
            assert(Not targets Is Nothing)
            If targets.empty() Then
                Return True
            End If
            Dim index As String = strcat(targets(0), "@index")
            assert(value_declaration.declare_primitive_type(_integer.type_name, index, o))
            Return targets.stream().
                           map(Function(ByVal target As String) As Boolean
                                   assert(Not target.null_or_whitespace())
                                   ' TODO: Should include bstyle_constant.h automatically.
                                   Return builders.of_cut_slice(target, source, index).to(o) AndAlso
                                          builders.of_add(index, index, "@@prefixes@constants@int_1").to(o)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function

        Private Shared Function define(ByVal type As String,
                                       ByVal name As String,
                                       ByVal v As scope.struct_def) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            assert(Not v Is Nothing)
            Return streams.of(scope.struct_def.nested(type, name)).
                           concat(v.nesteds()).
                           map(Function(ByVal s As builders.parameter) As Boolean
                                   assert(Not s Is Nothing)
                                   assert(Not s.ref)
                                   Return scope.current().variables().define(s.non_ref_type(), s.name)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function

        ' Forward the definition of, or declare the {type, name} pair in the scope.variable in stack.
        Public Shared Sub forward_in_stack(ByVal type_node As typed_node, ByVal name_node As typed_node)
            assert(Not type_node Is Nothing)
            assert(Not name_node Is Nothing)
            Dim type As String = type_node.input_without_ignored()
            ' May use word().str()
            Dim name As String = name_node.input_without_ignored()
            Dim v As scope.struct_def = Nothing
            If scope.current().structs().resolve(type, name, v) Then
                define(type, name, v)
            End If
        End Sub

        Public Shared Function define_in_stack(ByVal type_node As typed_node,
                                               ByVal name_node As typed_node,
                                               ByVal o As logic_writer) As Boolean
            assert(Not type_node Is Nothing)
            assert(Not name_node Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = type_node.input_without_ignored()
            ' May use word().str()
            Dim name As String = name_node.input_without_ignored()
            Dim v As scope.struct_def = Nothing
            If Not scope.current().structs().resolve(type, name, v) OrElse
               Not define(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return v.for_each_primitive(Function(ByVal m As builders.parameter) As Boolean
                                            assert(Not m Is Nothing)
                                            Return value_declaration.declare_primitive_type(m.non_ref_type(), m.name, o)
                                        End Function)
        End Function

        Public Shared Function define_in_heap(ByVal type_node As typed_node,
                                              ByVal name_node As typed_node,
                                              ByVal length As typed_node,
                                              ByVal o As logic_writer) As Boolean
            assert(Not type_node Is Nothing)
            assert(Not name_node Is Nothing)
            assert(Not length Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = type_node.input_without_ignored()
            ' May use word().str()
            Dim name As String = name_node.input_without_ignored()
            Dim v As scope.struct_def = Nothing
            If Not scope.current().structs().resolve(type, name, v) OrElse Not define(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return heap_name.build(length,
                                   o,
                                   Function(ByVal len_name As String) As Boolean
                                       assert(Not v Is Nothing)
                                       Return v.for_each_primitive(
                                                  Function(ByVal m As builders.parameter) As Boolean
                                                      assert(Not m Is Nothing)
                                                      Return heap_declaration.
                                                                 declare_primitive_type(m.non_ref_type(),
                                                                                        m.name,
                                                                                        len_name,
                                                                                        o)
                                                  End Function)
                                   End Function)
        End Function

        Public Shared Function dealloc_from_heap(ByVal name As String, ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)
            Dim v As scope.struct_def = Nothing
            If Not scope.current().structs().variables().resolve(name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return v.for_each_primitive(Function(ByVal m As builders.parameter) As Boolean
                                            assert(Not m Is Nothing)
                                            Return builders.of_dealloc_heap(m.name).to(o)
                                        End Function)
        End Function

        Public Shared Function undefine(ByVal name As String, ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)
            Dim v As scope.struct_def = Nothing
            If Not scope.current().structs().variables().resolve(name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return scope.current().variables().undefine(name) AndAlso
               v.for_each_primitive(Function(ByVal m As builders.parameter) As Boolean
                                        assert(Not m Is Nothing)
                                        Return scope.current().variables().undefine(m.name) AndAlso
                                               builders.of_undefine(m.name).to(o)
                                    End Function)
        End Function

        Public Shared Function redefine(ByVal name As String, ByVal type As String, ByVal o As logic_writer) As Boolean
            assert(Not o Is Nothing)
            Dim v As scope.struct_def = Nothing
            ' Note: the variable name should be suffixed by the variables in "type" struct.
            If Not scope.current().structs().resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return scope.current().variables().redefine(type, name) AndAlso
                   v.for_each_primitive(
                       Function(ByVal m As builders.parameter) As Boolean
                           assert(Not m Is Nothing)
                           Return scope.current().variables().redefine(m.non_ref_type(), m.name) AndAlso
                                  builders.of_redefine(m.name, m.non_ref_type()).to(o)
                       End Function)
        End Function

        Public Shared Function parse_struct_body(ByVal n As typed_node) As stream(Of builders.parameter)
            Return n.children_of("struct-body").
                     stream().
                     filter(Function(ByVal c As typed_node) As Boolean
                                assert(Not c Is Nothing)
                                assert(c.child_count() <= 2)
                                Return c.child_count() = 2
                            End Function).
                     map(Function(ByVal c As typed_node) As typed_node
                             Return c.child(0)
                         End Function).
                     map(Function(ByVal c As typed_node) As builders.parameter
                             ' TODO: Support value_definition.str_bytes_val
                             assert(Not c Is Nothing)
                             assert(c.type_name.Equals("value-declaration"))
                             assert(c.child_count() = 2)
                             Return builders.parameter.non_ref(c.child(0).input_without_ignored(),
                                                               c.child(1).input_without_ignored())
                         End Function)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Return scope.current().structs().define(
                       scope.normalized_type.parameter_type_of(n.child(1)).full_type(),
                       parse_struct_body(n).map(AddressOf scope.struct_def.nested).
                                            collect_to(Of vector(Of builders.parameter))(),
                       Sub(ByVal type As String, ByVal size As UInt32)
                           assert(builders.of_type(type, size).to(o))
                       End Sub)
        End Function
    End Class
End Class

