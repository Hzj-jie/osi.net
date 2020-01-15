
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public NotInheritable Class [default]
        Inherits rewriter_wrapper
        Implements rewriter

        Public Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Public Shared Sub register(ByVal l As rewriters, ByVal s As String)
            assert(Not l Is Nothing)
            assert(Not s.null_or_whitespace())
            l.register(s, New [default](l))
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            If n.leaf() Then
                o.append(n)
                Return True
            End If
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function
    End Class
End Class
