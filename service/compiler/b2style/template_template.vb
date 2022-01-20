
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class template_template
        Private Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("template", "template", "dump"))
        Private ReadOnly w As New template_writer()
        Private ReadOnly _extended_type_name As extended_type_name_t
        Private ReadOnly type_refs As New vector(Of ref(Of String))()

        Private NotInheritable Class extended_type_name_t
            Public ReadOnly name As String
            Private types As vector(Of String)

            Public Sub New(ByVal name As String)
                assert(Not name.null_or_whitespace())
                Me.name = name
            End Sub

            Public Sub apply(ByVal types As vector(Of String))
                assert(Not types Is Nothing)
                assert(Me.types Is Nothing)
                Me.types = types.stream().
                                 map(Function(ByVal type As String) As String
                                         ' Remove referneces in the name.
                                         Return New builders.parameter_type(type).type
                                     End Function).
                                 collect(Of vector(Of String))()
            End Sub

            Public Overrides Function ToString() As String
                assert(Not types Is Nothing)
                Using defer.to(Sub()
                                   types = Nothing
                               End Sub)
                    Return strcat(name, "__", types.str("__"))
                End Using
            End Function
        End Class

        Private Sub New(ByVal n As typed_node, ByVal name_node As typed_node, ByVal types As vector(Of String))
            assert(Not n Is Nothing)
            assert(Not name_node Is Nothing)
            assert(n.type_name.Equals("template-body"))
            n = n.child()
            assert(Not types.null_or_empty())
            Me._extended_type_name = New extended_type_name_t(template_name(name_node, types.size()))
            Me.type_refs.resize(types.size(), New ref(Of String)())
            n.dfs(Sub(ByVal node As typed_node, ByVal stop_navigating_sub_nodes As Action)
                      assert(Not node Is Nothing)
                      assert(Not stop_navigating_sub_nodes Is Nothing)
                      If Object.ReferenceEquals(name_node, node) Then
                          assert(w.append(_extended_type_name))
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

        Public Shared Function template_name(ByVal n As typed_node, ByVal type_count As UInt32) As String
            assert(Not n Is Nothing)
            Return strcat(n.children_word_str(), "__", type_count)
        End Function

        ' @VisibleForTesting
        Public Shared Function [of](ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef o As template_template) As Boolean
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("template"))
            assert(n.child_count() = 5)
            Dim name_node As typed_node = n.child(4).child()
            If name_node.type_name.Equals("class") Then
                name_node = name_node.child(1)
            ElseIf name_node.type_name.Equals("delegate-with-semi-colon") Then
                name_node = name_node.child(0).child(2)
            Else
                assert(False)
            End If
            Dim v As vector(Of String) = l.of_all_children(n.child(2)).dump()
            assert(Not v.null_or_empty())
            If v.size() > v.stream().collect_by(stream(Of String).collectors.unique()).size() Then
                raise_error(error_type.user,
                            "Template ",
                            name_node.children_word_str(),
                            " has duplicated template type parameters: ",
                            v)
                Return False
            End If
            o = New template_template(n.child(4), name_node, v)
            Return True
        End Function

        Public Shared Function [of](ByVal n As typed_node, ByRef o As template_template) As Boolean
            Return [of](b2style.code_builder.current().code_gens, n, o)
        End Function

        Public Function name() As String
            Return _extended_type_name.name
        End Function

        Public Function extended_type_name(ByVal types As vector(Of String)) As String
            _extended_type_name.apply(types)
            Return _extended_type_name.ToString()
        End Function

        Public Function apply(ByVal types As vector(Of String), ByRef impl As String) As Boolean
            assert(Not types Is Nothing)
            If types.size() <> Me.type_refs.size() Then
                raise_error(error_type.user,
                            "Template ",
                            _extended_type_name.name,
                            " expectes ",
                            Me.type_refs.size(),
                            " template types, but received [",
                            types,
                            "].")
                Return False
            End If
            assert(types.size() > 0)
            _extended_type_name.apply(types)
            For i As UInt32 = 0 To types.size() - uint32_1
                Me.type_refs(i).set(types(i))
            Next
            impl = w.dump()
            Return True
        End Function

        Private NotInheritable Class template_writer
            Inherits typed_node_writer(Of debug_dump_t)

            Public NotInheritable Class debug_dump_t
                Inherits __void(Of String)

                Public Overrides Sub at(ByRef r As String)
                    If debug_dump Then
                        raise_error(error_type.user,
                                    "Debug dump of template_template ",
                                    r.Replace(";", ";" + newline.incode()).Replace("}", "}" + newline.incode()))
                    End If
                End Sub
            End Class
        End Class
    End Class
End Class
