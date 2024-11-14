
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class b3style
    Private NotInheritable Class function_call
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal raw_function_name As String,
                                      ByVal build_caller As Func(Of String, vector(Of String), Boolean),
                                      ByVal build_caller_ref As Func(Of String, vector(Of String), Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not build_caller Is Nothing)
            assert(Not build_caller_ref Is Nothing)
            Using targets As read_scoped(Of vector(Of String)).ref = value_list.current_targets()
                Dim parameters As vector(Of String) = +targets
                If scope.current().variables().defined(raw_function_name) Then
                    Return build_caller_ref(raw_function_name, parameters)
                End If

                Dim struct_func As tuple(Of String, String) = Nothing
                Dim function_name As String = Nothing
                If b2style.function_call.split_struct_function(raw_function_name, struct_func) Then
                    If Not raw_variable_name.build(struct_func.first(), o) Then
                        raise_error(error_type.user, "Cannot find class instance ", struct_func.first())
                        Return False
                    End If
                    parameters = (+scope.current().value_target().value()).names + parameters
                    function_name = scope.namespace_t.fully_qualified_name(struct_func.second())
                Else
                    function_name = scope.fully_qualified_function_name.of(raw_function_name)
                End If
                assert(Not function_name Is Nothing)
                Dim name As String = Nothing
                If Not logic_name.of_function_call(function_name, parameters, name) Then
                    Return False
                End If
                scope.current().call_hierarchy().to(name)
                Return build_caller(name, parameters)
            End Using
        End Function

        Private Shared Function build(ByVal n As typed_node,
                                      ByVal o As logic_writer,
                                      ByVal build_caller As Func(Of String, vector(Of String), Boolean),
                                      ByVal build_caller_ref As Func(Of String, vector(Of String), Boolean)) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 3)
            If n.child_count() = 3 Then
                value_list.with_empty()
            ElseIf Not code_gen_of(n.child(2)).build(o) Then
                Return False
            End If
            Return build(n.child(0).input_without_ignored(), build_caller, build_caller_ref, o)
        End Function

        Private Shared Function without_return_caller_builder(ByVal o As logic_writer) _
                                                             As Func(Of String, vector(Of String), Boolean)
            Return Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                       Return builders.of_caller(name, parameters).to(o)
                   End Function
        End Function

        Private Shared Function without_return_caller_ref_builder(ByVal o As logic_writer) _
                                                                 As Func(Of String, vector(Of String), Boolean)
            Return Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                       Return builders.of_caller_ref(name, parameters).to(o)
                   End Function
        End Function

        Public Shared Function without_return(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
            Return build(n, o, without_return_caller_builder(o), without_return_caller_ref_builder(o))
        End Function

        Public Shared Function without_return(ByVal function_name As String, ByVal o As logic_writer) As Boolean
            Return build(function_name, without_return_caller_builder(o), without_return_caller_ref_builder(o), o)
        End Function

        Private Shared Function builder(ByVal logic_builder As Func(Of String, String, vector(Of String), Boolean),
                                        ByVal return_type_of As _do_val_ref(Of String, String, Boolean),
                                        ByVal o As logic_writer) _
                                       As Func(Of String, vector(Of String), Boolean)
            assert(Not logic_builder Is Nothing)
            assert(Not return_type_of Is Nothing)
            assert(Not o Is Nothing)
            Return Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                       Dim return_type As String = Nothing
                       If Not return_type_of(name, return_type) Then
                           Return False
                       End If
                       If Not scope.current().structs().types().defined(return_type) Then
                           Return logic_builder(name,
                                                scope.current().value_target().with_temp_target(return_type, o).only(),
                                                parameters)
                       End If
                       ' TODO: Check the type consistency between function_call and variable receiver.
                       Dim return_value As String =
                               scope.current().temp_logic_name().variable() + "@" + name + "@return_value"
                       assert(value_declaration.declare_primitive_type(
                                      compiler.logic.scope.type_t.variable_type, return_value, o))
                       Return logic_builder(name, return_value, parameters) AndAlso
                              struct.unpack(return_value,
                                            scope.current().value_target().with_temp_target(return_type, o),
                                            o)
                   End Function
        End Function

        Private Shared Function caller_builder(ByVal o As logic_writer) As Func(Of String, vector(Of String), Boolean)
            Return builder(Function(ByVal name As String,
                                                ByVal result As String,
                                                ByVal parameters As vector(Of String)) As Boolean
                               Return builders.of_caller(name, result, parameters).to(o)
                           End Function,
                           Function(ByVal name As String, ByRef type As String) As Boolean
                               Return scope.current().functions().return_type_of(name, type)
                           End Function,
                           o)
        End Function

        Private Shared Function caller_ref_builder(ByVal o As logic_writer) _
                                                  As Func(Of String, vector(Of String), Boolean)
            Return builder(Function(ByVal name As String,
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
                           End Function,
                           o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return build(n, o, caller_builder(o), caller_ref_builder(o))
        End Function

        Public Shared Function build(ByVal function_name As String, ByVal o As logic_writer) As Boolean
            Return build(function_name, caller_builder(o), caller_ref_builder(o), o)
        End Function
    End Class
End Class

