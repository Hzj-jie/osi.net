﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Public MustInherit Class includes(Of PARSER As __do(Of String, WRITER, Boolean),
                                         _FOLDERS As __do(Of vector(Of String)),
                                         _IGNORE_DEFAULT_FOLDER As __do(Of Boolean),
                                         _DEFAULT_FOLDER As __do(Of String),
                                         _IGNORE_INCLUDE_ERROR As __do(Of Boolean),
                                         SCOPE_T As scope(Of SCOPE_T))
        Inherits reparser(Of PARSER)

        Private Shared ReadOnly folders As _FOLDERS = alloc(Of _FOLDERS)()
        Private Shared ReadOnly ignore_default_folder As _IGNORE_DEFAULT_FOLDER = alloc(Of _IGNORE_DEFAULT_FOLDER)()
        Private Shared ReadOnly default_folder As _DEFAULT_FOLDER = alloc(Of _DEFAULT_FOLDER)()
        Private Shared ReadOnly ignore_include_error As _IGNORE_INCLUDE_ERROR = alloc(Of _IGNORE_INCLUDE_ERROR)()

        Private Shared Function include_file(ByVal p As String,
                                             ByVal s As String,
                                             ByRef o As String) As Boolean
            'Dim f As String = Path.Combine(p, s).Replace("/"c, Path.PathSeparator).Replace("\"c, Path.PathSeparator)
            Dim f As String = Path.Combine(p, s)
            If File.Exists(f) Then
                o = File.ReadAllText(f)
                Return True
            End If
            Return False
        End Function

        Private Shared Function include_file(ByVal v As vector(Of String),
                                             ByVal s As String,
                                             ByRef o As String) As Boolean
            Dim i As UInt32 = 0
            While i < v.size_or_0()
                If include_file(v(i), s, o) Then
                    Return True
                End If
                i += uint32_1
            End While
            Return False
        End Function

        Protected Shared Function include_file(ByVal s As String, ByRef o As String) As Boolean
            If Not scope(Of SCOPE_T).current().includes().included(s) AndAlso
               (scope_arguments.do_not_include_twice Or True) Then
                Return True
            End If
            If include_file(+folders, s, o) Then
                Return True
            End If
            If Not (+ignore_default_folder) AndAlso include_file(+default_folder, s, o) Then
                Return True
            End If
            If Not (+ignore_include_error) Then
                raise_error(error_type.user, "Cannot find include file ", s)
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class include_with_string(Of PARSER As __do(Of String, WRITER, Boolean),
                                                    FOLDERS As __do(Of vector(Of String)),
                                                    IGNORE_DEFAULT_FOLDER As __do(Of Boolean),
                                                    DEFAULT_FOLDER As __do(Of String),
                                                    IGNORE_INCLUDE_ERROR As __do(Of Boolean),
                                                    SCOPE_T As scope(Of SCOPE_T))
        Inherits includes(Of PARSER, FOLDERS, IGNORE_DEFAULT_FOLDER, DEFAULT_FOLDER, IGNORE_INCLUDE_ERROR, SCOPE_T)

        Protected NotOverridable Overrides Function dump(ByVal n As typed_node, ByRef o As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class

    Public MustInherit Class include_with_file(Of PARSER As __do(Of String, WRITER, Boolean),
                                                  FOLDERS As __do(Of vector(Of String)),
                                                  IGNORE_DEFAULT_FOLDER As __do(Of Boolean),
                                                  DEFAULT_FOLDER As __do(Of String),
                                                  IGNORE_INCLUDE_ERROR As __do(Of Boolean),
                                                  SCOPE_T As scope(Of SCOPE_T))
        Inherits includes(Of PARSER, FOLDERS, IGNORE_DEFAULT_FOLDER, DEFAULT_FOLDER, IGNORE_INCLUDE_ERROR, SCOPE_T)

        Private Const kw_include As String = "#include"

        Protected NotOverridable Overrides Function dump(ByVal n As typed_node, ByRef o As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.leaf())
            Dim file As String = n.word().str()
            assert(file.StartsWith(kw_include))
            file = file.Substring(kw_include.Length()).Trim()
            assert(file.StartsWith(character.left_angle_bracket))
            assert(file.EndsWith(character.right_angle_bracket))
            Return include_file(file.Substring(1, file.Length() - 2), o)
        End Function
    End Class
End Class
