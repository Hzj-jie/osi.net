
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata

Namespace logic
    Public Class logic_rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                                       _syntaxer_rule As __do(Of Byte()),
                                       _prefixes As __do(Of vector(Of Action(Of prefixes))),
                                       _logic_gens As __do(Of vector(Of Action(Of logic_gens))))
        Inherits rule_wrapper(Of _nlexer_rule, _syntaxer_rule)

        Private Shared ReadOnly l As logic_gens
        Private Shared ReadOnly p As prefixes

        Shared Sub New()
            l = New logic_gens()
            p = New prefixes()
            init_prefixes()
            init_logic_gens()
        End Sub

        Public Shared Function build(ByVal root As typed_node, ByVal o As writer) As Boolean
            assert(Not root Is Nothing)
            assert(Not o Is Nothing)
            assert(root.type = typed_node.ROOT_TYPE)
            assert(strsame(root.type_name, typed_node.ROOT_TYPE_NAME))
            If root.leaf() Then
                Return False
            End If
            assert(root.child_count() > 0)
            For i As UInt32 = 0 To root.child_count() - uint32_1
                l.of(root.child(i)).build(o)
            Next
            Return True
        End Function

        Private Shared Sub init_prefixes()
            Dim v As vector(Of Action(Of prefixes)) = Nothing
            v = +alloc(Of _prefixes)()
            Dim i As UInt32 = 0
            While i < v.size()
                v(i)(p)
                i += uint32_1
            End While
        End Sub

        Private Shared Sub init_logic_gens()
            Dim v As vector(Of Action(Of logic_gens)) = Nothing
            v = +alloc(Of _logic_gens)()
            Dim i As UInt32 = 0
            While i < v.size()
                v(i)(l)
                i += uint32_1
            End While
        End Sub

        Protected Sub New()
        End Sub
    End Class

End Namespace
