
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Namespace primitive
    ' Each exportable instance should export several bytes or several characters only, so copying an instance should not
    ' have too much performance impact.
    ' An exportable instance always clears its internal state when executes import functions.
    ' .Net does not accept an instance larger than 4G, so using uint32 as byte array or string index is enough.
    Public Interface exportable
        Function export(ByRef b() As Byte) As Boolean
        Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean

        Function export(ByRef s As String) As Boolean
        Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean

        Function bytes_size() As UInt32
    End Interface

    Public Module _exportable
        Private Const comment_start As String = "##"
        Private Const comment_end As String = character.newline
        Private ReadOnly separators() As String
        Private ReadOnly surround_strs() As String

        Sub New()
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            For i As Int32 = 0 To CInt(strlen(space_chars)) - 1
                v.emplace_back(space_chars(i))
            Next
            separators = (+v)
            surround_strs = Nothing
        End Sub

        <Extension()> Public Function import(ByVal e As exportable, ByVal a() As Byte) As Boolean
            Dim p As UInt32 = 0
            Return assert(Not e Is Nothing) AndAlso
                   e.import(a, p) AndAlso
                   (p = array_size(a))
        End Function

        <Extension()> Public Function import(ByVal e As exportable, ByVal v As vector(Of String)) As Boolean
            Dim p As UInt32 = 0
            Return assert(Not e Is Nothing) AndAlso
                   Not v.null_or_empty() AndAlso
                   e.import(v, p) AndAlso
                   (p = v.size())
        End Function

        <Extension()> Public Function import(ByVal e As exportable, ByVal s As String) As Boolean
            s.kick_between(comment_start, comment_end, recursive:=False)
            Dim v As vector(Of String) = Nothing
            Return strsplit(s, separators, surround_strs, v) AndAlso
                   assert(Not v.null_or_empty()) AndAlso
                   e.import(v)
        End Function

        <Extension()> Public Function export(ByVal e As exportable) As String
            assert(Not e Is Nothing)
            Dim r As String = Nothing
            assert(e.export(r))
            Return r
        End Function

        <Extension()> Public Function load_bitcode(ByVal e As exportable, ByVal f As String) As Boolean
            Dim a() As Byte = Nothing
            Try
                a = File.ReadAllBytes(f)
            Catch ex As Exception
                raise_error(error_type.warning, "failed to read from file ", f, ", ex ", ex)
                Return False
            End Try
            Return assert(Not e Is Nothing) AndAlso
                   e.import(a)
        End Function

        <Extension()> Public Function write_bitcode(ByVal e As exportable, ByVal f As String) As Boolean
            assert(Not e Is Nothing)
            Dim a() As Byte = Nothing
            If e.export(a) Then
                Try
                    File.WriteAllBytes(f, a)
                Catch ex As Exception
                    raise_error(error_type.warning, "failed to write to file ", f, ", ex ", ex)
                    Return False
                End Try
                Return True
            Else
                Return False
            End If
        End Function

        <Extension()> Public Function load_asccode(ByVal e As exportable, ByVal f As String) As Boolean
            Dim s As String = Nothing
            Try
                s = File.ReadAllText(f)
            Catch ex As Exception
                raise_error(error_type.warning, "failed to read from file ", f)
                Return False
            End Try
            Return assert(Not e Is Nothing) AndAlso
                   e.import(s)
        End Function

        <Extension()> Public Function write_asccode(ByVal e As exportable, ByVal f As String) As Boolean
            assert(Not e Is Nothing)
            Dim s As String = Nothing
            If e.export(s) Then
                Try
                    File.WriteAllText(f, s)
                Catch ex As Exception
                    raise_error(error_type.warning, "failed to write to file ", f, ", ex ", ex)
                    Return False
                End Try
                Return True
            Else
                Return False
            End If
        End Function

        ' assemble() and following functions convert various formats of asccode into bitcode.
        <Extension()> Public Function assemble(ByVal e As exportable, ByVal s As String, ByRef o() As Byte) As Boolean
            assert(Not e Is Nothing)
            Return e.import(s) AndAlso
                   e.export(o)
        End Function

        <Extension()> Public Function assemble(ByVal e As exportable, ByVal s As String, ByVal o As String) As Boolean
            assert(Not e Is Nothing)
            Return e.import(s) AndAlso
                   e.write_bitcode(o)
        End Function

        <Extension()> Public Function load_and_assemble(ByVal e As exportable,
                                                        ByVal f As String,
                                                        ByRef o() As Byte) As Boolean
            assert(Not e Is Nothing)
            Return e.load_asccode(f) AndAlso
                   e.export(o)
        End Function

        <Extension()> Public Function load_and_assemble(ByVal e As exportable,
                                                        ByVal f As String,
                                                        ByVal o As String) As Boolean
            assert(Not e Is Nothing)
            Return e.load_asccode(f) AndAlso
                   e.write_bitcode(o)
        End Function
    End Module
End Namespace
