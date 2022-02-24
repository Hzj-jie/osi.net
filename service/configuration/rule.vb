
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Public MustInherit Class rule
    Public Const comment_start As Char = character.number_mark
    Public Const command_separator As Char = character.blank
    Public Const command_include As String = "INCLUDE"

    Private cur_file As String

    Protected MustOverride Function command_mapping() As map(Of String, Func(Of String, Boolean))
    Protected MustOverride Function [default](ByVal s As String, ByVal f As String) As Boolean

    Protected Function current_file() As String
        Return cur_file
    End Function

    Private Function include(ByVal s As String) As Boolean
        If Not File.Exists(s) AndAlso File.Exists(current_file()) Then
            s = pather.default.combine(pather.default.parent_path(current_file()), s)
        End If
        Return File.Exists(s) AndAlso parse_file_impl(s)
    End Function

    Protected Overridable Function finish() As Boolean
        Return True
    End Function

    Private Function parse_impl(ByVal ls() As String) As Boolean
        If isemptyarray(ls) Then
            Return True
        End If

        For i As Int32 = 0 To array_size_i(ls) - 1
            If ls(i).null_or_whitespace() Then
                Continue For
            End If

            ls(i) = ls(i).Trim()
            If ls(i).strstartwith(comment_start) Then
                Continue For
            End If

            Dim f As String = Nothing
            Dim s As String = Nothing
            If Not strsep(ls(i), f, s, command_separator) Then
                f = ls(i)
            End If

            assert(Not String.IsNullOrEmpty(f))
            Dim x As Func(Of String, Boolean) = Nothing
            If strsame(f, command_include) Then
                x = AddressOf include
            Else
                assert(Not command_mapping() Is Nothing)
                Dim it As map(Of String, Func(Of String, Boolean)).iterator = Nothing
                it = command_mapping().find(f)
                If it = command_mapping().end() Then
                    x = Function(y As String) As Boolean
                            Return [default](y, f)
                        End Function
                Else
                    x = (+it).second
                End If
            End If
            assert(Not x Is Nothing)
            If Not x(s) Then
                raise_error(error_type.user, "failed to execute command ", ls(i))
                Return False
            End If
        Next

        Return True
    End Function

    Public Function parse(ByVal ParamArray ls() As String) As Boolean
        Return parse_impl(ls) AndAlso finish()
    End Function

    Private Function parse_file_impl(ByVal rule_file As String) As Boolean
        Dim ls() As String = Nothing
        Try
            ls = File.ReadAllLines(rule_file)
        Catch ex As Exception
            raise_error(error_type.warning, "failed to read rule-file ", rule_file, ", ex ", ex.Message())
            Return False
        End Try

        Dim last_file As String = cur_file
        cur_file = rule_file
        Using defer.to(Sub()
                           cur_file = last_file
                       End Sub)
            Return parse_impl(ls)
        End Using
    End Function

    Public Function parse_file(ByVal rule_file As String) As Boolean
        Return parse_file_impl(rule_file) AndAlso finish()
    End Function

    Public Function parse_content(ByVal content As String) As Boolean
        Static newline_strs() As String = {newline.incode(), character.newline}
        Static empty_surround_strs() As pair(Of String, String) = Nothing
        Dim vs As vector(Of String) = Nothing
        Return content.strsplit(newline_strs, empty_surround_strs, vs, True, True) AndAlso parse(+vs)
    End Function
End Class
