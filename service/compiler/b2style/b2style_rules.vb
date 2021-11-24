
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(932), _
        "H4sIAAAAAAAEAF1UzW7bMAy+6ynYFCs2x07uXZIe1g0YUOy0nWyvUCzaESJLriQ3LdY92Q57pL3CKFlJfw4m+VGU9Ik//vfnLzuH7zvpoJUK4SCVgi1CY/Q9Wo8CpPYGLDoz2gZzcAbOv377dPPj+jMIgw608XAwdr+gc74YpcxB6g4EtlJLL412wG04b5B0WGtND1vnHxUuGyOwQ73UCh/Q3tpR4cI/+AXbHwrZgmyDgcohBBFAayzQF8zDLrINMkBhiEwwlDEDBBGART9aDZMKjq1Fvocop+BONhAlY1vF9R7KalvPCRijoPR2xLzldHvNKA3E1kI5z4v6qqwEhW1lN5IfIipVflOzsVWGT55sMUU5b0NGZmU1y7OnWZ3NGGtM33OocjKU0XDJ7kZ0IVtFz+0ermgTt74YuOWd5cMOfjHU4gX+nSK2ljd79PA+rh/Rh7Tq7kZK/sldlTHqrbdmDntZTFQ+Msadk53ukV62ZlTVa9m2aAMMNXKMkXCFsQXSQQpWa9ZRRj3aZ99mzSZjvWbUIMl7tp62+h3XsDrtinBD9woB1Zz1Uo8OCtaPystBPUKVMSHvpUBYst4IeMcGc6BS/KQC+IJrARfRou6onljEFywCgqE9z+iBqi2kbuh8uiEigQ0UBRFqKU872XpYrZiV3e4IN0QpRk681hNI7I7omWPyHJkeAwLfZCfWCZ24v8CRdMLTOxJIr0noFefke82c6kb9rXmPbuANwsmKfkqEGonkeTJY0sVB+l0R/wPHpTgPqzJ72tTzzbS51TTcFBB1HL4w67Rl0nFqtaAJPo+Kkkjtr7BQtFiExg+dtFzSmZWus6nMbxarLKxmyzojEW/1jwOGa5OOPhqssfEwqehpFPUuRMmYoMovGAtPp3k85Lf55WUdjEpEO1/Q7f8BVTfzbQAFAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1480), _
        "H4sIAAAAAAAEALVXzY7cKBC+I+UdUOYWmTxALlGkHa1y2Y4yc4ksK8I27kZNgxdwWvP2Kf5sbNM92Sh76Yaqoqj6KD7K6PPf/xy+Pn5//vbl8Qm3gspzhQ2XR8GI4JKRTl0uTNoKXyZh+UqGvh4Oz3GlVsoS+zIyhOYhrodJdpYrWeEfVEyM9GzgkjsJuXJ7IoZdOHgTuUUnqKZlE0kvzIy0Y6TCXHZi6lmFvU/454OEIblqOo6sr7BQR97tfbjAvN1WYayeOsjzxOh4L4wGoZSWDyj8GEu1Ja2m3ZlZXI+w+CK4sc1HzGQ/KwKIBsBjEtJwZkcI+ITmBXFt2NUBTZt32IvQVhHE2I+DMsRSt9wSKnvY280RWnDD5yuZZ3nkcyS4ns8PNnaxL0GizCrlUN3MqUFJtlgXYM81arIrpG8tw/W2ngr1k0QgmAzEqZmdtJyn/CiVZkQzA/GTdKSko0JUDqZWM3qOZeTCFANZ+4uVtK+YCo/gWI0sFpBfBkJl7FYKR7Rkhe5ggetOyT6mej1xAQ4HpYlQaoSKvHUG+9Ndjmd3ujsEl/pGW7yz0qfGAJaOEALgaItHMPNSN3qDchxhjea0BbopO/PAB9SUdoUEU9r38UQuXE4mjR0Go3iJ057/4I4fgk6lFaO6Mh3H8Z5kM5VUi3gWCTbAiZz4YKNA8+MpSpo3aFchm8zWiYTkdrDuCn2He1YurzLm/k7ky18jul1d54vRXI7urvBhQ4B+4xXzZSXIhHFvSFzffERrgXPoJDgrTV/xThEGv7wZcHW8JG4xjLdEfZM1FshXdzSuKMlmYihwxV2y+S9scSPLAEGMw59jNErpJEaJ4gbtTV9DFWl6JXGjTQarSndvs2VHd8tafpy4ax6mQSgK/61Swj+10GHkMWShwQOU9smDX7BYxT5LcT1Jql/2QLa8JG9Q0Rz48ZcPAxVMcS1VogfoUBIVsa6JN/4BP0GyosdmGkelbdF1xL4uuEFF++xkSmt2m5YxSdt6eo3MupBq4lNPpZFFZwJN3OknbiCYMcSeKNynIzykluk48wq4k+zfiYpFuUjiH+AYJAm5GzFnqf+ZuBeSz/j9/0gIrd+HOtyd+ZmMw9CWekmDZmXeuhlwCpW4v7drBdq6W7bCvbKxVbxDVHg1W7rgoCx1weGe7ttgtChWnJXa3YDPVhEzC+0uWnVzjtyDYOZnBFX/eGlZj2no4VxwlvnOoueaddYVh+BnFtVvgajApgcqwvR7oKi3KOjAffLhxKj4bZG2yR7J+JESLf1a/1wGcfKWpnXBfP7QCcIBXj9o99bfOuEFdpLY/+cdvHtM4WEdUPhOcvM4Cgf+gD8Joa6+m32HrcItw4AowGYAuRDEe5S+msJnXSjPxOJR5zyn4co8n6Abn1/JaIXeA34+/HX4gJ8ia20fZhSK2W0cR+UPmlf7pG0rvG50BPSkbo8w+M0tqvn23NvsJ23GkLiODwAA"))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
