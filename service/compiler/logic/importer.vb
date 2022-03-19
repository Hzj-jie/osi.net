
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Partial Public NotInheritable Class importer
        Private Const comment_start As String = "##"
        Private Const comment_end As String = character.newline
        Private Shared ReadOnly separators() As String = build_separators()
        Private Shared ReadOnly surround_strs() As String = Nothing

        Private ReadOnly functions As interrupts

        Private Shared Function build_separators() As String()
            Dim v As New vector(Of String)()
            For i As UInt32 = 0 To strlen(space_chars) - uint32_1
                v.emplace_back(space_chars(CInt(i)))
            Next
            Return +v
        End Function

        Public Sub New(ByVal functions As interrupts)
            assert(Not functions Is Nothing)
            Me.functions = functions
        End Sub

        Public Sub New()
            Me.New(New interrupts())
        End Sub

        Public Function import(ByVal s As vector(Of String), ByVal o As vector(Of instruction_gen)) As Boolean
            If s Is Nothing OrElse o Is Nothing Then
                Return False
            End If
            Dim p As UInt32 = 0
            Dim e As instruction_gen = Nothing
            While parse(s, p, e)
                o.emplace_back(e)
            End While
            Return p = s.size()
        End Function

        Public Function import(ByVal s As String, ByVal o As vector(Of instruction_gen)) As Boolean
            s.kick_between(comment_start, comment_end, recursive:=False)
            Dim v As vector(Of String) = Nothing
            Return strsplit(s, separators, surround_strs, v) AndAlso
                   assert(Not v.null_or_empty()) AndAlso
                   import(v, o)
        End Function

        Private Function import_proxy(Of T)(ByVal i As T,
                                            ByVal f As Func(Of T, vector(Of instruction_gen), Boolean),
                                            ByVal e As exportable) As Boolean
            assert(Not f Is Nothing)
            If e Is Nothing Then
                Return False
            End If

            Dim o As New vector(Of instruction_gen)()
            Return f(i, o) AndAlso e.import(+o)
        End Function

        Public Function import(ByVal s As vector(Of String), ByVal e As exportable) As Boolean
            Return import_proxy(s, AddressOf import, e)
        End Function

        Public Function import(ByVal s As String, ByVal e As exportable) As Boolean
            Return import_proxy(s, AddressOf import, e)
        End Function

        Public Function import(ByVal s As String, ByRef o As executor) As Boolean
            o = New simulator(functions)
            If import(s, direct_cast(Of exportable)(o)) Then
                Return True
            End If
            o = Nothing
            Return False
        End Function

        Public Function import(ByVal s As String) As executor
            Dim o As executor = Nothing
            assert(import(s, o))
            Return o
        End Function
    End Class
End Class
