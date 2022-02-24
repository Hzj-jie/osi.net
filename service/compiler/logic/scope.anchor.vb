
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class scope
        Public NotInheritable Class anchor
            Inherits function_signature
            Public ReadOnly begin As lazier(Of data_ref)

            Public Sub New(ByVal name As String,
                           ByVal begin As UInt32,
                           ByVal return_type As String,
                           ByVal parameters() As builders.parameter_type)
                MyBase.New(name, return_type, parameters)
                Me.begin = lazier.of(Function() As data_ref
                                         Return data_ref.abs(begin)
                                     End Function)
            End Sub

            Public Sub New(ByVal name As String,
                           ByVal begin As lazier(Of data_ref),
                           ByVal return_type As String,
                           ByVal parameters As const_array(Of builders.parameter_type))
                MyBase.New(name, return_type, parameters)
                assert(Not begin Is Nothing)
                Me.begin = begin
            End Sub
        End Class

        Public NotInheritable Class anchor_t
            Private ReadOnly m As New unordered_map(Of String, anchor)()

            Public Function define(Of T As builders.parameter_type) _
                                  (ByVal name As String,
                                   ByVal o As vector(Of String),
                                   ByVal return_type As String,
                                   ByVal parameters() As T) As Boolean
                assert(Not name.null_or_whitespace())
                assert(Not o Is Nothing)
                assert(Not return_type.null_or_whitespace())
                Dim a As New anchor(name, o.size(), return_type, parameters)
                If m.emplace(a.name, a).second() Then
                    Return True
                End If
                errors.anchor_redefined(a.name, +a.begin, +m(a.name).begin)
                Return False
            End Function

            Public Function [of](ByVal name As String, ByRef o As anchor) As Boolean
                Return m.find(name, o)
            End Function
        End Class

        Public Function anchors() As anchor_t
            If is_root() Then
                assert(Not a Is Nothing)
                Return a
            End If
            assert(a Is Nothing)
            Return (+root).anchors()
        End Function
    End Class
End Namespace
