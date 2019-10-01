
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class matcher_creator
        Private Shared ReadOnly matchers As vector(Of pair(Of String, Func(Of matcher)))

        Shared Sub New()
            matchers = New vector(Of pair(Of String, Func(Of matcher)))()
            digit_matcher.register()
            en_char_matcher.register()
            space_matcher.register()
        End Sub

        Public Shared Sub register(ByVal s As String, ByVal f As Func(Of matcher))
            matchers.emplace_back(emplace_make_pair(s, f))
        End Sub

        Public Shared Function [next](ByVal i As String, ByVal pos As UInt32) As pair(Of UInt32, matcher)

        End Function

        Private Sub New()
        End Sub
    End Class
End Class
