
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Public MustInherit Class includes
        Inherits reparser

        Private ReadOnly folders As vector(Of String)
        ' TODO: Remove
        Private ReadOnly ignore_default_folder As Boolean
        ' TODO: Remove
        Private ReadOnly default_folder As String
        Private ReadOnly ignore_include_error As Boolean

        Protected Sub New(ByVal folders As vector(Of String),
                          ByVal ignore_default_folder As Boolean,
                          ByVal default_folder As String,
                          ByVal ignore_include_error As Boolean)
            assert(Not default_folder.null_or_whitespace())
            Me.folders = folders
            Me.ignore_default_folder = ignore_default_folder
            Me.default_folder = default_folder
            Me.ignore_include_error = ignore_include_error
        End Sub

        Protected NotOverridable Overrides Function parse(ByVal s As String, ByVal o As WRITER) As Boolean
            Return file_parse(s, o)
        End Function

        ' Rename to make the functionality more clear.
        Protected MustOverride Function file_parse(ByVal s As String, ByVal o As WRITER) As Boolean
        Protected MustOverride Function should_include(ByVal s As String) As Boolean

        Private Shared Function include_file(ByVal p As String,
                                             ByVal s As String,
                                             ByRef o As String) As Boolean
            'Dim f As String = Path.Combine(p, s).Replace("/"c, Path.PathSeparator).Replace("\"c, Path.PathSeparator)
            Dim f As String = Path.Combine(p, s)
            If File.Exists(f) Then
                o = f
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

        Protected Function include_file(ByVal s As String, ByRef o As String) As Boolean
            If Not should_include(s) AndAlso (arguments.include_once Or True) Then
                Return True
            End If
            If include_file(folders, s, o) Then
                Return True
            End If
            If Not ignore_default_folder AndAlso include_file(default_folder, s, o) Then
                Return True
            End If
            If Not ignore_include_error Then
                raise_error(error_type.user, "Cannot find include file ", s)
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class include_with_string
        Inherits includes

        Protected Sub New(ByVal folders As vector(Of String),
                          ByVal ignore_default_folder As Boolean,
                          ByVal default_folder As String,
                          ByVal ignore_include_error As Boolean)
            MyBase.New(folders, ignore_default_folder, default_folder, ignore_include_error)
        End Sub

        Protected NotOverridable Overrides Function dump(ByVal n As typed_node, ByRef o As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class

    Public MustInherit Class include_with_file
        Inherits includes

        Private Const kw_include As String = "#include"

        Protected Sub New(ByVal folders As vector(Of String),
                          ByVal ignore_default_folder As Boolean,
                          ByVal default_folder As String,
                          ByVal ignore_include_error As Boolean)
            MyBase.New(folders, ignore_default_folder, default_folder, ignore_include_error)
        End Sub

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
