
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template
Imports osi.service.automata

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Public MustInherit Class reparser(Of _PARSER As __do(Of String, WRITER, Boolean))
        Implements code_gen(Of WRITER)

        Private Shared ReadOnly parser As _PARSER = alloc(Of _PARSER)()

        Public Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim s As String = Nothing
            If Not dump(n, s) Then
                If handle_not_dumpable(n, o) Then
                    Return True
                End If
                raise_error(error_type.user, "Failed to dump ", n)
                Return False
            End If
            Using wrapper(n)
                If Not parser(s, o) Then
                    raise_error(error_type.user, "Failed to parse ", n)
                    Return False
                End If
                Return True
            End Using
        End Function

        Protected MustOverride Function dump(ByVal n As typed_node, ByRef s As String) As Boolean

        Protected Overridable Function wrapper(ByVal n As typed_node) As IDisposable
            Return defer.to(Sub()
                            End Sub)
        End Function

        Protected Overridable Function handle_not_dumpable(ByVal n As typed_node, ByVal o As WRITER) As Boolean
            Return False
        End Function
    End Class
End Class
