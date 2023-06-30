
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    ' Do not add extra functionalities into this class, uses out of the current function set should use parser directly.
    Public MustInherit Class reparser
        Implements code_gen(Of WRITER)

        Private Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim s As String = Nothing
            If Not dump(n, s) Then
                raise_error(error_type.user, "Failed to dump ", n)
                Return False
            End If
            If Not parse(s, o) Then
                raise_error(error_type.user, "Failed to parse ", n)
                Return False
            End If
            Return True
        End Function

        Protected MustOverride Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
        Protected MustOverride Function parse(ByVal s As String, ByVal o As WRITER) As Boolean
    End Class
End Class
