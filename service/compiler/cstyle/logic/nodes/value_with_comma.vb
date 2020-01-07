
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Partial Public NotInheritable Class value_with_comma
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_with_comma)()
        End Sub

        Public Function build_value(ByVal n As typed_node, ByVal o As writer, ByRef r As String) As Boolean
            assert(Not o Is Nothing)
            Dim ref As write_scoped(Of String).ref = Nothing
            ref = logic_gen_of(Of value).with_value_target(n, o)
            r = +ref
            Using ref
                If Not b.of(n).build(o) Then
                    o.err("@value-with-comma value ", n)
                    Return False
                End If
                Return True
            End Using
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return build_value(n.child(0), o, Nothing)
        End Function
    End Class
End Class
