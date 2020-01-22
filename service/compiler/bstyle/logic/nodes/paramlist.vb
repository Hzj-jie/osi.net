
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
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly rs As read_scoped(Of vector(Of pair(Of String, String)))

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
            Me.rs = New read_scoped(Of vector(Of pair(Of String, String)))()
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of paramlist)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            Dim v As vector(Of pair(Of String, String)) = Nothing
            v = New vector(Of pair(Of String, String))()
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                Using c As read_scoped(Of pair(Of String, String)).ref = code_gen_of(Of param)().current_target()
                    v.emplace_back(+c)
                End Using
                i += uint32_1
            End While
            push(v)
            Return True
        End Function

        Private Sub push(ByVal v As vector(Of pair(Of String, String)))
            rs.push(v)
            ' No nesting paramlist is expected.
            assert(rs.size() = 1)
        End Sub

        Public Sub empty_paramlist()
            push(vector.of(Of pair(Of String, String))())
        End Sub

        Public Function current_target() As read_scoped(Of vector(Of pair(Of String, String))).ref
            Return rs.pop()
        End Function
    End Class
End Class
