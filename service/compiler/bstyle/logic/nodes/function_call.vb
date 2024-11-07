
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class function_call
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal n As typed_node,
                                      ByVal o As logic_writer,
                                      ByVal build_caller As Func(Of String, vector(Of String), Boolean),
                                      ByVal build_caller_ref As Func(Of String, vector(Of String), Boolean)) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(Not build_caller Is Nothing)
            assert(n.child_count() >= 3)
            Dim bc As Func(Of vector(Of String), Boolean) =
                Function(ByVal parameters As vector(Of String)) As Boolean
                    If scope.current().variables().defined(n.child(0).input_without_ignored()) Then
                        Return build_caller_ref(n.child(0).input_without_ignored(), parameters)
                    End If

                    Dim name As String = Nothing
                    If Not logic_name.of_function_call(n.child(0).input_without_ignored(), parameters, name) Then
                        Return False
                    End If
                    scope.current().call_hierarchy().to(name)
                    Return build_caller(name, parameters)
                End Function

            If n.child_count() = 3 Then
                Return bc(vector.of(Of String)())
            End If
            If Not code_gen_of(n.child(2)).build(o) Then
                Return False
            End If
            Using targets As read_scoped(Of vector(Of String)).ref = value_list.current_targets()
                Return bc(+targets)
            End Using
        End Function

        Public Shared Function without_return(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
            Return build(n,
                         o,
                         Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                             Return builders.of_caller(name, parameters).to(o)
                         End Function,
                         Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                             Return builders.of_caller_ref(name, parameters).to(o)
                         End Function)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 3)
            Dim b As Func(Of Func(Of String, String, vector(Of String), Boolean),
                             _do_val_ref(Of String, String, Boolean),
                             Func(Of String, vector(Of String), Boolean)) =
                Function(ByVal builder As Func(Of String, String, vector(Of String), Boolean),
                         ByVal return_type_of As _do_val_ref(Of String, String, Boolean)) _
                        As Func(Of String, vector(Of String), Boolean)
                    assert(Not builder Is Nothing)
                    Return Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                               Dim return_type As String = Nothing
                               If Not return_type_of(name, return_type) Then
                                   Return False
                               End If
                               If Not scope.current().structs().types().defined(return_type) Then
                                   Return builder(
                                              name,
                                              scope.current().value_target().with_temp_target(return_type, o).only(),
                                              parameters)
                               End If
                               ' TODO: Check the type consistency between function_call and variable receiver.
                               Dim return_value As String =
                                       scope.current().temp_logic_name().variable() + "@" + name + "@return_value"
                               assert(value_declaration.declare_primitive_type(
                                          compiler.logic.scope.type_t.variable_type, return_value, o))
                               Return builder(name, return_value, parameters) AndAlso
                                      struct.unpack(return_value,
                                                    scope.current().value_target().with_temp_target(return_type, o),
                                                    o)
                           End Function
                End Function
            Return build(n,
                         o,
                         b(Function(ByVal name As String,
                                    ByVal result As String,
                                    ByVal parameters As vector(Of String)) As Boolean
                               Return builders.of_caller(name, result, parameters).to(o)
                           End Function,
                           Function(ByVal name As String, ByRef type As String) As Boolean
                               Return scope.current().functions().return_type_of(name, type)
                           End Function),
                         b(Function(ByVal name As String,
                                    ByVal result As String,
                                    ByVal parameters As vector(Of String)) As Boolean
                               Return builders.of_caller_ref(name, result, parameters).to(o)
                           End Function,
                           Function(ByVal name As String, ByRef type As String) As Boolean
                               Dim signature As New ref(Of function_signature)()
                               Dim delegate_type As String = Nothing
                               If Not scope.current().variables().resolve(name, delegate_type, signature) Then
                                   Return False
                               End If
                               If Not signature Then
                                   raise_error(error_type.user,
                                               "Delegate type ",
                                               delegate_type,
                                               " for ",
                                               name,
                                               " is not defined.")
                                   Return False
                               End If
                               type = signature.get().return_type
                               Return True
                           End Function))
        End Function
    End Class
End Class

