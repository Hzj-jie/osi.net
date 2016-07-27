
Namespace filesystem
    Public Module _filesystem
        Public Const this_level_path_mark As Char = character.dot
        Public Const extension_prefix As Char = character.dot
        Public Const multi_pattern_matching_character As Char = character.asterisk
        Public Const single_pattern_matching_character As Char = character.question_mark
        Public Const parent_level_path_mark As String = character.dot + character.dot

        Public Class extensions
            Public Const dynamic_link_library As String = "dll"
            Public Const executable_file As String = "exe"
            Public Const program_database As String = "pdb"
        End Class
    End Module
End Namespace
