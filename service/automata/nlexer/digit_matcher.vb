
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class digit_matcher
        Inherits single_char_matcher

        Private Shared ReadOnly instance As digit_matcher
        Private Shared ReadOnly reverse As reverse_matcher

        Shared Sub New()
            instance = New digit_matcher()
            reverse = New reverse_matcher(instance)
        End Sub

        Private Sub New()
        End Sub

        Public Shared Sub register()
            matcher_creator.register("\d", Function() As matcher
                                               Return instance
                                           End Function)
            matcher_creator.register("\D", Function() As matcher
                                               Return reverse
                                           End Function)
        End Sub

        Protected Overrides Function check(ByVal c As Char) As Boolean
            Return c.digit()
        End Function
    End Class
End Class
