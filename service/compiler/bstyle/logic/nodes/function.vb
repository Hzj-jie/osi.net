
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [function]
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        Private Shared remove_unused_functions As argument(Of Boolean)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of [function])()
        End Sub

        Private NotInheritable Class function_writer
            Private ReadOnly o As writer
            Private ReadOnly scope As scope
            Private ReadOnly f As String

            Public Sub New(ByVal o As writer, ByVal f As String)
                assert(Not o Is Nothing)
                assert(Not f.null_or_whitespace())
                Me.o = o
                Me.scope = scope.current()
                Me.f = f
            End Sub

            Public Overrides Function ToString() As String
                If (remove_unused_functions Or True) AndAlso
                   Not scope.function_calls().can_reach_main(f) Then
                    Return empty_string
                End If
                Return o.dump()
            End Function
        End Class

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using New scope_wrapper()
                Dim fo As New writer()
                Dim has_paramlist As Boolean = strsame(n.child(3).type_name, "paramlist")
                If has_paramlist Then
                    If Not l.of(n.child(3)).build(fo) Then
                        Return False
                    End If
                Else
                    l.typed_code_gen(Of paramlist)().empty_paramlist()
                End If
                Using params As read_scoped(Of vector(Of builders.parameter)).ref =
                          l.typed_code_gen(Of paramlist)().current_target()
                    If Not logic_name.of_callee(n.child(1).word().str(),
                                                n.child(0).word().str(),
                                                +params,
                                                Function() As Boolean
                                                    Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                    Return l.of(n.child(gi)).build(fo)
                                                End Function,
                                                fo) Then
                        Return False
                    End If
                    o.append(New function_writer(fo, logic_name.of_function(n.child(1).word().str(), +params)))
                    Return True
                End Using
            End Using
        End Function
    End Class
End Class
