Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(11108), 
            "JAAAAGNsYXNzLWluaGVyaXRhbmNlLWJhc2UtdmFyaWFibGVzLnR4dPABAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBBIHsNCiAgaW50IGE7DQp9Ow0KDQpjbGFzcyBCIHsNCiAgaW50IGI7DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIGludCBjOw0KfTsNCg0KY2xhc3MgRCA6IEEsIEMgew0KICBpbnQgZDsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgRCBkOw0KICBkLmE9MTsNCiAgZC5iPTI7DQogIGQuYz0zOw0KICBkLmQ9NDsNCiAgLy8gVGhlICJ0eXBlLWlkInMgYXJlIG5vdCB0ZXN0ZWQuDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYiwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYywgMyk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuZCwgNCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9JQAAAHN0cnVjdF9tZW1iZXJfdHlwZV93aXRoX25hbWVzcGFjZS50eHT4AAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KbmFtZXNwYWNlIE4gew0Kc3RydWN0IFMgew0KICA6OmIyc3R5bGU6OmludCBpOw0KfTsNCn0gIC8vIG5hbWVzcGFjZSBODQoNCnZvaWQgbWFpbigpIHsNCiAgTjo6UyBzOw0KICBzLmkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHMuaSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0qAAAAZGVmaW5lLWNsYXNzLWNvbnN0cnVjdG9yLWZvci1ub24tY2xhc3MudHh0bgEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgY29uc3RydWN0KGludCYgdGhpcywgaW50IHYpIHsNCiAgdGhpcyA9IHYgKyAxOw0KfQ0KDQp2b2lkIGNvbnN0cnVjdChpbnQmIHRoaXMpIHsNCiAgdGhpcy5jb25zdHJ1Y3QoMSk7DQp9DQoNCnZvaWQgZGVzdHJ1Y3QoaW50JiB0aGlzKSB7fQ0KDQp2b2lkIG1haW4oKSB7DQogIGludCB4KDEwMCk7DQogIGludCB5KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIDEwMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHksIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRsAAAB0ZW1wbGF0ZV9jbGFzc193aXRoX3JlZi50eHQBAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKFQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMzIHsNCiAgdm9pZCBmKFQgdCkgew0KICAgIGludCB4ID0gMDsNCgl0LmYoeCk7DQoJYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAxKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8aW50Jj4gYzI7DQogIC8vIFRPRE86IFJlbW92ZSByaWdodC1zaGlmdCBhbmQgYXZvaWQgYWRkaW5nIGEgc3BhY2UgYmV0d2VlbiB0d28gPj4uDQogIEMzPEM8aW50PiA+IHI7DQogIHIuZihjKTsNCiAgQzM8QzI8aW50Jj4gPiByMjsNCiAgcjIuZihjMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9IgAAAGNsYXNzLWluaGVyaXRhbmNlLWNvbXBpbGUtb25seS50eHTmAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQiB7DQogIGludCB4Ow0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICBpbnQgeTsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLnkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0PAAAAc2VsZi1oZWFsdGgudHh0jgAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoaAAAAYWNjZXNzLWJpZ3VpbnQtYXMtaGVhcC50eHQ9AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeFsxMDBdOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgeFtpXSA9IGkgKyAxOw0KICB9DQogIHR5cGVfcHRyIHkgPSB4Ow0KICByZWludGVycHJldF9jYXN0KHksIGludCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih5W2ldLCBpICsgMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0eAAAAdHdvLWNsb3NpbmctYW5nbGUtYnJhY2tldHMudHh0QwEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQyB7DQogIHZvaWQgZihUIHQsIFQgdDIpIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0ID09IHQyKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMiB7DQogIHZvaWQgZihUIHQpIHsNCiAgICB0LmYoMTAwLCAxMDApOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEM8aW50PiBjOw0KICBDMjxDPGludD4+IGMyOw0KICBjMi5mKGMpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DwAAAHZlY3Rvci10ZXN0LnR4dFgCAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8c3RkL3ZlY3Rvcj4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgc3RkOjp2ZWN0b3I8aW50PiB2KCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIHYucHVzaF9iYWNrKGkgKyAxKTsNCiAgfQ0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LnNpemUoKSwgMTApOw0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHYuZ2V0KGkpLCBpICsgMSk7DQogIH0NCg0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsNCiAgICB2LnNldChpLCBpICsgMik7DQogIH0NCg0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LmdldChpKSwgaSArIDIpOw0KICB9DQoNCiAgdi5jbGVhcigpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LnNpemUoKSwgMCk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0TAAAAbW9kX2VxdWFsc190b18xLnR4dM0AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZSgoKDEwICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSgoKDExICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KIwAAAGNsYXNzLWZ1bmN0aW9uLWNhbGwtd2l0aC1zcGFjZXMudHh07gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEMgew0KICB2b2lkIGYoaW50JiB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgaW50IHggPSAwOw0KICBjIC4gZiAoIHggKTsNCiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBhc3NlcnRfdHJ1ZSggeCA9PSAxICk7DQogIGIyc3R5bGU6OiB0ZXN0aW5nOjogZmluaXNoZWQoKTsNCn0QAAAAYXNzZXJ0LWVxdWFsLnR4dNgAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQsIGludD4oMSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmcsIHN0cmluZz4oImFiYyIsICJhYmMiKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0lAAAAZGVsZWdhdGUtZnVuY3Rpb25zLXdpdGgtc2FtZS1uYW1lLnR4dHcBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQppbnQgZihpbnQgeCkgew0KICByZXR1cm4geCArIDE7DQp9DQoNCnN0cmluZyBmKHN0cmluZyB4KSB7DQogIHJldHVybiB4Ow0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OmZ1bmN0aW9uPGludCwgaW50PiBkID0gZjsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZCgxKSwgMik7DQoNCiAgYjJzdHlsZTo6ZnVuY3Rpb248c3RyaW5nLCBzdHJpbmc+IGQyID0gZjsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oZDIoImFiYyIpLCAiYWJjIik7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0UAAAAcmVmX3ZhbHVlX2NsYXVzZS50eHSbAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBmKGludCYgeCwgaW50IGksIGludCBqKSB7DQogIHggPSBpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLCBqKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgaik7DQogIHggPSBpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLCBqKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgaik7DQp9DQoNCnZvaWQgZyhpbnQgeCwgaW50IGksIGludCBqKSB7DQogIHggPSBpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLCBqKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgaik7DQogIHggPSBpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLCBqKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgaik7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgaW50IHg7DQogIGYoeCwgMSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIDEpOw0KICBmKDEsIDEsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRkAAAB0ZW1wbGF0ZS1pbi1uYW1lc3BhY2UudHh09gEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCm5hbWVzcGFjZSBOIHsNCg0KdHlwZWRlZiA6OnZvaWQgdm9pZDsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKCkgew0KICAgIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KZGVsZWdhdGUgdm9pZCBmKCk7DQoNCnRlbXBsYXRlIDxUPg0Kdm9pZCBmMigpIHsNCiAgICA6OmIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOw0KfQ0KDQp9ICAvLyBuYW1lc3BhY2UgTg0KDQp2b2lkIGYoKSB7DQogIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgTjo6QzxpbnQ+IGM7DQogIGMuZigpOw0KICBOOjpmPGludD4gZjIgPSBmOw0KICBmMigpOw0KICBOOjpmMjxpbnQ+KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQofAAAAcmVkZWZpbmUtc3RydWN0LW1lbWJlci10eXBlLnR4dPUAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0eXBlZGVmIGludCBJTlQ7DQoNCnN0cnVjdCBTIHsNCiAgSU5UIGk7DQp9Ow0KDQp0eXBlZGVmIHN0cmluZyBJTlQ7DQoNCnZvaWQgbWFpbigpIHsNCiAgUyBzOw0KICBzLmkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHMuaSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCiEAAAB2aXJ0dWFsLWZ1bmN0aW9uLWNvbXBpbGUtb25seS50eHTlAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQyB7DQogIG92ZXJyaWRhYmxlIHZvaWQgZigpIHt9DQp9Ow0KDQpjbGFzcyBEIDogQyB7DQogIG92ZXJyaWRlIHZvaWQgZigpIHt9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgRCBkOw0KDQogIGMuZigpOw0KICBkLmYoKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAAB0ZW1wbGF0ZS1vdmVycmlkZS50eHR7AwAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgT3V0V3JpdGVyIHsNCiAgaW50IGM7DQoNCiAgdm9pZCB3cml0ZShzdHJpbmcgbXNnKSB7DQogICAgdGhpcy5jKys7DQogICAgLy8gRG8gbm90IHJlYWxseSB3cml0ZSB0byBzdGRvdXQsIGl0IHdpbGwgbWFzcyB1cCB0aGUgdGVzdGluZyBhc3NlcnRpb25zLg0KICAgIGIyc3R5bGU6OnN0ZF9lcnIobXNnKTsNCiAgfQ0KfTsNCg0KY2xhc3MgRXJyV3JpdGVyIHsNCiAgaW50IGM7DQoNCiAgdm9pZCB3cml0ZShzdHJpbmcgbXNnKSB7DQogICAgdGhpcy5jKys7DQogICAgYjJzdHlsZTo6c3RkX2Vycihtc2cpOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8Vz4NCmNsYXNzIExvZ2dlciB7DQogIFcgdzsNCg0KICB2b2lkIGxvZyhzdHJpbmcgbXNnKSB7DQogICAgdGhpcy53LndyaXRlKG1zZyk7DQogIH0NCg0KICBpbnQgY291bnRfb2ZfbG9nX2xpbmVzKCkgew0KICAgIHJldHVybiB0aGlzLncuYzsNCiAgfQ0KfTsNCg0KdHlwZWRlZiBMb2dnZXI8T3V0V3JpdGVyPiBPdXRMb2dnZXI7DQp0eXBlZGVmIExvZ2dlcjxFcnJXcml0ZXI+IEVyckxvZ2dlcjsNCg0Kdm9pZCBtYWluKCkgew0KICBPdXRMb2dnZXIgb2w7DQogIEVyckxvZ2dlciBlbDsNCg0KICBvbC5sb2coIm91dCIpOw0KICBvbC5sb2coIm91dCIpOw0KICBvbC5sb2coIm91dCIpOw0KICBlbC5sb2coImVyciIpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KG9sLmNvdW50X29mX2xvZ19saW5lcygpLCAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZWwuY291bnRfb2ZfbG9nX2xpbmVzKCksIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KGAAAAHJlaW50ZXJwcmV0X2Nhc3RfcmVmLnR4dLQBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBCIHsNCiAgaW50IHg7DQogIHZvaWQgZigpIHsNCiAgICB0aGlzLngrKzsNCiAgfQ0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICB2b2lkIGYyKCkgew0KICAgIHJlaW50ZXJwcmV0X2Nhc3QodGhpcywgQik7DQoJdGhpcy5mKCk7DQogICAgZih0aGlzKTsNCiAgfQ0KDQogIHZvaWQgZjMoKSB7DQogICAgcmVpbnRlcnByZXRfY2FzdCh0aGlzLCBCKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy54ID0gMTAwOw0KICBjLmYyKCk7DQogIGMuZjMoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy54LCAxMDQpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KGwAAAHVzZS1iYXNlLWNsYXNzLWZ1bmN0aW9uLnR4dKYBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBCIHsNCiAgaW50IHg7DQogIHZvaWQgZigpIHsNCgl0aGlzLngrKzsNCiAgfQ0KfTsNCg0KY2xhc3MgQjI6IEIgew0KICB2b2lkIGYyKCkgew0KICAgIHRoaXMueCs9MjsNCiAgfQ0KfTsNCg0KY2xhc3MgQjMgew0KICB2b2lkIGZpbmlzaCgpIHsNCgliMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KICB9DQp9Ow0KDQpjbGFzcyBDOiBCMiwgQjMgew0KICB2b2lkIGYzKCkgew0KICAgIHRoaXMueCs9MzsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDIGM7DQogIGMuZigpOw0KICBjLmYyKCk7DQogIGMuZjMoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy54LCA2KTsNCiAgYy5maW5pc2goKTsNCn0NCigAAABjbGFzcy1pbmhlcml0YW5jZS10ZW1wbGF0ZS12YXJpYWJsZXMudHh04QEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQiB7DQogIFQgeDsNCn07DQoNCmNsYXNzIEMgOiBCPGludD4gew0KICBpbnQgeTsNCn07DQoNCmNsYXNzIEMyIDogQjxzdHJpbmc+IHsNCiAgaW50IHk7DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy54PSAxOw0KICBjLnk9MjsNCg0KICBDMiBjMjsNCiAgYzIueD0iYSI7DQogIGMyLnk9MzsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDEpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihjMi54LCAiYSIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjMi55LCAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0OAAAAYm9vbF9hcnJheS50eHRvAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBib29sIGFbMTAwXTsNCiAgYm9vbCBiOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYVtpXSA9ICgoaSAlIDIpID09IDEpOw0KICB9DQogIHJlaW50ZXJwcmV0X2Nhc3QoYiwgaW50KTsNCiAgYiA9IGE7DQogIHJlaW50ZXJwcmV0X2Nhc3QoYiwgYm9vbCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8Ym9vbD4oYltpXSwgKGkgJSAyKSA9PSAxKTsNCiAgfQ0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAABjb3B5LWZ1bmN0aW9uLXB0ci50eHTbAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KZGVsZWdhdGUgdm9pZCBwKCk7DQoNCnZvaWQgZigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgcCB4ID0gZjsNCiAgcCB5ID0geDsNCiAgeCgpOw0KICB5KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9LAAAAHN0cnVjdC1hbmQtcHJpbWl0aXZlLXR5cGUtd2l0aC1zYW1lLW5hbWUudHh04gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnR5cGVkZWYgaW50IElOVDsNCg0Kc3RydWN0IElOVCB7DQogIGludCB4Ow0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBJTlQgaTsNCiAgaS54ID0gMTAwOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLngsIDEwMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQofAAAAbW9yZS1jbG9zaW5nLWFuZ2xlLWJyYWNrZXRzLnR4dDQBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoKSB7DQogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoKSB7DQogICAgVCB0Ow0KCXQuZigpOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMyPEMyPEMyPEMyPEMyPEM8dm9pZD4+Pj4+PiBjOw0KICBjLmYoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChUAAABmdW5jdGlvbi10ZW1wbGF0ZS50eHQlAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpUIGFkZChUJiB4LCBUIHkpIHsNCiAgeCArPSB5Ow0KICByZXR1cm4geDsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeCA9IDE7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKGFkZDxpbnQ+KHgsIDEpID09IDIpOw0KICBhZGQ8aW50Pih4LCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAzKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0SAAAAdW5tYW5hZ2VkX2hlYXAudHh0VgIAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgYXJyYXkgew0KICBUIGE7DQoNCiAgdm9pZCBjb25zdHJ1Y3QoaW50IHNpemUpIHsNCiAgICBUIHhbc2l6ZV07DQoJdGhpcy5hID0geDsNCglsb2dpYyAidW5kZWZpbmUgeCI7DQogIH0NCg0KICB2b2lkIGRlc3RydWN0KCkgew0KICAgIGxvZ2ljICJkZWFsbG9jX2hlYXAgdGhpcy5hIjsNCiAgfQ0KDQogIFQgZ2V0KGludCBpbmRleCkgew0KICAgIHJldHVybiB0aGlzLmFbaW5kZXhdOw0KICB9DQoNCiAgdm9pZCBzZXQoaW50IGluZGV4LCBUIHYpIHsNCiAgICB0aGlzLmFbaW5kZXhdID0gdjsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBhcnJheTxpbnQ+IGEoMTAwKTsNCg0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgYS5zZXQoaSwgaSArIDEpOw0KICB9DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihhLmdldChpKSwgaSArIDEpOw0KICB9DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQo="
        ))
End Class
