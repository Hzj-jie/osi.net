
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [function]
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private Shared ReadOnly s As write_scoped(Of ref)

        Shared Sub New()
            s = New write_scoped(Of ref)()
        End Sub

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [function])()
        End Sub

        Public NotInheritable Class ref
            Private ReadOnly n As typed_node

            Public Sub New(ByVal n As typed_node)
                assert(Not n Is Nothing)
                assert(n.child_count() = 5 OrElse n.child_count() = 6)
                Me.n = n
            End Sub

            Public Function allow_return_value() As Boolean
                Return Not strsame(n.word(0).str(), "void")
            End Function

            Public Function name() As String
                Return n.word(1).str()
            End Function
        End Class

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using s.push(New ref(n))
                o.append("callee")
                If Not l.of(n.child(1)).build(o) Then
                    Return False
                End If
                Dim has_paramlist As Boolean = False
                has_paramlist = strsame(n.child(3).type_name, "paramlist")
                If has_paramlist Then
                    If Not l.of(n.child(3)).build(o) Then
                        Return False
                    End If
                End If
                Dim gi As UInt32 = 0
                gi = CUInt(If(has_paramlist, 5, 4))
                If Not l.of(n.child(gi)).build(o) Then
                    Return False
                End If
                Return True
            End Using
        End Function
    End Class
End Class
