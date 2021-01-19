
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
    Partial Public NotInheritable Class value_list
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly rs As read_scoped(Of vector(Of String))

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
            Me.rs = New read_scoped(Of vector(Of String))()
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_list)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() > 0)
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                Using r As read_scoped(Of String).ref = code_gen_of(Of value)().read_target()
                    v.emplace_back(+r)
                End Using
                i += uint32_1
            End While
            rs.push(v)
            Return True
        End Function

        Public Function current_targets() As read_scoped(Of vector(Of String)).ref
            Return rs.pop()
        End Function
    End Class
End Class
