﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class type_alias_t
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
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
                Return s.ta.define([alias], canonical)
            End Function

            Private Function retrieve(ByVal [alias] As String) As String
                assert(Not builders.parameter_type.is_ref_type([alias]))
                Dim s As scope = Me.s
                While Not s Is Nothing
                    [alias] = s.ta([alias])
                    s = s.parent
                End While
                Return [alias]
            End Function

            Public Function canonical_of(ByVal [alias] As String) As String
                Return canonical_of(New builders.parameter_type([alias])).logic_type()
            End Function

            Default Public ReadOnly Property _D(ByVal [alias] As String) As String
                Get
                    Return canonical_of([alias])
                End Get
            End Property

            Public Function canonical_of(ByVal p As builders.parameter_type) As builders.parameter_type
                assert(Not p Is Nothing)
                Return p.map_type(AddressOf retrieve)
            End Function

            Public Function canonical_of(ByVal p As builders.parameter) As builders.parameter
                assert(Not p Is Nothing)
                Return p.map_type(AddressOf retrieve)
            End Function

            Public Sub remove(ByVal [alias] As String)
                s.ta.remove([alias])
            End Sub
        End Structure

        Public Function type_alias() As type_alias_proxy
            Return New type_alias_proxy(Me)
        End Function
    End Class
End Class
