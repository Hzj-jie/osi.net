
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
    Public NotInheritable Class paramlist
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        Private ReadOnly rs As New read_scoped(Of vector(Of builders.parameter))()

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of paramlist)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            Dim v As New vector(Of builders.parameter)()
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                Using c As read_scoped(Of vector(Of builders.parameter)).ref =
                        l.typed_code_gen(Of param)().current_target()
                    v.emplace_back(+c)
                End Using
                i += uint32_1
            End While
            push(v)
            Return True
        End Function

        Private Sub push(ByVal v As vector(Of builders.parameter))
            rs.push(v)
            ' No nesting paramlist is expected.
            assert(rs.size() = 1)
        End Sub

        Public Sub empty_paramlist()
            push(vector.of(Of builders.parameter)())
        End Sub

        Public Function current_target() As read_scoped(Of vector(Of builders.parameter)).ref
            Return rs.pop()
        End Function
    End Class
End Class
