Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(8516), 
            "GgAAAGFjY2Vzcy1iaWd1aW50LWFzLWhlYXAudHh0HwEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHhbMTAwXTsNCiAgYmlndWludCB5ID0geDsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIHhbaV0gPSBpICsgMTsNCiAgfQ0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeVtpXSwgaSArIDEpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EAAAAGFzc2VydC1lcXVhbC50eHTYAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50LCBpbnQ+KDEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nLCBzdHJpbmc+KCJhYmMiLCAiYWJjIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9IwAAAGNsYXNzLWZ1bmN0aW9uLWNhbGwtd2l0aC1zcGFjZXMudHh07gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEMgew0KICB2b2lkIGYoaW50JiB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgaW50IHggPSAwOw0KICBjIC4gZiAoIHggKTsNCiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBhc3NlcnRfdHJ1ZSggeCA9PSAxICk7DQogIGIyc3R5bGU6OiB0ZXN0aW5nOjogZmluaXNoZWQoKTsNCn0kAAAAY2xhc3MtaW5oZXJpdGFuY2UtYmFzZS12YXJpYWJsZXMudHh08AEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEEgew0KICBpbnQgYTsNCn07DQoNCmNsYXNzIEIgew0KICBpbnQgYjsNCn07DQoNCmNsYXNzIEMgOiBCIHsNCiAgaW50IGM7DQp9Ow0KDQpjbGFzcyBEIDogQSwgQyB7DQogIGludCBkOw0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBEIGQ7DQogIGQuYT0xOw0KICBkLmI9MjsNCiAgZC5jPTM7DQogIGQuZD00Ow0KICAvLyBUaGUgInR5cGUtaWQicyBhcmUgbm90IHRlc3RlZC4NCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC5hLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC5iLCAyKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC5jLCAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC5kLCA0KTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0iAAAAY2xhc3MtaW5oZXJpdGFuY2UtY29tcGlsZS1vbmx5LnR4dOYAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBCIHsNCiAgaW50IHg7DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIGludCB5Ow0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDIGM7DQogIGMueSA9IDEwMDsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy55LCAxMDApOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfSgAAABjbGFzcy1pbmhlcml0YW5jZS10ZW1wbGF0ZS12YXJpYWJsZXMudHh04QEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQiB7DQogIFQgeDsNCn07DQoNCmNsYXNzIEMgOiBCPGludD4gew0KICBpbnQgeTsNCn07DQoNCmNsYXNzIEMyIDogQjxzdHJpbmc+IHsNCiAgaW50IHk7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy54PSAxOw0KICBjLnk9MjsNCg0KICBDMiBjMjsNCiAgYzIueD0iYSI7DQogIGMyLnk9MzsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihjMi54LCAiYSIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjMi55LCAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0VAAAAY29weS1mdW5jdGlvbi1wdHIudHh02wAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmRlbGVnYXRlIHZvaWQgcCgpOw0KDQp2b2lkIGYoKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIHAgeCA9IGY7DQogIHAgeSA9IHg7DQogIHgoKTsNCiAgeSgpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfSoAAABkZWZpbmUtY2xhc3MtY29uc3RydWN0b3ItZm9yLW5vbi1jbGFzcy50eHRuAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBjb25zdHJ1Y3QoaW50JiB0aGlzLCBpbnQgdikgew0KICB0aGlzID0gdiArIDE7DQp9DQoNCnZvaWQgY29uc3RydWN0KGludCYgdGhpcykgew0KICB0aGlzLmNvbnN0cnVjdCgxKTsNCn0NCg0Kdm9pZCBkZXN0cnVjdChpbnQmIHRoaXMpIHt9DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHgoMTAwKTsNCiAgaW50IHkoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgMTAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeSwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9JQAAAGRlbGVnYXRlLWZ1bmN0aW9ucy13aXRoLXNhbWUtbmFtZS50eHR3AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KaW50IGYoaW50IHgpIHsNCiAgcmV0dXJuIHggKyAxOw0KfQ0KDQpzdHJpbmcgZihzdHJpbmcgeCkgew0KICByZXR1cm4geDsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjpmdW5jdGlvbjxpbnQsIGludD4gZCA9IGY7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQoMSksIDIpOw0KDQogIGIyc3R5bGU6OmZ1bmN0aW9uPHN0cmluZywgc3RyaW5nPiBkMiA9IGY7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGQyKCJhYmMiKSwgImFiYyIpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9FQAAAGZ1bmN0aW9uLXRlbXBsYXRlLnR4dCUBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0ZW1wbGF0ZSA8VD4NClQgYWRkKFQmIHgsIFQgeSkgew0KICB4ICs9IHk7DQogIHJldHVybiB4Ow0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIGludCB4ID0gMTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoYWRkPGludD4oeCwgMSkgPT0gMik7DQogIGFkZDxpbnQ+KHgsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh4ID09IDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfR8AAABtb3JlLWNsb3NpbmctYW5nbGUtYnJhY2tldHMudHh0NAEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQyB7DQogIHZvaWQgZigpIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMiB7DQogIHZvaWQgZigpIHsNCiAgICBUIHQ7DQoJdC5mKCk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQzI8QzI8QzI8QzI8QzI8Qzx2b2lkPj4+Pj4+IGM7DQogIGMuZigpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KHwAAAHJlZGVmaW5lLXN0cnVjdC1tZW1iZXItdHlwZS50eHT1AAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdHlwZWRlZiBpbnQgSU5UOw0KDQpzdHJ1Y3QgUyB7DQogIElOVCBpOw0KfTsNCg0KdHlwZWRlZiBzdHJpbmcgSU5UOw0KDQp2b2lkIG1haW4oKSB7DQogIFMgczsNCiAgcy5pID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzLmksIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoYAAAAcmVpbnRlcnByZXRfY2FzdF9yZWYudHh0tQEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCiAgdm9pZCBmKCkgew0KICAgIHRoaXMueCsrOw0KICB9DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIHZvaWQgZjIoKSB7DQogICAgcmVpbnRlcnByZXRfY2FzdCh0aGlzLCBCKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQoNCiAgdm9pZCBmMygpIHsNCiAgICByZWludGVycHJldF9jYXN0KHRoaXMsIEImKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy54ID0gMTAwOw0KICBjLmYyKCk7DQogIGMuZjMoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy54LCAxMDQpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KDwAAAHNlbGYtaGVhbHRoLnR4dI4AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KLAAAAHN0cnVjdC1hbmQtcHJpbWl0aXZlLXR5cGUtd2l0aC1zYW1lLW5hbWUudHh04gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnR5cGVkZWYgaW50IElOVDsNCg0Kc3RydWN0IElOVCB7DQogIGludCB4Ow0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBJTlQgaTsNCiAgaS54ID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLngsIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQolAAAAc3RydWN0X21lbWJlcl90eXBlX3dpdGhfbmFtZXNwYWNlLnR4dPgAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpuYW1lc3BhY2UgTiB7DQpzdHJ1Y3QgUyB7DQogIDo6YjJzdHlsZTo6aW50IGk7DQp9Ow0KfSAgLy8gbmFtZXNwYWNlIE4NCg0Kdm9pZCBtYWluKCkgew0KICBOOjpTIHM7DQogIHMuaSA9IDEwMDsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4ocy5pLCAxMDApOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRkAAAB0ZW1wbGF0ZS1pbi1uYW1lc3BhY2UudHh09gEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCm5hbWVzcGFjZSBOIHsNCg0KdHlwZWRlZiA6OnZvaWQgdm9pZDsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKCkgew0KICAgIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KZGVsZWdhdGUgdm9pZCBmKCk7DQoNCnRlbXBsYXRlIDxUPg0Kdm9pZCBmMigpIHsNCiAgICA6OmIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KfQ0KDQp9ICAvLyBuYW1lc3BhY2UgTg0KDQp2b2lkIGYoKSB7DQogIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgTjo6QzxpbnQ+IGM7DQogIGMuZigpOw0KICBOOjpmPGludD4gZjIgPSBmOw0KICBmMigpOw0KICBOOjpmMjxpbnQ+KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoVAAAAdGVtcGxhdGUtb3ZlcnJpZGUudHh0ewMAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIE91dFdyaXRlciB7DQogIGludCBjOw0KDQogIHZvaWQgd3JpdGUoc3RyaW5nIG1zZykgew0KICAgIHRoaXMuYysrOw0KICAgIC8vIERvIG5vdCByZWFsbHkgd3JpdGUgdG8gc3Rkb3V0LCBpdCB3aWxsIG1hc3MgdXAgdGhlIHRlc3RpbmcgYXNzZXJ0aW9ucy4NCiAgICBiMnN0eWxlOjpzdGRfZXJyKG1zZyk7DQogIH0NCn07DQoNCmNsYXNzIEVycldyaXRlciB7DQogIGludCBjOw0KDQogIHZvaWQgd3JpdGUoc3RyaW5nIG1zZykgew0KICAgIHRoaXMuYysrOw0KICAgIGIyc3R5bGU6OnN0ZF9lcnIobXNnKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFc+DQpjbGFzcyBMb2dnZXIgew0KICBXIHc7DQoNCiAgdm9pZCBsb2coc3RyaW5nIG1zZykgew0KICAgIHRoaXMudy53cml0ZShtc2cpOw0KICB9DQoNCiAgaW50IGNvdW50X29mX2xvZ19saW5lcygpIHsNCiAgICByZXR1cm4gdGhpcy53LmM7DQogIH0NCn07DQoNCnR5cGVkZWYgTG9nZ2VyPE91dFdyaXRlcj4gT3V0TG9nZ2VyOw0KdHlwZWRlZiBMb2dnZXI8RXJyV3JpdGVyPiBFcnJMb2dnZXI7DQoNCnZvaWQgbWFpbigpIHsNCiAgT3V0TG9nZ2VyIG9sOw0KICBFcnJMb2dnZXIgZWw7DQoNCiAgb2wubG9nKCJvdXQiKTsNCiAgb2wubG9nKCJvdXQiKTsNCiAgb2wubG9nKCJvdXQiKTsNCiAgZWwubG9nKCJlcnIiKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihvbC5jb3VudF9vZl9sb2dfbGluZXMoKSwgMyk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGVsLmNvdW50X29mX2xvZ19saW5lcygpLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChsAAAB0ZW1wbGF0ZV9jbGFzc193aXRoX3JlZi50eHQBAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKFQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMzIHsNCiAgdm9pZCBmKFQgdCkgew0KICAgIGludCB4ID0gMDsNCgl0LmYoeCk7DQoJYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAxKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8aW50Jj4gYzI7DQogIC8vIFRPRE86IFJlbW92ZSByaWdodC1zaGlmdCBhbmQgYXZvaWQgYWRkaW5nIGEgc3BhY2UgYmV0d2VlbiB0d28gPj4uDQogIEMzPEM8aW50PiA+IHI7DQogIHIuZihjKTsNCiAgQzM8QzI8aW50Jj4gPiByMjsNCiAgcjIuZihjMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9HgAAAHR3by1jbG9zaW5nLWFuZ2xlLWJyYWNrZXRzLnR4dEMBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoVCB0LCBUIHQyKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodCA9PSB0Mik7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB0KSB7DQogICAgdC5mKDEwMCwgMTAwKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8QzxpbnQ+PiBjMjsNCiAgYzIuZihjKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRsAAAB1c2UtYmFzZS1jbGFzcy1mdW5jdGlvbi50eHSmAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQiB7DQogIGludCB4Ow0KICB2b2lkIGYoKSB7DQoJdGhpcy54Kys7DQogIH0NCn07DQoNCmNsYXNzIEIyOiBCIHsNCiAgdm9pZCBmMigpIHsNCiAgICB0aGlzLngrPTI7DQogIH0NCn07DQoNCmNsYXNzIEIzIHsNCiAgdm9pZCBmaW5pc2goKSB7DQoJYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCiAgfQ0KfTsNCg0KY2xhc3MgQzogQjIsIEIzIHsNCiAgdm9pZCBmMygpIHsNCiAgICB0aGlzLngrPTM7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLmYoKTsNCiAgYy5mMigpOw0KICBjLmYzKCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueCwgNik7DQogIGMuZmluaXNoKCk7DQp9DQohAAAAdmlydHVhbC1mdW5jdGlvbi1jb21waWxlLW9ubHkudHh05QAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEMgew0KICBvdmVycmlkYWJsZSB2b2lkIGYoKSB7fQ0KfTsNCg0KY2xhc3MgRCA6IEMgew0KICBvdmVycmlkZSB2b2lkIGYoKSB7fQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDIGM7DQogIEQgZDsNCg0KICBjLmYoKTsNCiAgZC5mKCk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0="
        ))
End Class
