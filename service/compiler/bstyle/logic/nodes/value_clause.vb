
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
                                      ByVal value As typed_node,
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
                ' TODO: Avoid copying.
                Dim target_function_name As String = logic_name.of_function(
                                                         value.input_without_ignored(),
                                                         +delegate_definition.get().parameters)
                If scope.current().functions().is_defined(target_function_name) Then
                    ' Use address-of to copy a function address to the target.
                    ' TODO: Need to use logic_name here.
                    scope.current().call_hierarchy().to(target_function_name)
                    Return builders.of_address_of(name.input_without_ignored(), target_function_name).to(o)
                End If
                Return builders.of_copy(name.input_without_ignored(), value.input_without_ignored()).to(o)
            End If
            If Not code_gen_of(value).build(o) Then
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

        Public Shared Function stack_name_build(ByVal name As typed_node,
                                                ByVal value As typed_node,
                                                ByVal o As logic_writer) As Boolean
            assert(Not name Is Nothing)
            assert(name.type_name.Equals("name"), name)
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
            Dim name As typed_node = n.child(0).child()
            Dim value As typed_node = n.child(2)
            If n.child(0).type_name.Equals("variable-name") AndAlso
               name.type_name.Equals("raw-variable-name") Then
                Return stack_name_build(name.child(), value, o)
            End If
            If name.type_name.Equals("heap-name") Then
                Return heap_name.build(
                           name.child(2),
                           o,
                           Function(ByVal indexstr As String) As Boolean
                               Return build(name.child(0),
                                            value,
                                            Function(ByVal r As vector(Of String)) As Boolean
                                                Return struct.copy(r,
                                                                   name.child(0).input_without_ignored(),
                                                                   indexstr,
                                                                   o)
                                            End Function,
                                            Function(ByVal r As String) As Boolean
                                                Return builders.of_copy(
                                                           variable.name_of(
                                                               name.child(0).input_without_ignored(),
                                                               indexstr),
                                                       r).to(o)
                                            End Function,
                                     o)
                           End Function)
            End If
            assert(False, "Unsupported assignee: ", name.type_name, " from [", n.input(), "]")
            Return False
        End Function
    End Class
End Class

