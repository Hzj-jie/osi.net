
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

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
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
            Dim r As String = Nothing
            For i As UInt32 = 0 To n.child_count() - uint32_2
                If Not logic_gen_of(Of value_with_comma).build(n.child(i), o, r) Then
                    v.emplace_back(r)
                    Return False
                End If
            Next
            If Not logic_gen_of(Of value_with_comma).build_value(n.child(n.child_count() - uint32_1), o, r) Then
                v.emplace_back(r)
                Return False
            End If
            with_current_targets(v)
            Return True
        End Function
    End Class
End Class
