Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(1344), 
            "EwAAAHR3by1yaWdodC1zaGlmdC50eHRDAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKFQgdCwgVCB0Mikgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHQgPT0gdDIpOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMyIHsNCiAgdm9pZCBmKFQgdCkgew0KICAgIHQuZigxMDAsIDEwMCk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQzxpbnQ+IGM7DQogIEMyPEM8aW50Pj4gYzI7DQogIGMyLmYoYyk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0bAAAAdGVtcGxhdGVfY2xhc3Nfd2l0aF9yZWYudHh0AQIAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQyB7DQogIHZvaWQgZihUJiB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMyIHsNCiAgdm9pZCBmKFQgeCkgew0KICAgIHgrKzsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMyB7DQogIHZvaWQgZihUIHQpIHsNCiAgICBpbnQgeCA9IDA7DQoJdC5mKHgpOw0KCWIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHggPT0gMSk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQzxpbnQ+IGM7DQogIEMyPGludCY+IGMyOw0KICAvLyBUT0RPOiBSZW1vdmUgcmlnaHQtc2hpZnQgYW5kIGF2b2lkIGFkZGluZyBhIHNwYWNlIGJldHdlZW4gdHdvID4+Lg0KICBDMzxDPGludD4gPiByOw0KICByLmYoYyk7DQogIEMzPEMyPGludCY+ID4gcjI7DQogIHIyLmYoYzIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ8AAABzZWxmLWhlYWx0aC50eHSOAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCiMAAABjbGFzcy1mdW5jdGlvbi1jYWxsLXdpdGgtc3BhY2VzLnR4dO4AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBDIHsNCiAgdm9pZCBmKGludCYgeCkgew0KICAgIHgrKzsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDIGM7DQogIGludCB4ID0gMDsNCiAgYyAuIGYgKCB4ICk7DQogIGIyc3R5bGU6OiB0ZXN0aW5nOjogYXNzZXJ0X3RydWUoIHggPT0gMSApOw0KICBiMnN0eWxlOjogdGVzdGluZzo6IGZpbmlzaGVkKCk7DQp9"
        ))
End Class
