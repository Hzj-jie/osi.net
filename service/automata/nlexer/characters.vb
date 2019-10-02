
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class characters
        Public Const matcher_separator As Char = character.comma
        Public Const group_separator As Char = character.sheffer
        Public Const group_start As Char = character.left_mid_bracket
        Public Const group_end As Char = character.right_mid_bracket

        Private Sub New()
        End Sub
    End Class
End Class
