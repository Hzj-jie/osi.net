Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_lib
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(24406), 
            "CAAAAGFzc2VydC5oXgIAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9BU1NFUlRfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9BU1NFUlRfSA0KDQojaW5jbHVkZSA8YjJzdHlsZS9zdGRpby5oPg0KI2luY2x1ZGUgPGIyc3R5bGUvdHlwZXMuaD4NCiNpbmNsdWRlIDxic3R5bGUvc3RyLmg+DQoNCnZvaWQgYXNzZXJ0KHN0cmluZyBzdGF0ZW1lbnQsIGJvb2wgdiwgc3RyaW5nIG1zZykgew0KICBpZiAodikgcmV0dXJuOw0KICBpZiAoYnN0eWxlOjpzdHJfZW1wdHkoc3RhdGVtZW50KSkgew0KICAgIGIyc3R5bGU6OnN0ZF9vdXQobXNnKTsNCiAgfSBlbHNlIHsNCiAgICBiMnN0eWxlOjpzdGRfb3V0KHN0YXRlbWVudCArICI6ICIgKyBtc2cpOw0KICB9DQogIGxvZ2ljICJzdG9wIjsNCn0NCg0Kdm9pZCBhc3NlcnQoYm9vbCB2LCBzdHJpbmcgbXNnKSB7DQogIGFzc2VydCgiIiwgdiwgbXNnKTsNCn0NCg0Kdm9pZCBhc3NlcnQoc3RyaW5nIHN0YXRlbWVudCwgYm9vbCB2KSB7DQogIGFzc2VydChzdGF0ZW1lbnQsIHYsICJBc3NlcnRpb24gZmFpbHVyZSIpOw0KfQ0KDQp2b2lkIGFzc2VydChib29sIHYpIHsNCiAgYXNzZXJ0KCIiLCB2KTsNCn0NCg0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9BU1NFUlRfSAkAAABiMnN0eWxlLmi7AAAA77u/DQojaWZuZGVmIEIyU1RZTEVfTElCX0IyU1RZTEVfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX0gNCg0KI2luY2x1ZGUgPGJzdHlsZS5oPg0KDQojaW5jbHVkZSA8YjJzdHlsZS9vcGVyYXRvcnMuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL3VmbG9hdC5oPg0KDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX0IyU1RZTEVfSAcAAABydW4uY21kawAAAA0KZGVsIC9zICoudW5+DQouLlwuLlwuLlxyZXNvdXJjZVxnZW5cdGFyX2dlblxvc2kucm9vdC51dHQgdGFyX2dlbiAtLW91dHB1dD1iMnN0eWxlX2xpYg0KbW92ZSAvWSAqLnZiIC4uXA0KCQAAAHRlc3RpbmcuaJoBAAANCiNpZm5kZWYgVEVTVElOR19URVNUSU5HX0gNCiNkZWZpbmUgVEVTVElOR19URVNUSU5HX0gNCg0KI2luY2x1ZGUgPGIyc3R5bGUvc3RkaW8uaD4NCiNpbmNsdWRlIDx0ZXN0aW5nL3R5cGVzLmg+DQojaW5jbHVkZSA8dGVzdGluZy9hc3NlcnQuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KbmFtZXNwYWNlIHRlc3Rpbmcgew0KDQp2b2lkIGZpbmlzaGVkKCkgew0KICA6OmIyc3R5bGU6OnN0ZF9vdXQoIlRvdGFsIGFzc2VydGlvbnM6ICIpOw0KICA6OmIyc3R5bGU6OnN0ZF9vdXQoX2Fzc2VydGlvbl9jb3VudCk7DQogIDo6YjJzdHlsZTo6c3RkX291dCgiXG4iKTsNCn0NCg0KfSAgLy8gbmFtZXNwYWNlIHRlc3RpbmcNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gVEVTVElOR19URVNUSU5HX0gNChMAAABiMnN0eWxlL2RlbGVnYXRlcy5o1QcAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX0RFTEVHQVRFU19IDQojZGVmaW5lIEIyU1RZTEVfTElCX0IyU1RZTEVfREVMRUdBVEVTX0gNCg0KI2luY2x1ZGUgPGIyc3R5bGUvdHlwZXMuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQp0ZW1wbGF0ZSA8UlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbigpOw0KDQp0ZW1wbGF0ZSA8VCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihUKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyLCBUMywgVDQsIFQ1KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBSVD4NCmRlbGVnYXRlIFJUIGZ1bmN0aW9uKFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDkpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTApOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExKTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgVDEwLCBUMTEsIFQxMiwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIsIFQxMywgVDE0KTsNCg0KdGVtcGxhdGUgPFQsIFQyLCBUMywgVDQsIFQ1LCBUNiwgVDcsIFQ4LCBUOSwgVDEwLCBUMTEsIFQxMiwgVDEzLCBUMTQsIFQxNSwgUlQ+DQpkZWxlZ2F0ZSBSVCBmdW5jdGlvbihULCBUMiwgVDMsIFQ0LCBUNSwgVDYsIFQ3LCBUOCwgVDksIFQxMCwgVDExLCBUMTIsIFQxMywgVDE0LCBUMTUpOw0KDQp0ZW1wbGF0ZSA8VCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgVDE1LCBUMTYsIFJUPg0KZGVsZWdhdGUgUlQgZnVuY3Rpb24oVCwgVDIsIFQzLCBUNCwgVDUsIFQ2LCBUNywgVDgsIFQ5LCBUMTAsIFQxMSwgVDEyLCBUMTMsIFQxNCwgVDE1LCBUMTYpOw0KDQp9ICAvLyBuYW1lc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX0IyU1RZTEVfREVMRUdBVEVTX0gSAAAAYjJzdHlsZS9oZWFwX3B0ci5oZAQAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX0hFQVBfUFRSX0gNCiNkZWZpbmUgQjJTVFlMRV9MSUJfQjJTVFlMRV9IRUFQX1BUUl9IDQoNCiNpbmNsdWRlIDxiMnN0eWxlL3R5cGVzLmg+DQojaW5jbHVkZSA8YXNzZXJ0Lmg+DQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCg0KLy8gVHlwZSBUIGlzIHJlcXVpcmVkIHRvIHVzZSB0aGUgZGVhbGxvYyBpbnN0cnVjdGlvbi4NCnRlbXBsYXRlIDxUPg0KY2xhc3MgaGVhcF9wdHIgew0KICBUIF9hOw0KICBpbnQgX3M7DQoNCiAgaW50IHNpemUoKSB7DQogICAgcmV0dXJuIHRoaXMuX3M7DQogIH0NCg0KICBib29sIGVtcHR5KCkgew0KICAgIHJldHVybiB0aGlzLnNpemUoKSA9PSAwOw0KICB9DQoNCiAgdm9pZCBjb25zdHJ1Y3QoKSB7DQogICAgdGhpcy5fcyA9IDA7DQogIH0NCg0KICB2b2lkIGRlc3RydWN0KCkgew0KICAgIGlmICh0aGlzLmVtcHR5KCkpIHJldHVybjsNCiAgICBkZWFsbG9jKHRoaXMuX2EpOw0KICAgIHRoaXMuY29uc3RydWN0KCk7DQogIH0NCg0KICB2b2lkIGFsbG9jKGludCBzaXplKSB7DQogICAgdGhpcy5kZXN0cnVjdCgpOw0KICAgIDo6YXNzZXJ0KHNpemUgPiAwKTsNCiAgICBUIHhbc2l6ZV07DQogICAgdGhpcy5fYSA9IHg7DQogICAgdGhpcy5fcyA9IHNpemU7DQogICAgdW5kZWZpbmUoeCk7DQogIH0NCg0KICB2b2lkIGNvbnN0cnVjdChpbnQgc2l6ZSkgew0KICAgIHRoaXMuY29uc3RydWN0KCk7DQogICAgdGhpcy5hbGxvYyhzaXplKTsNCiAgfQ0KDQogIFQgZ2V0KGludCBpbmRleCkgew0KICAgIDo6YXNzZXJ0KGluZGV4ID49IDApOw0KICAgIDo6YXNzZXJ0KGluZGV4IDwgdGhpcy5zaXplKCkpOw0KICAgIHJldHVybiB0aGlzLl9hW2luZGV4XTsNCiAgfQ0KDQogIHZvaWQgc2V0KGludCBpbmRleCwgVCB2KSB7DQogICAgOjphc3NlcnQoaW5kZXggPj0gMCk7DQogICAgOjphc3NlcnQoaW5kZXggPCB0aGlzLnNpemUoKSk7DQogICAgdGhpcy5fYVtpbmRleF0gPSB2Ow0KICB9DQoNCiAgdm9pZCByZWxlYXNlKCkgew0KICAgIHRoaXMuY29uc3RydWN0KCk7DQogIH0NCn07DQoNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfQjJTVFlMRV9IRUFQX1BUUl9IFwAAAGIyc3R5bGUvbG9hZGVkX21ldGhvZC5ogwIAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX0xPQURFRF9NRVRIT0RfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX0xPQURFRF9NRVRIT0RfSA0KDQojaW5jbHVkZSA8YjJzdHlsZS90eXBlcy5oPg0KI2luY2x1ZGUgPGJzdHlsZS9sb2FkX21ldGhvZC5oPg0KDQpuYW1lc3BhY2UgYjJzdHlsZSB7DQoNCnZvaWQgbG9hZF9tZXRob2Qoc3RyaW5nIG0pIHsNCiAgOjpic3R5bGU6OmxvYWRfbWV0aG9kKG0pOw0KfQ0KDQp0ZW1wbGF0ZSA8VD4NClQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kKCkgew0KICBUIHJlc3VsdDsNCiAgbG9naWMgImludGVycnVwdCBleGVjdXRlX2xvYWRlZF9tZXRob2QgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmcgYjJzdHlsZV9fcmVzdWx0IjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KdGVtcGxhdGUgPFQsIFJUPg0KUlQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kKFQgcCkgew0KICBSVCByZXN1bHQ7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZXhlY3V0ZV9sb2FkZWRfbWV0aG9kIGIyc3R5bGVfX3AgYjJzdHlsZV9fcmVzdWx0IjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCgkNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfQjJTVFlMRV9MT0FERURfTUVUSE9EX0gTAAAAYjJzdHlsZS9vcGVyYXRvcnMuaEglAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfQjJTVFlMRV9PUEVSQVRPUlNfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX09QRVJBVE9SU19IDQoNCiNpbmNsdWRlIDxiMnN0eWxlL3R5cGVzLmg+DQojaW5jbHVkZSA8YnN0eWxlL2ludC5oPg0KI2luY2x1ZGUgPGJzdHlsZS9zdHIuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQpib29sIGFuZChib29sIGksIGJvb2wgaikgew0KICBpZiAoaSkgcmV0dXJuIGo7DQogIHJldHVybiBmYWxzZTsNCn0NCg0KYm9vbCBvcihib29sIGksIGJvb2wgaikgew0KICBpZiAoaSkgcmV0dXJuIHRydWU7DQogIGlmIChqKSByZXR1cm4gdHJ1ZTsNCiAgcmV0dXJuIGZhbHNlOw0KfQ0KDQpib29sIG5vdChib29sIGkpIHsNCiAgaWYgKGkpIHJldHVybiBmYWxzZTsNCiAgcmV0dXJuIHRydWU7DQp9DQoNCi8vIFRPRE86IENvbnNpZGVyIHRvIGF2b2lkIGFkZGluZyAiX3ByZSIgZm9yIG9wZXJhdG9yICEuDQpib29sIG5vdF9wcmUoYm9vbCBpKSB7DQogIHJldHVybiBub3QoaSk7DQp9DQoNCi8vIFRPRE86IFVzZSBhIGJldHRlciB3YXkgdG8gY29tcGFyZSBzdHJpbmdzLCB0cmVhdGluZyB0aGVtIGFzIGJpZ191aW50cyBpcyBub3QgYWNjdXJhdGUgb3IgZWZmaWNpZW50Lg0KYm9vbCBlcXVhbChzdHJpbmcgaSwgc3RyaW5nIGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJlcXVhbCBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KYm9vbCBub3RfZXF1YWwoc3RyaW5nIGksIHN0cmluZyBqKSB7DQogIGJvb2wgcmVzdWx0Ow0KICBsb2dpYyAiZXF1YWwgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBub3QocmVzdWx0KTsNCn0NCg0KYm9vbCBlcXVhbChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImVxdWFsIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQpib29sIG5vdF9lcXVhbChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImVxdWFsIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gbm90KHJlc3VsdCk7DQp9DQoNCmJvb2wgZXF1YWwobG9uZyBpLCBsb25nIGopIHsNCiAgcmV0dXJuIGVxdWFsKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbm90X2VxdWFsKGxvbmcgaSwgbG9uZyBqKSB7DQogIHJldHVybiBub3RfZXF1YWwoOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYm9vbCBlcXVhbChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIGVxdWFsKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbm90X2VxdWFsKGludCBpLCBpbnQgaikgew0KICByZXR1cm4gbm90X2VxdWFsKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgZXF1YWwoYm9vbCBpLCBib29sIGopIHsNCiAgaWYgKGkpIHJldHVybiBqOw0KICByZXR1cm4gIWo7DQp9DQoNCmJvb2wgbm90X2VxdWFsKGJvb2wgaSwgYm9vbCBqKSB7DQogIGlmIChpKSByZXR1cm4gIWo7DQogIHJldHVybiBqOw0KfQ0KDQpib29sIGdyZWF0ZXJfdGhhbihiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgIm1vcmUgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgZ3JlYXRlcl90aGFuKGxvbmcgaSwgbG9uZyBqKSB7DQogIHJldHVybiBncmVhdGVyX3RoYW4oOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYm9vbCBncmVhdGVyX3RoYW4oaW50IGksIGludCBqKSB7DQogIHJldHVybiBncmVhdGVyX3RoYW4oOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKTsNCn0NCg0KYm9vbCBsZXNzX3RoYW4oYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJsZXNzIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQpib29sIGxlc3NfdGhhbihsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gbGVzc190aGFuKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbGVzc190aGFuKGludCBpLCBpbnQgaikgew0KICByZXR1cm4gbGVzc190aGFuKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICByZXR1cm4gb3IobGVzc190aGFuKGksIGopLCBlcXVhbChpLCBqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gb3IobGVzc190aGFuKGksIGopLCBlcXVhbChpLCBqKSk7DQp9DQoNCmJvb2wgbGVzc19vcl9lcXVhbChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIG9yKGxlc3NfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgcmV0dXJuIG9yKGdyZWF0ZXJfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwobG9uZyBpLCBsb25nIGopIHsNCiAgcmV0dXJuIG9yKGdyZWF0ZXJfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGdyZWF0ZXJfb3JfZXF1YWwoaW50IGksIGludCBqKSB7DQogIHJldHVybiBvcihncmVhdGVyX3RoYW4oaSwgaiksIGVxdWFsKGksIGopKTsNCn0NCg0KYmlndWludCBhZGQoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGFkZChsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhpKTsNCn0NCg0KaW50IGFkZChpbnQgaSwgaW50IGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChpKTsNCn0NCg0KYnl0ZSBhZGQoYnl0ZSBpLCBieXRlIGopIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2J5dGUoaSk7DQp9DQoNCnN0cmluZyBhZGQoc3RyaW5nIGksIHN0cmluZyBqKSB7DQogIHJldHVybiA6OmJzdHlsZTo6c3RyX2NvbmNhdChpLCBqKTsNCn0NCg0KYmlndWludCBtaW51cyhiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gaTsNCn0NCg0KbG9uZyBtaW51cyhsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKGkpOw0KfQ0KDQppbnQgbWludXMoaW50IGksIGludCBqKSB7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChpKTsNCn0NCg0KYmlndWludCBtdWx0aXBseShiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBsb2dpYyAibXVsdGlwbHkgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gaTsNCn0NCg0KbG9uZyBtdWx0aXBseShsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAibXVsdGlwbHkgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKGkpOw0KfQ0KDQppbnQgbXVsdGlwbHkoaW50IGksIGludCBqKSB7DQogIGxvZ2ljICJtdWx0aXBseSBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChpKTsNCn0NCg0KYmlndWludCBkaXZpZGUoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYmlndWludCByZXN1bHQ7DQogIGxvZ2ljICJkaXZpZGUgYjJzdHlsZV9fcmVzdWx0IEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmxvbmcgZGl2aWRlKGxvbmcgaSwgbG9uZyBqKSB7DQogIGxvbmcgcmVzdWx0Ow0KICBsb2dpYyAiZGl2aWRlIGIyc3R5bGVfX3Jlc3VsdCBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9sb25nKHJlc3VsdCk7DQp9DQoNCmludCBkaXZpZGUoaW50IGksIGludCBqKSB7DQogIGludCByZXN1bHQ7DQogIGxvZ2ljICJkaXZpZGUgYjJzdHlsZV9fcmVzdWx0IEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludChyZXN1bHQpOw0KfQ0KDQpiaWd1aW50IG1vZChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBiaWd1aW50IHJlc3VsdDsNCiAgbG9naWMgImRpdmlkZSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KbG9uZyBtb2QobG9uZyBpLCBsb25nIGopIHsNCiAgbG9uZyByZXN1bHQ7DQogIGxvZ2ljICJkaXZpZGUgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmcgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcocmVzdWx0KTsNCn0NCg0KaW50IG1vZChpbnQgaSwgaW50IGopIHsNCiAgaW50IHJlc3VsdDsNCiAgbG9naWMgImRpdmlkZSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBiMnN0eWxlX19yZXN1bHQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5faW50KHJlc3VsdCk7DQp9DQoNCmJpZ3VpbnQgcG93ZXIoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgbG9naWMgInBvd2VyIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmxvbmcgcG93ZXIobG9uZyBpLCBsb25nIGopIHsNCiAgbG9naWMgInBvd2VyIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhpKTsNCn0NCg0KaW50IHBvd2VyKGludCBpLCBpbnQgaikgew0KICBsb2dpYyAicG93ZXIgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQoaSk7DQp9DQoNCmJpZ3VpbnQgYml0X2FuZChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBsb2dpYyAiYW5kIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmxvbmcgYml0X2FuZChsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAiYW5kIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmludCBiaXRfYW5kKGludCBpLCBpbnQgaikgew0KICBsb2dpYyAiYW5kIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmJpZ3VpbnQgYml0X29yKGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGxvZ2ljICJvciBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGJpdF9vcihsb25nIGksIGxvbmcgaikgew0KICBsb2dpYyAib3IgYjJzdHlsZV9faSBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gaTsNCn0NCg0KaW50IGJpdF9vcihpbnQgaSwgaW50IGopIHsNCiAgbG9naWMgIm9yIGIyc3R5bGVfX2kgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmJpZ3VpbnQgc2VsZl9pbmNfcG9zdChiaWd1aW50JiB4KSB7DQogIGJpZ3VpbnQgciA9IHg7DQogIGxvZ2ljICJhZGQgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmxvbmcgc2VsZl9pbmNfcG9zdChsb25nJiB4KSB7DQogIGxvbmcgciA9IHg7DQogIGxvZ2ljICJhZGQgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyhyKTsNCn0NCg0KaW50IHNlbGZfaW5jX3Bvc3QoaW50JiB4KSB7DQogIGludCByID0geDsNCiAgbG9naWMgImFkZCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQocik7DQp9DQoNCmJpZ3VpbnQgc2VsZl9kZWNfcG9zdChiaWd1aW50JiB4KSB7DQogIGJpZ3VpbnQgciA9IHg7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gcjsNCn0NCg0KbG9uZyBzZWxmX2RlY19wb3N0KGxvbmcmIHgpIHsNCiAgbG9uZyByID0geDsNCiAgbG9naWMgInN1YnRyYWN0IGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcocik7DQp9DQoNCmludCBzZWxmX2RlY19wb3N0KGludCYgeCkgew0KICBpbnQgciA9IHg7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4gOjpic3R5bGU6OmZpdF9pbl9pbnQocik7DQp9DQoNCmJpZ3VpbnQgc2VsZl9pbmNfcHJlKGJpZ3VpbnQmIHgpIHsNCiAgbG9naWMgImFkZCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4geDsNCn0NCg0KbG9uZyBzZWxmX2luY19wcmUobG9uZyYgeCkgew0KICBsb2dpYyAiYWRkIGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2xvbmcoeCk7DQp9DQoNCmludCBzZWxmX2luY19wcmUoaW50JiB4KSB7DQogIGxvZ2ljICJhZGQgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5faW50KHgpOw0KfQ0KDQpiaWd1aW50IHNlbGZfZGVjX3ByZShiaWd1aW50JiB4KSB7DQogIGxvZ2ljICJzdWJ0cmFjdCBiMnN0eWxlX194IGIyc3R5bGVfX3ggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEiOw0KICByZXR1cm4geDsNCn0NCg0KbG9uZyBzZWxmX2RlY19wcmUobG9uZyYgeCkgew0KICBsb2dpYyAic3VidHJhY3QgYjJzdHlsZV9feCBiMnN0eWxlX194IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIjsNCiAgcmV0dXJuIDo6YnN0eWxlOjpmaXRfaW5fbG9uZyh4KTsNCn0NCg0KaW50IHNlbGZfZGVjX3ByZShpbnQmIHgpIHsNCiAgbG9naWMgInN1YnRyYWN0IGIyc3R5bGVfX3ggYjJzdHlsZV9feCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMSI7DQogIHJldHVybiA6OmJzdHlsZTo6Zml0X2luX2ludCh4KTsNCn0NCg0KYmlndWludCBleHRyYWN0KGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGJpZ3VpbnQgcjsNCiAgbG9naWMgImV4dHJhY3QgYjJzdHlsZV9fciBAQHByZWZpeGVzQHRlbXBzQGJpZ3VpbnQgYjJzdHlsZV9faSBiMnN0eWxlX19qIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmJpZ3VpbnQgZXh0cmFjdF9yZW1haW5kZXIoYmlndWludCBpLCBiaWd1aW50IGopIHsNCiAgYmlndWludCByOw0KICBsb2dpYyAiZXh0cmFjdCBAQHByZWZpeGVzQHRlbXBzQGJpZ3VpbnQgYjJzdHlsZV9fciBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcjsNCn0NCg0KYmlndWludCBsZWZ0X3NoaWZ0KGJpZ3VpbnQgaSwgYmlndWludCBqKSB7DQogIGJpZ3VpbnQgcjsNCiAgbG9naWMgImxlZnRfc2hpZnQgYjJzdHlsZV9fciBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcjsNCn0NCg0KYmlndWludCByaWdodF9zaGlmdChiaWd1aW50IGksIGJpZ3VpbnQgaikgew0KICBiaWd1aW50IHI7DQogIGxvZ2ljICJyaWdodF9zaGlmdCBiMnN0eWxlX19yIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByOw0KfQ0KDQpiaWd1aW50IGxlZnRfc2hpZnQoYmlndWludCBpLCBpbnQgaikgew0KICByZXR1cm4gbGVmdF9zaGlmdChpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmJpZ3VpbnQgcmlnaHRfc2hpZnQoYmlndWludCBpLCBpbnQgaikgew0KICByZXR1cm4gcmlnaHRfc2hpZnQoaSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpiaWd1aW50IGxlZnRfc2hpZnQoYmlndWludCBpLCBsb25nIGopIHsNCiAgcmV0dXJuIGxlZnRfc2hpZnQoaSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpOw0KfQ0KDQpiaWd1aW50IHJpZ2h0X3NoaWZ0KGJpZ3VpbnQgaSwgbG9uZyBqKSB7DQogIHJldHVybiByaWdodF9zaGlmdChpLCA6OmJzdHlsZTo6dG9fYmlndWludChqKSk7DQp9DQoNCmludCBsZWZ0X3NoaWZ0KGludCBpLCBpbnQgaikgew0KICByZXR1cm4gOjpic3R5bGU6OnRvX2ludCgNCiAgICBsZWZ0X3NoaWZ0KDo6YnN0eWxlOjp0b19iaWd1aW50KGkpLA0KICAgICAgICAgICAgICAgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpDQogICk7DQp9DQoNCmludCByaWdodF9zaGlmdChpbnQgaSwgaW50IGopIHsNCiAgcmV0dXJuIDo6YnN0eWxlOjp0b19pbnQoDQogICAgcmlnaHRfc2hpZnQoOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSksDQogICAgICAgICAgICAgICAgOjpic3R5bGU6OnRvX2JpZ3VpbnQoaikpDQogICk7DQp9DQoNCmxvbmcgbGVmdF9zaGlmdChsb25nIGksIGxvbmcgaikgew0KICByZXR1cm4gOjpic3R5bGU6OnRvX2xvbmcoDQogICAgbGVmdF9zaGlmdCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwNCiAgICAgICAgICAgICAgIDo6YnN0eWxlOjp0b19iaWd1aW50KGopKQ0KICApOw0KfQ0KDQpsb25nIHJpZ2h0X3NoaWZ0KGxvbmcgaSwgbG9uZyBqKSB7DQogIHJldHVybiA6OmJzdHlsZTo6dG9fbG9uZygNCiAgICByaWdodF9zaGlmdCg6OmJzdHlsZTo6dG9fYmlndWludChpKSwNCiAgICAgICAgICAgICAgICA6OmJzdHlsZTo6dG9fYmlndWludChqKSkNCiAgKTsNCn0NCg0KfSAgLy8gbmFtZXNwYWNlIGIyc3R5bGUNCg0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9CMlNUWUxFX09QRVJBVE9SU19IDQAAAGIyc3R5bGUvcmVmLmhxAwAA77u/DQojaWZuZGVmIEIyU1RZTEVfTElCX0IyU1RZTEVfUkVGX0gNCiNkZWZpbmUgQjJTVFlMRV9MSUJfQjJTVFlMRV9SRUZfSA0KDQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPGIyc3R5bGUvaGVhcF9wdHIuaD4NCiNpbmNsdWRlIDxhc3NlcnQuaD4NCg0KbmFtZXNwYWNlIGIyc3R5bGUgew0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIHJlZiB7DQogIGhlYXBfcHRyPFQ+IF9hOw0KDQogIHZvaWQgZGVzdHJ1Y3QoKSB7DQogICAgdGhpcy5fYS5kZXN0cnVjdCgpOw0KICB9DQoNCiAgYm9vbCBlbXB0eSgpIHsNCiAgICByZXR1cm4gdGhpcy5fYS5lbXB0eSgpOw0KICB9DQoNCiAgdm9pZCBzZXQoVCB2KSB7DQogICAgaWYgKHRoaXMuZW1wdHkoKSkgdGhpcy5fYS5hbGxvYygxKTsNCiAgICB0aGlzLl9hLnNldCgwLCB2KTsNCiAgfQ0KDQogIHZvaWQgYWxsb2MoKSB7DQogICAgVCB2Ow0KICAgIHRoaXMuc2V0KHYpOw0KICB9DQoNCiAgVCBnZXQoKSB7DQogICAgOjphc3NlcnQoIXRoaXMuZW1wdHkoKSk7DQogICAgcmV0dXJuIHRoaXMuX2EuZ2V0KDApOw0KICB9DQoNCiAgVCByZWxlYXNlKCkgew0KICAgIFQgciA9IHRoaXMuZ2V0KCk7DQoJdGhpcy5kZXN0cnVjdCgpOw0KICAgIHJldHVybiByOw0KICB9DQoNCiAgdm9pZCBjb25zdHJ1Y3QoKSB7fQ0KDQogIHZvaWQgY29uc3RydWN0KFQgdikgew0KICAgIHRoaXMuY29uc3RydWN0KCk7DQogICAgdGhpcy5zZXQodik7DQogIH0NCg0KICB2b2lkIGNvbnN0cnVjdChyZWY8VD4mIG90aGVyKSB7DQogICAgdGhpcy5jb25zdHJ1Y3Qob3RoZXIucmVsZWFzZSgpKTsNCiAgfQ0KfTsNCg0KfSAgLy8gbmFtZXNwYWNlIGIyc3R5bGUNCg0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9CMlNUWUxFX1JFRl9IDQoPAAAAYjJzdHlsZS9zdGRpby5oNgYAAO+7vw0KI2lmbmRlZiBCMlNUWUxFX0xJQl9CMlNUWUxFX1NURElPX0gNCiNkZWZpbmUgQjJTVFlMRV9MSUJfQjJTVFlMRV9TVERJT19IDQoNCiNpbmNsdWRlIDxiMnN0eWxlL3R5cGVzLmg+DQojaW5jbHVkZSA8YjJzdHlsZS9vcGVyYXRvcnMuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL2xvYWRlZF9tZXRob2QuaD4NCiNpbmNsdWRlIDxic3R5bGUvaW50Lmg+DQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCg0Kdm9pZCBzdGRfb3V0KHN0cmluZyBpKSB7DQogIGxvZ2ljICJpbnRlcnJ1cHQgc3Rkb3V0IGIyc3R5bGVfX2kgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmciOw0KfQ0KDQp2b2lkIHN0ZF9lcnIoc3RyaW5nIGkpIHsNCiAgbG9naWMgImludGVycnVwdCBzdGRlcnIgYjJzdHlsZV9faSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyI7DQp9DQoNCnZvaWQgc3RkX291dChib29sIGkpIHsNCiAgaWYgKGkpIHsNCiAgICBzdGRfb3V0KCJUcnVlIik7DQogIH0gZWxzZSB7DQogICAgc3RkX291dCgiRmFsc2UiKTsNCiAgfQ0KfQ0KDQp2b2lkIHN0ZF9lcnIoYm9vbCBpKSB7DQogIGlmIChpKSB7DQogICAgc3RkX2VycigiVHJ1ZSIpOw0KICB9IGVsc2Ugew0KICAgIHN0ZF9lcnIoIkZhbHNlIik7DQogIH0NCn0NCg0Kc3RyaW5nIGxlZ2FjeV9iaWd1aW50X3RvX3N0cihiaWd1aW50IGkpIHsNCiAgaWYgKGkgPT0gMEwpIHsNCiAgICByZXR1cm4gIjAiOw0KICB9DQogIHN0cmluZyBzOw0KICB3aGlsZSAoaSA+IDBMKSB7DQogICAgaW50IGIgPSA6OmJzdHlsZTo6dG9faW50KG1vZChpLCAxMEwpKTsNCiAgICBpIC89IDEwTDsNCiAgICBiICs9IDQ4Ow0KICAgIHMgPSA6OmJzdHlsZTo6c3RyX2NvbmNhdCg6OmJzdHlsZTo6dG9fc3RyKDo6YnN0eWxlOjp0b19ieXRlKGIpKSwgcyk7DQogIH0NCiAgcmV0dXJuIHM7DQp9DQoNCnN0cmluZyBiaWd1aW50X3RvX3N0cihiaWd1aW50IGkpIHsNCiAgbG9hZF9tZXRob2QoImJpZ191aW50X3RvX3N0ciIpOw0KICByZXR1cm4gZXhlY3V0ZV9sb2FkZWRfbWV0aG9kPGJpZ3VpbnQsIHN0cmluZz4oaSk7DQp9DQoNCnN0cmluZyBpbnRfdG9fc3RyKGludCBpKSB7DQogIGlmIChpIDw9IDIxNDc0ODM2NDcpIHsNCiAgICByZXR1cm4gYmlndWludF90b19zdHIoOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSkpOw0KICB9DQogIGkgLT0gMjE0NzQ4MzY0NzsNCiAgaSA9IDIxNDc0ODM2NDcgLSBpOw0KICBpICs9IDI7DQogIHJldHVybiA6OmJzdHlsZTo6c3RyX2NvbmNhdCgiLSIsIGJpZ3VpbnRfdG9fc3RyKDo6YnN0eWxlOjp0b19iaWd1aW50KGkpKSk7DQp9DQoNCnZvaWQgc3RkX291dChiaWd1aW50IGkpIHsNCiAgc3RkX291dChiaWd1aW50X3RvX3N0cihpKSk7DQp9DQoNCnZvaWQgc3RkX2VycihiaWd1aW50IGkpIHsNCiAgc3RkX2VycihiaWd1aW50X3RvX3N0cihpKSk7DQp9DQoNCnZvaWQgc3RkX291dChpbnQgaSkgew0KICBzdGRfb3V0KGludF90b19zdHIoaSkpOw0KfQ0KDQp2b2lkIHN0ZF9lcnIoaW50IGkpIHsNCiAgc3RkX2VycihpbnRfdG9fc3RyKGkpKTsNCn0NCg0KfSAgLy8gbmFtZXNwYWNlIGIyc3R5bGUNCg0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9CMlNUWUxFX1NURElPX0gNCg8AAABiMnN0eWxlL3R5cGVzLmivAQAA77u/DQojaWZuZGVmIEIyU1RZTEVfTElCX0IyU1RZTEVfVFlQRVNfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX1RZUEVTX0gNCg0KI2luY2x1ZGUgPGJzdHlsZS90eXBlcy5oPg0KDQovLyBUT0RPOiBTZWFyY2ggdHlwZXMgaW4gcGFyZW50IHNjb3Blcy4NCm5hbWVzcGFjZSBiMnN0eWxlIHsNCg0KdHlwZWRlZiA6OnN0cmluZyBzdHJpbmc7DQp0eXBlZGVmIDo6dm9pZCB2b2lkOw0KdHlwZWRlZiA6OmJvb2wgYm9vbDsNCnR5cGVkZWYgOjpiaWd1aW50IGJpZ3VpbnQ7DQp0eXBlZGVmIDo6bG9uZyBsb25nOw0KdHlwZWRlZiA6OmludCBpbnQ7DQp0eXBlZGVmIDo6Ynl0ZSBieXRlOw0KdHlwZWRlZiA6OnVmbG9hdCB1ZmxvYXQ7DQoNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfQjJTVFlMRV9UWVBFU19IDQoQAAAAYjJzdHlsZS91ZmxvYXQuaOcMAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfQjJTVFlMRV9VRkxPQVRfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9CMlNUWUxFX1VGTE9BVF9IDQoNCiNpbmNsdWRlIDxiMnN0eWxlL2xvYWRlZF9tZXRob2QuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL3N0ZGlvLmg+DQojaW5jbHVkZSA8YjJzdHlsZS90eXBlcy5oPg0KDQpuYW1lc3BhY2UgYjJzdHlsZSB7DQoNCmJvb2wgZXF1YWwodWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIGJvb2wgcmVzdWx0Ow0KICBsb2dpYyAiZmxvYXRfZXF1YWwgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgbm90X2VxdWFsKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImZsb2F0X2VxdWFsIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gbm90KHJlc3VsdCk7DQp9DQoNCmJvb2wgZ3JlYXRlcl90aGFuKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICBib29sIHJlc3VsdDsNCiAgbG9naWMgImZsb2F0X21vcmUgYjJzdHlsZV9fcmVzdWx0IGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCmJvb2wgZ3JlYXRlcl9vcl9lcXVhbCh1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgcmV0dXJuIG9yKGdyZWF0ZXJfdGhhbihpLCBqKSwgZXF1YWwoaSwgaikpOw0KfQ0KDQpib29sIGxlc3NfdGhhbih1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgYm9vbCByZXN1bHQ7DQogIGxvZ2ljICJmbG9hdF9sZXNzIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQpib29sIGxlc3Nfb3JfZXF1YWwodWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIHJldHVybiBvcihsZXNzX3RoYW4oaSwgaiksIGVxdWFsKGksIGopKTsNCn0NCg0KdWZsb2F0IGFkZCh1ZmxvYXQgaSwgdWZsb2F0IGopIHsNCiAgbG9naWMgImZsb2F0X2FkZCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQp1ZmxvYXQgbWludXModWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIGxvZ2ljICJmbG9hdF9zdWJ0cmFjdCBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQp1ZmxvYXQgbXVsdGlwbHkodWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIGxvZ2ljICJmbG9hdF9tdWx0aXBseSBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQp1ZmxvYXQgZGl2aWRlKHVmbG9hdCBpLCB1ZmxvYXQgaikgew0KICB1ZmxvYXQgcmVzdWx0Ow0KICBsb2dpYyAiZmxvYXRfZGl2aWRlIGIyc3R5bGVfX3Jlc3VsdCBiMnN0eWxlX19pIGIyc3R5bGVfX2oiOw0KICByZXR1cm4gcmVzdWx0Ow0KfQ0KDQp1ZmxvYXQgcG93ZXIodWZsb2F0IGksIHVmbG9hdCBqKSB7DQogIGxvZ2ljICJmbG9hdF9wb3dlciBiMnN0eWxlX19pIGIyc3R5bGVfX2kgYjJzdHlsZV9faiI7DQogIHJldHVybiBpOw0KfQ0KDQpzdHJpbmcgdWZsb2F0X3RvX3N0cih1ZmxvYXQgaSkgew0KICBsb2FkX21ldGhvZCgiYmlnX3VkZWNfdG9fc3RyIik7DQogIHJldHVybiBleGVjdXRlX2xvYWRlZF9tZXRob2Q8dWZsb2F0LCBzdHJpbmc+KGkpOw0KfQ0KDQp2b2lkIHN0ZF9vdXQodWZsb2F0IGkpIHsNCiAgc3RkX291dCh1ZmxvYXRfdG9fc3RyKGkpKTsNCn0NCg0Kdm9pZCBzdGRfZXJyKHVmbG9hdCBpKSB7DQogIHN0ZF9lcnIodWZsb2F0X3RvX3N0cihpKSk7DQp9DQoNCnVmbG9hdCBzZWxmX2luY19wb3N0KHVmbG9hdCYgeCkgew0KICB1ZmxvYXQgciA9IHg7DQoJeCA9IGFkZCh4LCAxLjApOw0KICByZXR1cm4gcjsNCn0NCg0KdWZsb2F0IHNlbGZfZGVjX3Bvc3QodWZsb2F0JiB4KSB7DQogIHVmbG9hdCByID0geDsNCgl4ID0gbWludXMoeCwgMS4wKTsNCiAgcmV0dXJuIHI7DQp9DQoNCnVmbG9hdCBzZWxmX2luY19wcmUodWZsb2F0JiB4KSB7DQoJeCA9IGFkZCh4LCAxLjApOw0KICByZXR1cm4geDsNCn0NCg0KdWZsb2F0IHNlbGZfZGVjX3ByZSh1ZmxvYXQmIHgpIHsNCgl4ID0gbWludXMoeCwgMS4wKTsNCiAgcmV0dXJuIHg7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2Zyb20oYmlndWludCBpKSB7DQogIGxvYWRfbWV0aG9kKCJiaWdfdWludF90b19iaWdfdWRlYyIpOw0KICByZXR1cm4gZXhlY3V0ZV9sb2FkZWRfbWV0aG9kPGJpZ3VpbnQsIHVmbG9hdD4oaSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2Zyb20oaW50IGkpIHsNCiAgcmV0dXJuIHVmbG9hdF9fZnJvbSg6OmJzdHlsZTo6dG9fYmlndWludChpKSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2Zyb20obG9uZyBpKSB7DQogIHJldHVybiB1ZmxvYXRfX2Zyb20oOjpic3R5bGU6OnRvX2JpZ3VpbnQoaSkpOw0KfQ0KDQp1ZmxvYXQgdWZsb2F0X19mcmFjdGlvbihiaWd1aW50IG4sIGJpZ3VpbnQgZCkgew0KICB1ZmxvYXQgcmVzdWx0ID0gdWZsb2F0X19mcm9tKG4pOw0KICB1ZmxvYXQgdWQgPSB1ZmxvYXRfX2Zyb20oZCk7DQogIHJldHVybiBkaXZpZGUocmVzdWx0LCB1ZCk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGJpZ3VpbnQgbiwgaW50IGQpIHsNCiAgcmV0dXJuIHVmbG9hdF9fZnJhY3Rpb24obiwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoZCkpOw0KfQ0KDQp1ZmxvYXQgdWZsb2F0X19mcmFjdGlvbihiaWd1aW50IG4sIGxvbmcgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbihuLCA6OmJzdHlsZTo6dG9fYmlndWludChkKSk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGludCBuLCBiaWd1aW50IGQpIHsNCiAgcmV0dXJuIHVmbG9hdF9fZnJhY3Rpb24oOjpic3R5bGU6OnRvX2JpZ3VpbnQobiksIGQpOw0KfQ0KDQp1ZmxvYXQgdWZsb2F0X19mcmFjdGlvbihsb25nIG4sIGJpZ3VpbnQgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbig6OmJzdHlsZTo6dG9fYmlndWludChuKSwgZCk7DQp9DQoNCnVmbG9hdCB1ZmxvYXRfX2ZyYWN0aW9uKGludCBuLCBpbnQgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbig6OmJzdHlsZTo6dG9fYmlndWludChuKSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoZCkpOw0KfQ0KDQp1ZmxvYXQgdWZsb2F0X19mcmFjdGlvbihsb25nIG4sIGxvbmcgZCkgew0KICByZXR1cm4gdWZsb2F0X19mcmFjdGlvbig6OmJzdHlsZTo6dG9fYmlndWludChuKSwgOjpic3R5bGU6OnRvX2JpZ3VpbnQoZCkpOw0KfQ0KDQp9ICAvLyBuYW1lc3BhY2UgYjJzdHlsZQ0KI2VuZGlmICAvLyBCMlNUWUxFX0xJQl9CMlNUWUxFX1VGTE9BVF9IDQoLAAAAc3RkL3R5cGVzLmhwAQAA77u/DQojaWZuZGVmIEIyU1RZTEVfTElCX1NURF9UWVBFU19IDQojZGVmaW5lIEIyU1RZTEVfTElCX1NURF9UWVBFU19IDQoNCiNpbmNsdWRlIDxic3R5bGUvdHlwZXMuaD4NCg0KbmFtZXNwYWNlIHN0ZCB7DQoNCnR5cGVkZWYgOjpzdHJpbmcgc3RyaW5nOw0KdHlwZWRlZiA6OnZvaWQgdm9pZDsNCnR5cGVkZWYgOjpib29sIGJvb2w7DQp0eXBlZGVmIDo6YmlndWludCBiaWd1aW50Ow0KdHlwZWRlZiA6OmxvbmcgbG9uZzsNCnR5cGVkZWYgOjppbnQgaW50Ow0KdHlwZWRlZiA6OmJ5dGUgYnl0ZTsNCnR5cGVkZWYgOjp1ZmxvYXQgdWZsb2F0Ow0KDQp9ICAvLyBuYW1lc3BhY2Ugc3RkDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfU1REX1RZUEVTX0gKAAAAc3RkL3ZlY3RvcmcFAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfU1REX1ZFQ1RPUg0KI2RlZmluZSBCMlNUWUxFX0xJQl9TVERfVkVDVE9SDQoNCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8YXNzZXJ0Lmg+DQojaW5jbHVkZSA8YjJzdHlsZS9oZWFwX3B0ci5oPg0KI2luY2x1ZGUgPHN0ZC90eXBlcy5oPg0KDQpuYW1lc3BhY2Ugc3RkIHsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyB2ZWN0b3Igew0KICA6OmIyc3R5bGU6OmhlYXBfcHRyPFQ+IF9hOw0KICBpbnQgX3M7DQoNCiAgaW50IHNpemUoKSB7DQogICAgcmV0dXJuIHRoaXMuX3M7DQogIH0NCg0KICBpbnQgY2FwYWNpdHkoKSB7DQogICAgcmV0dXJuIHRoaXMuX2Euc2l6ZSgpOw0KICB9DQoNCiAgVCBnZXQoaW50IGluZGV4KSB7DQogICAgOjphc3NlcnQoaW5kZXggPj0gMCwgImdldCIpOw0KICAgIDo6YXNzZXJ0KGluZGV4IDwgdGhpcy5zaXplKCksICJnZXQiKTsNCiAgICByZXR1cm4gdGhpcy5fYS5nZXQoaW5kZXgpOw0KICB9DQoNCiAgdm9pZCBzZXQoaW50IGluZGV4LCBUIHYpIHsNCiAgICA6OmFzc2VydChpbmRleCA+PSAwLCAic2V0Iik7DQogICAgOjphc3NlcnQoaW5kZXggPCB0aGlzLnNpemUoKSwgInNldCIpOw0KICAgIHRoaXMuX2Euc2V0KGluZGV4LCB2KTsNCiAgfQ0KDQogIHZvaWQgZGVzdHJ1Y3QoKSB7DQogICAgdGhpcy5fYS5kZXN0cnVjdCgpOw0KICB9DQoNCiAgdm9pZCByZXNlcnZlKGludCBzaXplKSB7DQogICAgOjpiMnN0eWxlOjpoZWFwX3B0cjxUPiB4KHNpemUpOw0KICAgIGZvciAoaW50IGkgPSAwOyBpIDwgdGhpcy5zaXplKCk7IGkrKykgew0KICAgICAgeC5zZXQoaSwgdGhpcy5nZXQoaSkpOw0KICAgIH0NCgl0aGlzLmRlc3RydWN0KCk7DQogICAgdGhpcy5fYSA9IHg7DQoJeC5yZWxlYXNlKCk7DQogIH0NCg0KICB2b2lkIGNvbnN0cnVjdChpbnQgc2l6ZSkgew0KICAgIHRoaXMucmVzZXJ2ZShzaXplKTsNCiAgfQ0KDQogIHZvaWQgY29uc3RydWN0KCkgew0KICAgIHRoaXMuY29uc3RydWN0KDQpOw0KICB9DQoNCiAgdm9pZCBwdXNoX2JhY2soVCB2KSB7DQogICAgaWYgKHRoaXMuc2l6ZSgpID09IHRoaXMuY2FwYWNpdHkoKSkgew0KICAgICAgdGhpcy5yZXNlcnZlKHRoaXMuY2FwYWNpdHkoKSA8PCAxKTsNCiAgICB9DQogICAgdGhpcy5fcysrOw0KICAgIHRoaXMuc2V0KHRoaXMuc2l6ZSgpIC0gMSwgdik7DQogIH0NCg0KICB2b2lkIHBvcF9iYWNrKCkgew0KICAgIDo6YXNzZXJ0KHRoaXMuc2l6ZSgpID4gMCwgInBvcF9iYWNrIik7DQogICAgdGhpcy5fcy0tOw0KICB9DQoNCiAgdm9pZCBjbGVhcigpIHsNCiAgICB0aGlzLl9zID0gMDsNCiAgfQ0KfTsNCg0KfSAgLy8gbmFtZXNwYWNlIHN0ZA0KDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX1NURF9WRUNUT1IQAAAAdGVzdGluZy9hc3NlcnQuaHIEAADvu78NCiNpZm5kZWYgQjJTVFlMRV9MSUJfVEVTVElOR19BU1NFUlRfSA0KI2RlZmluZSBCMlNUWUxFX0xJQl9URVNUSU5HX0FTU0VSVF9IDQoNCiNpbmNsdWRlIDxiMnN0eWxlL3N0ZGlvLmg+DQojaW5jbHVkZSA8YnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy90eXBlcy5oPg0KDQpuYW1lc3BhY2UgYjJzdHlsZSB7DQpuYW1lc3BhY2UgdGVzdGluZyB7DQoNCmludCBfYXNzZXJ0aW9uX2NvdW50ID0gMDsNCg0Kdm9pZCBhc3NlcnRfdHJ1ZShib29sIHYsIHN0cmluZyBtc2cpIHsNCiAgX2Fzc2VydGlvbl9jb3VudCsrOw0KICBzdHJpbmcgcHJlZml4Ow0KICBpZiAodikgew0KICAgIHByZWZpeCA9ICJTdWNjZXNzOiAiOw0KICB9IGVsc2Ugew0KICAgIHByZWZpeCA9ICJGYWlsdXJlOiAiOw0KICB9DQogIDo6YjJzdHlsZTo6c3RkX291dChwcmVmaXgpOw0KICA6OmIyc3R5bGU6OnN0ZF9vdXQobXNnKTsNCiAgOjpiMnN0eWxlOjpzdGRfb3V0KCJcbiIpOw0KfQ0KDQp2b2lkIGFzc2VydF90cnVlKGJvb2wgdikgew0KICBhc3NlcnRfdHJ1ZSh2LCAibm8gZXh0cmEgaW5mb3JtYXRpb24uIik7DQp9DQoNCnZvaWQgYXNzZXJ0X2ZhbHNlKGJvb2wgdiwgc3RyaW5nIG1zZykgew0KICBhc3NlcnRfdHJ1ZSghdiwgbXNnKTsNCn0NCg0Kdm9pZCBhc3NlcnRfZmFsc2UoYm9vbCB2KSB7DQogIGFzc2VydF90cnVlKCF2KTsNCn0NCg0KLyoNClRPRE86IE1ha2UgdGhpcyB3b3JrLiBDdXJyZW50bHkgYXNzZXJ0X2VxdWFsX18yIGNvbmZsaWN0cyB3aXRoIHRoZSBmb2xsb3dpbmcgb25lLg0KdGVtcGxhdGUgPFQsIFQyPg0Kdm9pZCBhc3NlcnRfZXF1YWwoVCB0LCBUMiB0Miwgc3RyaW5nIG1zZykgew0KICBhc3NlcnRfdHJ1ZSh0ID09IHQyLCBtc2cpOw0KfQ0KKi8NCg0KdGVtcGxhdGUgPFQsIFQyPg0Kdm9pZCBhc3NlcnRfZXF1YWwoVCB0LCBUMiB0Mikgew0KICBhc3NlcnRfdHJ1ZSh0ID09IHQyKTsNCn0NCg0KdGVtcGxhdGUgPFQ+DQp2b2lkIGFzc2VydF9lcXVhbChUIHQsIFQgdDIpIHsNCiAgYXNzZXJ0X2VxdWFsPFQsIFQ+KHQsIHQyKTsNCn0NCg0KfSAgLy8gbmFtZXNwYWNlIHRlc3RpbmcNCn0gIC8vIG5hbWVzcGFjZSBiMnN0eWxlDQoNCiNlbmRpZiAgLy8gQjJTVFlMRV9MSUJfVEVTVElOR19BU1NFUlRfSA0KDwAAAHRlc3RpbmcvdHlwZXMuaE8BAAANCiNpZm5kZWYgQjJTVFlMRV9MSUJfVEVTVElOR19UWVBFU19IDQojZGVmaW5lIEIyU1RZTEVfTElCX1RFU1RJTkdfVFlQRVNfSA0KDQojaW5jbHVkZSA8YnN0eWxlL3R5cGVzLmg+DQoNCm5hbWVzcGFjZSBiMnN0eWxlIHsNCm5hbWVzcGFjZSB0ZXN0aW5nIHsNCg0KdHlwZWRlZiA6OnN0cmluZyBzdHJpbmc7DQp0eXBlZGVmIDo6dm9pZCB2b2lkOw0KdHlwZWRlZiA6OmJvb2wgYm9vbDsNCnR5cGVkZWYgOjppbnQgaW50Ow0KDQoNCn0gIC8vIG5hbWVzcGFjZSB0ZXN0aW5nDQp9ICAvLyBuYWVtc3BhY2UgYjJzdHlsZQ0KDQojZW5kaWYgIC8vIEIyU1RZTEVfTElCX1RFU1RJTkdfVFlQRVNfSA=="
        ))
End Class
