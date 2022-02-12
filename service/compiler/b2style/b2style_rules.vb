
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
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(604), _
        "H4sIAAAAAAAEAIVTwW7cIBS8I/EPL1s1lZJlV8oxsn1Je4hUtZf0VLcRa55jVBZcwOtU6p/10E/qLxSw8e7mkguaGR68NyP49+cvJfef7j5+ef8BNput67hFsdUKn9E+2kHhxj97Sij5MTLN9+h63iAsKOmN4s5BWhP3uO8V9wgZJNUc0Fop+E4hnODTvWUDY8c3cGf2PbfSGU2JQueYsQx/DlxBUVLyZDHcbY9iFcQJlQFp42f9opyP+45rKI4nE6+mXp97tNwbSwkXAuprSvZSDw5YAIPysle/oL6iRMhDHHQbZCPgLSW9GdHCd0p20jOuBVxO0Fiof4fbohKkRKMQ5oILShyqlkndhFaxWaICG2AsDtt65jrZeiiKOJyVT11WZIja6FbJxsMofbekDMVD8XBTVS8PVMlhajBZK2c2G1zo0WaWstmlJFrOZDae6WL/VEiuszBFkdkcSKZnnrN4bqOMPoQZwqthjVFGw+1tNLs6GCkChnYV04kBc6XCdGIN0oPrzKAE7HApbFdrML5DO0qHoeTdy0hTYazbUGL59PJZ3GFn7b/W4/rxW1xrEcBVHOeV73Qz/af/1fTFI3gDAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1316), _
        "H4sIAAAAAAAEALUXy27bOPAuQP8wQG8LywX26Mse0j0sUCSHdk9CUNDS2CJCiypJJfF+fYcvkZLl1Fm0J4vzfs+4LP65v/v876e/Ybv9qDumsP2oz71hr6i+qVHg1ryasigLJaWpzHlAqPdMYzW9N9CzE+qBNfTZCKb1BgyeBsEMPjpOfuxMpTt+MHBUSGBVmY71sweRsZfKSqpeuOmqVhqIEKBHWXyAL+MwSGWAbe93uz0cxr4xXPbQMCGA9S0hdjuL2lrqB9OheuEagRvQnRxFC3t0xkLdynEvsGqkkP3jX1AvlWfYPy6xhoARVhZe4hrNO9Rs4G0l9stGu/of9icpZZHJiElKcr0R09NnDw+osG8Q9txUFGULnDIOT160fzi52jBlqoEpdlRs6Mi2WClkCvZtQllJH+COKgGZ5uIMrv5AyCNvwDoC+Mq14f0RGtkiZTWmPPM1euGVC9TaF5fVcrJaBYlYlNokxlbOQsxbKrxre8WaJzRQPzMxYmXlUwqsZwFTFvzYS0U9gnoUprpJ3SWuLFw3VbznhjPB/0MFKX1ZrG8waP+nNmcqCY29scn0mjSeuK8SqOcmd8iGShs1NgvzN3CzbxvQKA6VN4pcGTWBBmKVAyrmmByOgFKbS+iF9+RUsrgsrvvyM2834EbYNbSre2e7t0kqqN2TtW1w6sT7UcdvigMfxDk8W/7M2+j8SUaOQb6gCt+hk7KXjKgEnkACD2F8BkA2UB+DoXmQ4ZkpzuwA8DUyc8QRWv8OUlFsG+oL4qhXMnVzQbwjpS7suWY/YmjoOAKo31DzZqU5wd5+i5Hj1BWkYRUeu8UJmgz1E9BhZoyJAuqxZ+q84htfg5OUVXqobw4biVihhbqXsSAomrH4sHmMOaaF6beeDntzTTgEYStySO0aQ5atVaYLteuBiYpdT4V2Sp0Um8j1T2idqWtiw7iH/Zim/mY26AOCyg2/j0wkZIKEHwqlh0zBu2J05vyvMTy1dtbVv8Wj+VSYdZrf/Q7iP8P2X9IkEnuSgb8pbpwTcBUz13Od6X0bzxbi14dPD7vpbCRWQ4dFVJDdQr7FG3k6sWzDujf519MdyQ227obRdDetsNFpM4HT3naMzN5OfjEtRFFOuDKUo8lpe07JZ1SKtzZVma0BimukOZ1TbXH+Izv3Loyy63R5q4VQ72VLtby0jmp2aUUay8vjbraqKdYXx+b81rzxcnP0jsAlPSTDAy6z4eAzpmWiHTRmOoOkk7lFgUdr+PLKiIiZo5ObNoIh5hu4JiOLXmK1yZu+U1SWns/+Sc3UlsUPcIsuEtUNAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
