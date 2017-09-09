
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'test_config
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module _configuration_test_cases
    Public ReadOnly test_config() As Byte

    Sub New()
        test_config = Convert.FromBase64String(strcat_hint(CUInt(5344), _
        "77u/DQo7IGxhbmcgdXNlcyBhIHN0cmluZyBjYXNlLWluc2Vuc2l0aXZlIG1hdGNoLg0KbGFuZz1zdHJpbmcNCjsgYnJhbmQgdXNlcyBzdHJpbmcgcGF0dGVybiBtYXRjaC4NCmJyYW5kPXN0ci1wYXQNCjsgdGltZSBpcyB0aGUgbGFzdCBkaWdpdCBvZiBjdXJyZW50IHRpbWUgaW4gbWlsbGlzZWNvbmRzLiBJdCB1c2VzIGludGVnZXIgc2VyaWFsIGZpbHRlci4NCnRpbWU9aW50LXNlcg0KOyBwZXJjZW50YWdlIGlzIGluIHJhbmdlIFswLCAxXS4gSXQgdXNlcyBkb3VibGUgaW50ZXJ2YWwgZmlsdGVyLg0KcGVyY2VudGFnZT1kYmwtaW50DQo7IG5hbWUgdXNlcyBzdHJpbmcgc2VyaWFsIGZpbHRlci4NCm5hbWU9c3RyLXNlcg0KOyBlbWFpbCB1c2VzIG11bHRpcGxlIHN0cmluZyBwYXR0ZXJuIG1hdGNoLg0KZW1haWw9c3RyLXBhdHMNCjsgdXNlci1uYW1lIHVzZXMgc3RyaW5nIGNvbXBhcmUgZmlsdGVyLiBJLmUuIEluc3RlYWQgb2Ygc3RyaW5nIGZ1bGwtbWF0Y2gsID5hYmMgb3IgPGFiYyBjYW4gYmUgdXNlZC4NCnVzZXItbmFtZT1zdHItY29tDQo7IHJldmVyc2UtaG9zdCBpcyB0aGUgcmV2ZXJzZWQgaG9zdCBuYW1lLiBJdCB1c2VzIHN0cmluZyBwcmVmaXggbWF0Y2guDQpyZXZlcnNlLWhvc3Q9c3RyLXByZQ0KOyBwYXRoIGlzIHRoZSBwYXJ0IG9mIHVybCBhZnRlciB0aGUgdGhpcmQgc2xhc2ggKC8pLiBJdCB1c2VzIG11bHRpcGxlIHN0cmluZyBwcmVmaXhlcyBtYXRjaC4gSS5lLiBzZXZlcmFsIGRpZmZlcmVudCBwYXRoDQo7IHN0cmluZ3MgY2FuIG1hdGNoIG9uZSBjb25maWd1cmF0aW9uLg0KcGF0aD1zdHItcHJlcw0KOyBwYXNzd29yZCB1c2VzIHN0cmluZyBjYXNlIHNlbnNpdGl2ZSBtYXRjaC4NCnBhc3N3b3JkPXN0ci1jYXNlDQo7IHByb2Nlc3Nvci11c2FnZSBpcyB0aGUgY3VycmVudCBwcm9jZXNzb3IgdXNhZ2UgaW4gcmFuZ2UgWzAsIDEwMF0uIEl0IHVzZXMgZG91YmxlIGNvbXBhcmUgZmlsdGVyLg0KcHJvY2Vzc29yLXVzYWdlPWRibC1jb20NCg0KW2I6ZGVidWdidWlsZCRidWlsZF0NCmRlYnVnYnVpbGQ9dHJ1ZQ0KDQpbYjpkZWJ1Z2J1aWxkJGJ1aWxkXQ0KcmVsZWFzZWJ1aWxkPWZhbHNlDQoNCltiOnJlbGVhc2VidWlsZCRidWlsZF0NCmRlYnVnYnVpbGQ9ZmFsc2UNCg0KW2I6cmVsZWFzZWJ1aWxkJGJ1aWxkXQ0KcmVsZWFzZWJ1aWxkPXRydWUNCg0KW2FwcGxpY2F0aW9uX2RpcmVjdG9yeV0NCmFkOkRlYnVnJHZhbHVlPURlYnVnDQoNClthZDpSZWxlYXNlJGFwcGxpY2F0aW9uX2RpcmVjdG9yeV0NCnZhbHVlPVJlbGVhc2UNCg0KW2FwcGxpY2F0aW9uX2RpcmVjdG9yeV0NCnZhbHVlPVVua25vd24NCg0KW2Vudmlyb25tZW50XQ0KYjpkZWJ1Z2J1aWxkJmFkOlJlbGVhc2UkdmFsdWU9c3RyYW5nZQ0KYjpyZWxlYXNlYnVpbGQmYWQ6RGVidWckdmFsdWU9c3RyYW5nZQ0KYWQ6UmVsZWFzZSR2YWx1ZT1kZXYNCmFkOkRlYnVnJHZhbHVlPWRldg0KdmFsdWU9dXR0LXJ1bg0KDQpbc2VjdGlvbkBlbWFpbDphbm5veW1vdXNAYW5ub3ltb3VzLmNvbSZwYXRoOmRvd25sb2FkPyZyZXZlcnNlLWhvc3Q6Y29tLmFubm95bW91c10NCndlbGNvbWU9Tm90IFdlbGNvbWUNCnRpdGxlPU5vdCBXZWxjb21lDQpsdWNreT1mYWxzZQ0Kcm91bmQ9LTENCmdlbmRlcj1vdGhlcg0KZG9tYWluPWFubm95bW91cy5jb20NCnRvcDUwPWZhbHNlDQpib3R0b201MD1mYWxzZQ0KcHJpb3JpdHk9OTk5OTk5OTkNCnNlYXJjaD1mYWxzZQ0KZW5jcnlwdGVkPWZhbHNlDQpyZXN0cmljdGVkPXRydWUNCm1lc3NhZ2U9WW91IEFyZSBOb3QgV2VsY29tZWQgVG8gVXNlIE91ciBTZXJ2aWNlDQoNCltzZWN0aW9uXQ0Kd2VsY29tZUBsYW5nOmVuPXdlbGNvbWUNCndlbGNvbWVAbGFuZzp6aD3mrKLov44NCndlbGNvbWVAbGFuZzpkdT13ZWxrb20NCndlbGNvbWVAbGFuZzpqYT15b29rb3NvDQp3ZWxjb21lPVdlbGNvbWUNCg0KdGl0bGVAYnJhbmQ6bGF2ZSo9TGF2ZSBTZWFyY2gNCnRpdGxlQGJyYW5kOmRpbmcqPURpbmcgU2VhcmNoDQp0aXRsZT1ObyBCcmFuZCBTZWFyY2gNCg0KbHVja3lAdGltZToxLDMsNSw3LDk9dHJ1ZQ0KbHVja3lAdGltZTowLDIsNCw2LDg9ZmFsc2UNCmx1Y2t5PXVua25vd24NCg0Kcm91bmRAcGVyY2VudGFnZTowLTAuNDk5OTk5OT0wDQpyb3VuZEBwZXJjZW50YWdlOjAuNS0xPTENCnJvdW5kPS0xDQoNCmdlbmRlckBuYW1lOnJvc2UsbHVjeSxsaWxpPWZlbWFsZQ0KZ2VuZGVyQG5hbWU6amFjayx4aWFvbWluZyxqb2huPW1hbGUNCmdlbmRlcj1vdGhlcg0KDQpkb21haW5AZW1haWw6KkBqbWFpbC5jb20sKkBqb29qbGUuY29tPWpvb2psZS5jb20NCmRvbWFpbkBlbWFpbDoqQGxhdmUuY29tLCpAaW5sb29rLmNvbSwqQG1hY3Jvc29mdC5jb209bWFjcm9zb2Z0LmNvbQ0KZG9tYWluPWV4YW1wbGUuY29tDQoNCnRvcDUwQHVzZXItbmFtZTo8bXp6enp6enp6enp6enp6enp6enp6PXRydWUNCnRvcDUwPWZhbHNlDQpib3R0b201MEB1c2VyLW5hbWU6Pm16enp6enp6enp6enp6enp6enp6ej10cnVlDQpib3R0b201MD1mYWxzZQ0KDQpwcmlvcml0eUByZXZlcnNlLWhvc3Q6Y29tLmpvb2psZT0xDQpwcmlvcml0eUByZXZlcnNlLWhvc3Q6Y29tLmxhdmU9Mg0KcHJpb3JpdHlAcmV2ZXJzZS1ob3N0OmNvbS5pbmxvb2s9Mw0KcHJpb3JpdHk9OTk5OTkNCg0Kc2VhcmNoQHBhdGg6c2VhcmNoPyxzPyxzb3VzdW8/PXRydWUNCnNlYXJjaD1mYWxzZQ0KDQplbmNyeXB0ZWRAcGFzc3dvcmQ6VGhpc0lzQVBhc3N3b3JkPXRydWUNCmVuY3J5cHRlZD1mYWxzZQ0KDQpyZXN0cmljdGVkQHByb2Nlc3Nvci11c2FnZTo+NTA9dHJ1ZQ0KcmVzdHJpY3RlZD1mYWxzZQ0KDQpbc2VjdGlvbl0NCm1lc3NhZ2VAbGFuZzplbiZicmFuZDpsYXZlKiZwYXRoOnNlYXJjaD8scz8sc291c3VvPyZyZXZlcnNlLWhvc3Q6Y29tLmxhdmU9V2VsY29tZSB0byBVc2UgTGF2ZSBTZWFyY2ggKGh0dHA6Ly93d3cubGF2ZS5jb20pDQptZXNzYWdlQGxhbmc6ZW4mYnJhbmQ6ZGluZyomcGF0aDpzZWFyY2g/LHM/LHNvdXN1bz8mcmV2ZXJzZS1ob3N0OmNvbS5kaW5nPVdlbGNvbWUgdG8gVXNlIERpbmcgU2VhcmNoIChodHRwOi8vd3d3LmRpbmcuY29tKQ0KbWVzc2FnZUBsYW5nOmVuJmJyYW5kOmxhdmUqJnJldmVyc2UtaG9zdDpjb20ubGF2ZT1XZWxjb21lIHRvIFVzZSBMYXZlIChodHRwOi8vd3d3LmxhdmUuY29tKQ0KbWVzc2FnZUBsYW5nOmVuJmJyYW5kOmRpbmcqJnJldmVyc2UtaG9zdDpjb20uZGluZz1XZWxjb21lIHRvIFVzZSBEaW5nIChodHRwOi8vd3d3LmxhdmUuY29tKQ0KbWVzc2FnZUBsYW5nOmVuJmJyYW5kOmxhdmUqPVdlbGNvbWUgdG8gVXNlIExhdmUNCm1lc3NhZ2VAbGFuZzplbiZicmFuZDpkaW5nKj1XZWxjb21lIHRvIFVzZSBEaW5nDQptZXNzYWdlQGxhbmc6ZW49V2VsY29tZSB0byBVc2UgT3VyIFNlcnZpY2UNCg0KbWVzc2FnZUBsYW5nOnpoJmJyYW5kOmxhdmUqJnBhdGg6c2VhcmNoPyxzPyxzb3VzdW8/JnJldmVyc2UtaG9zdDpjb20ubGF2ZT3mrKLov47kvb/nlKhMYXZh5pCc57SiIChodHRwOi8vd3d3LmxhdmUuY29tKQ0KbWVzc2FnZUBsYW5nOnpoJmJyYW5kOmRpbmcqJnBhdGg6c2VhcmNoPyxzPyxzb3VzdW8/JnJldmVyc2UtaG9zdDpjb20uZGluZz3mrKLov47kvb/nlKhEaW5n5pCc57SiIChodHRwOi8vd3d3LmRpbmcuY29tKQ0KbWVzc2FnZUBsYW5nOnpoJmJyYW5kOmxhdmUqJnJldmVyc2UtaG9zdDpjb20ubGF2ZT3mrKLov47kvb/nlKhMYXZhIChodHRwOi8vd3d3LmxhdmUuY29tKQ0KbWVzc2FnZUBsYW5nOnpoJmJyYW5kOmRpbmcqJnJldmVyc2UtaG9zdDpjb20uZGluZz3mrKLov47kvb/nlKhEaW5nIChodHRwOi8vd3d3LmxhdmUuY29tKQ0KbWVzc2FnZUBsYW5nOnpoJmJyYW5kOmxhdmUqPeasoui/juS9v+eUqExhdmUNCm1lc3NhZ2VAbGFuZzp6aCZicmFuZDpkaW5nKj3mrKLov47kvb/nlKhEaW5nDQptZXNzYWdlQGxhbmc6emg95qyi6L+O5L2/55So5oiR5Lus55qE5pyN5YqhDQoNCm1lc3NhZ2U9V2VsY29tZSBUbyBVc2UgT3VyIFNlcnZpY2UNCg0KbWVzc2FnZS1oZWFkPVllcz8NCg=="))
    End Sub
End Module
