
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class ifndef_wrapped
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly d As unordered_set(Of String)

        Private Sub New(ByVal i As logic_gens, ByVal parameters As parameters_t)
            MyBase.New(i)
            assert(Not parameters Is Nothing)
            d = parameters.defines
            assert(Not d Is Nothing)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal parameters As parameters_t)
            assert(Not b Is Nothing)
            b.register(New ifndef_wrapped(b, parameters))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 3)
            If d.find(n.child(1).word().str()) <> d.end() Then
                Return True
            End If
            For i As UInt32 = 2 To n.child_count() - uint32_2
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class

    Public NotInheritable Class define
        Implements logic_gen

        Private ReadOnly d As unordered_set(Of String)

        Private Sub New(ByVal parameters As parameters_t)
            assert(Not parameters Is Nothing)
            d = parameters.defines
            assert(Not d Is Nothing)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal parameters As parameters_t)
            assert(Not b Is Nothing)
            b.register(New define(parameters))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            d.insert(n.child(1).word().str())
            Return True
        End Function
    End Class
End Class
