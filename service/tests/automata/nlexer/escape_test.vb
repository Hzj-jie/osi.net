
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports nl = osi.service.automata.nlexer

Namespace nlexer
    <test>
    Public NotInheritable Class escape_test
        <test>
        <repeat(50000, 500000)>
        Private Shared Sub run()
            Dim s As String = Nothing
            Dim unescaped As String = Nothing
            s = rnd_ascii_display_chars(1000)
            s = nl.unescape(s)
            unescaped = s
            For Each c As Char In nl.characters.all
                s = s.Replace(c, nl.characters.escape_char + c)
            Next
            assertion.equal(unescaped, nl.unescape(nl.escape(s)))
            assertion.is_false(nl.escape(s).contains_any(nl.characters.all))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
