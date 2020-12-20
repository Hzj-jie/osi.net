
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
                               ByVal build_caller As Action(Of String, vector(Of String))) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(Not build_caller Is Nothing)
            assert(n.child_count() >= 3)
            Dim callee_name As String = Nothing
            callee_name = n.child(0).word().str()
            If n.child_count() = 3 Then
                build_caller(callee_name, vector.of(Of String)())
                Return True
            End If
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using targets As read_scoped(Of vector(Of String)).ref = code_gen_of(Of value_list)().current_targets()
                build_caller(callee_name, +targets)
                Return True
            End Using
        End Function

        Public Function without_return(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return build(n,
                         o,
                         Sub(ByVal callee_name As String, ByVal parameters As vector(Of String))
                             function_name.of_caller(callee_name, parameters).to(o)
                         End Sub)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 3)
            Return build(n,
                         o,
                         Sub(ByVal callee_name As String, ByVal parameters As vector(Of String))
                             Dim name As String = Nothing
                             name = function_name.of_function_call(callee_name, parameters)
                             Dim value_name As String = Nothing
                             value_name = code_gen_of(Of value)().with_temp_target(macros.return_type_of(name), n, o)
                             builders.of_caller(name, value_name, parameters).to(o)
                         End Sub)
        End Function
    End Class
End Class
