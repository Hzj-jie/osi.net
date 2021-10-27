
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public NotInheritable Class typed_node_writer
        Private ReadOnly v As vector(Of Object)

        Public Sub New()
            v = New vector(Of Object)()
        End Sub

        Public Function append(ByVal n As typed_node) As typed_node_writer
            assert(Not n Is Nothing)
            v.emplace_back(n.input())
            Return Me
        End Function

        Public Function append(ByVal s As String) As typed_node_writer
            assert(Not s Is Nothing)
            If Not s.empty_or_whitespace() Then
                v.emplace_back(s)
            End If
            Return Me
        End Function

        Public Function append(ByVal s As StringBuilder) As typed_node_writer
            assert(Not s Is Nothing)
            Return append(Convert.ToString(s))
        End Function

        Public Function dump() As String
            Dim r As String = Nothing
            r = v.str(character.blank)
            If debug_dump Then
                raise_error(error_type.user, "Debug dump of typed_node_writer ", r)
            End If
            Return r
        End Function
    End Class
End Class
