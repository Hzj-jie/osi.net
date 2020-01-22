
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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
            Dim c As [function].ref = Nothing
            c = code_gen_of(Of [function])().target()
            If c.allow_return_value() Then
                If n.child_count() <> 2 Then
                    o.err("Expect return value ", n.child().trace_back_str())
                    Return False
                End If
            Else
                If n.child_count() <> 1 Then
                    o.err("Unexpected return value ", n.child(1).trace_back_str())
                    Return False
                End If
            End If
            If n.child_count() = 2 Then
                If Not l.of(n.child(1)).build(o) Then
                    Return False
                End If
                Using r As read_scoped(Of String).ref = code_gen_of(Of value)().read_target()
                    builders.of_return(c.name(), +r).to(o)
                End Using
            Else
                builders.of_return(c.name()).to(o)
            End If
            Return True
        End Function
    End Class
End Class
