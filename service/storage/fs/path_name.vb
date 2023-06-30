
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.utils

Public Module _path_name
    Public Const path_separator As Char = character.left_slash
    Public Const magic_char As Char = character.dollar
    Public Const this_level_path As Char = character.dot
    Public Const parent_level_path As String = character.dot + character.dot
    Public Const subnode_property_name As String = magic_char + this_level_path
    Public Const properties_property_name As String = magic_char
    Public Const lock_property_name As String = magic_char
    Private ReadOnly p As pather(Of _path_separators, _this_level_paths, _parent_level_paths)

    Private Class _path_separators
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            r = {path_separator}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class

    Private Class _this_level_paths
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            r = {this_level_path}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class

    Private Class _parent_level_paths
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            r = {parent_level_path}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class

    'can detect both valid inode name or iproperty name
    <Extension()> Public Function valid_name(ByVal this As String) As Boolean
        Return Not this.null_or_empty() AndAlso
               Not this.Contains(path_separator) AndAlso
               Not this.Contains(magic_char)
    End Function

    'detect whether this is a valid inode or normal property [exclude subnode / properties / lock] full path
    <Extension()> Public Function valid_path(ByVal this As String) As Boolean
        Return Not this.null_or_empty() AndAlso
               this.StartsWith(path_separator) AndAlso
               Not this.EndsWith(path_separator) AndAlso
               Not this.Contains(magic_char)
    End Function

    <Extension()> Public Function combine_path(ByVal path As String, ByVal name As String) As String
        assert(path.valid_path())
        assert(name.valid_name())
        Return p.combine(path, name)
    End Function

    <Extension()> Public Function property_key(ByVal path As String, ByVal name As String) As String
        Return combine_path(path, name)
    End Function

    <Extension()> Public Function subnode_property_key(ByVal path As String) As String
        assert(path.valid_path())
        Return p.combine(path, subnode_property_name)
    End Function

    <Extension()> Public Function properties_property_key(ByVal path As String) As String
        assert(path.valid_path())
        Return p.combine(path, properties_property_name)
    End Function

    <Extension()> Public Function lock_property_key(ByVal property_key As String) As String
        Return strcat(property_key, lock_property_name)
    End Function

    <Extension()> Public Sub normalize_path(ByRef this As String)
        this = p(this)
    End Sub

    <Extension()> Public Function parent_path(ByVal this As String) As String
        assert(this.valid_path())
        Return p.parent_path(this)
    End Function

    <Extension()> Public Function last_level_name(ByVal this As String) As String
        assert(this.valid_path())
        Return p.last_level_name(this)
    End Function
End Module
