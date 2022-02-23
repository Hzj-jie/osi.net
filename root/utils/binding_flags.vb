
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

<global_init(global_init_level.functor)>
Public Module _binding_flags
    Private ReadOnly m As map(Of String, BindingFlags) = calculate_m()

    Private Function calculate_m() As map(Of String, BindingFlags)
        Dim m As map(Of String, BindingFlags) = New map(Of String, BindingFlags)()
        Dim it As unordered_map(Of String, BindingFlags).iterator = Nothing
        it = enum_def(Of BindingFlags).string_map().begin()
        While it <> enum_def(Of BindingFlags).string_map().end()
            assert(m.emplace((+it).first, (+it).second).second)
            it += 1
        End While

        assert(m.emplace("private", BindingFlags.NonPublic).second)
        assert(m.emplace("protected", BindingFlags.NonPublic).second)
        Return m
    End Function

    <Extension()> Public Function from_str(ByRef bf As BindingFlags, ByVal s As String) As Boolean
        If String.IsNullOrEmpty(s) Then
            bf = bf Or BindingFlags.Default
            Return True
        End If

        Dim v As vector(Of String) = Nothing
        If Not s.strsplit(strsplitter.with_default_separators(character.sheffer, character.comma),
                          strsplitter.with_default_surround_strs(),
                          v) Then
            Return False
        End If

        assert(v IsNot Nothing)
        If v.empty() Then
            bf = bf Or BindingFlags.Default
            Return True
        End If

        For i As UInt32 = 0 To v.size() - uint32_1
            Dim it As map(Of String, BindingFlags).iterator = Nothing
            it = m.find(strtolower(v(i)))
            If it = m.end() Then
                Return False
            End If

            bf = bf Or (+it).second
        Next
        Return True
    End Function

    <Extension()> Public Function method_from_str(ByRef bf As BindingFlags, ByVal s As String) As Boolean
        bf = bf Or BindingFlags.InvokeMethod
        Return from_str(bf, s)
    End Function

    Private Sub init()
        ' Trigger bytes_serializer registering.
        enum_def(Of BindingFlags)()
        string_serializer.register(Function(ByVal i As StringReader, ByRef o As method_binding_flags) As Boolean
                                       assert(i IsNot Nothing)
                                       Dim bf As BindingFlags = Nothing
                                       If string_serializer.from_str(i, bf) Then
                                           o = New method_binding_flags(bf)
                                           Return True
                                       Else
                                           Return False
                                       End If
                                   End Function)
    End Sub
End Module
