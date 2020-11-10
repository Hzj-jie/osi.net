
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public NotInheritable Class filesystem
    Public Const this_level_path_mark As Char = character.dot
    Public Const extension_prefix As Char = character.dot
    Public Const multi_pattern_matching_character As Char = character.asterisk
    Public Const single_pattern_matching_character As Char = character.question_mark
    Public Const parent_level_path_mark As String = character.dot + character.dot

    Public NotInheritable Class extensions
        Public Const dynamic_link_library As String = "dll"
        Public Const executable_file As String = "exe"
        Public Const program_database As String = "pdb"

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class

Public Module _filesystem
    <Extension()>
    Public Function with_file_extension(ByVal filename As String, ByVal extension As String) As String
        Return String.Concat(filename, filesystem.extension_prefix, extension)
    End Function
End Module
