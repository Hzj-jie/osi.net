
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class reinterpret_cast
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New reinterpret_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6 OrElse n.child_count() = 7)
            Dim type As String =
                scope.current().type_alias()(builders.parameter_type.remove_ref(n.child(4).input_without_ignored()))
            Dim name As String = n.child(2).input_without_ignored()
            Dim s As [optional](Of struct_def) = scope.current().structs().resolve(type, name)
            Dim f As Func(Of String, String, Boolean) = Nothing
            If n.child_count() = 6 Then
                f = Function(ByVal x As String, ByVal y As String) As Boolean
                        Return builders.of_redefine(x, y).to(o)
                    End Function
            Else
                f = Function(ByVal x As String, ByVal y As String) As Boolean
                        Return builders.of_redefine_heap(x, y).to(o)
                    End Function
            End If
            If s.empty() Then
                If Not f(name, type) Then
                    Return False
                End If
            Else
                If Not (+s).primitives().
                            map(Function(ByVal p As builders.parameter) As Boolean
                                    assert(Not p Is Nothing)
                                    Return f(p.name, p.type)
                                End Function).
                            aggregate(bool_stream.aggregators.all_true) Then
                    Return False
                End If
            End If
            Return scope.current().variables().redefine(type, name)
        End Function
    End Class
End Class
