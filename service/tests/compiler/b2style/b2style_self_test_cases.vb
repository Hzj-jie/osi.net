Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(12197), 
            "JAAAAGNsYXNzLWluaGVyaXRhbmNlLWJhc2UtdmFyaWFibGVzLnR4dPABAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBBIHsNCiAgaW50IGE7DQp9Ow0KDQpjbGFzcyBCIHsNCiAgaW50IGI7DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIGludCBjOw0KfTsNCg0KY2xhc3MgRCA6IEEsIEMgew0KICBpbnQgZDsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgRCBkOw0KICBkLmE9MTsNCiAgZC5iPTI7DQogIGQuYz0zOw0KICBkLmQ9NDsNCiAgLy8gVGhlICJ0eXBlLWlkInMgYXJlIG5vdCB0ZXN0ZWQuDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYiwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYywgMyk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuZCwgNCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9JQAAAHN0cnVjdF9tZW1iZXJfdHlwZV93aXRoX25hbWVzcGFjZS50eHT4AAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KbmFtZXNwYWNlIE4gew0Kc3RydWN0IFMgew0KICA6OmIyc3R5bGU6OmludCBpOw0KfTsNCn0gIC8vIG5hbWVzcGFjZSBODQoNCnZvaWQgbWFpbigpIHsNCiAgTjo6UyBzOw0KICBzLmkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHMuaSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0qAAAAZGVmaW5lLWNsYXNzLWNvbnN0cnVjdG9yLWZvci1ub24tY2xhc3MudHh0bgEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgY29uc3RydWN0KGludCYgdGhpcywgaW50IHYpIHsNCiAgdGhpcyA9IHYgKyAxOw0KfQ0KDQp2b2lkIGNvbnN0cnVjdChpbnQmIHRoaXMpIHsNCiAgdGhpcy5jb25zdHJ1Y3QoMSk7DQp9DQoNCnZvaWQgZGVzdHJ1Y3QoaW50JiB0aGlzKSB7fQ0KDQp2b2lkIG1haW4oKSB7DQogIGludCB4KDEwMCk7DQogIGludCB5KCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHgsIDEwMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHksIDIpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRsAAAB0ZW1wbGF0ZV9jbGFzc193aXRoX3JlZi50eHQBAgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDIHsNCiAgdm9pZCBmKFQmIHgpIHsNCiAgICB4Kys7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQzIgew0KICB2b2lkIGYoVCB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMzIHsNCiAgdm9pZCBmKFQgdCkgew0KICAgIGludCB4ID0gMDsNCgl0LmYoeCk7DQoJYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAxKTsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDPGludD4gYzsNCiAgQzI8aW50Jj4gYzI7DQogIC8vIFRPRE86IFJlbW92ZSByaWdodC1zaGlmdCBhbmQgYXZvaWQgYWRkaW5nIGEgc3BhY2UgYmV0d2VlbiB0d28gPj4uDQogIEMzPEM8aW50PiA+IHI7DQogIHIuZihjKTsNCiAgQzM8QzI8aW50Jj4gPiByMjsNCiAgcjIuZihjMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9IgAAAGNsYXNzLWluaGVyaXRhbmNlLWNvbXBpbGUtb25seS50eHTmAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQiB7DQogIGludCB4Ow0KfTsNCg0KY2xhc3MgQyA6IEIgew0KICBpbnQgeTsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLnkgPSAxMDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueSwgMTAwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0PAAAAc2VsZi1oZWFsdGgudHh0jgAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoaAAAAYWNjZXNzLWJpZ3VpbnQtYXMtaGVhcC50eHQ9AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeFsxMDBdOw0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7DQogICAgeFtpXSA9IGkgKyAxOw0KICB9DQogIHR5cGVfcHRyIHkgPSB4Ow0KICByZWludGVycHJldF9jYXN0KHksIGludCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih5W2ldLCBpICsgMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0eAAAAdHdvLWNsb3NpbmctYW5nbGUtYnJhY2tldHMudHh0QwEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KY2xhc3MgQyB7DQogIHZvaWQgZihUIHQsIFQgdDIpIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0ID09IHQyKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBDMiB7DQogIHZvaWQgZihUIHQpIHsNCiAgICB0LmYoMTAwLCAxMDApOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEM8aW50PiBjOw0KICBDMjxDPGludD4+IGMyOw0KICBjMi5mKGMpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DwAAAHZlY3Rvci10ZXN0LnR4dFgCAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8c3RkL3ZlY3Rvcj4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgc3RkOjp2ZWN0b3I8aW50PiB2KCk7DQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIHYucHVzaF9iYWNrKGkgKyAxKTsNCiAgfQ0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LnNpemUoKSwgMTApOw0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHYuZ2V0KGkpLCBpICsgMSk7DQogIH0NCg0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsNCiAgICB2LnNldChpLCBpICsgMik7DQogIH0NCg0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LmdldChpKSwgaSArIDIpOw0KICB9DQoNCiAgdi5jbGVhcigpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih2LnNpemUoKSwgMCk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0TAAAAbW9kX2VxdWFsc190b18xLnR4dM0AAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZSgoKDEwICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSgoKDExICUgMikgPT0gMSkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KIwAAAGNsYXNzLWZ1bmN0aW9uLWNhbGwtd2l0aC1zcGFjZXMudHh07gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEMgew0KICB2b2lkIGYoaW50JiB4KSB7DQogICAgeCsrOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgaW50IHggPSAwOw0KICBjIC4gZiAoIHggKTsNCiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBhc3NlcnRfdHJ1ZSggeCA9PSAxICk7DQogIGIyc3R5bGU6OiB0ZXN0aW5nOjogZmluaXNoZWQoKTsNCn0QAAAAYXNzZXJ0LWVxdWFsLnR4dNgAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQsIGludD4oMSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmcsIHN0cmluZz4oImFiYyIsICJhYmMiKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0lAAAAZGVsZWdhdGUtZnVuY3Rpb25zLXdpdGgtc2FtZS1uYW1lLnR4dJcBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8YjJzdHlsZS9kZWxlZ2F0ZXMuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmludCBmKGludCB4KSB7DQogIHJldHVybiB4ICsgMTsNCn0NCg0Kc3RyaW5nIGYoc3RyaW5nIHgpIHsNCiAgcmV0dXJuIHg7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6ZnVuY3Rpb248aW50LCBpbnQ+IGQgPSBmOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkKDEpLCAyKTsNCg0KICBiMnN0eWxlOjpmdW5jdGlvbjxzdHJpbmcsIHN0cmluZz4gZDIgPSBmOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihkMigiYWJjIiksICJhYmMiKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRQAAAByZWZfdmFsdWVfY2xhdXNlLnR4dJsCAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIGYoaW50JiB4LCBpbnQgaSwgaW50IGopIHsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCn0NCg0Kdm9pZCBnKGludCB4LCBpbnQgaSwgaW50IGopIHsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCiAgeCA9IGk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGksIGopOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCBqKTsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgeDsNCiAgZih4LCAxLCAxKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeCwgMSk7DQogIGYoMSwgMSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EQAAAHZvaWQtdmFyaWFibGUudHh03gAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgbWFpbigpIHsNCiAgdm9pZCB2Ow0KICB2b2lkIHYyWzEwMF07DQogIC8vIHZvaWQgaXMgbm90IGFzc2lnbmFibGUsIGJ1dCBkZWZpbmluZyBhIHZhcmlhYmxlIHdpdGggdm9pZCB0eXBlIHNob3VsZCBzdGlsbCBiZSBhbGxvd2VkLg0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQwAAAByZWZfdGVzdC50eHQ9AgAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPGIyc3R5bGUvcmVmLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHMoKTsNCiAgYjJzdHlsZTo6cmVmPHN0cmluZz4gczIoImFiYyIpOw0KICBiMnN0eWxlOjpyZWY8c3RyaW5nPiBzMyhzMik7DQogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHM0KCk7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUocy5lbXB0eSgpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoczIuZW1wdHkoKSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZShzMy5lbXB0eSgpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oczMuZ2V0KCksICJhYmMiKTsNCiAgczQuYWxsb2MoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2ZhbHNlKHM0LmVtcHR5KCkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihzNC5nZXQoKSwgIiIpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9GQAAAHRlbXBsYXRlLWluLW5hbWVzcGFjZS50eHT2AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KbmFtZXNwYWNlIE4gew0KDQp0eXBlZGVmIDo6dm9pZCB2b2lkOw0KDQp0ZW1wbGF0ZSA8VD4NCmNsYXNzIEMgew0KICB2b2lkIGYoKSB7DQogICAgOjpiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCiAgfQ0KfTsNCg0KdGVtcGxhdGUgPFQ+DQpkZWxlZ2F0ZSB2b2lkIGYoKTsNCg0KdGVtcGxhdGUgPFQ+DQp2b2lkIGYyKCkgew0KICAgIDo6YjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7DQp9DQoNCn0gIC8vIG5hbWVzcGFjZSBODQoNCnZvaWQgZigpIHsNCiAgOjpiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBOOjpDPGludD4gYzsNCiAgYy5mKCk7DQogIE46OmY8aW50PiBmMiA9IGY7DQogIGYyKCk7DQogIE46OmYyPGludD4oKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCh8AAAByZWRlZmluZS1zdHJ1Y3QtbWVtYmVyLXR5cGUudHh09QAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnR5cGVkZWYgaW50IElOVDsNCg0Kc3RydWN0IFMgew0KICBJTlQgaTsNCn07DQoNCnR5cGVkZWYgc3RyaW5nIElOVDsNCg0Kdm9pZCBtYWluKCkgew0KICBTIHM7DQogIHMuaSA9IDEwMDsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4ocy5pLCAxMDApOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KIQAAAHZpcnR1YWwtZnVuY3Rpb24tY29tcGlsZS1vbmx5LnR4dOUAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBDIHsNCiAgb3ZlcnJpZGFibGUgdm9pZCBmKCkge30NCn07DQoNCmNsYXNzIEQgOiBDIHsNCiAgb3ZlcnJpZGUgdm9pZCBmKCkge30NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBEIGQ7DQoNCiAgYy5mKCk7DQogIGQuZigpOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9FQAAAHRlbXBsYXRlLW92ZXJyaWRlLnR4dHsDAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBPdXRXcml0ZXIgew0KICBpbnQgYzsNCg0KICB2b2lkIHdyaXRlKHN0cmluZyBtc2cpIHsNCiAgICB0aGlzLmMrKzsNCiAgICAvLyBEbyBub3QgcmVhbGx5IHdyaXRlIHRvIHN0ZG91dCwgaXQgd2lsbCBtYXNzIHVwIHRoZSB0ZXN0aW5nIGFzc2VydGlvbnMuDQogICAgYjJzdHlsZTo6c3RkX2Vycihtc2cpOw0KICB9DQp9Ow0KDQpjbGFzcyBFcnJXcml0ZXIgew0KICBpbnQgYzsNCg0KICB2b2lkIHdyaXRlKHN0cmluZyBtc2cpIHsNCiAgICB0aGlzLmMrKzsNCiAgICBiMnN0eWxlOjpzdGRfZXJyKG1zZyk7DQogIH0NCn07DQoNCnRlbXBsYXRlIDxXPg0KY2xhc3MgTG9nZ2VyIHsNCiAgVyB3Ow0KDQogIHZvaWQgbG9nKHN0cmluZyBtc2cpIHsNCiAgICB0aGlzLncud3JpdGUobXNnKTsNCiAgfQ0KDQogIGludCBjb3VudF9vZl9sb2dfbGluZXMoKSB7DQogICAgcmV0dXJuIHRoaXMudy5jOw0KICB9DQp9Ow0KDQp0eXBlZGVmIExvZ2dlcjxPdXRXcml0ZXI+IE91dExvZ2dlcjsNCnR5cGVkZWYgTG9nZ2VyPEVycldyaXRlcj4gRXJyTG9nZ2VyOw0KDQp2b2lkIG1haW4oKSB7DQogIE91dExvZ2dlciBvbDsNCiAgRXJyTG9nZ2VyIGVsOw0KDQogIG9sLmxvZygib3V0Iik7DQogIG9sLmxvZygib3V0Iik7DQogIG9sLmxvZygib3V0Iik7DQogIGVsLmxvZygiZXJyIik7DQoNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4ob2wuY291bnRfb2ZfbG9nX2xpbmVzKCksIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihlbC5jb3VudF9vZl9sb2dfbGluZXMoKSwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoYAAAAcmVpbnRlcnByZXRfY2FzdF9yZWYudHh0tAEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCiAgdm9pZCBmKCkgew0KICAgIHRoaXMueCsrOw0KICB9DQp9Ow0KDQpjbGFzcyBDIDogQiB7DQogIHZvaWQgZjIoKSB7DQogICAgcmVpbnRlcnByZXRfY2FzdCh0aGlzLCBCKTsNCgl0aGlzLmYoKTsNCiAgICBmKHRoaXMpOw0KICB9DQoNCiAgdm9pZCBmMygpIHsNCiAgICByZWludGVycHJldF9jYXN0KHRoaXMsIEIpOw0KCXRoaXMuZigpOw0KICAgIGYodGhpcyk7DQogIH0NCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLnggPSAxMDA7DQogIGMuZjIoKTsNCiAgYy5mMygpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDEwNCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQobAAAAdXNlLWJhc2UtY2xhc3MtZnVuY3Rpb24udHh0pgEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCmNsYXNzIEIgew0KICBpbnQgeDsNCiAgdm9pZCBmKCkgew0KCXRoaXMueCsrOw0KICB9DQp9Ow0KDQpjbGFzcyBCMjogQiB7DQogIHZvaWQgZjIoKSB7DQogICAgdGhpcy54Kz0yOw0KICB9DQp9Ow0KDQpjbGFzcyBCMyB7DQogIHZvaWQgZmluaXNoKCkgew0KCWIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQogIH0NCn07DQoNCmNsYXNzIEM6IEIyLCBCMyB7DQogIHZvaWQgZjMoKSB7DQogICAgdGhpcy54Kz0zOw0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIEMgYzsNCiAgYy5mKCk7DQogIGMuZjIoKTsNCiAgYy5mMygpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDYpOw0KICBjLmZpbmlzaCgpOw0KfQ0KKAAAAGNsYXNzLWluaGVyaXRhbmNlLXRlbXBsYXRlLXZhcmlhYmxlcy50eHThAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KdGVtcGxhdGUgPFQ+DQpjbGFzcyBCIHsNCiAgVCB4Ow0KfTsNCg0KY2xhc3MgQyA6IEI8aW50PiB7DQogIGludCB5Ow0KfTsNCg0KY2xhc3MgQzIgOiBCPHN0cmluZz4gew0KICBpbnQgeTsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgQyBjOw0KICBjLng9IDE7DQogIGMueT0yOw0KDQogIEMyIGMyOw0KICBjMi54PSJhIjsNCiAgYzIueT0zOw0KDQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueCwgMSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMueSwgMik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGMyLngsICJhIik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMyLnksIDMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ4AAABib29sX2FycmF5LnR4dG8BAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQp2b2lkIG1haW4oKSB7DQogIGJvb2wgYVsxMDBdOw0KICBib29sIGI7DQogIGZvciAoaW50IGkg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IFQgdikgew0KICAgIHRoaXMuYVtpbmRleF0gPSB2Ow0KICB9DQp9Ow0KDQp2b2lkIG1haW4oKSB7DQogIGFycmF5PGludD4gYSgxMDApOw0KDQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsNCiAgICBhLnNldChpLCBpICsgMSk7DQogIH0NCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgew0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGEuZ2V0KGkpLCBpICsgMSk7DQogIH0NCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NChYAAABjYWxsLWZ1bmMtaW4tcGFyYW0udHh0uwAAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnZvaWQgZihpbnQgeCkgew0KICB4Kys7DQp9DQoNCnZvaWQgZigpIHt9DQoNCmludCBnKCkgew0KICByZXR1cm4gMTAwOw0KfQ0KDQp2b2lkIG1haW4oKSB7DQogIGYoZygpKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0="
        ))
End Class
