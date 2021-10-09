
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
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(496), _
        "H4sIAAAAAAAEAF1QS07EMAzd+xTWsIFMw+z5DBfgBm2E0qnTiZomQ5pqhICTseBIXAEnLQix6PN79nNt5+vjE4aztAatyYTcRJghCxMi8pfp+WgdYcEsu4BdyMSFcMIMWURKc/S4hJxoI+kBCy7m3h6wIJSp/uDmjvBiJbBGebbpKE2e+FOqm1Zt72rxtlfb/dJsfEeGDSWWpchYzy1LLNf4ji+7KAFgsr53JB0X5SGMI/mEux3/s/FKwDi7ZP8VG5GrYqcEQ5maXk6Ux64RoHXaD1jWYxGCwzrFmSqj+REVX5Sop4j1tpLqoW46trW2nzmPRdWuelQwGxf0khHXi2tKkffFTd1sKvG2UWIDkBfT2FRMXPB4A88zTckGL0cdB3zgJh2TPOmo+6hPR3wFvv2Pfl8dbdSHgRJelvqPulqr0/OsI/2mm7q4/mcVTDRauaxyC6Cnyfa+PNw9gNcj8T3n6kllbDomAuAbWJFtKHACAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(936), _
        "H4sIAAAAAAAEAI1VzW7bMAy+8ymI9lZYe4Vih2DYZRm6XgbDKGRbToTIUiDLDfr2oyzJluOk3cWW+CNRHz+S8PPHr/3L7u317+/dH6wV16cCB6kPSjAltWCN6XuhXYH9qJxcyeBlv3+NntYYx9zHWQDMSyy7UTdOGl3gO1ejYK3opJZewi7SHdkgekmnKW8hdaPGVhQ4GdFfdpqW7GL5+SzaApU5yGbr52+a7NaKCiDdjpr3InwGx61jteXNSTgsz9zyXsnBVc8odDsrwlsHeqPQjWDe7EBhHGF2iL7hVo8Hr55wEsG1IohxWgflEhDAV8BskMNFBzBHhmWKtrgbfQVJtlhv8VxpzOhWmN5zw/I6zCXljaIAchEJxoHitMKNVs9bedDGCmbFQPGzlDzWcKUKPF0oOYKfIg3mtFPiMjw+iR3Lxug2hnY5SkU3dsYyZcyZuHIPs8iYGzhTvj1lFm5sUpnxjg8DPc8XTcAANuBknMhRuuMO82M8MrK7IvZktGJ09gChBl/C0b96hrXAH+glmL1swssrwuK/L6MajBB7Z1pfF+BdjiworjIcPW7JImBXlXydnmQ88TcapTsTaaK4gq3pV0+HmyctjTCy+Z1byWtqsT67vvU5cRC2wFoeRumb7dgpw+lfG0Pmg7PUkSv/hMwvNpBPygZXu6UhBuWthhjC33ZEWBQrBFPny0mddb6AT+h8sCp3z4cgmFMK8Ii7vhYt8lDkPjgnJta30orGqQ8qf3kSUf1AUJFNS0gjfwsgPUDQ0fHpDC+Gm8MjXZM11DiFouXkO1VYEKfT0ra8YT5PsiDsqGAoc+thForWS0IWynlmEpa+/qgWOwiD0O/jKiT8Eb8rZS5TB3xCZ7AWSIgSbAMhF4L4BmkshkEceJZ4FHX+5LRcmecbuDNfk1GO3j9I8ErRTQgAAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
