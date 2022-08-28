
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Protected NotInheritable Class class_t(Of WRITER As New, CODE_GENS As func_t(Of code_gens(Of WRITER)))
        Private ReadOnly m As New unordered_map(Of name_with_namespace, class_def(Of WRITER, CODE_GENS))()

        Public Function define(ByVal name As name_with_namespace,
                               ByVal def As class_def(Of WRITER, CODE_GENS)) As Boolean
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

        Public Function resolve(ByVal name As String, ByRef o As class_def(Of WRITER, CODE_GENS)) As Boolean
            Return m.find(name_with_namespace.of(name), o)
        End Function
    End Class

    Public Structure class_proxy(Of WRITER As New, CODE_GENS As func_t(Of code_gens(Of WRITER)))
        Public Function define(ByVal name As String, ByVal def As class_def(Of WRITER, CODE_GENS)) As Boolean
            Return scope(Of T).current().
                               myself().
                               classes(Of WRITER, CODE_GENS)().
                               define(name_with_namespace.of(name), def)
        End Function

        Public Function resolve(ByVal name As String, ByRef o As class_def(Of WRITER, CODE_GENS)) As Boolean
            Dim s As scope(Of T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().classes(Of WRITER, CODE_GENS)().resolve(name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            raise_error(error_type.user, "Class ", name, " has not been defined.")
            Return False
        End Function
    End Structure
End Class
