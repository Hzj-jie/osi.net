
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Namespace primitive
    ' Each exportable instance should export several bytes or several characters only, so copying an instance should not
    ' have too much performance impact.
    ' .Net does not accept an instance, which is larger than 4G, so using uint32 as byte array or string index is enough.
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
            For i As UInt32 = 0 To strlen(space_chars) - uint32_1
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
            s.kick_between(comment_start, comment_end)
            Dim v As vector(Of String) = Nothing
            Return strsplit(s, separators, surround_strs, v) AndAlso
                   assert(Not v.null_or_empty()) AndAlso
                   e.import(v)
        End Function
    End Module
End Namespace
