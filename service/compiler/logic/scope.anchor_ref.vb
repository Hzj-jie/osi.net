
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class scope
        Public Class anchor_ref
            ' TODO: name may not be really necessary, the user should have the name to search from anchors.
            Public ReadOnly name As String
            Public ReadOnly return_type As String
            Public ReadOnly parameters As const_array(Of builders.parameter)

            Public Sub New(ByVal name As String,
                            ByVal return_type As String,
                            ByVal parameters() As builders.parameter)
                Me.New(name, return_type, const_array.of(parameters))
            End Sub

            Public Sub New(ByVal name As String,
                           ByVal return_type As String,
                           ByVal parameters As const_array(Of builders.parameter))
                assert(Not name.null_or_whitespace())
                assert(Not return_type.null_or_whitespace())
                assert(Not parameters Is Nothing)
                Me.name = name
                Me.return_type = return_type
                Me.parameters = parameters
            End Sub

            Public Function with_begin(ByVal begin As data_ref) As anchor
                Return New anchor(name, begin, return_type, parameters)
            End Function
        End Class

        ' This class stores only the signatures of callee_refs, but not their variable definitions. Variable definitions
        ' are still in scope.variable_t. Callers should first set the variable, it also ensures the anchor_refs won't be
        ' name-conflicted.
        Private NotInheritable Class anchor_ref_t
            Private ReadOnly m As New unordered_map(Of String, anchor_ref)()

            Public Sub define(ByVal name As String,
                              ByVal return_type As String,
                              ByVal parameters() As builders.parameter)
                assert(Not name.null_or_whitespace())
                assert(Not return_type.null_or_whitespace())
                assert(m.emplace(name, New anchor_ref(name, return_type, parameters)).second())
            End Sub

            Public Function [of](ByVal name As String, ByRef o As anchor_ref) As Boolean
                Return m.find(name, o)
            End Function
        End Class

        Public Structure anchor_ref_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Sub define(ByVal name As String,
                              ByVal return_type As String,
                              ByVal parameters() As builders.parameter)
                s.ar.define(name, return_type, parameters)
            End Sub

            Public Function [of](ByVal name As String) As anchor_ref
                Dim r As anchor_ref = Nothing
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.ar.of(name, r) Then
                        Return r
                    End If
                    s = s.parent
                End While
                assert(False)
                Return Nothing
            End Function
        End Structure

        Public Function anchor_refs() As anchor_ref_proxy
            Return New anchor_ref_proxy(Me)
        End Function
    End Class
End Namespace
