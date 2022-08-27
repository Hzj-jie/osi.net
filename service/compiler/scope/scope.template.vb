
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Protected NotInheritable Class template_t(Of WRITER As {lazy_list_writer, New},
                                                 _BUILDER As func_t(Of String, WRITER, Boolean))
        Private Shared ReadOnly builder As func_t(Of String, WRITER, Boolean) = alloc(Of _BUILDER)()
        Private ReadOnly m As New unordered_map(Of name_with_namespace, definition)()

        Private NotInheritable Class definition
            Private ReadOnly template As template_template
            Private ReadOnly injected_types As New unordered_set(Of vector(Of String))()

            Public Sub New(ByVal t As template_template)
                assert(Not t Is Nothing)
                template = t
            End Sub

            Public Function apply(ByVal types As vector(Of String), ByRef o As String) As Boolean
                ' TODO: Should resolve type-aliases.
                If injected_types.emplace(types).second() Then
                    Return template.apply(types, o)
                End If
                ' Injected already.
                o = Nothing
                Return True
            End Function

            Public Function extended_type_name(ByVal types As vector(Of String)) As String
                Return template.extended_type_name(types)
            End Function
        End Class

        Public Function define(ByVal name As String, ByVal t As template_template) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not t Is Nothing)
            Dim d As New definition(t)
            If m.emplace(name_with_namespace.of(name), d).second() Then
                Return True
            End If
            raise_error(error_type.user,
                        "Template [",
                        name_with_namespace.of(name),
                        "] has been defined already.")
            Return False
        End Function

        Public Function resolve(ByVal input_name As String,
                                ByVal types As vector(Of String),
                                ByRef extended_type_name As String) As ternary
            assert(Not types.null_or_empty())
            Dim name As name_with_namespace = name_with_namespace.of(input_name)
            Dim d As definition = Nothing
            If Not m.find(name, d) Then
                Return ternary.unknown
            End If
            assert(Not d Is Nothing)
            Dim s As String = Nothing
            If Not d.apply(types, s) Then
                Return ternary.false
            End If
            assert(s Is Nothing OrElse Not s.empty_or_whitespace())
            If Not s Is Nothing Then
                Using If(name.namespace().empty_or_whitespace(),
                         empty_idisposable.instance,
                         scope(Of T).current().current_namespace().define(name.namespace()))
                    If Not builder.run(s, scope(Of T).current().root_type_injector(Of WRITER)().current()) Then
                        Return ternary.false
                    End If
                End Using
            End If
            extended_type_name = scope(Of T).current_namespace_t.with_namespace(
                                     name.namespace(),
                                     d.extended_type_name(types))
            Return ternary.true
        End Function
    End Class

    Public Structure template_proxy(Of WRITER As {lazy_list_writer, New},
                                       BUILDER As func_t(Of String, WRITER, Boolean))
        Public Function define(ByVal name As String, ByVal t As template_template) As Boolean
            Return scope(Of T).current().myself().template(Of WRITER, BUILDER)().define(name, t)
        End Function

        Public Function resolve(ByVal name As String,
                                ByVal types As vector(Of String),
                                ByRef extended_type_name As String,
                                ByVal msg As Object) As Boolean
            Dim s As scope(Of T) = scope(Of T).current()
            While Not s Is Nothing
                Dim r As ternary = s.myself().
                                     template(Of WRITER, BUILDER)().
                                     resolve(name, types, extended_type_name)
                If Not r.unknown_() Then
                    Return r
                End If
                s = s.parent
            End While
            raise_error(error_type.user,
                        "Template [",
                        name,
                        "] has not been defined for ",
                        msg)
            Return False
        End Function
    End Structure
End Class
