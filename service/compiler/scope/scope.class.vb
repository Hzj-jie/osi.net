
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Protected NotInheritable Class class_t
        Private ReadOnly m As New unordered_map(Of name_with_namespace, class_def)()

        Public Function define(ByVal name As name_with_namespace,
                               ByVal def As class_def) As Boolean
            assert(Not def Is Nothing)
            If Not def.check() Then
                Return False
            End If
            If m.emplace(name, def).second() Then
                Return True
            End If
            raise_error(error_type.user, "Class ", name, " has been defined already.")
            Return False
        End Function

        Public Function resolve(ByVal name As String, ByRef o As class_def) As Boolean
            Return m.find(name_with_namespace.of(name), o)
        End Function
    End Class

    Public Structure class_proxy
        Public Function define(ByVal name As String, ByVal def As class_def) As Boolean
            Return scope(Of T).current().
                               myself().
                               classes().
                               define(name_with_namespace.of(name), def)
        End Function

        Public Function resolve(ByVal name As String, ByRef o As class_def) As Boolean
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().classes().resolve(name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            raise_error(error_type.user, "Class ", name, " has not been defined.")
            Return False
        End Function

        Public Function is_defined(ByVal name As String) As Boolean
            Return resolve(name, Nothing)
        End Function
    End Structure
End Class
