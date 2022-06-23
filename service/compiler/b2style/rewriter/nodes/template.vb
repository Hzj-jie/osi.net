
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
            Return f(code_gens().typed(Of name)(n.child(1).child().type_name).of(n),
                     l.typed(Of template_head)().type_param_list(n.child(0)),
                     n.child(1),
                     name_node_of(n),
                     o)
        End Function

        Public Shared Function type_param_count(ByVal n As typed_node) As UInt32
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Return code_gens().typed(Of template_head)().type_param_count(n.child(0))
        End Function

        Public Shared Function name_node_of(ByVal n As typed_node) As typed_node
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            n = n.child(1).child()
            Return code_gens().typed(Of name_node)(n.type_name).of(n)
        End Function

        ' TODO: Remove
        Public Shared Function name_of(ByVal name As String, ByVal type_count As UInt32) As String
            assert(Not name.null_or_whitespace())
            assert(type_count > 0)
            Return String.Concat(name, "__", type_count)
        End Function

        Public Shared Function name_of(ByVal n As typed_node, ByVal type_count As UInt32) As String
            assert(Not n Is Nothing)
            Return name_of(n.input_without_ignored(), type_count)
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
