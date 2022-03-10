Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(11948), 
            "GgAAAGFjY2Vzcy1iaWd1aW50LWFzLWhlYXAudHh0PQEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHhbMTAwXTsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIHhbaV0gPSBpICsgMTsNCiAgfQ0KICB0eXBlX3B0ciB5ID0geDsNCiAgcmVpbnRlcnByZXRfY2FzdCh5LCBpbnQpOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeVtpXSwgaSArIDEpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EAAAAGFzc2VydC1lcXVhbC50eHTYAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50LCBpbnQ+KDEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nLCBzdHJpbmc+KCJhYmMiLCAiYWJjIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DgAAAGJvb2xfYXJyYXkudHh0bwEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYm9vbCBhWzEwMF07DQogIGJvb2wgYjsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIGFbaV0gPSAoKGkgJSAyKSA9PSAxKTsNCiAgfQ0KICByZWludGVycHJldF9jYXN0KGIsIGludCk7DQogIGIgPSBhOw0KICByZWludGVycHJldF9jYXN0KGIsIGJvb2wpOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGJvb2w+KGJbaV0sIChpICUgMikgPT0gMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0jAAAAY2xhc3MtZnVuY3Rpb24tY2FsbC13aXRoLXNwYWNlcy50eHTuAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQyB7DQogIHZvaWQgZihpbnQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBpbnQgeCA9IDA7DQogIGMgLiBmICggeCApOw0KICBiMnN0eWxlOjogdGVzdGluZzo6IGFzc2VydF90cnVlKCB4ID09IDEgKTsNCiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBmaW5pc2hlZCgpOw0KfSQAAABjbGFzcy1pbmhlcml0YW5jZS1iYXNlLXZhcmlhYmxlcy50eHTwAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQSB7DQogIGludCBhOw0KfTsNCg0KY2xhc3MgQiB7DQogIGludCBiOw0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICBpbnQgYzsNCn07DQoNCmNsYXNzIEQgOiBBLCBDIHsNCiAgaW50IGQ7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEQgZDsNCiAgZC5hPTE7DQogIGQuYj0yOw0KICBkLmM9MzsNCiAgZC5kPTQ7DQogIC8vIFRoZSAidHlwZS1pZCJzIGFyZSBub3QgdGVzdGVkLg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmIsIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmMsIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmQsIDQpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfSIAAABjbGFzcy1pbmhlcml0YW5jZS1jb21waWxlLW9ubHkudHh05gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCn07DQoNCmNsYXNzIEMgOiBCIHsNCiAgaW50IHk7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy55ID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9KAAAAGNsYXNzLWluaGVyaXRhbmNlLXRlbXBsYXRlLXZhcmlhYmxlcy50eHThAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBCIHsNCiAgVCB4Ow0KfTsNCg0KY2xhc3MgQyA6IEI8aW50PiB7DQogIGludCB5Ow0KfTsNCg0KY2xhc3MgQzIgOiBCPHN0cmluZz4gew0KICBpbnQgeTsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLng9IDE7DQogIGMueT0yOw0KDQogIEMyIGMyOw0KICBjMi54PSJhIjsNCiAgYzIueT0zOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueCwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueSwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGMyLngsICJhIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMyLnksIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAABjb3B5LWZ1bmN0aW9uLXB0ci50eHTbAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KZGVsZWdhdGUgdm9pZCBwKCk7DQoNCnZvaWQgZigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgcCB4ID0gZjsNCiAgcCB5ID0geDsNCiAgeCgpOw0KICB5KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9KgAAAGRlZmluZS1jbGFzcy1jb25zdHJ1Y3Rvci1mb3Itbm9uLWNsYXNzLnR4dG4BAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIGNvbnN0cnVjdChpbnQmIHRoaXMsIGludCB2KSB7DQogIHRoaXMgPSB2ICsgMTsNCn0NCg0Kdm9pZCBjb25zdHJ1Y3QoaW50JiB0aGlzKSB7DQogIHRoaXMuY29uc3RydWN0KDEpOw0KfQ0KDQp2b2lkIGRlc3RydWN0KGludCYgdGhpcykge30NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeCgxMDApOw0KICBpbnQgeSgpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCAxMDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih5LCAyKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0lAAAAZGVsZWdhdGUtZnVuY3Rpb25zLXdpdGgtc2FtZS1uYW1lLnR4dHcBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQppbnQgZihpbnQgeCkgew0KICByZXR1cm4geCArIDE7DQp9DQoNCnN0cmluZyBmKHN0cmluZyB4KSB7DQogIHJldHVybiB4Ow0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OmZ1bmN0aW9uPGludCwgaW50PiBkID0gZjsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZCgxKSwgMik7DQoNCiAgYjJzdHlsZTo6ZnVuY3Rpb248c3RyaW5nLCBzdHJpbmc+IGQyID0gZjsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oZDIoImFiYyIpLCAiYWJjIik7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0VAAAAZnVuY3Rpb24tdGVtcGxhdGUudHh0JQEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KVCBhZGQoVCYgeCwgVCB5KSB7DQogIHggKz0geTsNCiAgcmV0dXJuIHg7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHggPSAxOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZShhZGQ8aW50Pih4LCAxKSA9PSAyKTsNCiAgYWRkPGludD4oeCwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHggPT0gMyk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EwAAAG1vZF9lcXVhbHNfdG9fMS50eHTNAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZmFsc2UoKCgxMCAlIDIpID09IDEpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoKCgxMSAlIDIpID09IDEpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCh8AAABtb3JlLWNsb3NpbmctYW5nbGUtYnJhY2tldHMudHh0NAEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQyB7DQogIHZvaWQgZigpIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMiB7DQogIHZvaWQgZigpIHsNCiAgICBUIHQ7DQoJdC5mKCk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQzI8QzI8QzI8QzI8QzI8Qzx2b2lkPj4+Pj4+IGM7DQogIGMuZigpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KHwAAAHJlZGVmaW5lLXN0cnVjdC1tZW1iZXItdHlwZS50eHT1AAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdHlwZWRlZiBpbnQgSU5UOw0KDQpzdHJ1Y3QgUyB7DQogIElOVCBpOw0KfTsNCg0KdHlwZWRlZiBzdHJpbmcgSU5UOw0KDQp2b2lkIG1haW4oKSB7DQogIFMgczsNCiAgcy5pID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzLmksIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoMAAAAcmVmX3Rlc3QudHh0PQIAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL3JlZi5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjpyZWY8c3RyaW5nPiBzKCk7DQogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHMyKCJhYmMiKTsNCiAgYjJzdHlsZTo6cmVmPHN0cmluZz4gczMoczIpOw0KICBiMnN0eWxlOjpyZWY8c3RyaW5nPiBzNCgpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHMuZW1wdHkoKSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHMyLmVtcHR5KCkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZmFsc2UoczMuZW1wdHkoKSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KHMzLmdldCgpLCAiYWJjIik7DQogIHM0LmFsbG9jKCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZShzNC5lbXB0eSgpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oczQuZ2V0KCksICIiKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRQAAAByZWZfdmFsdWVfY2xhdXNlLnR4dJsCAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIGYoaW50JiB4LCBpbnQgaSwgaW50IGopIHsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCn0NCg0Kdm9pZCBnKGludCB4LCBpbnQgaSwgaW50IGopIHsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeDsNCiAgZih4LCAxLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgMSk7DQogIGYoMSwgMSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9GAAAAHJlaW50ZXJwcmV0X2Nhc3RfcmVmLnR4dLQBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBCIHsNCiAgaW50IHg7DQogIHZvaWQgZigpIHsNCiAgICB0aGlzLngrKzsNCiAgfQ0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICB2b2lkIGYyKCkgew0KICAgIHJlaW50ZXJwcmV0X2Nhc3QodGhpcywgQik7DQoJdGhpcy5mKCk7DQogICAgZih0aGlzKTsNCiAgfQ0KDQogIHZvaWQgZjMoKSB7DQogICAgcmVpbnRlcnByZXRfY2FzdCh0aGlzLCBCKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy54ID0gMTAwOw0KICBjLmYyKCk7DQogIGMuZjMoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy54LCAxMDQpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KDwAAAHNlbGYtaGVhbHRoLnR4dI4AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KLAAAAHN0cnVjdC1hbmQtcHJpbWl0aXZlLXR5cGUtd2l0aC1zYW1lLW5hbWUudHh04gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnR5cGVkZWYgaW50IElOVDsNCg0Kc3RydWN0IElOVCB7DQogIGludCB4Ow0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBJTlQgaTsNCiAgaS54ID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLngsIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQolAAAAc3RydWN0X21lbWJlcl90eXBlX3dpdGhfbmFtZXNwYWNlLnR4dPgAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpuYW1lc3BhY2UgTiB7DQpzdHJ1Y3QgUyB7DQogIDo6YjJzdHlsZTo6aW50IGk7DQp9Ow0KfSAgLy8gbmFtZXNwYWNlIE4NCg0Kdm9pZCBtYWluKCkgew0KICBOOjpTIHM7DQogIHMuaSA9IDEwMDsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4ocy5pLCAxMDApOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRkAAAB0ZW1wbGF0ZS1pbi1uYW1lc3BhY2UudHh09gEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCm5hbWVzcGFjZSBOIHsNCg0KdHlwZWRlZiA6OnZvaWQgdm9pZDsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKCkgew0KICAgIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KZGVsZWdhdGUgdm9pZCBmKCk7DQoNCnRlbXBsYXRlIDxUPg0Kdm9pZCBmMigpIHsNCiAgICA6OmIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KfQ0KDQp9ICAvLyBuYW1lc3BhY2UgTg0KDQp2b2lkIGYoKSB7DQogIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgTjo6QzxpbnQ+IGM7DQogIGMuZigpOw0KICBOOjpmPGludD4gZjIgPSBmOw0KICBmMigpOw0KICBOOjpmMjxpbnQ+KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoVAAAAdGVtcGxhdGUtb3ZlcnJpZGUudHh0ewMAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIE91dFdyaXRlciB7DQogIGludCBjOw0KDQogIHZvaWQgd3JpdGUoc3RyaW5nIG1zZykgew0KICAgIHRoaXMuYysrOw0KICAgIC8vIERvIG5vdCByZWFsbHkgd3JpdGUgdG8gc3Rkb3V0LCBpdCB3aWxsIG1hc3MgdXAgdGhlIHRlc3RpbmcgYXNzZXJ0aW9ucy4NCiAgICBiMnN0eWxlOjpzdGRfZXJyKG1zZyk7DQogIH0NCn07DQoNCmNsYXNzIEVycldyaXRlciB7DQogIGludCBjOw0KDQogIHZvaWQgd3JpdGUoc3RyaW5nIG1zZykgew0KICAgIHRoaXMuYysrOw0KICAgIGIyc3R5bGU6OnN0ZF9lcnIobXNnKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFc+DQpjbGFzcyBMb2dnZXIgew0KICBXIHc7DQoNCiAgdm9pZCBsb2coc3RyaW5nIG1zZykgew0KICAgIHRoaXMudy53cml0ZShtc2cpOw0KICB9DQoNCiAgaW50IGNvdW50X29mX2xvZ19saW5lcygpIHsNCiAgICByZXR1cm4gdGhpcy53LmM7DQogIH0NCn07DQoNCnR5cGVkZWYgTG9nZ2VyPE91dFdyaXRlcj4gT3V0TG9nZ2VyOw0KdHlwZWRlZiBMb2dnZXI8RXJyV3JpdGVyPiBFcnJMb2dnZXI7DQoNCnZvaWQgbWFpbigpIHsNCiAgT3V0TG9nZ2VyIG9sOw0KICBFcnJMb2dnZXIgZWw7DQoNCiAgb2wubG9nKCJvdXQiKTsNCiAgb2wubG9nKCJvdXQiKTsNCiAgb2wubG9nKCJvdXQiKTsNCiAgZWwubG9nKCJlcnIiKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihvbC5jb3VudF9vZl9sb2dfbGluZXMoKSwgMyk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGVsLmNvdW50X29mX2xvZ19saW5lcygpLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChsAAAB0ZW1wbGF0ZV9jbGFzc193aXRoX3JlZi50eHQBAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKFQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMzIHsNCiAgdm9pZCBmKFQgdCkgew0KICAgIGludCB4ID0gMDsNCgl0LmYoeCk7DQoJYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAxKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8aW50Jj4gYzI7DQogIC8vIFRPRE86IFJlbW92ZSByaWdodC1zaGlmdCBhbmQgYXZvaWQgYWRkaW5nIGEgc3BhY2UgYmV0d2VlbiB0d28gPj4uDQogIEMzPEM8aW50PiA+IHI7DQogIHIuZihjKTsNCiAgQzM8QzI8aW50Jj4gPiByMjsNCiAgcjIuZihjMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9HgAAAHR3by1jbG9zaW5nLWFuZ2xlLWJyYWNrZXRzLnR4dEMBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoVCB0LCBUIHQyKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodCA9PSB0Mik7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB0KSB7DQogICAgdC5mKDEwMCwgMTAwKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8QzxpbnQ+PiBjMjsNCiAgYzIuZihjKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRIAAAB1bm1hbmFnZWRfaGVhcC50eHRWAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBhcnJheSB7DQogIFQgYTsNCg0KICB2b2lkIGNvbnN0cnVjdChpbnQgc2l6ZSkgew0KICAgIFQgeFtzaXplXTsNCgl0aGlzLmEgPSB4Ow0KCWxvZ2ljICJ1bmRlZmluZSB4IjsNCiAgfQ0KDQogIHZvaWQgZGVzdHJ1Y3QoKSB7DQogICAgbG9naWMgImRlYWxsb2NfaGVhcCB0aGlzLmEiOw0KICB9DQoNCiAgVCBnZXQoaW50IGluZGV4KSB7DQogICAgcmV0dXJuIHRoaXMuYVtpbmRleF07DQogIH0NCg0KICB2b2lkIHNldChpbnQgaW5kZXgsIFQgdikgew0KICAgIHRoaXMuYVtpbmRleF0gPSB2Ow0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIGFycmF5PGludD4gYSgxMDApOw0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBhLnNldChpLCBpICsgMSk7DQogIH0NCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGEuZ2V0KGkpLCBpICsgMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChsAAAB1c2UtYmFzZS1jbGFzcy1mdW5jdGlvbi50eHSmAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQiB7DQogIGludCB4Ow0KICB2b2lkIGYoKSB7DQoJdGhpcy54Kys7DQogIH0NCn07DQoNCmNsYXNzIEIyOiBCIHsNCiAgdm9pZCBmMigpIHsNCiAgICB0aGlzLngrPTI7DQogIH0NCn07DQoNCmNsYXNzIEIzIHsNCiAgdm9pZCBmaW5pc2goKSB7DQoJYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCiAgfQ0KfTsNCg0KY2xhc3MgQzogQjIsIEIzIHsNCiAgdm9pZCBmMygpIHsNCiAgICB0aGlzLngrPTM7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLmYoKTsNCiAgYy5mMigpOw0KICBjLmYzKCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueCwgNik7DQogIGMuZmluaXNoKCk7DQp9DQoPAAAAdmVjdG9yLXRlc3QudHh0WAIAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDxzdGQvdmVjdG9yPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBzdGQ6OnZlY3RvcjxpbnQ+IHYoKTsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDsgaSsrKSB7DQogICAgdi5wdXNoX2JhY2soaSArIDEpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHYuc2l6ZSgpLCAxMCk7DQoNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4odi5nZXQoaSksIGkgKyAxKTsNCiAgfQ0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIHYuc2V0KGksIGkgKyAyKTsNCiAgfQ0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHYuZ2V0KGkpLCBpICsgMik7DQogIH0NCg0KICB2LmNsZWFyKCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHYuc2l6ZSgpLCAwKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfSEAAAB2aXJ0dWFsLWZ1bmN0aW9uLWNvbXBpbGUtb25seS50eHTlAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQyB7DQogIG92ZXJyaWRhYmxlIHZvaWQgZigpIHt9DQp9Ow0KDQpjbGFzcyBEIDogQyB7DQogIG92ZXJyaWRlIHZvaWQgZigpIHt9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgRCBkOw0KDQogIGMuZigpOw0KICBkLmYoKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfREAAAB2b2lkLXZhcmlhYmxlLnR4dN4AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIHZvaWQgdjsNCiAgdm9pZCB2MlsxMDBdOw0KICAvLyB2b2lkIGlzIG5vdCBhc3NpZ25hYmxlLCBidXQgZGVmaW5pbmcgYSB2YXJpYWJsZSB3aXRoIHZvaWQgdHlwZSBzaG91bGQgc3RpbGwgYmUgYWxsb3dlZC4NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0="
        ))
End Class
