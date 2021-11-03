
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
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1484), _
        "H4sIAAAAAAAEALVXzY7kJhC+I+07oJ3bqp0HyGUVKaMol3S0M5fIslbYxt2oaXAAT2vePgUUNrbpno2UXLqhfnD9UF8V5Pff/jh+e/7++tefzy+0lUxdDtQKdZK8kkLxqtPXK1fuQK+TdGJFI9+Ox1fUNFq7yr2PnJB5SethUp0TWh3oG5MTr3o+CCU8pboJd64svwo4TeYSnWSGlUUUu3I7so5XBypUJ6eeH2g4E/7FoGBZ3QwbR94fqNQn0e3P8IYFuS3DOjN14OeZs/GRGQ0hya1gUPyxjhlXtYZ1F+5oPYLyVQrrmq+Uq35mxCBaCB5X4IYXO4HBZzIroG78qg80a77QQCJbRiTTsI7MaEvdClcx1cO3/Z6QJW70cqvmXW75bAmt5/zBh73ti5Ekk0o+HO761JBEW6QLYc85enKrSN9To/X2PhXuTyIBYbJgp+FuMmreipPShleGW7C/SimtOiblwYepNZxd8Bp5M+VQrc/Dm7S/MUhJcmFT0B/h63rkeMsCD4jaui0V8ri4Th4EjNadVj3G43YWEg4ctKmk1iNc23uJ2l+BJLO/AbsoLzVAtjnJyoNZC/H2oBGTQrYxi2KB6lefSB6rR2egRD2rpoCHcg6UZqf6iYR8xDhr4x2GLet7TPRVqMmmtY/aKN9x24s34WEn8nTSGPWNG1xj+WU7nVgLeSZJPkAOz2JwSDDidEZKg4bug7G2P4vHXn6OTFFpl7Vdre3Sml3GD0F7X5a5+kdYuyutXJnMl92Xqxg2GBw+vALf7IJzaX0bQ/3mK1kT/IGeQrObH+rJM+Lihz8G7QJL0CvDetsr7gLXEvIVAqBGiTZjSwFuHuLdv8GiO17GEKAdIY8olNxJeIXkhuxFP4oqMexW4Yc2HrwxI1gLU0uGAWU48LOD4ydfrq04TcIPN9MgNYP/VmsZRgGYgHIDM7uhQSYjcs+WQK0cm6m0nhQz7/sot6JEb0hRHKaDH84UKYjSWumEMzBBJUzjXYNw8ERfwFnZUzuNozaueDQmpi4cQ4ryWdpKOruPlmOSPhtwGiF6QecEzAGTEY5nJE4gHDZ+Ibm1lTszKLYTNHrHDe4CAwqW/z0xuTAXCv5BHCMlRe6OzZnr/43dS7fIGsX/4RBZlVRs8UsvyWZHC1pw1fZVu2aQbSFmnanXDmfVBzBFV7tlDI/M0hgeC3E/h5OFsUKsNG/HAGwZ6Fmct8lqnPTQHgkzOhO41s/XlveUxSHSG+d4GEJ6YXjnfPaluHBkfwYkApkesIay7xGDPpPIg+PTGZ5Mio+b9JmsReIrCSWDbmiWkZxOS9u6ID6/tCJxgN4Ho+T6sRX7r6fgAyR/QvhWCm11IPGh5ve4igl/or9IqW9hnP5CnaYtpxBRCJuFyEUjfiLp2RbflRHGE0wjz5+clivxfEPuvP+S0Cp6T/T1+OvxZ/qCsLRtyyReZv9hXJVfVB9OSds5O7fiH1ZIbLSiDwAA"))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
