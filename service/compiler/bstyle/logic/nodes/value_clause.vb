
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class value_clause
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal name As typed_node,
                                      ByVal value As Func(Of Boolean),
                                      ByVal raw_value_str As String,
                                      ByVal struct_copy As Func(Of vector(Of String), Boolean),
                                      ByVal primitive_copy As Func(Of String, Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(Not struct_copy Is Nothing)
            assert(Not primitive_copy Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            Dim delegate_definition As New ref(Of function_signature)()
            If Not scope.current().variables().resolve(name.input_without_ignored(), type, delegate_definition) Then
                ' Emmmm, scope.variable should log the error already.
                Return False
            End If
            If delegate_definition Then
                If raw_value_str.null_or_whitespace() Then
                    raise_error(error_type.user, "Unsupported delegate target, no function name provided.")
                End If
                ' TODO: Avoid copying.
                Dim target_function_name As String = logic_name.of_function(
                                                         scope.current_namespace_t.of(raw_value_str),
                                                         +delegate_definition.get().parameters)
                If scope.current().functions().is_defined(target_function_name) Then
                    ' Use address-of to copy a function address to the target.
                    ' TODO: Need to use logic_name here.
                    scope.current().call_hierarchy().to(target_function_name)
                    Return builders.of_address_of(name.input_without_ignored(), target_function_name).to(o)
                End If
                Return builders.of_copy(name.input_without_ignored(), raw_value_str).to(o)
            End If
            If Not value() Then
                Return False
            End If
            If scope.current().structs().types().defined(type) Then
                Using r As read_scoped(Of scope.value_target_t.target).ref = scope.current().value_target().value()
                    If Not (+r).type.Equals(type) Then
                        raise_error(error_type.user,
                                    "Type ",
                                    type,
                                    " of ",
                                    name.input_without_ignored(),
                                    " does not match the rvalue ",
                                    (+r).type)
                        Return False
                    End If
                    Return struct_copy((+r).names)
                End Using
            End If
            Using r As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                    scope.current().value_target().primitive_type()
                ' The type check of primitive-type target will be handled by logic.
                assert(Not r Is Nothing)
                Dim s As String = Nothing
                If Not r.retrieve(s) Then
                    raise_error(error_type.user,
                                "Failed to retrieve a primitive-type target from the r-value, received a struct?")
                    Return False
                End If
                Return primitive_copy(s)
            End Using
        End Function

        Private Shared Function build(ByVal name As typed_node,
                                      ByVal value As typed_node,
                                      ByVal struct_copy As Func(Of vector(Of String), Boolean),
                                      ByVal primitive_copy As Func(Of String, Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not value Is Nothing)
            Return build(name,
                         Function() As Boolean
                             Return code_gen_of(value).build(o)
                         End Function,
                         value.input_without_ignored(),
                         struct_copy,
                         primitive_copy,
                         o)
        End Function

        Public Shared Function stack_name_build(ByVal name As typed_node,
                                                ByVal value As typed_node,
                                                ByVal o As logic_writer) As Boolean
            assert(Not name Is Nothing)
            assert(name.type_name.Equals("name") OrElse name.child().type_name.Equals("name"), name)
            ' TODO: If the value on the right is a temporary value (rvalue), move can be used to reduce memory copy.
            Return build(name,
                         value,
                         Function(ByVal r As vector(Of String)) As Boolean
                             Return struct.copy(r, name.input_without_ignored(), o)
                         End Function,
                         Function(ByVal r As String) As Boolean
                             Return builders.of_copy(name.input_without_ignored(), r).to(o)
                         End Function,
                         o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            If Not n.child(0).child().type_name.Equals("heap-name") Then
                Return stack_name_build(n.child(0).child(), n.child(2), o)
            End If
            Return heap_name.build(
                       n.child(0).child().child(2),
                       o,
                       Function(ByVal indexstr As String) As Boolean
                           Return build(n.child(0).child().child(0),
                                        n.child(2),
                                        Function(ByVal r As vector(Of String)) As Boolean
                                            Return struct.copy(r,
                                                               n.child(0).child().child(0).input_without_ignored(),
                                                               indexstr,
                                                               o)
                                        End Function,
                                        Function(ByVal r As String) As Boolean
                                            Return builders.of_copy(
                                                       variable.name_of(
                                                           n.child(0).child().child(0).input_without_ignored(),
                                                           indexstr),
                                                   r).to(o)
                                        End Function,
                                 o)
                       End Function)
        End Function
    End Class
End Class

