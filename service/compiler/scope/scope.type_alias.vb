
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Protected NotInheritable Class type_alias_t
        Private ReadOnly m As New unordered_map(Of String, String)()

        Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
            assert(Not [alias].null_or_whitespace())
            assert(Not canonical.null_or_whitespace())
            If builders.parameter_type.is_ref_type([alias]) Then
                raise_error(error_type.user, "Reference type ", [alias], " is not allowed to be aliased. ")
                Return False
            End If
            If builders.parameter_type.is_ref_type(canonical) Then
                raise_error(error_type.user,
                            "Reference type ",
                            canonical,
                            " is not allowed to be used as a canonical type. ")
                Return False
            End If
            If [alias].Equals(canonical) Then
                raise_error(error_type.user,
                            "Alias ",
                            [alias],
                            " cannot be defined to itself.")
                Return False
            End If
            If [alias].Equals(Me(canonical)) Then
                raise_error(error_type.user,
                            "Cycle typedefs detected, alias ",
                            [alias],
                            " equals to its canonical ",
                            canonical)
                Return False
            End If
            m([alias]) = Me(canonical)
            Return True
        End Function

        Default Public ReadOnly Property _D(ByVal [alias] As String) As String
            Get
                assert(Not [alias].null_or_whitespace())
                [alias] = m.find_or([alias], [alias])
                assert(Not [alias].null_or_whitespace())
                Return [alias]
            End Get
        End Property

        Public Sub remove(ByVal [alias] As String)
            m.erase([alias])
        End Sub
    End Class

    Public Structure type_alias_proxy
        Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
            assert(current().features().with_type_alias())
            Return current().myself().type_alias().define([alias], canonical)
        End Function

        Default Public ReadOnly Property _D(ByVal [alias] As String) As String
            Get
                assert(Not builders.parameter_type.is_ref_type([alias]))
                Dim s As T = current()
                If Not s.features().with_type_alias() Then
                    Return [alias]
                End If
                While Not s Is Nothing
                    [alias] = s.myself().type_alias()([alias])
                    s = s.parent
                End While
                Return [alias]
            End Get
        End Property

        Public Sub remove(ByVal [alias] As String)
            assert(current().features().with_type_alias())
            current().myself().type_alias().remove([alias])
        End Sub
    End Structure
End Class
