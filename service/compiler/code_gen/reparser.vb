
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.service.automata

Partial Public Class code_gens(Of WRITER)
    Public MustInherit Class reparser(Of _PARSER As __do(Of String, WRITER, Boolean))
        Implements code_gen(Of WRITER)

        Private Shared ReadOnly parser As _PARSER = alloc(Of _PARSER)()

        Public Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim s As String = Nothing
            Return dump(n, s) AndAlso parser(s, o)
        End Function

        Protected MustOverride Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
    End Class
End Class
