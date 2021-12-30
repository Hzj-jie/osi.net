
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

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using New scope_wrapper()
                Dim fo As New writer()
                Dim params As vector(Of builders.parameter)
                Dim has_paramlist As Boolean = strsame(n.child(3).type_name, "paramlist")
                If has_paramlist Then
                    params = l.typed_code_gen(Of paramlist).build(n.child(3))
                Else
                    params = New vector(Of builders.parameter)()
                End If
                Return logic_name.of_callee(n.child(1).word().str(),
                                            n.child(0).word().str(),
                                            params,
                                            Function() As Boolean
                                                Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                Return l.of(n.child(gi)).build(fo)
                                            End Function,
                                            fo) AndAlso
                       o.append(scope.current().call_hierarchy().filter(
                                    logic_name.of_function(n.child(1).word().str(), params),
                                    AddressOf fo.dump))
            End Using
        End Function
    End Class
End Class
