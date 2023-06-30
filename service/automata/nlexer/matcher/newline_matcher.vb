
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class newline_matcher
        Inherits single_char_matcher

        Private Shared ReadOnly instance As New newline_matcher()
        Private Shared ReadOnly reverse As New reverse_matcher(instance)

        Private Sub New()
        End Sub

        Public Shared Shadows Sub register()
            matchers.register("\n", Function() As matcher
                                        Return instance
                                    End Function)
            matchers.register("\N", Function() As matcher
                                        Return reverse
                                    End Function)
        End Sub

        Protected Overrides Function check(ByVal c As Char) As Boolean
            Return newline_chars.Contains(c)
        End Function
    End Class
End Class
