
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
    Private Class function_call(Of _name_builder As func_t(Of String, Boolean))
        Implements code_gen(Of logic_writer)

        Private Shared ReadOnly name_builder As _name_builder = alloc(Of _name_builder)()

        Private Shared Function build_function_caller(
                                    ByVal function_name As String,
                                    ByVal parameters As vector(Of String),
                                    ByVal build_caller As Func(Of String, vector(Of String), Boolean)) As Boolean
            assert(Not function_name.null_or_whitespace())
            assert(Not parameters Is Nothing)
            assert(Not build_caller Is Nothing)
            Dim name As String = Nothing
            If Not logic_name.of_function_call(function_name, parameters, name) Then
                Return False
            End If
            scope.current().call_hierarchy().to(name)
            Return build_caller(name, parameters)
        End Function

        Private Shared Function build(ByVal raw_function_name As String,
                                      ByVal build_caller As Func(Of String, vector(Of String), Boolean),
                                      ByVal build_caller_ref As Func(Of String, vector(Of String), Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not build_caller Is Nothing)
            assert(Not build_caller_ref Is Nothing)
            Using targets As read_scoped(Of vector(Of String)).ref = value_list.current_targets()
                Dim parameters As vector(Of String) = +targets
                Dim struct_func As tuple(Of String, String) = Nothing
                ' The split_struct_function needs to be executed first, otherwise the
                ' scope.current().variables().defined() will check is_heap_name which expects the [] to be at the end
                ' or triggers an assertion failure.
                If b2style.function_call.split_struct_function(raw_function_name, struct_func) Then
                    If Not name_builder.run(struct_func.first()) Then
                        raise_error(error_type.user, "Cannot find class instance ", struct_func.first())
                        Return False
                    End If
                    Return build_function_caller(scope.namespace_t.fully_qualified_name(struct_func.second()),
                                                 (+scope.current().value_target().value()).names + parameters,
                                                 build_caller)
                End If
                If scope.current().variables().defined(raw_function_name) Then
                    Return build_caller_ref(raw_function_name, parameters)
                End If
                Return build_function_caller(scope.fully_qualified_function_name.of(raw_function_name),
                                             parameters,
                                             build_caller)
            End Using
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
                               Dim signature As function_signature = Nothing
                               If Not scope.current().variables().delegate_of(name, signature) Then
                                   Return False
                               End If
                               assert(Not signature Is Nothing)
                               type = signature.return_type
                               Return True
                           End Function,
                           o)
        End Function

        Public NotInheritable Class ignore_parameters
            Public Shared Function without_return(ByVal function_name As String, ByVal o As logic_writer) As Boolean
                Return function_call(Of _name_builder).build(function_name,
                                                             without_return_caller_builder(o),
                                                             without_return_caller_ref_builder(o),
                                                             o)
            End Function

            Public Shared Function build(ByVal function_name As String, ByVal o As logic_writer) As Boolean
                Return function_call(Of _name_builder).build(function_name, caller_builder(o), caller_ref_builder(o), o)
            End Function

            Private Sub New()
            End Sub
        End Class

        Public NotInheritable Class with_parameters
            Private Shared Function build(ByVal name As String,
                                          ByVal n As typed_node,
                                          ByVal build_caller As Func(Of String, vector(Of String), Boolean),
                                          ByVal build_caller_ref As Func(Of String, vector(Of String), Boolean),
                                          ByVal o As logic_writer) As Boolean
                assert(Not n Is Nothing)
                assert(Not o Is Nothing)
                If name Is Nothing Then
                    name = scope.function_name.of(n.child(0))
                End If
                assert(n.child_count() >= 3)
                If n.child_count() = 3 Then
                    value_list.with_empty()
                ElseIf Not code_gen_of(n.child(2)).build(o) Then
                    Return False
                End If
                Return function_call(Of _name_builder).build(name, build_caller, build_caller_ref, o)
            End Function

            Public Shared Function without_return(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                Return build(Nothing, n, without_return_caller_builder(o), without_return_caller_ref_builder(o), o)
            End Function

            Public Shared Function without_return(ByVal function_name As String,
                                                  ByVal n As typed_node,
                                                  ByVal o As logic_writer) As Boolean
                Return build(function_name,
                             n,
                             without_return_caller_builder(o),
                             without_return_caller_ref_builder(o),
                             o)
            End Function

            Public Shared Function build(ByVal function_name As String,
                                         ByVal n As typed_node,
                                         ByVal o As logic_writer) As Boolean
                Return build(function_name, n, caller_builder(o), caller_ref_builder(o), o)
            End Function

            Public Shared Function build(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                Return build(Nothing, n, o)
            End Function

            Private Sub New()
            End Sub
        End Class

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return with_parameters.build(n, o)
        End Function

        ' Reuse by function_call_with_template; with_parameters.
        ' Needed by the reuse of b2style.function_call_with_paramter.
        Public Shared Function build(ByVal function_name As String,
                                     ByVal n As typed_node,
                                     ByVal o As logic_writer) As Boolean
            Return with_parameters.build(function_name, n, o)
        End Function
    End Class

    Private NotInheritable Class function_call
        Inherits function_call(Of raw_variable_name_of)

        Public Structure raw_variable_name_of
            Implements func_t(Of String, Boolean)

            Public Function run(ByVal i As String) As Boolean Implements func_t(Of String, Boolean).run
                Return raw_variable_name.build(i)
            End Function
        End Structure
    End Class

    Private NotInheritable Class heap_struct_function_call
        Inherits function_call(Of heap_struct_name_of)

        Public Structure heap_struct_name_of
            Implements func_t(Of String, Boolean)

            Public Function run(ByVal i As String) As Boolean Implements func_t(Of String, Boolean).run
                debugpause()
                Return False
                ' Return heap_struct_name.build(i)
            End Function
        End Structure
    End Class
End Class

