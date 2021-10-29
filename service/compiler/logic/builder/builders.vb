
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Namespace logic
    Partial Public NotInheritable Class builders
        Public Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("compiler", "debug", "dump"))

        Public NotInheritable Class parameter
            Private Const type_ref_suffix As Char = character.and_mark
            Public ReadOnly type As String
            Public ReadOnly name As String
            Public ReadOnly ref As Boolean

            Public Sub New(ByVal type As String, ByVal name As String)
                assert(Not type Is Nothing)
                assert(Not name Is Nothing)
                type = type.Trim()
                name = name.Trim()
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                ' TODO: Try to avoid allowing only "&" as parameter type.
                ' assert(Not type.Equals(type_ref_suffix))
                Me.ref = type.EndsWith(type_ref_suffix) AndAlso Not type.Equals(type_ref_suffix)
                If Me.ref Then
                    Me.type = type.Remove(type.Length() - 1)
                Else
                    Me.type = type
                End If
                Me.name = name
            End Sub

            Public Shared Function of_ref(ByVal type As String, ByVal name As String) As parameter
                assert(Not type.null_or_whitespace())
                Return New parameter(strcat(type, type_ref_suffix), name)
            End Function

            Public Shared Function from_logic_callee_input(ByVal parameters() As pair(Of String, String)) As parameter()
                If parameters Is Nothing Then
                    Return Nothing
                End If
                Return streams.of(parameters).
                               map(Function(ByVal p As pair(Of String, String)) As parameter
                                       assert(Not p Is Nothing)
                                       Return New parameter(p.second, p.first)
                                   End Function).
                               to_array()
            End Function
        End Class

        Public Shared Function of_callee(ByVal name As String,
                                         ByVal type As String,
                                         ByVal parameters As vector(Of parameter),
                                         ByVal paragraph As Func(Of Boolean)) As callee_builder_15
            Return of_callee(name,
                             type,
                             parameters.map(Function(ByVal i As parameter) As pair(Of String, String)
                                                assert(Not i Is Nothing)
                                                Return pair.emplace_of(i.name, i.type)
                                            End Function),
                             paragraph)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
