
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class en_char_matcher
        Inherits single_char_matcher

        Private Shared ReadOnly instance As en_char_matcher
        Private Shared ReadOnly reverse As reverse_matcher

        Shared Sub New()
            instance = New en_char_matcher()
            reverse = New reverse_matcher(instance)
        End Sub

        Public Shared Shadows Sub register()
            matchers.register("\w", Function() As matcher
                                        Return instance
                                    End Function)
            matchers.register("\W", Function() As matcher
                                        Return reverse
                                    End Function)
        End Sub

        Private Sub New()
        End Sub

        Protected Overrides Function check(ByVal c As Char) As Boolean
            Return c.alpha()
        End Function
    End Class
End Class
