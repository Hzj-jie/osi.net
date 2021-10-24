
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
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of function_call)()
        End Sub

        Private Function build(ByVal n As typed_node,
                               ByVal o As writer,
                               ByVal build_caller As Func(Of String, vector(Of String), Boolean)) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(Not build_caller Is Nothing)
            assert(n.child_count() >= 3)
            Dim callee_name As String = Nothing
            callee_name = n.child(0).word().str()
            If n.child_count() = 3 Then
                Return build_caller(callee_name, vector.of(Of String)())
            End If
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using targets As read_scoped(Of vector(Of String)).ref = code_gen_of(Of value_list)().current_targets()
                Return build_caller(callee_name, +targets)
            End Using
        End Function

        Public Function without_return(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return build(n,
                         o,
                         Function(ByVal callee_name As String, ByVal parameters As vector(Of String)) As Boolean
                             Return logic_name.of_caller(callee_name, parameters, o)
                         End Function)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 3)
            Return build(n,
                         o,
                         Function(ByVal callee_name As String, ByVal parameters As vector(Of String)) As Boolean
                             Dim name As String = Nothing
                             If Not logic_name.of_function_call(callee_name, parameters, name) Then
                                 Return False
                             End If
                             Dim return_type As String = Nothing
                             If Not scope.current().functions().return_type_of(name, return_type) Then
                                 Return False
                             End If
                             If scope.current().structs().defined(return_type) Then
                                 ' TODO: Check the type consistency between function_call and variable receiver.
                                 Dim return_value As String =
                                         strcat(logic_name.temp_variable(n), "@", name, "@return_value")
                                 assert(value_declaration.declare_internal_typed(
                                            types.variable_type, return_value, o))
                                 builders.of_caller(name, return_value, parameters).to(o)
                                 Return struct.unpack(return_value,
                                                      code_gen_of(Of value)().with_temp_target(return_type, n, o),
                                                      o)
                             End If
                             builders.of_caller(name,
                                                code_gen_of(Of value)().with_internal_typed_temp_target(
                                                    return_type, n, o),
                                                parameters).
                                      to(o)
                             Return True
                         End Function)
        End Function
    End Class
End Class
