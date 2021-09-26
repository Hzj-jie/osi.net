
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'stdio_h cstdio
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_lib
    Public ReadOnly stdio_h() As Byte
    Public ReadOnly cstdio() As Byte

    Sub New()
        stdio_h = Convert.FromBase64String(strcat_hint(CUInt(132), _
        "H4sIAAAAAAAEAHu/ez8vV2ZeiYKrv5uCrUKSUXFJZU6qlVVqfpqGpjUvF0QyPbUkOSOxSENToZqXS0GhKLWktCgPoRguDdRQC9NTUAoRBLEzcWiEqwHrBADtu7A5jAAAAA=="))

        assert(stdio_h.ungzip(stdio_h))
        cstdio = Convert.FromBase64String(strcat_hint(CUInt(60), _
        "H4sIAAAAAAAEAHu/ez8vl3JmXnJOaUqqgk1xSUpmvl6GHQBW1mjsFwAAAA=="))

        assert(cstdio.ungzip(cstdio))
    End Sub
End Module
