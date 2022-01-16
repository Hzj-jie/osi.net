
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
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(564), _
        "H4sIAAAAAAAEAIVSsW7bMBTcCfAfXlykARzTBjoalhc3Q4CinTpVqUGLTxFRilRIKkqB/FmGflJ/oSQlyvWURbg7HfXuqPf37Q8l918PX75/voP1euMablFstMIXtEfbK1z7F08JJb8GpnmLruMVwoySXinuHKRn4h7bTnGPkEE8/wEOpu24lc5oShQ6x4xl+NRzBbuCkkeLwWnP4j6IIyoC0sZP+lUxHfcN17A7n0x8P8761qHl3lhKuBBQ3lLSSt07YAH0ystO/YZySYmQz1IgbIJsBFxT0pkBLfyk5CQ941rAxxEaC+Vr+FpUgpRoFEIuuKLEoaqZ1FUYFYclKrACxmLY2jPXyNrDLsS18rHJfJ/iJveYs5jYlHam58xZyslnS8yfydQi07nL/0KqkIWxV2ZTu0wvCmTxskYRewjTnxSyyiijYbuN/2HxbKQIGOoFSAfxtrhSIZ1YgfTgGtMrASecjfViBcY3aAfpMFhuwmIZXStZeRikb0Zj9K3DVfJxKVl8wy7G/yiH1fEhPksRwDLGeWfTP42r/g/I8gliEwMAAA=="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1096), _
        "H4sIAAAAAAAEALVWO2/bMBDeBeg/HJCtMB2go5cOTYcCRTK0nQSjoKSzRYQWVZKK439fPkXJplO3aCeR9/zuxVNZfH78+OX7wydYr+9VRyW29+rUa/qK8occOa71qy6LspBCaKJPA0JVU4Vkuq+gpwdUA23MseFUqRVoPAycatw6TXokVoQcme5IKzRECphLWdzB13EYhNRA14+bTQ27sW80Ez00lHOgfWsYm41lra30k+5QHplCYBpUJ0beQo0OBVStGGuOpBFc9NsPUJ07n3HfXXK1IUZaWXiLOZk/cLOCt53Yk00j+Qv8yUpZzGzE7Ce7HsR09WXBHUrsG4SaaWKybIlTKeHZm/YXZ1dpKjUZqKR7SYfOYIstYKBg3yaWtVS/V/pksCrstfXiA1B4YB4+VGzfC2kaCdXINemQDkRpOTaaxAYgtgFWoJDvyAvlo4mc01GZcAajKAaU1Ik5niEKpc+pJofJZ1lcR/M7vCtwbX+N7VLqgHoAQkLlrrRtQwQH1o8qnk3IbOCncG3ZC2sx8kTUGMQRZTiHIs1uIrISeSJx3GmiOrbTgSDZvguUbQA6zyi8UMmo7S1f6kUgTtDGtxOSsL6RaDIRwluW5eaS3ly/kPa55+lRcQJQveHG6XqItl5i1KSWtHlGm4QsHapETlj8/PiGmismCajGnspTBj7L0Y2VrDxUt3d2WWRkoepFrLlJWOwvbLaxjOa59W+mCq9uzjgEYxk7xm1OYVaQrNKF23xiomM3NmFi0rDEOXEjEqZjGow4E+5iDxyVIrqjpnx70zkaZbg5huko/DlSnpiJEj4mlZ4yJe8K6Fnw/wZ4mt7Z4P6XiJaDvxgmvzkcxR/D7jiXSSJ2oYPfSDc+BXCVs/RzXWm2m85mmDOlzftvV1Pg2JfjDr49PTxtpp8Oo6pZM/10lIX7hyG1aE+mlb0Pe1lNItsgY3ekP6Tle7Ekk7XzLblYTGaBL7a2lTq4CM73uee416cRhwM1ZjMCeXtJK6cDjpPVTBGes13Uy5+YqUtzTnxM8+4N/y6LiK8FOo8vH9ZlNImS/pciMFvC6TzD/QZc/49bFr8A00bBjToLAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
