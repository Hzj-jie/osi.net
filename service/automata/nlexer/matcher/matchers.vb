
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class matchers
        Private Shared ReadOnly m As map(Of String, Func(Of matcher))

        Shared Sub New()
            m = New map(Of String, Func(Of matcher))()
            digit_matcher.register()
            en_char_matcher.register()
            single_char_matcher.register()
            space_matcher.register()
            newline_matcher.register()
        End Sub

        Public Shared Sub register(ByVal s As String, ByVal f As Func(Of matcher))
            assert(s IsNot Nothing)
            assert(f IsNot Nothing)
            assert(m.find(s) = m.end())
            m.emplace(s, f)
        End Sub

        Private Shared Function [new](ByVal i As String) As matcher
            Dim it As map(Of String, Func(Of matcher)).iterator = Nothing
            it = m.find(i)
            If it = m.end() Then
                Return New string_matcher(i)
            End If
            Return (+it).second()
        End Function

        Private Shared Function new_all(ByVal vs As vector(Of String)) As vector(Of matcher)
            Dim r As vector(Of matcher) = Nothing
            r = New vector(Of matcher)(vs.size())
            Dim i As UInt32 = 0
            While i < vs.size()
                r.emplace_back([new](vs(i)))
                i += uint32_1
            End While
            Return r
        End Function

        ' Process a,b,c
        Public Shared Function [of](ByVal i As String) As matcher
            assert(i IsNot Nothing)
            Dim vs As vector(Of String) = Nothing
            vs = vector.emplace_of(i.Split(characters.matcher_separator))
            If vs.empty() Then
                Return never_matcher.instance
            End If
            If vs.size() = uint32_1 Then
                Return [new](vs(0))
            End If
            Return New or_matcher(new_all(vs))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
