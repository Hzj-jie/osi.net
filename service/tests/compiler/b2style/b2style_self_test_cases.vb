Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(13402), 
            "GgAAAGFjY2Vzcy1iaWd1aW50LWFzLWhlYXAudHh0PQEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHhbMTAwXTsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIHhbaV0gPSBpICsgMTsNCiAgfQ0KICB0eXBlX3B0ciB5ID0geDsNCiAgcmVpbnRlcnByZXRfY2FzdCh5LCBpbnQpOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeVtpXSwgaSArIDEpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EAAAAGFzc2VydC1lcXVhbC50eHTYAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50LCBpbnQ+KDEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nLCBzdHJpbmc+KCJhYmMiLCAiYWJjIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DgAAAGJvb2xfYXJyYXkudHh0bwEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYm9vbCBhWzEwMF07DQogIGJvb2wgYjsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIGFbaV0gPSAoKGkgJSAyKSA9PSAxKTsNCiAgfQ0KICByZWludGVycHJldF9jYXN0KGIsIGludCk7DQogIGIgPSBhOw0KICByZWludGVycHJldF9jYXN0KGIsIGJvb2wpOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGJvb2w+KGJbaV0sIChpICUgMikgPT0gMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0jAAAAY2xhc3MtZnVuY3Rpb24tY2FsbC13aXRoLXNwYWNlcy50eHTuAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQyB7DQogIHZvaWQgZihpbnQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBpbnQgeCA9IDA7DQogIGMgLiBmICggeCApOw0KICBiMnN0eWxlOjogdGVzdGluZzo6IGFzc2VydF90cnVlKCB4ID09IDEgKTsNCiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBmaW5pc2hlZCgpOw0KfSQAAABjbGFzcy1pbmhlcml0YW5jZS1iYXNlLXZhcmlhYmxlcy50eHTwAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQSB7DQogIGludCBhOw0KfTsNCg0KY2xhc3MgQiB7DQogIGludCBiOw0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICBpbnQgYzsNCn07DQoNCmNsYXNzIEQgOiBBLCBDIHsNCiAgaW50IGQ7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEQgZDsNCiAgZC5hPTE7DQogIGQuYj0yOw0KICBkLmM9MzsNCiAgZC5kPTQ7DQogIC8vIFRoZSAidHlwZS1pZCJzIGFyZSBub3QgdGVzdGVkLg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmIsIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmMsIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmQsIDQpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfSIAAABjbGFzcy1pbmhlcml0YW5jZS1jb21waWxlLW9ubHkudHh05gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCn07DQoNCmNsYXNzIEMgOiBCIHsNCiAgaW50IHk7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy55ID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9KAAAAGNsYXNzLWluaGVyaXRhbmNlLXRlbXBsYXRlLXZhcmlhYmxlcy50eHThAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBCIHsNCiAgVCB4Ow0KfTsNCg0KY2xhc3MgQyA6IEI8aW50PiB7DQogIGludCB5Ow0KfTsNCg0KY2xhc3MgQzIgOiBCPHN0cmluZz4gew0KICBpbnQgeTsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLng9IDE7DQogIGMueT0yOw0KDQogIEMyIGMyOw0KICBjMi54PSJhIjsNCiAgYzIueT0zOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueCwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueSwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGMyLngsICJhIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMyLnksIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAABjb3B5LWZ1bmN0aW9uLXB0ci50eHTbAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KZGVsZWdhdGUgdm9pZCBwKCk7DQoNCnZvaWQgZigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgcCB4ID0gZjsNCiAgcCB5ID0geDsNCiAgeCgpOw0KICB5KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9KgAAAGRlZmluZS1jbGFzcy1jb25zdHJ1Y3Rvci1mb3Itbm9uLWNsYXNzLnR4dG4BAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIGNvbnN0cnVjdChpbnQmIHRoaXMsIGludCB2KSB7DQogIHRoaXMgPSB2ICsgMTsNCn0NCg0Kdm9pZCBjb25zdHJ1Y3QoaW50JiB0aGlzKSB7DQogIHRoaXMuY29uc3RydWN0KDEpOw0KfQ0KDQp2b2lkIGRlc3RydWN0KGludCYgdGhpcykge30NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeCgxMDApOw0KICBpbnQgeSgpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCAxMDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih5LCAyKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0lAAAAZGVsZWdhdGUtZnVuY3Rpb25zLXdpdGgtc2FtZS1uYW1lLnR4dJcBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8YjJzdHlsZS9kZWxlZ2F0ZXMuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmludCBmKGludCB4KSB7DQogIHJldHVybiB4ICsgMTsNCn0NCg0Kc3RyaW5nIGYoc3RyaW5nIHgpIHsNCiAgcmV0dXJuIHg7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6ZnVuY3Rpb248aW50LCBpbnQ+IGQgPSBmOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkKDEpLCAyKTsNCg0KICBiMnN0eWxlOjpmdW5jdGlvbjxzdHJpbmcsIHN0cmluZz4gZDIgPSBmOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihkMigiYWJjIiksICJhYmMiKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAABmdW5jdGlvbi10ZW1wbGF0ZS50eHQlAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpUIGFkZChUJiB4LCBUIHkpIHsNCiAgeCArPSB5Ow0KICByZXR1cm4geDsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeCA9IDE7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKGFkZDxpbnQ+KHgsIDEpID09IDIpOw0KICBhZGQ8aW50Pih4LCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0IAAAAaGVhcC50eHT+AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL3N0ZGlvLmg+DQoNCnN0cnVjdCBTIHsNCiAgaW50IHg7DQogIHN0cmluZyB5Ow0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBTIHM7DQogIHsNCiAgICBTIHhbMTBdOw0KICAgIHMgPSB4Ow0KICAgIHVuZGVmaW5lKHgpOw0KICB9DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIHNbaV0ueCA9IGkgKyAxOw0KICAgIHNbaV0ueSA9IGIyc3R5bGU6OmludF90b19zdHIoaSArIDIpOw0KICB9DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHNbaV0ueCwgaSArIDEpOw0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KHNbaV0ueSwgYjJzdHlsZTo6aW50X3RvX3N0cihpICsgMikpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EQAAAGhlYXBfcHRyX3Rlc3QudHh0ZwMAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQojaW5jbHVkZSA8YjJzdHlsZS9zdGRpby5oPg0KDQpzdHJ1Y3QgUyB7DQogIGludCB4Ow0KICBzdHJpbmcgeTsNCn07DQoNCiNpbmNsdWRlIDxiMnN0eWxlL2hlYXBfcHRyLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6aGVhcF9wdHI8aW50PiBoMSgxMDApOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihoMS5zaXplKCksIDEwMCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBoMS5zZXQoaSwgaSArIDEpOw0KICB9DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihoMS5nZXQoaSksIGkgKyAxKTsNCiAgfQ0KDQogIGIyc3R5bGU6OmhlYXBfcHRyPFM+IGgyKDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGgyLnNpemUoKSwgMTAwKTsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIFMgczsNCiAgICBzLnggPSBpICsgMTsNCiAgICBzLnkgPSBiMnN0eWxlOjppbnRfdG9fc3RyKGkgKyAyKTsNCiAgICBoMi5zZXQoaSwgcyk7DQogIH0NCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIFMgcyA9IGgyLmdldChpKTsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzLngsIGkgKyAxKTsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihzLnksIGIyc3R5bGU6OmludF90b19zdHIoaSArIDIpKTsNCiAgfQ0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoTAAAAbW9kX2VxdWFsc190b18xLnR4dM0AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZSgoKDEwICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSgoKDExICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KHwAAAG1vcmUtY2xvc2luZy1hbmdsZS1icmFja2V0cy50eHQ0AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKCkgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMyIHsNCiAgdm9pZCBmKCkgew0KICAgIFQgdDsNCgl0LmYoKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDMjxDMjxDMjxDMjxDMjxDPHZvaWQ+Pj4+Pj4gYzsNCiAgYy5mKCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQofAAAAcmVkZWZpbmUtc3RydWN0LW1lbWJlci10eXBlLnR4dPUAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0eXBlZGVmIGludCBJTlQ7DQoNCnN0cnVjdCBTIHsNCiAgSU5UIGk7DQp9Ow0KDQp0eXBlZGVmIHN0cmluZyBJTlQ7DQoNCnZvaWQgbWFpbigpIHsNCiAgUyBzOw0KICBzLmkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHMuaSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCgwAAAByZWZfdGVzdC50eHQ9AgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPGIyc3R5bGUvcmVmLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHMoKTsNCiAgYjJzdHlsZTo6cmVmPHN0cmluZz4gczIoImFiYyIpOw0KICBiMnN0eWxlOjpyZWY8c3RyaW5nPiBzMyhzMik7DQogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHM0KCk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUocy5lbXB0eSgpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoczIuZW1wdHkoKSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZShzMy5lbXB0eSgpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oczMuZ2V0KCksICJhYmMiKTsNCiAgczQuYWxsb2MoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2ZhbHNlKHM0LmVtcHR5KCkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihzNC5nZXQoKSwgIiIpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9FAAAAHJlZl92YWx1ZV9jbGF1c2UudHh0mwIAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgZihpbnQmIHgsIGludCBpLCBpbnQgaikgew0KICB4ID0gaTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oaSwgaik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIGopOw0KICB4ID0gaTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oaSwgaik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIGopOw0KfQ0KDQp2b2lkIGcoaW50IHgsIGludCBpLCBpbnQgaikgew0KICB4ID0gaTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oaSwgaik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIGopOw0KICB4ID0gaTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oaSwgaik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIGopOw0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIGludCB4Ow0KICBmKHgsIDEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCAxKTsNCiAgZigxLCAxLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0YAAAAcmVpbnRlcnByZXRfY2FzdF9yZWYudHh0tAEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCiAgdm9pZCBmKCkgew0KICAgIHRoaXMueCsrOw0KICB9DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIHZvaWQgZjIoKSB7DQogICAgcmVpbnRlcnByZXRfY2FzdCh0aGlzLCBCKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQoNCiAgdm9pZCBmMygpIHsNCiAgICByZWludGVycHJldF9jYXN0KHRoaXMsIEIpOw0KCXRoaXMuZigpOw0KICAgIGYodGhpcyk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLnggPSAxMDA7DQogIGMuZjIoKTsNCiAgYy5mMygpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDEwNCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoPAAAAc2VsZi1oZWFsdGgudHh0jgAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQosAAAAc3RydWN0LWFuZC1wcmltaXRpdmUtdHlwZS13aXRoLXNhbWUtbmFtZS50eHTiAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdHlwZWRlZiBpbnQgSU5UOw0KDQpzdHJ1Y3QgSU5UIHsNCiAgaW50IHg7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIElOVCBpOw0KICBpLnggPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGkueCwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCiUAAABzdHJ1Y3RfbWVtYmVyX3R5cGVfd2l0aF9uYW1lc3BhY2UudHh0+AAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCm5hbWVzcGFjZSBOIHsNCnN0cnVjdCBTIHsNCiAgOjpiMnN0eWxlOjppbnQgaTsNCn07DQp9ICAvLyBuYW1lc3BhY2UgTg0KDQp2b2lkIG1haW4oKSB7DQogIE46OlMgczsNCiAgcy5pID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzLmksIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9GQAAAHRlbXBsYXRlLWluLW5hbWVzcGFjZS50eHT2AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KbmFtZXNwYWNlIE4gew0KDQp0eXBlZGVmIDo6dm9pZCB2b2lkOw0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoKSB7DQogICAgOjpiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpkZWxlZ2F0ZSB2b2lkIGYoKTsNCg0KdGVtcGxhdGUgPFQ+DQp2b2lkIGYyKCkgew0KICAgIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSBODQoNCnZvaWQgZigpIHsNCiAgOjpiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBOOjpDPGludD4gYzsNCiAgYy5mKCk7DQogIE46OmY8aW50PiBmMiA9IGY7DQogIGYyKCk7DQogIE46OmYyPGludD4oKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChUAAAB0ZW1wbGF0ZS1vdmVycmlkZS50eHR7AwAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgT3V0V3JpdGVyIHsNCiAgaW50IGM7DQoNCiAgdm9pZCB3cml0ZShzdHJpbmcgbXNnKSB7DQogICAgdGhpcy5jKys7DQogICAgLy8gRG8gbm90IHJlYWxseSB3cml0ZSB0byBzdGRvdXQsIGl0IHdpbGwgbWFzcyB1cCB0aGUgdGVzdGluZyBhc3NlcnRpb25zLg0KICAgIGIyc3R5bGU6OnN0ZF9lcnIobXNnKTsNCiAgfQ0KfTsNCg0KY2xhc3MgRXJyV3JpdGVyIHsNCiAgaW50IGM7DQoNCiAgdm9pZCB3cml0ZShzdHJpbmcgbXNnKSB7DQogICAgdGhpcy5jKys7DQogICAgYjJzdHlsZTo6c3RkX2Vycihtc2cpOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8Vz4NCmNsYXNzIExvZ2dlciB7DQogIFcgdzsNCg0KICB2b2lkIGxvZyhzdHJpbmcgbXNnKSB7DQogICAgdGhpcy53LndyaXRlKG1zZyk7DQogIH0NCg0KICBpbnQgY291bnRfb2ZfbG9nX2xpbmVzKCkgew0KICAgIHJldHVybiB0aGlzLncuYzsNCiAgfQ0KfTsNCg0KdHlwZWRlZiBMb2dnZXI8T3V0V3JpdGVyPiBPdXRMb2dnZXI7DQp0eXBlZGVmIExvZ2dlcjxFcnJXcml0ZXI+IEVyckxvZ2dlcjsNCg0Kdm9pZCBtYWluKCkgew0KICBPdXRMb2dnZXIgb2w7DQogIEVyckxvZ2dlciBlbDsNCg0KICBvbC5sb2coIm91dCIpOw0KICBvbC5sb2coIm91dCIpOw0KICBvbC5sb2coIm91dCIpOw0KICBlbC5sb2coImVyciIpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KG9sLmNvdW50X29mX2xvZ19saW5lcygpLCAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZWwuY291bnRfb2ZfbG9nX2xpbmVzKCksIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KGwAAAHRlbXBsYXRlX2NsYXNzX3dpdGhfcmVmLnR4dAECAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoVCYgeCkgew0KICAgIHgrKzsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMiB7DQogIHZvaWQgZihUIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzMgew0KICB2b2lkIGYoVCB0KSB7DQogICAgaW50IHggPSAwOw0KCXQuZih4KTsNCgliMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh4ID09IDEpOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEM8aW50PiBjOw0KICBDMjxpbnQmPiBjMjsNCiAgLy8gVE9ETzogUmVtb3ZlIHJpZ2h0LXNoaWZ0IGFuZCBhdm9pZCBhZGRpbmcgYSBzcGFjZSBiZXR3ZWVuIHR3byA+Pi4NCiAgQzM8QzxpbnQ+ID4gcjsNCiAgci5mKGMpOw0KICBDMzxDMjxpbnQmPiA+IHIyOw0KICByMi5mKGMyKTsNCiAgYjJzdHlsZTo6dGVz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"
        ))
End Class
