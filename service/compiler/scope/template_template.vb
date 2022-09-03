
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class template_template
        Private Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("template", "template", "dump"))
        Private ReadOnly w As New template_writer()
        Private ReadOnly _extended_type_name As extended_type_name_t
        ' One template type can be used multiple times in the generated code.
        Private ReadOnly type_refs As New vector(Of ref(Of String))()

        Private Structure extended_type_name_t
            Private ReadOnly name As String
            Private ReadOnly types As one_off(Of vector(Of String))

            Public Sub New(ByVal name As String)
                assert(Not name.null_or_whitespace())
                Me.name = name
                Me.types = New one_off(Of vector(Of String))()
            End Sub

            Public Sub apply(ByVal types As vector(Of String))
                assert(Me.types.set(
                   types.stream().
                         map(Function(ByVal type As String) As String
                                 ' Remove referneces in the name, and replace any characters out of the raw-name in
                                 ' nlexer_rule2 into an underscore. Knowing, this behavior may cause some naming
                                 ' conflicts.
                                 Dim r As New StringBuilder(builders.parameter_type.remove_ref(type))
                                 For i As Int32 = 0 To r.Length() - 1
                                     If r(i).alpha() OrElse r(i).digit() OrElse r(i) = character.underline Then
                                         Continue For
                                     End If
                                     r(i) = character.underline
                                 Next
                                 Return r.ToString()
                             End Function).
                         collect_to(Of vector(Of String))()))
            End Sub

            Public Function str() As String
                Return strcat(name, "__", types.get().str("__"))
            End Function
        End Structure

        Public Sub New(ByVal body As typed_node, ByVal name_node As typed_node, ByVal types As vector(Of String))
            assert(Not body Is Nothing)
            assert(Not name_node Is Nothing)
            assert(Not types.null_or_empty())
            Me._extended_type_name = New extended_type_name_t(name_node.input_without_ignored())
            Me.type_refs.resize(types.size(),
                            Function() As ref(Of String)
                                Return New ref(Of String)()
                            End Function)
            body.dfs(Sub(ByVal node As typed_node, ByVal stop_navigating_sub_nodes As Action)
                         assert(Not node Is Nothing)
                         assert(Not stop_navigating_sub_nodes Is Nothing)
                         If Object.ReferenceEquals(name_node, node) Then
                             assert(w.append(AddressOf _extended_type_name.str))
                             stop_navigating_sub_nodes()
                             Return
                         End If
                         If Not node.type_name.Equals("raw-type-name") Then
                             Return
                         End If

                         For i As UInt32 = 0 To types.size() - uint32_1
                             If node.input().Equals(types(i)) Then
                                 assert(w.append(type_refs(i)))
                                 stop_navigating_sub_nodes()
                                 Return
                             End If
                         Next
                     End Sub,
                 Sub(ByVal leaf As typed_node)
                     assert(Not leaf Is Nothing)
                     assert(w.append(leaf.word().str()))
                 End Sub)
        End Sub

        Public Function extended_type_name(ByVal type_param_list As vector(Of String)) As String
            _extended_type_name.apply(type_param_list)
            Return _extended_type_name.str()
        End Function

        Public Function apply(ByVal types As vector(Of String), ByRef impl As String) As Boolean
            assert(Not types.null_or_empty())
            assert(types.size() = Me.type_refs.size())
            _extended_type_name.apply(types)
            For i As UInt32 = 0 To types.size() - uint32_1
                assert(Not Me.type_refs(i))
                Me.type_refs(i).set(types(i))
            Next
            impl = w.dump()
            For i As UInt32 = 0 To types.size() - uint32_1
                Me.type_refs(i).set(Nothing)
            Next
            Return True
        End Function

        Private NotInheritable Class template_writer
            Inherits typed_node_writer(Of debug_dump_t)

            Public NotInheritable Class debug_dump_t
                Inherits __void(Of String)

                Public Overrides Sub at(ByRef r As String)
                    If debug_dump Then
                        raise_error(error_type.user, "Debug dump of template_template ", r)
                    End If
                End Sub
            End Class
        End Class
    End Class
End Class

