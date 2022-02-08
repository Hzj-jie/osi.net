
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
    Public NotInheritable Class struct
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Private Shared Function copy(ByVal sources As vector(Of String),
                                     ByVal target As String,
                                     ByVal target_naming As Func(Of String, String),
                                     ByVal o As writer) As Boolean
            assert(Not sources Is Nothing)
            assert(Not target.null_or_whitespace())
            assert(Not target_naming Is Nothing)
            Dim vs As struct_def = Nothing
            If Not scope.current().structs().resolve(target, vs) Then
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
                                    ByVal o As writer) As Boolean
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
                                    ByVal o As writer) As Boolean
            Return copy(sources,
                        target,
                        Function(ByVal n As String) As String
                            Return variable.name_of(n, indexstr)
                        End Function,
                        o)
        End Function

        Public Shared Function pack(ByVal sources As vector(Of String),
                                    ByVal target As String,
                                    ByVal o As writer) As Boolean
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
                                      ByVal o As writer) As Boolean
            assert(Not source.null_or_whitespace())
            assert(Not targets Is Nothing)
            If targets.empty() Then
                Return True
            End If
            Dim index As String = strcat(targets(0), "@index")
            assert(value_declaration.declare_single_data_slot(code_types.int, index, o))
            Return targets.stream().
                           map(Function(ByVal target As String) As Boolean
                                   assert(Not target.null_or_whitespace())
                                   ' TODO: Should include bstyle_constant.h automatically.
                                   Return builders.of_cut_slice(target, source, index).to(o) AndAlso
                                          builders.of_add(index, index, "@@prefixes@constants@int_1").to(o)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function

        Private Shared Function resolve(ByVal type As String,
                                        ByVal name As String,
                                        ByRef v As struct_def) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            If Not scope.current().structs().resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return streams.of(struct_def.nested(type, name)).
                           concat(v.nesteds()).
                           map(Function(ByVal s As builders.parameter) As Boolean
                                   assert(Not s Is Nothing)
                                   assert(Not s.ref)
                                   Return scope.current().variables().define(s.type, s.name)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function

        ' Forward the definition of, or declare the {type, name} pair in the scope.variable in stack.
        Public Shared Sub forward_in_stack(ByVal type As String, ByVal name As String)
            resolve(type, name, Nothing)
        End Sub

        Public Shared Function define_in_stack(ByVal type As String, ByVal name As String, ByVal o As writer) As Boolean
            assert(Not o Is Nothing)
            Dim v As struct_def = Nothing
            If Not resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return v.primitives().
                     map(Function(ByVal m As builders.parameter) As Boolean
                             assert(Not m Is Nothing)
                             Return value_declaration.declare_single_data_slot(m.type, m.name, o)
                         End Function).
                     aggregate(bool_stream.aggregators.all_true, True)
        End Function

        Public Function define_in_heap(ByVal type As String,
                                       ByVal name As String,
                                       ByVal length As typed_node,
                                       ByVal o As writer) As Boolean
            assert(Not length Is Nothing)
            assert(Not o Is Nothing)
            Dim v As struct_def = Nothing
            If Not resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return l.typed(Of heap_name).build(
                       length,
                       o,
                       Function(ByVal len_name As String) As Boolean
                           Return v.primitives().
                                    map(Function(ByVal m As builders.parameter) As Boolean
                                            assert(Not m Is Nothing)
                                            Return heap_declaration.declare_single_data_slot(
                                                       m.type, m.name, len_name, o)
                                        End Function).
                                    aggregate(bool_stream.aggregators.all_true)
                       End Function)
        End Function

        Public Shared Function create_id(ByVal name As String) As builders.parameter
            assert(Not name.null_or_whitespace())
            Return builders.parameter.no_ref(name + "__struct__type__id__type", name + "__struct__type__id")
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
                             Return builders.parameter.no_ref(c.child(0).input_without_ignored(),
                                                              c.child(1).input_without_ignored())
                         End Function)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Dim id As builders.parameter = create_id(n.child(1).word().str())
            assert(builders.of_type(id.type, uint32_1).to(o))
            Return scope.current().structs().define(
                       n.child(1).word().str(),
                       parse_struct_body(n).map(AddressOf struct_def.nested).
                                            collect(Of vector(Of builders.parameter))() +
                                            id)
        End Function
    End Class
End Class
