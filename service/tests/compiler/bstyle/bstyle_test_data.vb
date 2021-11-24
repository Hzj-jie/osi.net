
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'case1 case2 global_variable overload_function single_level_struct nested_struct function_struct return_struct call_struct_on_heap for_loop
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module _bstyle_test_data
    Public ReadOnly case1() As Byte
    Public ReadOnly case2() As Byte
    Public ReadOnly global_variable() As Byte
    Public ReadOnly overload_function() As Byte
    Public ReadOnly single_level_struct() As Byte
    Public ReadOnly nested_struct() As Byte
    Public ReadOnly function_struct() As Byte
    Public ReadOnly return_struct() As Byte
    Public ReadOnly call_struct_on_heap() As Byte
    Public ReadOnly for_loop() As Byte

    Sub New()
        case1 = Convert.FromBase64String(strcat_hint(CUInt(188), _
        "77u/CiNpbmNsdWRlIDxic3R5bGUuaD4KCnZvaWQgcHJpbnQoc3RyaW5nIGkpIHsKICBzdHJpbmcgdGVtcDsKICBsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCBpIHRlbXAiOwp9Cgp2b2lkIG1haW4oKSB7CiAgcHJpbnQoIkhlbGxvIFdvcmxkIik7Cn0K"))
        case2 = Convert.FromBase64String(strcat_hint(CUInt(1348), _
        "77u/CiNpbmNsdWRlIDxic3R5bGUuaD4KCmJvb2wgZXF1YWwoaW50IGksIGludCBqKSB7CiAgYm9vbCByZXN1bHQ7CiAgbG9naWMgImVxdWFsIHJlc3VsdCBpIGoiOwogIHJldHVybiByZXN1bHQ7Cn0KCmJvb2wgaXNfZXZlbihsb25nIGkpIHsKICBpbnQgcXVvdGllbnQ7CiAgaW50IHJlbWFpbmRlcjsKICBpbnQgdiA9IDI7CiAgbG9naWMgImRpdmlkZSBxdW90aWVudCByZW1haW5kZXIgaSB2IjsKICByZXR1cm4gZXF1YWwocmVtYWluZGVyLCAwKTsKfQoKdm9pZCBwcmludChib29sIGkpIHsKICBzdHJpbmcgdGVtcDsKICBzdHJpbmcgX3RydWUgPSAiVHJ1ZSI7CiAgc3RyaW5nIF9mYWxzZSA9ICJGYWxzZSI7CiAgaWYgKGkpIHsKCWxvZ2ljICJpbnRlcnJ1cHQgc3Rkb3V0IF90cnVlIHRlbXAiOwogIH0gZWxzZSB7Cglsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCBfZmFsc2UgdGVtcCI7CiAgfQp9Cgpsb25nIGN1cnJlbnRfbXMoKSB7CiAgbG9uZyByZXN1bHQ7CiAgaW50IHRlbXA7CiAgbG9naWMgImludGVycnVwdCBjdXJyZW50X21zIHRlbXAgcmVzdWx0IjsKICByZXR1cm4gcmVzdWx0Owp9Cgpsb25nIHNlbGZfZGVjcmVhc2UobG9uZyB4KSB7CiAgaW50IHRlbXAgPSAxOwogIGxvZ2ljICJzdWJ0cmFjdCB4IHggdGVtcCI7CiAgcmV0dXJuIHg7Cn0KCmludCBzZWxmX2luY3JlYXNlKGludCB4KSB7CiAgaW50IHRlbXAgPSAxOwogIGxvZ2ljICJhZGQgeCB4IHRlbXAiOwogIHJldHVybiB4Owp9Cgpib29sIGxlc3MoaW50IHgsIGludCB5KSB7CiAgYm9vbCByZXN1bHQ7CiAgbG9naWMgImxlc3MgcmVzdWx0IHggeSI7CiAgcmV0dXJuIHJlc3VsdDsKfQoKdm9pZCBtYWluKCkgewogIGZvciAoaW50IGkgPSAwOyBsZXNzKGksIDEwMCk7IGkgPSBzZWxmX2luY3JlYXNlKGkpKSB7CiAgICBsb25nIHg7CiAgICB4ID0gY3VycmVudF9tcygpOwogICAgaWYgKGlzX2V2ZW4oeCkpIHsKICAgICAgeCA9IHNlbGZfZGVjcmVhc2UoeCk7CiAgICB9CiAgICBwcmludChpc19ldmVuKHgpKTsKICB9Cn0K"))
        global_variable = Convert.FromBase64String(strcat_hint(CUInt(352), _
        "77u/CiNpbmNsdWRlIDxic3R5bGUuaD4KCnN0cmluZyBfdHJ1ZSA9ICJUcnVlIjsKc3RyaW5nIF9mYWxzZSA9ICJGYWxzZSI7Cgp2b2lkIHByaW50KGJvb2wgaSkgewogIHN0cmluZyB0ZW1wOwogIGlmIChpKSB7Cglsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCBfdHJ1ZSB0ZW1wIjsKICB9IGVsc2UgewoJbG9naWMgImludGVycnVwdCBzdGRvdXQgX2ZhbHNlIHRlbXAiOwogIH0KfQoKdm9pZCBtYWluKCkgewogIHByaW50KHRydWUpOwogIHByaW50KGZhbHNlKTsKfQo="))
        overload_function = Convert.FromBase64String(strcat_hint(CUInt(1088), _
        "77u/CiNpbmNsdWRlIDxic3R5bGUuaD4KCmJpZ3VpbnQgdG9fYmlndWludChpbnQgaSkgewogIHJldHVybiBpOwp9CgpiaWd1aW50IHRvX2JpZ3VpbnQobG9uZyBpKSB7CiAgcmV0dXJuIGk7Cn0KCmxvbmcgdG9fbG9uZyhpbnQgaSkgewogIHJldHVybiBpOwp9Cgpib29sIGVxdWFsKGJpZ3VpbnQgaSwgYmlndWludCBqKSB7CiAgYm9vbCByZXN1bHQ7CiAgbG9naWMgImVxdWFsIHJlc3VsdCBpIGoiOwogIHJldHVybiByZXN1bHQ7Cn0KCmJvb2wgbm90KGJvb2wgaSkgewogIGlmIChpKSByZXR1cm4gZmFsc2U7CiAgcmV0dXJuIHRydWU7Cn0KCmJvb2wgZXF1YWwoaW50IGksIGludCBqKSB7CiAgcmV0dXJuIGVxdWFsKHRvX2JpZ3VpbnQoaSksIHRvX2JpZ3VpbnQoaikpOwp9Cgpib29sIGVxdWFsKGxvbmcgaSwgbG9uZyBqKSB7CiAgcmV0dXJuIG5vdChlcXVhbCh0b19iaWd1aW50KGkpLCB0b19iaWd1aW50KGopKSk7Cn0KCnZvaWQgcHJpbnQoYm9vbCBpKSB7CiAgc3RyaW5nIHRlbXA7CiAgc3RyaW5nIF90cnVlID0gIlRydWUiOwogIHN0cmluZyBfZmFsc2UgPSAiRmFsc2UiOwogIGlmIChpKSB7Cglsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCBfdHJ1ZSB0ZW1wIjsKICB9IGVsc2UgewoJbG9naWMgImludGVycnVwdCBzdGRvdXQgX2ZhbHNlIHRlbXAiOwogIH0KfQoKdm9pZCBtYWluKCkgewogIHByaW50KGVxdWFsKDEwMCwgMTAwKSk7CiAgcHJpbnQoZXF1YWwoMTAwLCAyMDApKTsKICBwcmludChlcXVhbCh0b19sb25nKDEwMCksIHRvX2xvbmcoMTAwKSkpOwogIHByaW50KGVxdWFsKHRvX2xvbmcoMTAwKSwgdG9fbG9uZygyMDApKSk7Cn0K"))
        single_level_struct = Convert.FromBase64String(strcat_hint(CUInt(276), _
        "77u/DQojaW5jbHVkZSA8YnN0eWxlLmg+DQoNCnN0cnVjdCBzIHsNCiAgaW50IGk7DQogIHN0cmluZyBzOw0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBzIHg7DQogIHguaSA9IDEwMDsNCiAgeC5zID0gImFiYyI7DQogIGJzdHlsZV9fcHV0Y2hhcih4LmkpOw0KICBsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCB4LnMgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmciOw0KfQ=="))
        nested_struct = Convert.FromBase64String(strcat_hint(CUInt(372), _
        "77u/DQojaW5jbHVkZSA8YnN0eWxlLmg+DQoNCnN0cnVjdCBTMSB7DQogIGludCB4Ow0KfTsNCg0Kc3RydWN0IFMyIHsNCiAgUzEgeDsNCiAgaW50IHk7DQp9Ow0KDQp2b2lkIHByaW50KFMxIHMpIHsNCiAgYnN0eWxlX19wdXRjaGFyKHMueCk7DQp9DQoNCnZvaWQgcHJpbnQoUzIgcykgew0KICBwcmludChzLngpOw0KICBic3R5bGVfX3B1dGNoYXIocy55KTsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBTMiBzOw0KICBzLngueCA9IDEwMDsNCiAgcy55ID0gMTAwOw0KICBwcmludChzKTsNCn0="))
        function_struct = Convert.FromBase64String(strcat_hint(CUInt(336), _
        "77u/DQojaW5jbHVkZSA8YnN0eWxlLmg+DQoNCnN0cnVjdCBTIHsNCiAgc3RyaW5nIHM7DQogIHN0cmluZyBzMjsNCn07DQoNCnZvaWQgZihTIHMpIHsNCiAgc3RyaW5nIHRlbXA7DQogIGxvZ2ljICJpbnRlcnJ1cHQgc3Rkb3V0IHMucyB0ZW1wIjsNCiAgbG9naWMgImludGVycnVwdCBzdGRvdXQgcy5zMiB0ZW1wIjsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBTIHM7DQogIHMucyA9ICJhYmMiOw0KICBzLnMyID0gImRlZiI7DQogIGYocyk7DQp9"))
        return_struct = Convert.FromBase64String(strcat_hint(CUInt(356), _
        "77u/DQojaW5jbHVkZSA8YnN0eWxlLmg+DQoNCnN0cnVjdCBTIHsNCiAgc3RyaW5nIHM7DQogIHN0cmluZyBzMjsNCn07DQoNClMgZigpIHsNCiAgUyBzOw0KICBzLnMgPSAiYWJjIjsNCiAgcy5zMiA9ICJkZWYiOw0KICByZXR1cm4gczsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBTIHMgPSBmKCk7DQogIHN0cmluZyB0ZW1wOw0KICBsb2dpYyAiaW50ZXJydXB0IHN0ZG91dCBzLnMgdGVtcCI7DQogIGxvZ2ljICJpbnRlcnJ1cHQgc3Rkb3V0IHMuczIgdGVtcCI7DQp9DQo="))
        call_struct_on_heap = Convert.FromBase64String(strcat_hint(CUInt(432), _
        "77u/DQpzdHJ1Y3QgUyB7DQogIFN0cmluZyBzOw0KfTsNCg0KdHlwZTAgZihTIHMpIHsNCiAgU3RyaW5nIHRlbXA7DQogIGxvZ2ljICJpbnRlcnJ1cHQgc3Rkb3V0IHMucyB0ZW1wIjsNCn0NCg0KdHlwZTAgZihJbnRlZ2VyIGkpIHsNCiAgU3RyaW5nIHRlbXA7DQogIGxvZ2ljICJpbnRlcnJ1cHQgcHV0Y2hhciBpIHRlbXAiOw0KfQ0KDQp0eXBlMCBtYWluKCkgew0KICBTIHNbMV07DQogIC8vIFRPRE86IE1ha2Ugc1swXS5zIHdvcmsuDQogIHMuc1swXSA9ICJhYmMiOw0KICBmKHNbMF0pOw0KICBJbnRlZ2VyIHZbMV07DQogIHZbMF0gPSAxMDA7DQogIGYodlswXSk7DQp9"))
        for_loop = Convert.FromBase64String(strcat_hint(CUInt(532), _
        "77u/DQp0eXBlZGVmIEludGVnZXIgaW50Ow0KdHlwZWRlZiBCb29sZWFuIGJvb2w7DQp0eXBlZGVmIHR5cGUwIHZvaWQ7DQp0eXBlZGVmIFN0cmluZyBzdHJpbmc7DQoNCnZvaWQgZihpbnQmIHgpIHsNCiAgaW50IHkgPSAxOw0KICBsb2dpYyAiYWRkIHggeCB5IjsNCn0NCg0KYm9vbCBsZXNzKGludCB4LCBpbnQgeSkgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImxlc3MgcmVzdWx0IHggeSI7DQogIHJldHVybiByZXN1bHQ7DQp9DQogDQp2b2lkIG1haW4oKSB7DQogIGludCByOw0KICBmb3IgKGludCBpID0gMDsgbGVzcyhpLCA1KTsgZihpKSkgew0KICAgIGxvZ2ljICJhZGQgciByIGkiOw0KICB9DQogIHN0cmluZyB0ZW1wOw0KICBsb2dpYyAiaW50ZXJydXB0IHB1dGNoYXIgciB0ZW1wIjsNCn0="))
    End Sub
End Module
