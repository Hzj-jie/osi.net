

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class groups
        Private Shared ReadOnly m As map(Of Char, Func(Of matcher, matcher))

        Shared Sub New()
            m = New map(Of Char, Func(Of matcher, matcher))()
            _0_or_more_group.register()
            _1_or_more_group.register()
            optional_group.register()
        End Sub

        Public Shared Sub register(ByVal c As Char, ByVal f As Func(Of matcher, matcher))
            assert(Not f Is Nothing)
            m.emplace(c, f)
        End Sub

        ' Process [a,b,c|d,e,f]*
        Private Shared Function of_group(ByVal s As String, ByRef i As UInt32, ByRef o As matcher) As Boolean
            assert(s.char_at(i) = characters.group_start)
            Dim group_end As Int32 = 0
            group_end = s.next_index_of(characters.group_end, i)
            If group_end = npos Then
                Return False
            End If
            o = group.of(s.strmid(i + uint32_1, CUInt(group_end) - i - uint32_1))
            group_end += 1
            If (group_end < s.Length()) Then
                Dim it As map(Of Char, Func(Of matcher, matcher)).iterator = Nothing
                it = m.find(s(group_end))
                If it <> m.end() Then
                    o = (+it).second(o)
                    group_end += 1
                End If
            End If
            assert(Not o Is Nothing)
            i = CUInt(group_end)
            Return True
        End Function

        ' Process abc
        Private Shared Function of_raw_string(ByVal s As String, ByRef i As UInt32) As matcher
            assert(s.char_at(i) <> characters.group_start)
            Dim start As UInt32 = 0
            start = i
            Dim next_group_start As Int32 = 0
            next_group_start = s.next_index_of(characters.group_start, i)
            If next_group_start = npos Then
                i = strlen(s)
                Return group.of(s.strmid(start))
            End If
            i = CUInt(next_group_start)
            Return group.of(s.strmid(start, CUInt(next_group_start) - start))
        End Function

        Public Shared Function [of](ByVal s As String, ByRef i As UInt32, ByRef o As matcher) As Boolean
            assert(strlen(s) > i)
            If s.char_at(i) = characters.group_start Then
                Return of_group(s, i, o)
            End If
            o = of_raw_string(s, i)
            Return True
        End Function

        Public Shared Function end_of_groups(ByVal s As String, ByVal i As UInt32) As UInt32
            While i < s.Length()
                If m.find(s.char_at(i)) <> m.end() Then
                    i += uint32_1
                End If
            End While
            Return i
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
