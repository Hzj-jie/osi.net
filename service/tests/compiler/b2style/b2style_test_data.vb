
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'case1 case2
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module _b2style_test_data
    Public ReadOnly case1() As Byte
    Public ReadOnly case2() As Byte

    Sub New()
        case1 = Convert.FromBase64String(strcat_hint(CUInt(64), _
        "77u/DQp2b2lkIG1haW4oKSB7DQogIHN0ZF9vdXQoIkhlbGxvIFdvcmxkIik7DQp9"))
        case2 = Convert.FromBase64String(strcat_hint(CUInt(256), _
        "77u/DQpib29sIGlzX2V2ZW4oaW50IGkpIHsNCiAgcmV0dXJuIGVxdWFsKG1vZChpLCAyKSwgMCk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHg7DQogIHggPSBjdXJyZW50X21zKCk7DQogIGlmIChpc19ldmVuKHgpKSB7DQogICAgeCA9IHNlbGZfZGVjKHgpOw0KICB9DQogIGJvb2xfc3RkX291dChpc19ldmVuKHgpKTsNCn0NCg=="))
    End Sub
End Module
