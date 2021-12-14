
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class importer
        Private Const comment_start As String = "##"
        Private Const comment_end As String = character.newline
        Private Shared ReadOnly separators() As String = build_separators()
        Private Shared ReadOnly surround_strs() As String = Nothing

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly functions As interrupts

        Private Shared Function build_separators() As String()
            Dim v As New vector(Of String)()
            For i As UInt32 = 0 To strlen(space_chars) - uint32_1
                v.emplace_back(space_chars(CInt(i)))
            Next
            Return +v
        End Function

        Public Sub New(ByVal anchors As anchors, ByVal types As types, ByVal functions As interrupts)
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not functions Is Nothing)
            Me.anchors = anchors
            Me.types = types
            Me.functions = functions
        End Sub

        Public Sub New(ByVal functions As interrupts)
            Me.New(New anchors(), New types(), functions)
        End Sub

        Public Sub New()
            Me.New(New interrupts())
        End Sub

        Public Function import(ByVal s As vector(Of String), ByVal o As vector(Of exportable)) As Boolean
            If s Is Nothing OrElse o Is Nothing Then
                Return False
            End If
            Dim p As UInt32 = 0
            Dim e As exportable = Nothing
            While parse(s, p, e)
                o.emplace_back(e)
            End While
            Return p = s.size()
        End Function

        Public Function import(ByVal s As String, ByVal o As vector(Of exportable)) As Boolean
            s.kick_between(comment_start, comment_end, recursive:=False)
            Dim v As vector(Of String) = Nothing
            Return strsplit(s, separators, surround_strs, v) AndAlso
                   assert(Not v.null_or_empty()) AndAlso
                   import(v, o)
        End Function

        Private Shared Function import_proxy(Of T)(ByVal i As T,
                                                   ByVal f As Func(Of T, vector(Of exportable), Boolean),
                                                   ByVal e As interpreter.primitive.exportable) As Boolean
            assert(Not f Is Nothing)
            If e Is Nothing Then
                Return False
            End If

            Dim o As vector(Of exportable) = Nothing
            o = New vector(Of exportable)()
            Return f(i, o) AndAlso e.import(+o)
        End Function

        Public Function import(ByVal s As vector(Of String), ByVal e As interpreter.primitive.exportable) As Boolean
            Return import_proxy(s, AddressOf import, e)
        End Function

        Public Function import(ByVal s As String, ByVal e As interpreter.primitive.exportable) As Boolean
            Return import_proxy(s, AddressOf import, e)
        End Function

        Public Function import(ByVal s As String, ByRef o As executor) As Boolean
            o = New simulator(functions)
            If import(s, direct_cast(Of interpreter.primitive.exportable)(o)) Then
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
End Namespace
