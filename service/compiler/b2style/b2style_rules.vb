
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
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(800), _
        "H4sIAAAAAAAEAF1TwU7cMBC9R8o/DItatUsCd8rCobRSJdRTe0q2yIknWWsdO9gOAZV+WQ/9pP5CZ5xkgR7Wee95PH4z4/37+0+aHMO3nfLQKI0wKq2hQqituUcXUIIywYJDbwdXYwbewvGXrx9vvl9/AmnRg7EBRuv2p5zos9Xajsq0ILFRRgVljQfhOGGvKFvjbAeVD48az2orsUVzZjQ+oLt1g8bT8BAo0X7MVQOqiQi1R+AlssY6oF/E4y5a5jVyaclSRNraHniJzGEYnIHpE5XKodhDXOf4VtUQ1zRJk0oLs4eirLYnkVqroQhuwKwRZGSbJtQV8u6gOMny7VVRSo6sVDvQBkRa6OyGAodGWzFJ69M5zgfHLVoV5SpbP6226xXfUtuuE1BmjLQ1cJ4mdwN6bmHeCbeHKz4pXMh74UTrRL+Dn2mCRr4Qfi0xlRP1HgO8myIW+n7Z93cDjeWgl8UU979MFXjsVD5Z+sA+hfeqNR1SoRvmx3CtmgYdCzxDzyJ9fG5djpRPwwUFttTsgO5ZvCRxQhtC9Ixm/WgzHw87YeDi+WTkl9GClFBSIztlBg85gUEH1etHKNdpItW9kghnJFsJb9KktyON6gcPKOTCSHg7QXpK5RNlY4WkSFngJ33EhesmV6amq/iySCXWkOdssKEe7lQT4IIsOtXuFn4ZLcboyedmZrPbA332vEiL80MI+1/IXMVCD7W8FGIJizDVtbC5uoW+KmARX5cRp0t/DiM69L2oEQ6Id5jQux6z2+z8fMuglBOmGfwDIS7Bv1kEAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1096), _
        "H4sIAAAAAAAEALVWzW7cIBC+W/I7oORWLX2FnKKql6RKcqksK8I29qLF4ALOKm/f4dd47U1yaC+75pthmN8PyuLnj4fHp/vXl9+/7p9Rw4k4lcXT4+NLQPpZtIZJcUBvhM8Ud7RnglkEn5k5Yk1HhlvJrYYgI9UTaWlZlMUtepBIGzW3Bul5mqQy3y0eDTpt/6MNUQY3irQnalA1EUVGzrSp7xAVXRKMMzcMDhSGipZiqzYoMh3LIu0Im71rrRxHUn9DDgpKmcTjyH0H6eKT9fSzgDcZQYvM7k/pQKczXhZZyCkEVH09zxCRzUoWvvc+GIr5OVzNV10WEVzUt9VcSeRschcWC5ukVJf+LxG1HFzIIQBmDZ4qamYl0pINQiqKAxozg1vC+cHmslGUnA6Iy4G11k/e49wetE1eiA/iQFUrRRe8PB8Zh8N7qTCXcqptXq+lcFu/qLNXnU38eZ9tmmiZC6I1pGIEyz5fUdmHeU1nkw+vWDmcdF3I2MjErOO3DXPi72HZsTfW0SiTccckz1SF74YZTESXrWQULXCCOO0h7UfWmwAoNhwDUke/yyIVwxaZ9Re84NRWhJCln3IN4cb99V1ZrBFr0SJoVRhXcivyH18+z/FY6BO7Hb4vSexqzy+VXvVp2LGHpbnY6/Sr/nn3gwU3pEEtOhLHIcB17K5c97OUlIUiZxyOuhjUN6IYaTh11AdDDfMx2P5p2DDD4oDmnksC/42UoA5XBRPDyovMO1Slg3L/sZyoT+/a/QSjahZEvS9AtNGwPRys7OrDvQKEtDEySW12TOzookrI2PxMRNaC9kjtf4ueIWDexcty13ioQLVjB47d25DVZ3fT5tj9xMSDHX8E6lhYIxKG44pAE4khIjm4hf3gVGtsjgRGYgAmN1SFlRPAWNE/M+GLcEHCH6TSIyl5V5zOgv83ji80ljHYf4nItnM2P+lR8sHdiFar/K3lxXtvLT8z28dWnCb/qMomKz6qVhdS9qjy6Q6PKqCH/Ga3VOmBxHb+pXg/NrRDxN/o1kVD3Y3WMUVbYwvF2YkG8Q2QB+h0wA6IvHrauCkLL4QTohGLl8VfYXHXSeEKAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
