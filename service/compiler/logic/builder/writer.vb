
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class writer
        Private ReadOnly v As New vector(Of Object)()

        Public Function append(ByVal s As UInt32) As Boolean
            v.emplace_back(s)
            Return True
        End Function

        Public Function append(ByVal s As String) As Boolean
            ' Allow appending newline characters.
            assert(Not s.null_or_empty())
            v.emplace_back(s)
            Return True
        End Function

        Public Function append(ByVal s As data_block) As Boolean
            assert(Not s Is Nothing)
            v.emplace_back(s)
            Return True
        End Function

        Public Function append(ByVal v As vector(Of String)) As Boolean
            assert(Not v Is Nothing)
            If v.empty() Then
                Return True
            End If
            Return append(v.str(character.blank))
        End Function

        Public Function append(ByVal v As vector(Of pair(Of String, String))) As Boolean
            assert(Not v Is Nothing)
            If v.empty() Then
                Return True
            End If
            Return append(v.str(Function(ByVal x As pair(Of String, String)) As String
                                    assert(Not x Is Nothing)
                                    Return strcat(x.first, character.blank, x.second)
                                End Function,
                                character.blank))
        End Function

        Public Function append(ByVal a As Func(Of Boolean)) As Boolean
            assert(Not a Is Nothing)
            Return a()
        End Function

        Public Function append(ByVal obj As Object) As Boolean
            assert(Not obj Is Nothing)
            v.emplace_back(obj)
            Return True
        End Function

        Public Function dump() As String
            Dim r As String = v.str(character.blank)
            If builders.debug_dump Then
                raise_error(error_type.user, "Debug dump of logic ", r)
            End If
            Return r
        End Function
    End Class
End Namespace
