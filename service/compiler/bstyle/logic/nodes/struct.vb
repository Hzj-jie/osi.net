
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

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

        Private Shared Function copy(ByVal sources As vector(Of String),
                                     ByVal target As String,
                                     ByVal f As Func(Of vector(Of builders.parameter), Boolean)) As Boolean
            assert(Not sources Is Nothing)
            assert(Not target.null_or_whitespace())
            assert(Not f Is Nothing)
            Dim vs As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(target, vs) Then
                Return False
            End If
            assert(Not vs Is Nothing)
            If vs.size() <> sources.size() Then
                raise_error(error_type.user,
                            "Sources ",
                            sources,
                            " are not consistent with the type of ",
                            target,
                            ", ",
                            vs)
                Return False
            End If
            Return f(vs)
        End Function

        Public Function copy_heap(ByVal sources As vector(Of String),
                                  ByVal target As String,
                                  ByVal index As typed_node,
                                  ByVal o As writer) As Boolean
            Return copy(sources,
                        target,
                        Function(ByVal vs As vector(Of builders.parameter)) As Boolean
                            assert(Not vs Is Nothing)
                            assert(vs.size() = sources.size())
                            Return l.typed_code_gen(Of heap_clause)().move(
                                       index,
                                       Function(ByVal indexstr As String) As Boolean
                                           Dim i As UInt32 = 0
                                           While i < vs.size()
                                               If Not builders.of_copy_heap_in(vs(i).name, indexstr, sources(i)).to(o) Then
                                                   Return False
                                               End If
                                               i += uint32_1
                                           End While
                                           Return True
                                       End Function,
                                       o)
                        End Function)
        End Function

        Public Shared Function copy(ByVal sources As vector(Of String),
                                    ByVal target As String,
                                    ByVal o As writer) As Boolean
            Return copy(sources,
                        target,
                        Function(ByVal vs As vector(Of builders.parameter)) As Boolean
                            assert(Not vs Is Nothing)
                            assert(vs.size() = sources.size())
                            Dim i As UInt32 = 0
                            While i < vs.size()
                                If Not builders.of_copy(vs(i).name, sources(i)).to(o) Then
                                    Return False
                                End If
                                i += uint32_1
                            End While
                            Return True
                        End Function)
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

        Private Function resolve(ByVal type As String,
                                 ByVal name As String,
                                 ByRef v As vector(Of builders.parameter)) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            If Not scope.current().structs().resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            If Not scope.current().variables().define(type, name) Then
                Return False
            End If
            Return True
        End Function

        Public Function define_in_stack(ByVal type As String, ByVal name As String, ByVal o As writer) As Boolean
            assert(Not o Is Nothing)
            Dim v As vector(Of builders.parameter) = Nothing
            If Not resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return v.stream().
                     map(Function(ByVal m As builders.parameter) As Boolean
                             assert(Not m Is Nothing)
                             Return value_declaration.declare_single_data_slot(m.type, m.name, o)
                         End Function).
                     aggregate(bool_stream.aggregators.all_true)
        End Function

        Public Function define_in_heap(ByVal type As String,
                                       ByVal name As String,
                                       ByVal length As typed_node,
                                       ByVal o As writer) As Boolean
            assert(Not length Is Nothing)
            assert(Not o Is Nothing)
            Dim v As vector(Of builders.parameter) = Nothing
            If Not resolve(type, name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Return l.typed_code_gen(Of heap_declaration).build(
                length,
                o,
                Function(ByVal len_name As String) As Boolean
                    Return v.stream().
                             map(Function(ByVal m As builders.parameter) As Boolean
                                     assert(Not m Is Nothing)
                                     Return heap_declaration.declare_single_data_slot(m.type, m.name, len_name, o)
                                 End Function).
                             aggregate(bool_stream.aggregators.all_true)
                End Function)
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
