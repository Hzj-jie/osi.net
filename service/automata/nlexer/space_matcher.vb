
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class space_matcher
        Inherits single_char_matcher

        Private Shared ReadOnly instance As space_matcher
        Private Shared ReadOnly reverse As reverse_matcher

        Shared Sub New()
            instance = New space_matcher()
            reverse = New reverse_matcher(instance)
        End Sub

        Private Sub New()
        End Sub

        Public Shared Sub register()
            matchers.register("\b", Function() As matcher
                                               Return instance
                                           End Function)
            matchers.register("\B", Function() As matcher
                                               Return reverse
                                           End Function)
        End Sub

        Protected Overrides Function check(ByVal c As Char) As Boolean
            Return c.space()
        End Function
    End Class
End Class
