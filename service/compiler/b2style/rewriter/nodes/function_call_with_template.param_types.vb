
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Private NotInheritable Class function_call_with_template
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
    End Class
End Class
