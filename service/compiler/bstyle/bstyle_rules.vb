
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module bstyle_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(108), _
        "H4sIAAAAAAAEAHu/ez8vl6efs0+oi6uCnp5+cUZiUWqKfl5OakVqUXxRaU6qXklFCS8XL1dRalpqUWpecqqCGoiLV48RTBMAfAhv7l8AAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(264), _
        "H4sIAAAAAAAEAHWPzQ6CMBCE7036Dj0bCw+hHkyMN0+EmKWsoRFa0i5/by9SRA162535ZjPL2fG8O132BxFFsS/AYR77wRD06K6uKTGinjjjzFlLkoYaRQYe5bJOHnTSQIWy01TI3JJ4KWJcOJumZEWlm4Xj7HkrRN6aR0No1JzwWGmpbGlNqPDP5exmndRGORyxwH4qnLVQNiFmG5KZA3VHCuBPK3zYgtOQlXPL0PBbSwqEehq3YpVIOXsATs9CwG0BAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
