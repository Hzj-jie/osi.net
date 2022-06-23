
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class template
        Implements code_gen(Of typed_node_writer)

        Public Interface name
            Function [of](ByVal n As typed_node) As String
        End Interface

        Public Interface name_node
            Function [of](ByVal n As typed_node) As typed_node
        End Interface

        Private Shared Function build(Of T)(ByVal l As code_gens(Of typed_node_writer),
                                            ByVal n As typed_node,
                                            ByVal f As _do_val_val_val_val_ref(Of String,
                                                                                  vector(Of String),
                                                                                  typed_node,
                                                                                  typed_node,
                                                                                  T,
                                                                                  Boolean),
                                            ByRef o As T) As Boolean
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            assert(Not f Is Nothing)
            assert(n.child_count() = 2)
            Return f(name_of(n), type_param_list(l, n), n.child(1), name_node_of(n), o)
        End Function

        Private Shared Function signature_node_from(ByVal n As typed_node) As typed_node
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("template"))
            assert(n.child_count() = 2)
            Return n.child(1).child()
        End Function

        Private Shared Function type_param_list(ByVal l As code_gens(Of typed_node_writer),
                                                ByVal n As typed_node) As vector(Of String)
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("template"))
            assert(n.child_count() = 2)
            Return l.typed(Of template_head)().type_param_list(n.child(0))
        End Function

        Public Shared Function type_param_list(ByVal n As typed_node) As vector(Of String)
            Return type_param_list(code_gens(), n)
        End Function

        Public Shared Function name_node_of(ByVal n As typed_node) As typed_node
            Dim name_node As typed_node = signature_node_from(n)
            Return code_gens().typed(Of name_node)(name_node.type_name).of(name_node)
        End Function

        Public Shared Function name_of(ByVal name As String, ByVal type_count As UInt32) As String
            assert(Not name.null_or_whitespace())
            assert(type_count > 0)
            Return String.Concat(name, "__", type_count)
        End Function

        Public Shared Function name_of(ByVal n As typed_node, ByVal type_count As UInt32) As String
            assert(Not n Is Nothing)
            Return name_of(n.input_without_ignored(), type_count)
        End Function

        Public Shared Function default_name_of(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("template"))
            assert(n.child_count() = 2)
            Return name_of(name_node_of(n), type_param_list(n).size())
        End Function

        Public Shared Function name_of(ByVal n As typed_node) As String
            Return code_gens().typed(Of name)(signature_node_from(n).type_name).of(n)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Return build(code_gens(),
                         n,
                         Function(ByVal name As String,
                                  ByVal types As vector(Of String),
                                  ByVal body As typed_node,
                                  ByVal name_node As typed_node,
                                  ByRef unused As Int32) As Boolean
                             Return scope.current().template().define(name,
                                                                      types,
                                                                      body,
                                                                      name_node)
                         End Function,
                         0)
        End Function

        ' @VisibleForTesting
        ' TODO: Remove this function.
        Public Shared Function build(ByVal l As code_gens(Of typed_node_writer),
                                     ByVal n As typed_node,
                                     ByRef o As template_template) As Boolean
            Return build(l,
                         n,
                         Function(ByVal name As String,
                                  ByVal types As vector(Of String),
                                  ByVal body As typed_node,
                                  ByVal name_node As typed_node,
                                  ByRef x As template_template) As Boolean
                             Return template_template.of(types, body, name_node, x)
                         End Function,
                         o)
        End Function
    End Class
End Class
