﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class template_template
        Private Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("template", "template", "dump"))
        Private ReadOnly w As New template_writer()
        Private ReadOnly class_name As class_name_t
        Private ReadOnly types As New vector(Of ref(Of String))()

        Private NotInheritable Class class_name_t
            Public ReadOnly name As String
            Private types As vector(Of String)

            Public Sub New(ByVal name As String)
                assert(Not name.null_or_whitespace())
                Me.name = name
            End Sub

            Public Sub apply(ByVal types As vector(Of String))
                assert(Not types Is Nothing)
                Me.types = types
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

        Private Sub New(ByVal n As typed_node, ByVal types As vector(Of String))
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("class"))
            assert(Not types.null_or_empty())
            Me.class_name = New class_name_t(n.child(1).children_word_str())
            Me.types.resize(types.size(), New ref(Of String)())
            n.dfs(Sub(ByVal node As typed_node, ByVal stop_navigating_sub_nodes As Action)
                      assert(Not node Is Nothing)
                      assert(Not stop_navigating_sub_nodes Is Nothing)
                      If Object.ReferenceEquals(n.child(1), node) Then
                          ' type-name of class.
                          assert(w.append(class_name))
                          stop_navigating_sub_nodes()
                          Return
                      End If
                      If Not node.type_name.Equals("raw-type-name") Then
                          Return
                      End If

                      For i As UInt32 = 0 To types.size() - uint32_1
                          If node.input().Equals(types(i)) Then
                              assert(w.append(Me.types(i)))
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

        Public Shared Function [of](ByVal n As typed_node, ByRef o As template_template) As Boolean
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("template"))
            assert(n.child_count() = 5)
            Dim v As vector(Of String) = streams.range(0, n.child(2).child_count()).
                                                 map(Function(ByVal id As Int32) As typed_node
                                                         Return n.child(2).child(CUInt(id))
                                                     End Function).
                                                 map(Function(ByVal param As typed_node) As String
                                                         assert(Not param Is Nothing)
                                                         If param.type_name.Equals("type-param-with-comma") Then
                                                             param = param.child(0)
                                                         End If
                                                         assert(param.type_name.Equals("type-param"))
                                                         Return param.children_word_str()
                                                     End Function).
                                                 collect(Of vector(Of String))()
            If v.size() > v.stream().collect_by(stream(Of String).collectors.unique()).size() Then
                raise_error(error_type.user,
                            "Template ",
                            n.child(4).child(1).children_word_str(),
                            " has duplicated template type parameters: ",
                            v)
                Return False
            End If
            o = New template_template(n.child(4), v)
            Return True
        End Function

        Public Function name() As String
            Return class_name.name
        End Function

        Public Function extended_class_name(ByVal types As vector(Of String)) As String
            class_name.apply(types)
            Return class_name.ToString()
        End Function

        Public Function apply(ByVal types As vector(Of String), ByRef impl As String) As Boolean
            assert(Not types Is Nothing)
            If types.size() <> Me.types.size() Then
                raise_error(error_type.user,
                            "Template ",
                            class_name.name,
                            " expectes ",
                            Me.types.size(),
                            " template types, but received [",
                            types,
                            "].")
                Return False
            End If
            assert(types.size() > 0)
            class_name.apply(types)
            For i As UInt32 = 0 To types.size() - uint32_1
                Me.types(i).set(types(i))
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