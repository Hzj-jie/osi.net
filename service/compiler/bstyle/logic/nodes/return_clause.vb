
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class return_clause
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of return_clause)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 1 OrElse n.child_count() = 2)
            If scope.current().current_function().allow_return_value() Then
                If n.child_count() <> 2 Then
                    raise_error(error_type.user, "Expect return value ", n.child().trace_back_str())
                    Return False
                End If
            Else
                If n.child_count() <> 1 Then
                    raise_error(error_type.user, "Unexpected return value ", n.child(1).trace_back_str())
                    Return False
                End If
            End If
            If n.child_count() = 1 Then
                builders.of_return(scope.current().current_function().name()).to(o)
                Return True
            End If
            If Not l.of(n.child(1)).build(o) Then
                Return False
            End If
            Using r As read_scoped(Of vector(Of String)).ref = code_gen_of(Of value)().read_target()
                If scope.current().current_function().return_struct() Then
                    ' TODO: Check if return-type matches value-type.
                    Dim return_value As String = strcat(logic_name.temp_variable(n),
                                                        "@",
                                                        scope.current().current_function().name(),
                                                        "@return_value")
                    assert(value_declaration.declare_internal_typed_variable(
                               types.variable_type, return_value, o))
                    If Not struct.pack(+r, return_value, o) Then
                        Return False
                    End If
                    builders.of_return(scope.current().current_function().name(), return_value).to(o)
                Else
                    If (+r).size() <> 1 Then
                        raise_error(error_type.user,
                                    "Unexpected return value, do not expect a struct to be returned by ",
                                    scope.current().current_function().name())
                        Return False
                    End If
                    builders.of_return(scope.current().current_function().name(), (+r)(0)).to(o)
                End If
            End Using
            Return True
        End Function
    End Class
End Class
