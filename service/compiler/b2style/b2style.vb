
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Public NotInheritable Class b2style
    Inherits rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t)

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return b2style_rules.nlexer_rule
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return b2style_rules.syntaxer_rule
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
