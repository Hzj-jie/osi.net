
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'prefix ufloat
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_statements
    Public ReadOnly prefix() As Byte
    Public ReadOnly ufloat() As Byte

    Sub New()
        prefix = Convert.FromBase64String(strcat_hint(CUInt(2088), _
        "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"))

        assert(prefix.ungzip(prefix))
        ufloat = Convert.FromBase64String(strcat_hint(CUInt(688), _
        "H4sIAAAAAAAEAMWVwY7bIBCG75HyDignW9pTr1WlvAlywiRLhBkXhu2uqr57scEx9pqs4+y2N9vMzD//Nxi2mwOiYodvlt4UcA4/XaUKd1JYEZNPLD5dSvZ7u2GsCzZgnaLv7bvCszyyXRcUcuMqk+yy60IMkDN6SPqz3Wwnohrp64RTlSLElPNtnA1UBIbTc6VXdlKjgTUEemk0H4GYukJTzBvwqZfyaTrZ7mvGvwJrHzHf5q8x3+k+5nxofYntWPgaUwmRlx1Z9JHe2tSczBWupXZ2YWnrDmSqI91V3zOVjXpbKNGH3yMh5IsUkBeIr7k9EdKX7YqpdIO/wCy01sXe8mXJSH0eiodinJD7latILB2Da6BnFFxXNbAfyX7FSoDgYdUWu4M8cyfgGIvtyhSD1P6PNK4h1qbFpFHl/b4xcJKvYPcEdWP3QT3YiJ28xzvUhVc4OoJxWx5EyLnB+wWlGFxZEhwdTVG8W88QlPnfKwbyk8G68LCc7/0R1D69Ve2xfyrv7Hb+BN43uaRMpifdGGD/NSBoaSyGr9Cj/loVf4JJ1Ncxa38Ux0cxd2Kkwx41oMt0Hk5kA0U5e9vHcyuo+LNDrOo97TvPK+bp5NpJsIl12LpZ/UvtzLg+lJ7T1f4KvoN45/U/aa+ZdE720RH0HO6b/apu/gIoAd9D+QsAAA=="))

        assert(ufloat.ungzip(ufloat))
    End Sub
End Module
