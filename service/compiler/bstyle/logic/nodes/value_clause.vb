
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Private NotInheritable Class value_clause
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal name_node As typed_node,
                                      ByVal value As typed_node,
                                      ByVal struct_copy As Func(Of String, vector(Of String), Boolean),
                                      ByVal primitive_copy As Func(Of String, String, Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not name_node Is Nothing)
            assert(Not value Is Nothing)
            assert(Not struct_copy Is Nothing)
            assert(Not primitive_copy Is Nothing)
            assert(Not o Is Nothing)
            Dim name As String = scope.fully_qualified_variable_name.of(name_node)
            Dim type As String = Nothing
            Dim delegate_definition As New ref(Of function_signature)()
            If Not scope.current().variables().resolve(name, type, delegate_definition) Then
                ' Emmmm, scope.variable should log the error already.
                Return False
            End If
            If delegate_definition Then
                ' TODO: Avoid copying.
                Dim target_function_name As String = logic_name.of_function(
                                                             scope.fully_qualified_function_name.of(value),
                                                             +delegate_definition.get().parameters)
                If scope.current().functions().is_defined(target_function_name) Then
                    ' Use address-of to copy a function address to the target.
                    ' TODO: Need to use logic_name here.
                    scope.current().call_hierarchy().to(target_function_name)
                    Return builders.of_address_of(name, target_function_name).to(o)
                End If
                Return builders.of_copy(name, scope.function_name.of(value)).to(o)
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
                                    name,
                                    " does not match the rvalue ",
                                    (+r).type)
                        Return False
                    End If
                    Return struct_copy(name, (+r).names)
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
                Return primitive_copy(name, s)
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
                         Function(ByVal n As String, ByVal r As vector(Of String)) As Boolean
                             Return struct.copy(r, n, o)
                         End Function,
                         Function(ByVal n As String, ByVal r As String) As Boolean
                             Return builders.of_copy(n, r).to(o)
                         End Function,
                         o)
        End Function

        Private Shared Function heap_name_build(ByVal name As typed_node,
                                                ByVal value As typed_node,
                                                ByVal o As logic_writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(Not o Is Nothing)
            assert(name.type_name.Equals("heap-name"))
            Return heap_name.build(name.child(2),
                                   o,
                                   Function(ByVal indexstr As String) As Boolean
                                       Return build(name.child(0),
                                                    value,
                                                    Function(ByVal n2 As String,
                                                             ByVal r As vector(Of String)) As Boolean
                                                        Return struct.copy(r, n2, indexstr, o)
                                                    End Function,
                                                    Function(ByVal n2 As String, ByVal r As String) As Boolean
                                                        Return builders.of_copy(
                                                                   variable.name_of(n2, indexstr), r).
                                                                   to(o)
                                                    End Function,
                                                    o)
                                   End Function)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Dim name As typed_node = n.child(0).child()
            Dim value As typed_node = n.child(2)
            Return build(name, value, o)
        End Function
    End Class
End Class

