

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
        Public Shared Function [of](ByVal s As String) As matcher
            assert(Not s Is Nothing)
            If s(0) <> characters.group_start Then
                Return group.of(s)
            End If
            Dim e As Int32 = 0
            e = s.IndexOf(characters.group_end)
            assert(e >= 1)
            Dim g As matcher = Nothing
            g = group.of(s.Substring(1, e - 1))
            e += 1
            While e < s.Length()
                Dim it As map(Of Char, Func(Of matcher, matcher)).iterator = Nothing
                it = m.find(s(e))
                assert(it <> m.end())
                g = (+it).second(g)
                e += 1
            End While
            assert(Not g Is Nothing)
            Return g
        End Function

        Public Shared Function end_of_groups(ByVal s As String, ByVal i As UInt32) As UInt32
            While i < s.Length()
                If m.find(s(CInt(i))) <> m.end() Then
                    i += uint32_1
                End If
            End While
            Return i
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
