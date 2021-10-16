
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class ifndef_wrapped
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of ifndef_wrapped)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 3)
            If scope.current().defines().is_defined(n.child(1).word().str()) Then
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

        Private Shared ReadOnly instance As New define()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            scope.current().defines().define(n.child(1).word().str())
            Return True
        End Function
    End Class
End Class
