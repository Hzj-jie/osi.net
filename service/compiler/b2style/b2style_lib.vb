Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_lib
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(21589), 
            "BwAAAHJ1bi5jbWRrAAAADQpkZWwgL3MgKi51bn4NCi4uXC4uXC4uXHJlc291cmNlXGdlblx0YXJfZ2VuXG9zaS5yb290LnV0dCB0YXJfZ2VuIC0tb3V0cHV0PWIyc3R5bGVfbGliDQptb3ZlIC9ZICoudmIgLi5cDQoJAAAAYjJzdHlsZS5oEwEAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX0gNCiNkZWZpbmUgQjJTVFlMRV9MSUJfQjJTVFlMRV9IDQoNCiNpbmNsdWRlIDxic3R5bGUuaD4NCg0KI2luY2x1ZGUgPGIyc3R5bGVfc3RkaW8uaD4NCiNpbmNsdWRlIDxiMnN0eWxlX29wZXJhdG9ycy5oPg0KI2luY2x1ZGUgPGIyc3R5bGVfdWZsb2F0Lmg+DQojaW5jbHVkZSA8YjJzdHlsZV90eXBlcy5oPg0KI2luY2x1ZGUgPGIyc3R5bGVfZGVsZWdhdGVzLmg+DQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfQjJTVFlMRV9IDwAAAGIyc3R5bGVfdHlwZXMuaKkBAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfQjJTVFlMRV9UWVBFU19IDQojZGVmaW5lIEIyU1RZTEVfTElCX0IyU1RZTEVfVFlQRVNfSA0KDQojaW5jbHVkZSA8YnN0eWxlLmg+DQoNCi8vIFRPRE86IFNlYXJjaCB0eXBlcyBpbiBwYXJlbnQgc2NvcGVzLg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQp0eXBlZGVmIDo6c3RyaW5nIHN0cmluZzsNCnR5cGVkZWYgOjp2b2lkIHZvaWQ7DQp0eXBlZGVmIDo6Ym9vbCBib29sOw0KdHlwZWRlZiA6OmJpZ3VpbnQgYmlndWludDsNCnR5cGVkZWYgOjpsb25nIGxvbmc7DQp0eXBlZGVmIDo6aW50IGludDsNCnR5cGVkZWYgOjpieXRlIGJ5dGU7DQp0eXBlZGVmIDo6dWZsb2F0IHVmbG9hdDsNCg0KfSAgLy8gbmFtZXNwYWNlIGIyc3R5bGUNCg0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9CMlNUWUxFX1RZUEVTX0gNChMAAABiMnN0eWxlX2RlbGVnYXRlcy5o1QcAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX0RFTEVHQVRFU19IDQojZGVmaW5lIEIyU1RZTEVfTElCX0IyU1RZTEVfREVMRUdBVEVTX0gNCg0KI2luY2x1ZGUgPGIyc3R5bGVfdHlwZXMuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQp0ZW1wbGF0ZSA8UlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbigpOw0KDQp0ZW1wbGF0ZSA8VCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihUKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyLCBUMywgVDQsIFQ1KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDkpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTApOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgVDEwLCBUMTEsIFQxMiwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIsIFQxMywgVDE0KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgVDEwLCBUMTEsIFQxMiwgVDEzLCBUMTQsIFQxNSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIsIFQxMywgVDE0LCBUMTUpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgVDE1LCBUMTYsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgVDE1LCBUMTYpOw0KDQp9ICAvLyBuYW1lc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX0IyU1RZTEVfREVMRUdBVEVTX0gTAAAAYjJzdHlsZV9vcGVyYXRvcnMuaOcjAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfQjJTVFlMRV9PUEVSQVRPUlNfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX09QRVJBVE9SU19IDQoNCiNpbmNsdWRlIDxic3R5bGUuaD4NCiNpbmNsdWRlIDxiMnN0eWxlX3R5cGVzLmg+DQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCg0KYm9vbCBhbmQoYm9vbCBpLCBib29sIGopIHsNCiAgaWYgKGkpIHJldHVybiBqOw0KICByZXR1cm4gZmFsc2U7DQp9DQoNCmJvb2wgb3IoYm9vbCBpLCBib29sIGopIHsNCiAgaWYgKGkpIHJldHVybiB0cnVlOw0KICBpZiAoaikgcmV0dXJuIHRydWU7DQogIHJldHVybiBmYWxzZTsNCn0NCg0KYm9vbCBub3QoYm9vbCBpKSB7DQogIGlmIChpKSByZXR1cm4gZmFsc2U7DQogIHJldHVybiB0cnVlOw0KfQ0KDQovLyBUT0RPOiBVc2UgYSBiZXR0ZXIgd2F5IHRvIGNvbXBhcmUgc3RyaW5ncywgdHJlYXRpbmcgdGhlbSBhcyBiaWdfdWludHMgaXMgbm90IGFjY3VyYXRlIG9yIGVmZmljaWVudC4NCmJvb2wgZXF1YWwoc3RyaW5nIGksIHN0cmluZyBqKSB7DQogIGJvb2wgcmVzdWx0Ow0KICBsb2dpYyAiZXF1YWwgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgbm90X2VxdWFsKHN0cmluZyBpLCBzdHJpbmcgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImVxdWFsIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gbm90KHJlc3VsdCk7DQp9DQoNCmJvb2wgZXF1YWwoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJlcXVhbCBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KYm9vbCBub3RfZXF1YWwoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJlcXVhbCBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIG5vdChyZXN1bHQpOw0KfQ0KDQpib29sIGVxdWFsKGxvbmcgaSwgbG9uZyBqKSB7DQogIHJldHVybiBlcXVhbCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpib29sIG5vdF9lcXVhbChsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gbm90X2VxdWFsKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgZXF1YWwoaW50IGksIGludCBqKSB7DQogIHJldHVybiBlcXVhbCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpib29sIG5vdF9lcXVhbChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIG5vdF9lcXVhbCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfdGhhbihiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgIm1vcmUgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgZ3JlYXRlcl90aGFuKGxvbmcgaSwgbG9uZyBqKSB7DQogIHJldHVybiBncmVhdGVyX3RoYW4oOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYm9vbCBncmVhdGVyX3RoYW4oaW50IGksIGludCBqKSB7DQogIHJldHVybiBncmVhdGVyX3RoYW4oOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYm9vbCBsZXNzX3RoYW4oYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJsZXNzIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQpib29sIGxlc3NfdGhhbihsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gbGVzc190aGFuKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbGVzc190aGFuKGludCBpLCBpbnQgaikgew0KICByZXR1cm4gbGVzc190aGFuKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICByZXR1cm4gb3IobGVzc190aGFuKGksIGopLCBlcXVhbChpLCBqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gb3IobGVzc190aGFuKGksIGopLCBlcXVhbChpLCBqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIG9yKGxlc3NfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgcmV0dXJuIG9yKGdyZWF0ZXJfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwobG9uZyBpLCBsb25nIGopIHsNCiAgcmV0dXJuIG9yKGdyZWF0ZXJfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwoaW50IGksIGludCBqKSB7DQogIHJldHVybiBvcihncmVhdGVyX3RoYW4oaSwgaiksIGVxdWFsKGksIGopKTsNCn0NCg0KYmlndWludCBhZGQoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGFkZChsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhpKTsNCn0NCg0KaW50IGFkZChpbnQgaSwgaW50IGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChpKTsNCn0NCg0KYnl0ZSBhZGQoYnl0ZSBpLCBieXRlIGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2J5dGUoaSk7DQp9DQoNCmJpZ3VpbnQgbWludXMoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgInN1YnRyYWN0IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmxvbmcgbWludXMobG9uZyBpLCBsb25nIGopIHsNCiAgbG9naWMgInN1YnRyYWN0IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhpKTsNCn0NCg0KaW50IG1pbnVzKGludCBpLCBpbnQgaikgew0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQoaSk7DQp9DQoNCmJpZ3VpbnQgbXVsdGlwbHkoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgIm11bHRpcGx5IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmxvbmcgbXVsdGlwbHkobG9uZyBpLCBsb25nIGopIHsNCiAgbG9naWMgIm11bHRpcGx5IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhpKTsNCn0NCg0KaW50IG11bHRpcGx5KGludCBpLCBpbnQgaikgew0KICBsb2dpYyAibXVsdGlwbHkgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQoaSk7DQp9DQoNCmJpZ3VpbnQgZGl2aWRlKGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGJpZ3VpbnQgcmVzdWx0Ow0KICBsb2dpYyAiZGl2aWRlIGIyc3R5bGVfX3Jlc3VsdCBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQpsb25nIGRpdmlkZShsb25nIGksIGxvbmcgaikgew0KICBsb25nIHJlc3VsdDsNCiAgbG9naWMgImRpdmlkZSBiMnN0eWxlX19yZXN1bHQgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmcgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhyZXN1bHQpOw0KfQ0KDQppbnQgZGl2aWRlKGludCBpLCBpbnQgaikgew0KICBpbnQgcmVzdWx0Ow0KICBsb2dpYyAiZGl2aWRlIGIyc3R5bGVfX3Jlc3VsdCBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQocmVzdWx0KTsNCn0NCg0KYmlndWludCBtb2QoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYmlndWludCByZXN1bHQ7DQogIGxvZ2ljICJkaXZpZGUgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmcgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmxvbmcgbW9kKGxvbmcgaSwgbG9uZyBqKSB7DQogIGxvbmcgcmVzdWx0Ow0KICBsb2dpYyAiZGl2aWRlIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKHJlc3VsdCk7DQp9DQoNCmludCBtb2QoaW50IGksIGludCBqKSB7DQogIGludCByZXN1bHQ7DQogIGxvZ2ljICJkaXZpZGUgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmcgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChyZXN1bHQpOw0KfQ0KDQpiaWd1aW50IHBvd2VyKGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGxvZ2ljICJwb3dlciBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIHBvd2VyKGxvbmcgaSwgbG9uZyBqKSB7DQogIGxvZ2ljICJwb3dlciBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcoaSk7DQp9DQoNCmludCBwb3dlcihpbnQgaSwgaW50IGopIHsNCiAgbG9naWMgInBvd2VyIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5faW50KGkpOw0KfQ0KDQpiaWd1aW50IGJpdF9hbmQoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgImFuZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGJpdF9hbmQobG9uZyBpLCBsb25nIGopIHsNCiAgbG9naWMgImFuZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQppbnQgYml0X2FuZChpbnQgaSwgaW50IGopIHsNCiAgbG9naWMgImFuZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpiaWd1aW50IGJpdF9vcihiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBsb2dpYyAib3IgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gaTsNCn0NCg0KbG9uZyBiaXRfb3IobG9uZyBpLCBsb25nIGopIHsNCiAgbG9naWMgIm9yIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmludCBiaXRfb3IoaW50IGksIGludCBqKSB7DQogIGxvZ2ljICJvciBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpiaWd1aW50IHNlbGZfaW5jX3Bvc3QoYmlndWludCYgeCkgew0KICBiaWd1aW50IHIgPSB4Ow0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiByOw0KfQ0KDQpsb25nIHNlbGZfaW5jX3Bvc3QobG9uZyYgeCkgew0KICBsb25nIHIgPSB4Ow0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcocik7DQp9DQoNCmludCBzZWxmX2luY19wb3N0KGludCYgeCkgew0KICBpbnQgciA9IHg7DQogIGxvZ2ljICJhZGQgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5faW50KHIpOw0KfQ0KDQpiaWd1aW50IHNlbGZfZGVjX3Bvc3QoYmlndWludCYgeCkgew0KICBiaWd1aW50IHIgPSB4Ow0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmxvbmcgc2VsZl9kZWNfcG9zdChsb25nJiB4KSB7DQogIGxvbmcgciA9IHg7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKHIpOw0KfQ0KDQppbnQgc2VsZl9kZWNfcG9zdChpbnQmIHgpIHsNCiAgaW50IHIgPSB4Ow0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5faW50KHIpOw0KfQ0KDQpiaWd1aW50IHNlbGZfaW5jX3ByZShiaWd1aW50JiB4KSB7DQogIGxvZ2ljICJhZGQgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIHg7DQp9DQoNCmxvbmcgc2VsZl9pbmNfcHJlKGxvbmcmIHgpIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKHgpOw0KfQ0KDQppbnQgc2VsZl9pbmNfcHJlKGludCYgeCkgew0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludCh4KTsNCn0NCg0KYmlndWludCBzZWxmX2RlY19wcmUoYmlndWludCYgeCkgew0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIHg7DQp9DQoNCmxvbmcgc2VsZl9kZWNfcHJlKGxvbmcmIHgpIHsNCiAgbG9naWMgInN1YnRyYWN0IGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcoeCk7DQp9DQoNCmludCBzZWxmX2RlY19wcmUoaW50JiB4KSB7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQoeCk7DQp9DQoNCmJpZ3VpbnQgZXh0cmFjdChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBiaWd1aW50IHI7DQogIGxvZ2ljICJleHRyYWN0IGIyc3R5bGVfX3IgQEBwcmVmaXhlc0B0ZW1wc0BiaWd1aW50IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByOw0KfQ0KDQpiaWd1aW50IGV4dHJhY3RfcmVtYWluZGVyKGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGJpZ3VpbnQgcjsNCiAgbG9naWMgImV4dHJhY3QgQEBwcmVmaXhlc0B0ZW1wc0BiaWd1aW50IGIyc3R5bGVfX3IgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmJpZ3VpbnQgbGVmdF9zaGlmdChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBiaWd1aW50IHI7DQogIGxvZ2ljICJsZWZ0X3NoaWZ0IGIyc3R5bGVfX3IgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmJpZ3VpbnQgcmlnaHRfc2hpZnQoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYmlndWludCByOw0KICBsb2dpYyAicmlnaHRfc2hpZnQgYjJzdHlsZV9fciBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcjsNCn0NCg0KYmlndWludCBsZWZ0X3NoaWZ0KGJpZ3VpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIGxlZnRfc2hpZnQoaSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpiaWd1aW50IHJpZ2h0X3NoaWZ0KGJpZ3VpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIHJpZ2h0X3NoaWZ0KGksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYmlndWludCBsZWZ0X3NoaWZ0KGJpZ3VpbnQgaSwgbG9uZyBqKSB7DQogIHJldHVybiBsZWZ0X3NoaWZ0KGksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYmlndWludCByaWdodF9zaGlmdChiaWd1aW50IGksIGxvbmcgaikgew0KICByZXR1cm4gcmlnaHRfc2hpZnQoaSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQppbnQgbGVmdF9zaGlmdChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIDo6YnN0eWxlOjp0b19pbnQoDQogICAgbGVmdF9zaGlmdCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwNCiAgICAgICAgICAgICAgIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKQ0KICApOw0KfQ0KDQppbnQgcmlnaHRfc2hpZnQoaW50IGksIGludCBqKSB7DQogIHJldHVybiA6OmJzdHlsZTo6dG9faW50KA0KICAgIHJpZ2h0X3NoaWZ0KDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLA0KICAgICAgICAgICAgICAgIDo6YnN0eWxlOjp0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ZmxvYXRfbW9yZSBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KYm9vbCBncmVhdGVyX29yX2VxdWFsKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICByZXR1cm4gb3IoZ3JlYXRlcl90aGFuKGksIGopLCBlcXVhbChpLCBqKSk7DQp9DQoNCmJvb2wgbGVzc190aGFuKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImZsb2F0X2xlc3MgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbCh1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgcmV0dXJuIG9yKGxlc3NfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQp1ZmxvYXQgYWRkKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICBsb2dpYyAiZmxvYXRfYWRkIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnVmbG9hdCBtaW51cyh1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgbG9naWMgImZsb2F0X3N1YnRyYWN0IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnVmbG9hdCBtdWx0aXBseSh1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgbG9naWMgImZsb2F0X211bHRpcGx5IGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnVmbG9hdCBkaXZpZGUodWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIHVmbG9hdCByZXN1bHQ7DQogIGxvZ2ljICJmbG9hdF9kaXZpZGUgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCnVmbG9hdCBwb3dlcih1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgbG9naWMgImZsb2F0X3Bvd2VyIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnN0cmluZyB1ZmxvYXRfdG9fc3RyKHVmbG9hdCBpKSB7DQogIDo6YnN0eWxlOjpsb2FkX21ldGhvZCgiYmlnX3VkZWNfdG9fc3RyIik7DQogIHN0cmluZyByZXN1bHQ7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kIGIyc3R5bGVfX2kgYjJzdHlsZV9fcmVzdWx0IjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0Kdm9pZCBzdGRfb3V0KHVmbG9hdCBpKSB7DQogIHN0ZF9vdXQodWZsb2F0X3RvX3N0cihpKSk7DQp9DQoNCnZvaWQgc3RkX2Vycih1ZmxvYXQgaSkgew0KICBzdGRfZXJyKHVmbG9hdF90b19zdHIoaSkpOw0KfQ0KDQp1ZmxvYXQgc2VsZl9pbmNfcG9zdCh1ZmxvYXQmIHgpIHsNCiAgdWZsb2F0IHIgPSB4Ow0KCXggPSBhZGQoeCwgMS4wKTsNCiAgcmV0dXJuIHI7DQp9DQoNCnVmbG9hdCBzZWxmX2RlY19wb3N0KHVmbG9hdCYgeCkgew0KICB1ZmxvYXQgciA9IHg7DQoJeCA9IG1pbnVzKHgsIDEuMCk7DQogIHJldHVybiByOw0KfQ0KDQp1ZmxvYXQgc2VsZl9pbmNfcHJlKHVmbG9hdCYgeCkgew0KCXggPSBhZGQoeCwgMS4wKTsNCiAgcmV0dXJuIHg7DQp9DQoNCnVmbG9hdCBzZWxmX2RlY19wcmUodWZsb2F0JiB4KSB7DQoJeCA9IG1pbnVzKHgsIDEuMCk7DQogIHJldHVybiB4Ow0KfQ0KDQp1ZmxvYXQgdWZsb2F0X19mcm9tKGJpZ3VpbnQgaSkgew0KICA6OmJzdHlsZTo6bG9hZF9tZXRob2QoImJpZ191aW50X3RvX2JpZ191ZGVjIik7DQogIHVmbG9hdCByZXN1bHQ7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kIGIyc3R5bGVfX2kgYjJzdHlsZV9fcmVzdWx0IjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KdWZsb2F0IHVmbG9hdF9fZnJvbShpbnQgaSkgew0KICByZXR1cm4gdWZsb2F0X19mcm9tKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpKTsNCn0NCg0KdWZsb2F0IHVmbG9hdF9fZnJvbShsb25nIGkpIHsNCiAgcmV0dXJuIHVmbG9hdF9fZnJvbSg6OmJzdHlsZTo6dG9fYmlndWludChpKSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGJpZ3VpbnQgbiwgYmlndWludCBkKSB7DQogIHVmbG9hdCByZXN1bHQgPSB1ZmxvYXRfX2Zyb20obik7DQogIHVmbG9hdCB1ZCA9IHVmbG9hdF9fZnJvbShkKTsNCiAgcmV0dXJuIGRpdmlkZShyZXN1bHQsIHVkKTsNCn0NCg0KdWZsb2F0IHVmbG9hdF9fZnJhY3Rpb24oYmlndWludCBuLCBpbnQgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbihuLCA6OmJzdHlsZTo6dG9fYmlndWludChkKSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGJpZ3VpbnQgbiwgbG9uZyBkKSB7DQogIHJldHVybiB1ZmxvYXRfX2ZyYWN0aW9uKG4sIDo6YnN0eWxlOjp0b19iaWd1aW50KGQpKTsNCn0NCg0KdWZsb2F0IHVmbG9hdF9fZnJhY3Rpb24oaW50IG4sIGJpZ3VpbnQgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbig6OmJzdHlsZTo6dG9fYmlndWludChuKSwgZCk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGxvbmcgbiwgYmlndWludCBkKSB7DQogIHJldHVybiB1ZmxvYXRfX2ZyYWN0aW9uKDo6YnN0eWxlOjp0b19iaWd1aW50KG4pLCBkKTsNCn0NCg0KdWZsb2F0IHVmbG9hdF9fZnJhY3Rpb24oaW50IG4sIGludCBkKSB7DQogIHJldHVybiB1ZmxvYXRfX2ZyYWN0aW9uKDo6YnN0eWxlOjp0b19iaWd1aW50KG4pLCA6OmJzdHlsZTo6dG9fYmlndWludChkKSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGxvbmcgbiwgbG9uZyBkKSB7DQogIHJldHVybiB1ZmxvYXRfX2ZyYWN0aW9uKDo6YnN0eWxlOjp0b19iaWd1aW50KG4pLCA6OmJzdHlsZTo6dG9fYmlndWludChkKSk7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX0IyU1RZTEVfVUZMT0FUX0gPAAAAYjJzdHlsZV9zdGRpby5osgYAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX1NURElPX0gNCiNkZWZpbmUgQjJTVFlMRV9MSUJfQjJTVFlMRV9TVERJT19IDQoNCiNpbmNsdWRlIDxic3R5bGUuaD4NCiNpbmNsdWRlIDxiMnN0eWxlX3R5cGVzLmg+DQojaW5jbHVkZSA8YjJzdHlsZV9vcGVyYXRvcnMuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQp2b2lkIHN0ZF9vdXQoc3RyaW5nIGkpIHsNCiAgbG9naWMgImludGVycnVwdCBzdGRvdXQgYjJzdHlsZV9faSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyI7DQp9DQoNCnZvaWQgc3RkX2VycihzdHJpbmcgaSkgew0KICBsb2dpYyAiaW50ZXJydXB0IHN0ZGVyciBiMnN0eWxlX19pIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIjsNCn0NCg0Kdm9pZCBzdGRfb3V0KGJvb2wgaSkgew0KICBpZiAoaSkgew0KICAgIHN0ZF9vdXQoIlRydWUiKTsNCiAgfQ0KICBlbHNlIHsNCiAgICBzdGRfb3V0KCJGYWxzZSIpOw0KICB9DQp9DQoNCnZvaWQgc3RkX2Vycihib29sIGkpIHsNCiAgaWYgKGkpIHsNCiAgICBzdGRfZXJyKCJUcnVlIik7DQogIH0NCiAgZWxzZSB7DQogICAgc3RkX2VycigiRmFsc2UiKTsNCiAgfQ0KfQ0KDQpzdHJpbmcgbGVnYWN5X2JpZ3VpbnRfdG9fc3RyKGJpZ3VpbnQgaSkgew0KICBpZiAoZXF1YWwoaSwgMEwpKSB7DQogICAgcmV0dXJuICIwIjsNCiAgfQ0KICBzdHJpbmcgczsNCiAgd2hpbGUgKGdyZWF0ZXJfdGhhbihpLCAwTCkpIHsNCiAgICBpbnQgYiA9IDo6YnN0eWxlOjp0b19pbnQobW9kKGksIDEwTCkpOw0KICAgIGkgPSBkaXZpZGUoaSwgMTBMKTsNCiAgICBiID0gYWRkKGIsIDQ4KTsNCiAgICBzID0gOjpic3R5bGU6OnN0cl9jb25jYXQoOjpic3R5bGU6OnRvX3N0cig6OmJzdHlsZTo6dG9fYnl0ZShiKSksIHMpOw0KICB9DQogIHJldHVybiBzOw0KfQ0KDQpzdHJpbmcgYmlndWludF90b19zdHIoYmlndWludCBpKSB7DQogIDo6YnN0eWxlOjpsb2FkX21ldGhvZCgiYmlnX3VpbnRfdG9fc3RyIik7DQogIHN0cmluZyByZXN1bHQ7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kIGIyc3R5bGVfX2kgYjJzdHlsZV9fcmVzdWx0IjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0Kc3RyaW5nIGludF90b19zdHIoaW50IGkpIHsNCiAgaWYgKGdyZWF0ZXJfdGhhbihpLCAyMTQ3NDgzNjQ3KSkgew0KICAgIGkgPSBtaW51cyhpLCAyMTQ3NDgzNjQ3KTsNCiAgICBpID0gbWludXMoMjE0NzQ4MzY0NywgaSk7DQogICAgaSA9IGFkZChpLCAyKTsNCiAgICBzdHJpbmcgcyA9ICItIjsNCiAgICByZXR1cm4gOjpic3R5bGU6OnN0cl9jb25jYXQocywgYmlndWludF90b19zdHIoOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSkpKTsNCiAgfQ0KICByZXR1cm4gYmlndWludF90b19zdHIoOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSkpOw0KfQ0KDQp2b2lkIHN0ZF9vdXQoYmlndWludCBpKSB7DQogIHN0ZF9vdXQoYmlndWludF90b19zdHIoaSkpOw0KfQ0KDQp2b2lkIHN0ZF9lcnIoYmlndWludCBpKSB7DQogIHN0ZF9lcnIoYmlndWludF90b19zdHIoaSkpOw0KfQ0KDQp2b2lkIHN0ZF9vdXQoaW50IGkpIHsNCiAgc3RkX291dChpbnRfdG9fc3RyKGkpKTsNCn0NCg0Kdm9pZCBzdGRfZXJyKGludCBpKSB7DQogIHN0ZF9lcnIoaW50X3RvX3N0cihpKSk7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfQjJTVFlMRV9TVERJT19IDQoJAAAAdGVzdGluZy5ofgEAAA0KI2lmbmRlZiBURVNUSU5HX1RFU1RJTkdfSA0KI2RlZmluZSBURVNUSU5HX1RFU1RJTkdfSA0KDQojaW5jbHVkZSA8dGVzdGluZy90eXBlcy5oPg0KI2luY2x1ZGUgPHRlc3RpbmcvYXNzZXJ0Lmg+DQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCm5hbWVzcGFjZSB0ZXN0aW5nIHsNCg0Kdm9pZCBmaW5pc2hlZCgpIHsNCiAgOjpiMnN0eWxlOjpzdGRfb3V0KCJUb3RhbCBhc3NlcnRpb25zOiAiKTsNCiAgOjpiMnN0eWxlOjpzdGRfb3V0KF9hc3NlcnRpb25fY291bnQpOw0KICA6OmIyc3R5bGU6OnN0ZF9vdXQoIlxuIik7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSB0ZXN0aW5nDQp9ICAvLyBuYW1lc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIFRFU1RJTkdfVEVTVElOR19IDQoPAAAAdGVzdGluZy90eXBlcy5oEAEAAA0KI2lmbmRlZiBURVNUSU5HX1RZUEVTX0gNCiNkZWZpbmUgVEVTVElOR19UWVBFU19IDQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCm5hbWVzcGFjZSB0ZXN0aW5nIHsNCg0KdHlwZWRlZiA6OnN0cmluZyBzdHJpbmc7DQp0eXBlZGVmIDo6dm9pZCB2b2lkOw0KdHlwZWRlZiA6OmJvb2wgYm9vbDsNCnR5cGVkZWYgOjppbnQgaW50Ow0KDQoNCn0gIC8vIG5hbWVzcGFjZSB0ZXN0aW5nDQp9ICAvLyBuYWVtc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIFRFU1RJTkdfVFlQRVNfSA0KEAAAAHRlc3RpbmcvYXNzZXJ0Lmi1AwAA77u/DQojaWZuZGVmIFRFU1RJTkdfQVNTRVJUX0gNCiNkZWZpbmUgVEVTVElOR19BU1NFUlRfSA0KDQojaW5jbHVkZSA8YnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy90eXBlcy5oPg0KDQpuYW1lc3BhY2UgYjJzdHlsZSB7DQpuYW1lc3BhY2UgdGVzdGluZyB7DQoNCmludCBfYXNzZXJ0aW9uX2NvdW50ID0gMDsNCg0Kdm9pZCBhc3NlcnRfdHJ1ZShib29sIHYsIHN0cmluZyBtc2cpIHsNCiAgX2Fzc2VydGlvbl9jb3VudCsrOw0KICBzdHJpbmcgcHJlZml4Ow0KICBpZiAodikgew0KICAgIHByZWZpeCA9ICJTdWNjZXNzOiAiOw0KICB9IGVsc2Ugew0KICAgIHByZWZpeCA9ICJGYWlsdXJlOiAiOw0KICB9DQogIDo6YjJzdHlsZTo6c3RkX291dChwcmVmaXgpOw0KICA6OmIyc3R5bGU6OnN0ZF9vdXQobXNnKTsNCiAgOjpiMnN0eWxlOjpzdGRfb3V0KCJcbiIpOw0KfQ0KDQp2b2lkIGFzc2VydF90cnVlKGJvb2wgdikgew0KICBhc3NlcnRfdHJ1ZSh2LCAibm8gZXh0cmEgaW5mb3JtYXRpb24uIik7DQp9DQoNCi8qDQpUT0RPOiBNYWtlIHRoaXMgd29yay4gQ3VycmVudGx5IGFzc2VydF9lcXVhbF9fMiBjb25mbGljdHMgd2l0aCB0aGUgZm9sbG93aW5nIG9uZS4NCnRlbXBsYXRlIDxULCBUMj4NCnZvaWQgYXNzZXJ0X2VxdWFsKFQgdCwgVDIgdDIsIHN0cmluZyBtc2cpIHsNCiAgYXNzZXJ0X3RydWUodCA9PSB0MiwgbXNnKTsNCn0NCiovDQoNCnRlbXBsYXRlIDxULCBUMj4NCnZvaWQgYXNzZXJ0X2VxdWFsKFQgdCwgVDIgdDIpIHsNCiAgYXNzZXJ0X3RydWUodCA9PSB0Mik7DQp9DQoNCnRlbXBsYXRlIDxUPg0Kdm9pZCBhc3NlcnRfZXF1YWwoVCB0LCBUIHQyKSB7DQogIGFzc2VydF9lcXVhbDxULCBUPih0LCB0Mik7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSB0ZXN0aW5nDQp9ICAvLyBuYW1lc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIFRFU1RJTkdfQVNTRVJUX0gNCg=="
        ))
End Class
