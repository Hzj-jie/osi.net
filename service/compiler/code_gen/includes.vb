
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Public MustInherit Class includes(Of _SHOULD_INCLUDE As func_t(Of String, Boolean),
                                         _FOLDERS As func_t(Of vector(Of String)))
        Inherits reparser

        Private Shared ReadOnly should_include As _SHOULD_INCLUDE = alloc(Of _SHOULD_INCLUDE)()
        Private Shared ReadOnly folders As vector(Of String) = alloc(Of _FOLDERS)().run()

        Public Shared Function include_folders() As vector(Of String)
            Return folders.CloneT()
        End Function

        Protected NotOverridable Overrides Function parse(ByVal s As String, ByVal o As WRITER) As Boolean
            Return file_parse(s, o)
        End Function

        ' Rename to make the functionality more clear.
        Protected MustOverride Function file_parse(ByVal s As String, ByVal o As WRITER) As Boolean

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
            If Not should_include.run(s) AndAlso (arguments.include_once Or True) Then
                Return True
            End If
            If include_file(folders, s, o) Then
                Return True
            End If
            raise_error(error_type.user, "Cannot find include file ", s)
            Return False
        End Function
    End Class

    Public MustInherit Class include_with_string(Of SHOULD_INCLUDE As func_t(Of String, Boolean),
                                                    FOLDERS As func_t(Of vector(Of String)))
        Inherits includes(Of SHOULD_INCLUDE, FOLDERS)

        Protected NotOverridable Overrides Function dump(ByVal n As typed_node, ByRef o As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class

    Public MustInherit Class include_with_file(Of SHOULD_INCLUDE As func_t(Of String, Boolean),
                                                  FOLDERS As func_t(Of vector(Of String)))
        Inherits includes(Of SHOULD_INCLUDE, FOLDERS)

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
