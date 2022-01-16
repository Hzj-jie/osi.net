
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class function_call
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Private Function build(ByVal n As typed_node,
                               ByVal o As writer,
                               ByVal build_caller As Func(Of String, vector(Of String), Boolean)) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(Not build_caller Is Nothing)
            assert(n.child_count() >= 3)
            Dim bc As Func(Of vector(Of String), Boolean) =
                Function(ByVal parameters As vector(Of String)) As Boolean
                    Dim name As String = Nothing
                    If Not logic_name.of_function_call(n.child(0).children_word_str(), parameters, name) Then
                        Return False
                    End If
                    scope.current().call_hierarchy().to(name)
                    Return build_caller(name, parameters)
                End Function

            If n.child_count() = 3 Then
                Return bc(vector.of(Of String)())
            End If
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using targets As read_scoped(Of vector(Of String)).ref = value_list.current_targets()
                Return bc(+targets)
            End Using
        End Function

        Public Function without_return(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return build(n,
                         o,
                         Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                             Return builders.of_caller(name, parameters).to(o)
                         End Function)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 3)
            Return build(n,
                         o,
                         Function(ByVal name As String, ByVal parameters As vector(Of String)) As Boolean
                             Dim return_type As String = Nothing
                             If Not scope.current().functions().return_type_of(name, return_type) Then
                                 Return False
                             End If
                             If scope.current().structs().defined(return_type) Then
                                 ' TODO: Check the type consistency between function_call and variable receiver.
                                 Dim return_value As String =
                                         strcat(logic_name.temp_variable(n), "@", name, "@return_value")
                                 assert(value_declaration.declare_single_data_slot(
                                            compiler.logic.scope.type_t.variable_type, return_value, o))
                                 Return builders.of_caller(name, return_value, parameters).to(o) AndAlso
                                        struct.unpack(return_value,
                                                      value.with_temp_target(return_type, n, o),
                                                      o)
                             End If
                             Return builders.of_caller(name,
                                                       value.with_single_data_slot_temp_target(return_type, n, o),
                                                       parameters).
                                             to(o)
                         End Function)
        End Function
    End Class
End Class
