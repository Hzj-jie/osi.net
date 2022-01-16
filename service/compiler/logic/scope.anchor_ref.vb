
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
            ' TODO: should use string as type instead of builders.parameter
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

            Public Function with_begin(ByVal begin As Func(Of data_ref)) As anchor
                Return New anchor(name, lazier.of(begin), return_type, parameters)
            End Function

            Public Overrides Function ToString() As String
                Return strcat(return_type, " ", name, "(", parameters, ")")
            End Function
        End Class

        ' This class stores only the signatures of callee_refs, but not their variable definitions. Variable definitions
        ' are still in scope.variable_t. Callers should first set the variable, it also ensures the anchor_refs won't be
        ' name-conflicted.
        Private NotInheritable Class anchor_ref_t
            Private ReadOnly decls As New unordered_map(Of String, anchor_ref)()
            Private ReadOnly defs As New unordered_map(Of String, anchor_ref)()

            Public Function decl(ByVal type As String,
                                 ByVal return_type As String,
                                 ByVal parameters() As builders.parameter) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not return_type.null_or_whitespace())
                If decls.emplace(type, New anchor_ref(type, return_type, parameters)).second() Then
                    Return True
                End If
                errors.anchor_ref_redefined(type, decls(type))
                Return False
            End Function

            Public Sub define(ByVal name As String, ByVal anchor As anchor_ref)
                assert(defs.emplace(name, anchor).second())
            End Sub

            Public Function find_decl(ByVal type As String, ByRef o As anchor_ref) As Boolean
                Return decls.find(type, o)
            End Function

            Public Function [of](ByVal name As String, ByRef o As anchor_ref) As Boolean
                Return defs.find(name, o)
            End Function
        End Class

        Public Structure anchor_ref_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function decl(ByVal type As String,
                                 ByVal return_type As String,
                                 ByVal parameters() As builders.parameter) As Boolean
                Return s.ar.decl(type, return_type, parameters)
            End Function

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                Dim anchor As anchor_ref = Nothing
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.ar.find_decl(type, anchor) Then
                        Me.s.ar.define(name, anchor)
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function

            Public Function [of](ByVal name As String, ByRef o As anchor_ref) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.ar.of(name, o) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function
        End Structure

        Public Function anchor_refs() As anchor_ref_proxy
            Return New anchor_ref_proxy(Me)
        End Function
    End Class
End Namespace
