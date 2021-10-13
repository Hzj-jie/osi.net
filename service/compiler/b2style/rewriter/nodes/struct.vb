
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class struct
        Inherits [default]
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of struct)()
        End Sub

        Protected Overrides Function build(ByVal child As typed_node,
                                           ByVal index As UInt32,
                                           ByVal o As typed_node_writer) As Boolean
            assert(Not child Is Nothing)
            assert(Not o Is Nothing)
            If child.type_name.Equals("value-declaration-with-semi-colon") Then
                ' TODO: Support value-definition
                assert(child.child_count() = 2)
                assert(child.child(0).child_count() = 2)
                If Not l.of(child.child(0).child(0)).build(o) Then
                    Return False
                End If
                ' Ignore namespace prefix for variables within the structure.
                o.append(child.child(0).child(1))
                If Not l.of(child.child(1)).build(o) Then
                    Return False
                End If
            End If
            Return MyBase.build(child, index, o)
        End Function
    End Class
End Class
