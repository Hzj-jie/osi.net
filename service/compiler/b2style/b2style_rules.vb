
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
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(924), _
        "H4sIAAAAAAAEAF1UzW7bMAy+6ynYFCs2x07uXZIe1g0YUOy0nWyvUCzaESJLriQ3LdY92Q57pL3CKFlJfw4m+VGU9Ik//vfnLzuH7zvpoJUK4SCVgi1CY/Q9Wo8CpPYGLDoz2gZzcAbOv377dPPj+jMIgw608XAwdr+gc74YpcxB6g4EtlJLL412wG04b5B0WGtND1vnHxUuGyOwQ73UCh/Q3tpR4cI/+AXbHwrZgmyDgcohBBFAayzQF8zDLrINMkBhiEwwlDEDBBGART9aDZMKjq1Fvocop+BONhAlY1vF9R7KalvPCRijoPR2xLzldHvNKA3E1kI5z4v6qqwEhW1lN5IfIipVflOzsVWGT55sMUU5b0NGZmU1y7OnWZ3NGGtM33OocjKU0XDJ7kZ0IVtFz+0ermgTt74YuOWd5cMOfjHU4gX+nSK2ljd79PA+rh/Rh7Tq7kZK/sldlTHqrbdmDntZTFQ+Msadk53ukV62ZlTVa9m2aAMMNXKMkXCFsQXSQQpWa9ZRRj3aZ99mzSZjvWbUIMl7tp62+h3XsDrtinBD9woB1Zz1Uo8OCtaPystBPUKVMSHvpUBYst4IeMcGc6BS/KQC+IJrARfRou6onljEFywCgqE9z+iBqi2kbuh8uiEigQ0UBRFqKU872XpYrZiV3e4IN0QpRk681hNI7I7omWPyHJkeAwLfZCfWCZ24v8CRdMLTOxJIr0noFefke82c6kb9rXmPbuANwsmKfkqEGonkeTJY0sVB+l0R/wPHpTgPqzJ72tTzzbS51TTcFBB1HL4w67Rl0nFqtaAJPo+Kkkjtr7BQtFiExg+dtFzSmZWus6nMbxarLKxmyzojEW/1jwOGa5OOPhqssfEwKcYEVXrBWHgqzd8hv80vL+tgVCLa+YJu+w+ItGrF8AQAAA=="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1460), _
        "H4sIAAAAAAAEALVXS4/cKBC+I+U/oMwtavaQ416ilXYU7WV7lZlLZFkRtnE3ahocwNOaf5/iZWNDz+xh99IN9aIoqr4qo7++/n389vjj+fs/j0+4E1ReDthweRKMCC4Z6dX1yqQ94OssLN/Q0Lfj8TlqaqUssa8TQ2hZ4macZW+5kgf8QsXMyMBGLrmjkBu3Z2LYlYM1kUv0gmpaF5H0ysxEe0YOmMtezAM7YG8T/vkoYUlumk4TGw5YqBPvSxvOMS+3Zxir5x7ueWZ0esuNFqF0Le9Q+DGWaks6TfsLs7iZQPkquLHtF8zksDBCEA0Ej0m4hhM7gcNntChE3XCqCzRtP2FPQntGIGO/DszVIYTWYOHLjSy73N3leNwsjwanOYdXz1AmlRw/3L1IixJtla7EOueo2W7Ce08NN/skqiRNIgFhNuCnZnbWctnyk1SaEc0M+E/SO5KeCnFwYeo0o5eYO85NMZKtvZg+ZZpESpCDV1/9Rm/cFje9kkO8zO3MBRwxKk2EUhMk2r0ol++XZMrnK0KUJck+oFlCU2MgWK7MQ0TR/sJBzFPd6gPKA/WWjSjRLKoxdKEAPaUtVD8g/xhqYnC60u7CsKXDEF/pyuVs0tpFbRKvcTvwF+6AIvBU0pjUjem47rglVA7ZTiXWSl5Igo3whmc+2kjQ/HSOlDY6WgZj63+IRyH6eQ1oVaF4saJIiifNEvFdiC3rKVd/DxmLmsiV0ZLors74uENMf/AGKrPkZsK4phP12y9oS3AGHQVnWe9ryTHC4l8fBuAey88pw3qP7HcRZw35pvqjRo22gEqRBruusa/mpO/jH4WSGwljIrlFpeh70UCa3kg8aAeRL1Rz2sFskNVtvYRdh7bs5Eqs46eZuxFiHoWi8N8pJXzDhTkjdzDzGzpSciK/WawHF2RUo+JmllS/roRkouM1eouq4tCDoUkUNiZlbGmhIoobqRI2wJyScIj1bSzjB/wElxUDNvM0KW2rpuPDNBUzqCqfPVtNpzi0HpN0rMfWCKsroiYw9TgaIXRBzwScfuMWghlD7JlCkZygs1qm484zoNDYz5mKlblS4h/EMVBS5O74nF39v/F7RfgM3P+PC6FNSYW2vPaAbFgzoAWpVlbtloH2hZh1lEHZOBy+MQfhzW4ddgOzNuyGQiynXbQyNoiVptoQgD0j3ixMtWgzvzlIDoQFVRGk9eO1YwOmYWpzzlnmB4eBa9Zb9/qCX1hkfwQkApkBsAbTHwGDPqLAA/PJhiOj6idEOiZrbfFbJEp6Xd/kAjlZS9umIr58zwTiCD0Lxr/tJ03om44SXmEzs7sWCO1wROFzyO3jKjz4A/5DCHXz8+snbBXuGIaIQtgMRC448RtKH0fh6y3AeILpyHOW03Ijnm/Qna+sJLSJ3gN+Pv55/B0/RVjat1MUktkdHFf1T5h3p5v9bJx78Quyxe01CA8AAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
