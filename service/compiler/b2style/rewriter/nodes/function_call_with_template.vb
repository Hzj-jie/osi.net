
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_call_with_template
        Implements code_gen(Of typed_node_writer)

        Private Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            Dim t As tuple(Of String, String) = Nothing
            If Not function_call.split_struct_function(n.child(0).child(0).input_without_ignored(), t) Then
                t = Nothing
            End If
            Dim extended_type As String = Nothing
            Return scope.template_t.resolve(n.child(0), extended_type) AndAlso
                   code_gens().typed(Of function_call).build(scope.current_namespace_t.with_global_namespace(
                       If(t.is_null(),
                          extended_type,
                          function_call.build_struct_function(t.first(), extended_type))),
                       n,
                       o)
        End Function

        Private Shared Function param_types(ByVal n As typed_node, ByRef o As vector(Of String)) As Boolean
            assert(Not n Is Nothing)
            o.renew()
            Return True

            ' Following implementation does not work, b2style has no information about the types, the
            ' function_call_with_template cannot create correct parameter types.
#If 0 Then
            If n.child_count() = 3 Then
                Return True
            End If
            assert(n.child_count() = 4)
            Dim v As vector(Of String) = o
            Return n.child(2).
                     children().
                     map(Function(ByVal node As typed_node) As String
                             assert(Not node Is Nothing)
                             If node.type_name.Equals("value-with-comma") Then
                                 node = node.child(0)
                             End If
                             assert(node.type_name.Equals("value"))
                             Return node.input_without_ignored()
                         End Function).
                     map(Function(ByVal param As String) As Boolean
                             Dim type As String = Nothing
                             If Not scope.current().variables().try_resolve(param, type) Then
                                 raise_error(error_type.user, "Cannot find the type of parameter ", param)
                                 Return False
                             End If
                             v.emplace_back(type)
                             Return True
                         End Function).
                     aggregate(bool_stream.aggregators.all_true)
#End If
        End Function

        Public Shared Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Dim param_types As vector(Of String) = Nothing
            If Not function_call_with_template.param_types(my_node(Of function_call_with_template)(n), param_types) Then
                Return False
            End If
            Dim f As Func(Of String, String) = Function(ByVal function_name As String) As String
                                                   Return _function.template_name_of(function_name,
                                                                                     n.child(2).child_count(),
                                                                                     param_types)
                                               End Function
            Dim t As tuple(Of String, String) = Nothing
            If function_call.split_struct_function(n.child(0).input_without_ignored(), t) Then
                o = f(t.second())
            Else
                o = f(n.child(0).input_without_ignored())
            End If
            Return True
        End Function
    End Class
End Class
