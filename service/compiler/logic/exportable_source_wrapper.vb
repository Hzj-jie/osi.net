﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class exportable_source_wrapper
        Implements exportable

        Private Shared ReadOnly debug_logic_exportable As Boolean = env_bool(env_keys("debug", "logic", "exportable"))

        Private ReadOnly v As vector(Of String)
        Private ReadOnly start As UInt32
        Private ReadOnly [end] As UInt32
        Private ReadOnly e As exportable

        Private Sub New(ByVal v As vector(Of String),
                        ByVal start As UInt32,
                        ByVal [end] As UInt32,
                        ByVal e As exportable)
            assert(Not v Is Nothing)
            assert(start >= 0 AndAlso [end] > start AndAlso [end] <= v.size())
            assert(Not e Is Nothing)
            Me.v = New vector(Of String)()
            assert(Me.v.emplace_back(+v, start, [end] - start))
            Me.e = e
        End Sub

        Public Shared Function maybe_wrap(ByVal v As vector(Of String),
                                          ByVal start As UInt32,
                                          ByVal [end] As UInt32,
                                          ByVal e As exportable) As exportable
            If debug_logic_exportable OrElse isdebugmode() Then
                Return New exportable_source_wrapper(v, start, [end], e)
            End If
            Return e
        End Function

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim start As UInt32 = o.size()
            If Not e.export(scope, o) Then
                Return False
            End If
            If o.size() > start Then
                Dim comment As String = v.str(character.blank)
                ' The size of o cannot be changed.
                o(start) = strcat(o(start), character.tab, comment_builder.str(">>>", comment))
                If o.size() - uint32_1 > start Then
                    o(o.size() - uint32_1) = strcat(o(o.size() - uint32_1),
                                                    character.tab, comment_builder.str("<<<", comment))
                End If
            End If
            Return True
        End Function
    End Class
End Namespace
