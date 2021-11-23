
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
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of return_clause)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
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
                Return builders.of_return(scope.current().current_function().name()).to(o)
            End If
            If Not l.of(n.child(1)).build(o) Then
                Return False
            End If
            Using r As read_scoped(Of value.target).ref = l.typed_code_gen(Of value)().read_target()
                If scope.current().current_function().return_struct() Then
                    ' The return type check of single-data-slot-target will be handled by logic.
                    If Not scope.current().current_function().return_type().Equals((+r).type) Then
                        raise_error(error_type.user,
                                    "Return type ",
                                    scope.current().current_function().return_type(),
                                    " of function ",
                                    scope.current().current_function().name(),
                                    " does not match ",
                                    (+r).type)
                        Return False
                    End If
                    Dim return_value As String = strcat(logic_name.temp_variable(n),
                                                        "@",
                                                        scope.current().current_function().name(),
                                                        "@return_value")
                    assert(value_declaration.declare_single_data_slot(
                               types.variable_type, return_value, o))
                    Return struct.pack((+r).names, return_value, o) AndAlso
                           builders.of_return(scope.current().current_function().name(), return_value).to(o)
                End If
                If (+r).names.size() <> 1 Then
                    raise_error(error_type.user,
                                "Unexpected return value, do not expect a struct to be returned by ",
                                scope.current().current_function().name())
                    Return False
                End If
                Return builders.of_return(scope.current().current_function().name(), (+r).names(0)).to(o)
            End Using
        End Function
    End Class
End Class
