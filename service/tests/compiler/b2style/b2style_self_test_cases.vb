Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b2style_self_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(23063), 
            "GgAAAGFjY2Vzcy1iaWd1aW50LWFzLWhlYXAudHh0LgEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKdm9pZCBtYWluKCkgewogIGludCB4WzEwMF07CiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgewogICAgeFtpXSA9IGkgKyAxOwogIH0KICB0eXBlX3B0ciB5ID0geDsKICByZWludGVycHJldF9jYXN0KHksIGludCk7CiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgewogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oeVtpXSwgaSArIDEpOwogIH0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOwp9EAAAAGFzc2VydC1lcXVhbC50eHTQAAAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+Cgp2b2lkIG1haW4oKSB7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludCwgaW50PigxLCAxKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nLCBzdHJpbmc+KCJhYmMiLCAiYWJjIik7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfQ0AAABib29sLWhlYXAudHh0pAIAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgojaW5jbHVkZSA8YjJzdHlsZS9oZWFwX3B0ci5oPgoKdm9pZCBmKGJvb2wgcykgewogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsKICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxib29sPihzW2ldLCAoaSAlIDIpID09IDEpOwogIH0KfQoKdm9pZCBmKGIyc3R5bGU6OmhlYXBfcHRyPGJvb2w+IGgpIHsKICBmb3IgKGludCBpID0gMDsgaSA8IGguc2l6ZSgpOyBpKyspIHsKICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxib29sPihoLmdldChpKSwgKGkgJSAyKSA9PSAxKTsKICB9Cn0KCnZvaWQgbWFpbigpIHsKICBib29sIHNbMTAwXTsKICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7CiAgICBzW2ldID0gKChpICUgMikgPT0gMSk7CiAgfQogIGYocyk7CgogIGIyc3R5bGU6OmhlYXBfcHRyPGJvb2w+IGgoMTAwKTsKICBmb3IgKGludCBpID0gMDsgaSA8IGguc2l6ZSgpOyBpKyspIHsKICAgIGguc2V0KGksIChpICUgMikgPT0gMSk7CiAgfQogIGYoaCk7CgogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGIyc3R5bGU6OnRlc3Rpbmc6Ol9hc3NlcnRpb25fY291bnQsIDIwMCk7CgogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0OAAAAYm9vbF9hcnJheS50eHReAQAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+Cgp2b2lkIG1haW4oKSB7CiAgYm9vbCBhWzEwMF07CiAgYm9vbCBiOwogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsKICAgIGFbaV0gPSAoKGkgJSAyKSA9PSAxKTsKICB9CiAgcmVpbnRlcnByZXRfY2FzdChiLCBpbnQpOwogIGIgPSBhOwogIHJlaW50ZXJwcmV0X2Nhc3QoYiwgYm9vbCk7CiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDA7IGkrKykgewogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGJvb2w+KGJbaV0sIChpICUgMikgPT0gMSk7CiAgfQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0WAAAAY2FsbC1mdW5jLWluLXBhcmFtLnR4dKMAAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCnZvaWQgZihpbnQgeCkge30KCnZvaWQgZygpIHsKICBmKDEpOwp9Cgp2b2lkIGYoKSB7CiAgZygpOwp9Cgp2b2lkIG1haW4oKSB7CiAgZigpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0KGwAAAGNsYXNzLWZ1bmMtdXNlLW93bi10eXBlLnR4dE4BAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpjbGFzcyBDIHsNCiAgaW50IHg7DQoNCiAgdm9pZCBhZGQoQyB0aGF0KSB7DQogICAgdGhpcy54ICs9IHRoYXQueDsNCiAgfQ0KfTsNCg0Kdm9pZCBtYWluKCkgew0KICBDIGM7DQogIGMueCA9IDEwOw0KICBDIGQ7DQogIGQueCA9IDIwOw0KICBjLmFkZChkKTsNCg0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLngsIDMwKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC54LCAyMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DgAAAGNsYXNzLWZ1bmMudHh0HgEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKY2xhc3MgQyB7CiAgaW50IGJhc2U7CgogIGludCBpbmMoaW50IHgpIHsKICAgIHJldHVybiB4ICsgdGhpcy5iYXNlOwogIH0KCiAgdm9pZCBjb25zdHJ1Y3QoaW50IGJhc2UpIHsKCXRoaXMuYmFzZSA9IGJhc2U7CiAgfQp9OwoKdm9pZCBtYWluKCkgewogIEMgYygxKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLmluYygxKSwgMik7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfQojAAAAY2xhc3MtZnVuY3Rpb24tY2FsbC13aXRoLXNwYWNlcy50eHTeAAAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+CgpjbGFzcyBDIHsKICB2b2lkIGYoaW50JiB4KSB7CiAgICB4Kys7CiAgfQp9OwoKdm9pZCBtYWluKCkgewogIEMgYzsKICBpbnQgeCA9IDA7CiAgYyAuIGYgKCB4ICk7CiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBhc3NlcnRfdHJ1ZSggeCA9PSAxICk7CiAgYjJzdHlsZTo6IHRlc3Rpbmc6OiBmaW5pc2hlZCgpOwp9JAAAAGNsYXNzLWluaGVyaXRhbmNlLWJhc2UtdmFyaWFibGVzLnR4dNABAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCmNsYXNzIEEgewogIGludCBhOwp9OwoKY2xhc3MgQiB7CiAgaW50IGI7Cn07CgpjbGFzcyBDIDogQiB7CiAgaW50IGM7Cn07CgpjbGFzcyBEIDogQSwgQyB7CiAgaW50IGQ7Cn07Cgp2b2lkIG1haW4oKSB7CiAgRCBkOwogIGQuYT0xOwogIGQuYj0yOwogIGQuYz0zOwogIGQuZD00OwogIC8vIFRoZSAidHlwZS1pZCJzIGFyZSBub3QgdGVzdGVkLgogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuYSwgMSk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oZC5iLCAyKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkLmMsIDMpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGQuZCwgNCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfSIAAABjbGFzcy1pbmhlcml0YW5jZS1jb21waWxlLW9ubHkudHh01QAAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKY2xhc3MgQiB7CiAgaW50IHg7Cn07CgpjbGFzcyBDIDogQiB7CiAgaW50IHk7Cn07Cgp2b2lkIG1haW4oKSB7CiAgQyBjOwogIGMueSA9IDEwMDsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDEwMCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfSgAAABjbGFzcy1pbmhlcml0YW5jZS10ZW1wbGF0ZS12YXJpYWJsZXMudHh0wgEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKdGVtcGxhdGUgPFQ+CmNsYXNzIEIgewogIFQgeDsKfTsKCmNsYXNzIEMgOiBCPGludD4gewogIGludCB5Owp9OwoKY2xhc3MgQzIgOiBCPHN0cmluZz4gewogIGludCB5Owp9OwoKdm9pZCBtYWluKCkgewogIEMgYzsKICBjLng9IDE7CiAgYy55PTI7CgogIEMyIGMyOwogIGMyLng9ImEiOwogIGMyLnk9MzsKCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYy54LCAxKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLnksIDIpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGMyLngsICJhIik7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oYzIueSwgMyk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfSQAAABjbGFzcy10ZW1wbGF0ZS1mdW5jLWNvbXBpbGUtb25seS50eHR4AQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KY2xhc3MgQyB7DQogIHRlbXBsYXRlIDxUPg0KICB2b2lkIGYoKSB7fQ0KDQogIHRlbXBsYXRlIDxUPg0KICBvdmVycmlkYWJsZSB2b2lkIGcoKSB7fQ0KfTsNCg0KLyogVE9ETzogUmVtb3ZlIGNvbW1lbnRzLCB0aGUgZnVuY3Rpb24gdGVtcGxhdGUgbmVlZHMgdG8gY2FycnkgdG8gdHlwZSBpbmZvcm1hdGlvbiB0byBhdm9pZCBkdXBsaWNhdGluZyB3aXRoIG90aGVycy4NCmNsYXNzIEQgOiBDIHsNCiAgdGVtcGxhdGUgPFQ+DQogIG92ZXJyaWRlIHZvaWQgZygpIHt9DQp9Ow0KKi8NCg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfRUAAABjb3B5LWZ1bmN0aW9uLXB0ci50eHTLAAAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+CgpkZWxlZ2F0ZSB2b2lkIHAoKTsKCnZvaWQgZigpIHsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsKfQoKdm9pZCBtYWluKCkgewogIHAgeCA9IGY7CiAgcCB5ID0geDsKICB4KCk7CiAgeSgpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0qAAAAZGVmaW5lLWNsYXNzLWNvbnN0cnVjdG9yLWZvci1ub24tY2xhc3MudHh0WgEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKdm9pZCBjb25zdHJ1Y3QoaW50JiB0aGlzLCBpbnQgdikgewogIHRoaXMgPSB2ICsgMTsKfQoKdm9pZCBjb25zdHJ1Y3QoaW50JiB0aGlzKSB7CiAgdGhpcy5jb25zdHJ1Y3QoMSk7Cn0KCnZvaWQgZGVzdHJ1Y3QoaW50JiB0aGlzKSB7fQoKdm9pZCBtYWluKCkgewogIGludCB4KDEwMCk7CiAgaW50IHkoKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50Pih4LCAxMDEpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHksIDIpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0lAAAAZGVsZWdhdGUtZnVuY3Rpb25zLXdpdGgtc2FtZS1uYW1lLnR4dIIBAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPGIyc3R5bGUvZGVsZWdhdGVzLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+CgppbnQgZihpbnQgeCkgewogIHJldHVybiB4ICsgMTsKfQoKc3RyaW5nIGYoc3RyaW5nIHgpIHsKICByZXR1cm4geDsKfQoKdm9pZCBtYWluKCkgewogIGIyc3R5bGU6OmZ1bmN0aW9uPGludCwgaW50PiBkID0gZjsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihkKDEpLCAyKTsKCiAgYjJzdHlsZTo6ZnVuY3Rpb248c3RyaW5nLCBzdHJpbmc+IGQyID0gZjsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihkMigiYWJjIiksICJhYmMiKTsKCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfRUAAABkZWxlZ2F0ZV9pbl9jbGFzcy50eHR7AQAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+CiNpbmNsdWRlIDxiMnN0eWxlL2RlbGVnYXRlcy5oPgoKY2xhc3MgQyB7CiAgYjJzdHlsZTo6ZnVuY3Rpb248aW50LCBpbnQ+IGluYzsKICBpbnQgYmFzZTsKCiAgaW50IG15X2luYyhpbnQgeCkgewogICAgcmV0dXJuIHggKyB0aGlzLmJhc2U7CiAgfQoKICB2b2lkIGNvbnN0cnVjdChpbnQgYmFzZSkgewogICAgdGhpcy5pbmMgPSAgbXlfaW5jOwoJdGhpcy5iYXNlID0gYmFzZTsKICB9Cn07Cgp2b2lkIG1haW4oKSB7CiAgQyBjKDEpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGMuaW5jKDEpLCAyKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOwp9Ch0AAABkZWxlZ2F0ZV9pbl9jbGFzc19vbl9oZWFwLnR4dGIBAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KI2luY2x1ZGUgPGIyc3R5bGUvZGVsZWdhdGVzLmg+CgpjbGFzcyBDIHsKICBiMnN0eWxlOjpmdW5jdGlvbjxpbnQsIGludD4gaW5jOwoKICBpbnQgbXlfaW5jKGludCB4KSB7CiAgICByZXR1cm4geCArIDE7CiAgfQoKICB2b2lkIGNvbnN0cnVjdCgpIHsKICAgIHRoaXMuaW5jID0gIG15X2luYzsKICB9Cn07Cgp2b2lkIG1haW4oKSB7CiAgQyBjWzFdOwogIGNbMF0uY29uc3RydWN0KCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oY1swXS5pbmMoMSksIDIpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0SAAAAZW1wdHktYW5kLW51bGwudHh09wEAAO+7vw0KI2luY2x1ZGUgPGIyc3R5bGUuaD4NCiNpbmNsdWRlIDx0ZXN0aW5nLmg+DQoNCnRlbXBsYXRlIDxUPg0KYm9vbCBpc19udWxsKFQgdCkgew0KICBib29sIHI7DQogIGxvZ2ljICJlbXB0eSByIHQiOw0KICByZXR1cm4gcjsNCn0NCg0KdGVtcGxhdGUgPFQ+DQp2b2lkIHNldF9udWxsKFQmIHQpIHsNCiAgVCB4Ow0KICB0ID0geDsNCn0NCg0Kdm9pZCBtYWluKCkgew0KICBpbnQgaTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoaXNfbnVsbDxpbnQ+KGkpKTsNCiAgaSA9IDA7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZShpc19udWxsPGludD4oaSkpOw0KICBzZXRfbnVsbDxpbnQ+KGkpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZShpc19udWxsPGludD4oaSkpOw0KICBpID0gMTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2ZhbHNlKGlzX251bGw8aW50PihpKSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9FQAAAGZ1bmN0aW9uLXRlbXBsYXRlLnR4dBUBAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCnRlbXBsYXRlIDxUPgpUIGFkZChUJiB4LCBUIHkpIHsKICB4ICs9IHk7CiAgcmV0dXJuIHg7Cn0KCnZvaWQgbWFpbigpIHsKICBpbnQgeCA9IDE7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoYWRkPGludD4oeCwgMSkgPT0gMik7CiAgYWRkPGludD4oeCwgMSk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUoeCA9PSAzKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOwp9CAAAAGhlYXAudHh08gEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgojaW5jbHVkZSA8YjJzdHlsZS9zdGRpby5oPgoKc3RydWN0IFMgewogIGludCB4OwogIHN0cmluZyB5Owp9OwoKdm9pZCBtYWluKCkgewogIFMgczsKICB7CiAgICBTIHhbMTBdOwogICAgcyA9IHg7CiAgICB1bmRlZmluZSh4KTsKICB9CiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDsgaSsrKSB7CiAgICBzW2ldLnggPSBpICsgMTsKICAgIHNbaV0ueSA9IGIyc3R5bGU6OmludF90b19zdHIoaSArIDIpOwogIH0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsKICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHNbaV0ueCwgaSArIDEpOwogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oc1tpXS55LCBiMnN0eWxlOjppbnRfdG9fc3RyKGkgKyAyKSk7CiAgfQogIGRlYWxsb2Mocyk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfREAAABoZWFwX3B0cl90ZXN0LnR4dEEDAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KI2luY2x1ZGUgPGIyc3R5bGUvc3RkaW8uaD4KCnN0cnVjdCBTIHsKICBpbnQgeDsKICBzdHJpbmcgeTsKfTsKCiNpbmNsdWRlIDxiMnN0eWxlL2hlYXBfcHRyLmg+Cgp2b2lkIG1haW4oKSB7CiAgYjJzdHlsZTo6aGVhcF9wdHI8aW50PiBoMSgxMDApOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KGgxLnNpemUoKSwgMTAwKTsKICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7CiAgICBoMS5zZXQoaSwgaSArIDEpOwogIH0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwMDsgaSsrKSB7CiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihoMS5nZXQoaSksIGkgKyAxKTsKICB9CgogIGIyc3R5bGU6OmhlYXBfcHRyPFM+IGgyKDEwMCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oaDIuc2l6ZSgpLCAxMDApOwogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsKICAgIFMgczsKICAgIHMueCA9IGkgKyAxOwogICAgcy55ID0gYjJzdHlsZTo6aW50X3RvX3N0cihpICsgMik7CiAgICBoMi5zZXQoaSwgcyk7CiAgfQogIGZvciAoaW50IGkgPSAwOyBpIDwgMTAwOyBpKyspIHsKICAgIFMgcyA9IGgyLmdldChpKTsKICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHMueCwgaSArIDEpOwogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4ocy55LCBiMnN0eWxlOjppbnRfdG9fc3RyKGkgKyAyKSk7CiAgfQoKICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOwp9ChQAAABpZmRlZi1lbHNlLWlmZGVmLnR4dE0BAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQojaWZkZWYgTk8NCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGJvb2w+KF9fU1RBVEVNRU5UX18sIHRydWUsIGZhbHNlKTsNCn0NCiNlbHNlICNpZmRlZiBOT19BR0FJTg0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8Ym9vbD4oX19TVEFURU1FTlRfXywgZmFsc2UsIHRydWUpOw0KfQ0KI2Vsc2UNCnZvaWQgbWFpbigpIHsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCiNlbmRpZg0KI2VuZGlmDQoOAAAAaWZkZWYtZWxzZS50eHR+AAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KI2lmZGVmIE5PDQojZWxzZQ0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KI2VuZGlmCQAAAGlmZGVmLnR4dLwAAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQojZGVmaW5lIFlFUw0KI2lmZGVmIFlFUw0Kdm9pZCBtYWluKCkgew0KICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8Ym9vbD4odHJ1ZSwgdHJ1ZSk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQojZW5kaWYNChMAAABpbmNsdWRlX19GSUxFX18udHh0VwEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgojaW5jbHVkZSA8YjJzdHlsZS9zdHIuaD4KCnZvaWQgbWFpbigpIHsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZShiMnN0eWxlOjpzdHJfZW5kc193aXRoKGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9fRklMRV9fKCksICJ0ZXN0aW5nL2Fzc2VydC5oIikpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKGIyc3R5bGU6OnN0cl9lbmRzX3dpdGgoYjJzdHlsZTo6dGVzdGluZzo6dHlwZXNfX0ZJTEVfXygpLCAidGVzdGluZy90eXBlcy5oIikpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0PAAAAbGlua2VkLWxpc3QudHh00QIAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgojaW5jbHVkZSA8YXNzZXJ0Lmg+CgovKiBUT0RPOiBJbXBsZW1lbnQKdGVtcGxhdGUgPFQ+CmNsYXNzIGxpbmtlZF9ub2RlIHsKICBUIHY7CiAgdHlwZV9wdHIgX25leHQ7CgogIHZvaWQgY29uc3RydWN0KCkgewogICAgdGhpcy5fbmV4dCA9IDA7CiAgfQp9OwoKdGVtcGxhdGUgPFQ+CmNsYXNzIGxpbmtlZF9pdGVyYXRvciB7CiAgdHlwZV9wdHIgcDsKCiAgdm9pZCBjb25zdHJ1Y3QodHlwZV9wdHIgcCkgewogICAgYXNzZXJ0KF9fU1RBVEVNRU5UX18gKyAiIEAiICsgX19GSUxFX18sIHAgIT0gMCk7Cgl0aGlzLnAgPSBwOwogIH0KCiAgdm9pZCBzZXQoVCB2KSB7CiAgICB0eXBlX3B0ciB0ID0gdGhpcy5wOwoJc3RhdGljX2Nhc3QodCwgbGlua2VkX25vZGU8VD4pOwoJdFswXS52ID0gdjsKICB9CgogIFQgZ2V0KCkgewogICAgdHlwZV9wdHIgdCA9IHRoaXMucDsKCXN0YXRpY19jYXN0KHQsIGxpbmtlZF9ub2RlPFQ+KTsKCXJldHVybiB0WzBdLnY7CiAgfQp9OwoKdGVtcGxhdGUgPFQ+CmNsYXNzIGxpbmtlZF9saXN0IHsKICB0eXBlX3B0ciBoZWFkOwogIHR5cGVfcHRyIHRhaWw7CgogIHZvaWQgY29uc3RydWN0KCkgewogICAgdGhpcy5oZWFkID0gMDsKICAgIHRoaXMudGFpbCA9IDA7CiAgfQp9OwoqLwoKdm9pZCBtYWluKCkgewogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0TAAAAbW9kX2VxdWFsc190b18xLnR4dMQAAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCnZvaWQgbWFpbigpIHsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZmFsc2UoKCgxMCAlIDIpID09IDEpKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSgoKDExICUgMikgPT0gMSkpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0KHwAAAG1vcmUtY2xvc2luZy1hbmdsZS1icmFja2V0cy50eHQcAQAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+Cgp0ZW1wbGF0ZSA8VD4KY2xhc3MgQyB7CiAgdm9pZCBmKCkgewogICAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUodHJ1ZSk7CiAgfQp9OwoKdGVtcGxhdGUgPFQ+CmNsYXNzIEMyIHsKICB2b2lkIGYoKSB7CiAgICBUIHQ7Cgl0LmYoKTsKICB9Cn07Cgp2b2lkIG1haW4oKSB7CiAgQzI8QzI8QzI8QzI8QzI8Qzx2b2lkPj4+Pj4+IGM7CiAgYy5mKCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfQoRAAAAbXVsdGktZGVmaW5lcy50eHTQAAAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCg0KI2RlZmluZSBZRVMNCiNkZWZpbmUgWUVTMg0KDQojaWZuZGVmIFlFUyB8fCBOTw0Kdm9pZCBmKCkgew0KfQ0KI2VuZGlmDQoNCiNpZmRlZiBZRVMyIHx8IE5PDQp2b2lkIG1haW4oKSB7DQogIGYoKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsNCn0NCiNlbmRpZisAAABub24tb3ZlcnJpZGFibGUtY2xhc3MtdGVtcGxhdGUtZnVuY3Rpb24udHh0YAEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKY2xhc3MgQyB7CiAgdGVtcGxhdGUgPFQ+CiAgVCBmKFQgeCkgewogICAgcmV0dXJuIHg7CiAgfQp9OwoKdm9pZCBtYWluKCkgewogIEMgYzsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihjLmY8aW50PigxMDApLCAxMDApOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxib29sPihjLmY8Ym9vbD4odHJ1ZSksIHRydWUpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KGMuZjxzdHJpbmc+KCIxMDAiKSwgIjEwMCIpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0OAAAAcHRyX29mZnNldC50eHTdAQAA77u/DQojaW5jbHVkZSA8YjJzdHlsZS5oPg0KI2luY2x1ZGUgPHRlc3RpbmcuaD4NCiNpbmNsdWRlIDxiMnN0eWxlL3RvX3N0ci5oPg0KI2luY2x1ZGUgPGJzdHlsZS9jb25zdC5oPg0KI2luY2x1ZGUgPGJzdHlsZS9pbnQuaD4NCg0KbG9uZyBwdHJfb2Zmc2V0KCkgew0KICBsb2dpYyAicmV0dXJuIHB0cl9vZmZzZXQgQEBwcmVmaXhlc0Bjb25zdGFudHNAcHRyX29mZnNldCI7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgbG9uZyBNQVhfVUlOVF9QTFVTXzEgPSB0b19sb25nKDQyOTQ5NjcyOTZMKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGxvbmc+KHB0cl9vZmZzZXQoKSwgTUFYX1VJTlRfUExVU18xKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oYjJzdHlsZTo6dG9fc3RyKHB0cl9vZmZzZXQoKSksICI0Mjk0OTY3Mjk2Iik7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9FQAAAHJhdy1oZWFwLXB0ci10ZXN0LnR4dLEBAADvu78NCiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KLyogVE9ETzogSW1wbGVtZW50YXRpb24NCiNpbmNsdWRlIDxiMnN0eWxlL3Jhd19oZWFwX3B0ci5oPg0KDQpzdHJ1Y3QgUyB7DQogIGludCB4Ow0KICByYXdfaGVhcF9wdHIgbmV4dDsNCn07DQoNCnZvaWQgbWFpbigpIHsNCiAgUyBmaXJzdDsNCiAgZmlyc3QueCA9IDEwOw0KICBTIG5leHQ7DQogIGZpcnN0Lm5leHQuc2V0KG5leHQpOw0KICBuZXh0LnggPSAyMDsNCg0KICBuZXh0ID0gZmlyc3QubmV4dC5nZXQ8Uz4oKTsNCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4obmV4dC54LCAyMCk7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9DQoqLw0KDQp2b2lkIG1haW4oKSB7DQogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7DQp9EAAAAHJlYWxfX2ZpbGVfXy50eHSmAAAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+Cgp2b2lkIG1haW4oKSB7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oX19GSUxFX18sICJyZWFsX19maWxlX18udHh0Iik7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfR8AAAByZWRlZmluZS1zdHJ1Y3QtbWVtYmVyLXR5cGUudHh04wAAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKdHlwZWRlZiBpbnQgSU5UOwoKc3RydWN0IFMgewogIElOVCBpOwp9OwoKdHlwZWRlZiBzdHJpbmcgSU5UOwoKdm9pZCBtYWluKCkgewogIFMgczsKICBzLmkgPSAxMDA7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4ocy5pLCAxMDApOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0KDAAAAHJlZl90ZXN0LnR4dCkCAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPGIyc3R5bGUvcmVmLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+Cgp2b2lkIG1haW4oKSB7CiAgYjJzdHlsZTo6cmVmPHN0cmluZz4gcygpOwogIGIyc3R5bGU6OnJlZjxzdHJpbmc+IHMyKCJhYmMiKTsKICBiMnN0eWxlOjpyZWY8c3RyaW5nPiBzMyhzMik7CiAgYjJzdHlsZTo6cmVmPHN0cmluZz4gczQoKTsKCiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X3RydWUocy5lbXB0eSgpKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZShzMi5lbXB0eSgpKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZmFsc2UoczMuZW1wdHkoKSk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oczMuZ2V0KCksICJhYmMiKTsKICBzNC5hbGxvYygpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9mYWxzZShzNC5lbXB0eSgpKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihzNC5nZXQoKSwgIiIpOwoKICBiMnN0eWxlOjp0ZXN0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CiNpbmNsdWRlIDxiMnN0eWxlLmg+DQojaW5jbHVkZSA8dGVzdGluZy5oPg0KDQpzdHJ1Y3QgUyB7DQogIGJvb2wgeDsNCiAgYm9vbCB5Ow0KfTsNCg0Kdm9pZCBmKFMgcykgew0KICBmb3IgKGludCBpID0gMDsgaSA8IDEwOyBpKyspIHsNCiAgICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8Ym9vbD4oc1tpXS54LCAoaSAlIDIpID09IDApOw0KICAgIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxib29sPihzW2ldLnksIChpICUgMikgPT0gMSk7DQogIH0NCn0NCg0Kdm9pZCBmKHR5cGVfcHRyIHApIHsNCiAgc3RhdGljX2Nhc3QocCwgUyk7DQogIGYocCk7DQp9DQoNCnZvaWQgbWFpbigpIHsNCiAgUyBzWzEwXTsNCiAgZm9yIChpbnQgaSA9IDA7IGkgPCAxMDsgaSsrKSB7DQogICAgc1tpXS54ID0gKChpICUgMikgPT0gMCk7DQogICAgc1tpXS55ID0gKChpICUgMikgPT0gMSk7DQogIH0NCiAgc3RhdGljX2Nhc3QocywgdHlwZV9wdHIpOw0KICBmKHMpOw0KICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOw0KfQ0KEQAAAHN0ci1vcGVyYXRvcnMudHh0xQEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgojaW5jbHVkZSA8YnN0eWxlL3N0ci5oPgoKdm9pZCBtYWluKCkgewogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHN0cl9lbXB0eSgiIikpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHN0cl9lbXB0eSgiIiArICIiKSk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2ZhbHNlKHN0cl9lbXB0eSgiYSIgKyAiIikpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxpbnQ+KHN0cl9sZW4oIiIpLCAwKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzdHJfbGVuKCJhYmMiKSwgMyk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPGludD4oc3RyX2xlbigiYWJjIiArICJkZWYiKSwgNik7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfSwAAABzdHJ1Y3QtYW5kLXByaW1pdGl2ZS10eXBlLXdpdGgtc2FtZS1uYW1lLnR4dNIAAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCnR5cGVkZWYgaW50IElOVDsKCnN0cnVjdCBJTlQgewogIGludCB4Owp9OwoKdm9pZCBtYWluKCkgewogIElOVCBpOwogIGkueCA9IDEwMDsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihpLngsIDEwMCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfQolAAAAc3RydWN0X21lbWJlcl90eXBlX3dpdGhfbmFtZXNwYWNlLnR4dOkAAADvu78KI2luY2x1ZGUgPGIyc3R5bGUuaD4KI2luY2x1ZGUgPHRlc3RpbmcuaD4KCm5hbWVzcGFjZSBOIHsKc3RydWN0IFMgewogIDo6YjJzdHlsZTo6aW50IGk7Cn07Cn0gIC8vIG5hbWVzcGFjZSBOCgp2b2lkIG1haW4oKSB7CiAgTjo6UyBzOwogIHMuaSA9IDEwMDsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihzLmksIDEwMCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6ZmluaXNoZWQoKTsKfRkAAAB0ZW1wbGF0ZS1pbi1uYW1lc3BhY2UudHh00QEAAO+7vwojaW5jbHVkZSA8YjJzdHlsZS5oPgojaW5jbHVkZSA8dGVzdGluZy5oPgoKbmFtZXNwYWNlIE4gewoKdHlwZWRlZiA6OnZvaWQgdm9pZDsKCnRlbXBsYXRlIDxUPgpjbGFzcyBDIHsKICB2b2lkIGYoKSB7CiAgICA6OmIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOwogIH0KfTsKCnRlbXBsYXRlIDxUPgpkZWxlZ2F0ZSB2b2lkIGYoKTsKCnRlbXBsYXRlIDxUPgp2b2lkIGYyKCkgewogICAgOjpiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfdHJ1ZSh0cnVlKTsKfQoKfSAgLy8gbmFtZXNwYWNlIE4KCnZvaWQgZigpIHsKICA6OmIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKHRydWUpOwp9Cgp2b2lkIG1haW4oKSB7CiAgTjo6QzxpbnQ+IGM7CiAgYy5mKCk7CiAgTjo6ZjxpbnQ+IGYyID0gZjsKICBmMigpOwogIE46OmYyPGludD4oKTsKICBiMnN0eWxlOjp0ZXN0aW5nOjpmaW5pc2hlZCgpOwp9ChUAAAB0ZW1wbGF0ZS1vdmVycmlkZS50eHRHAwAA77u/CiNpbmNsdWRlIDxiMnN0eWxlLmg+CiNpbmNsdWRlIDx0ZXN0aW5nLmg+CgpjbGFzcyBPdXRXcml0ZXIgewogIGludCBjOwoKICB2b2lkIHdyaXRlKHN0cmluZyBtc2cpIHsKICAgIHRoaXMuYysrOwogICAgLy8gRG8gbm90IHJlYWxseSB3cml0ZSB0byBzdGRvdXQsIGl0IHdpbGwgbWFzcyB1cCB0aGUgdGVzdGluZyBhc3NlcnRpb25zLgogICAgYjJzdHlsZTo6c3RkX2Vycihtc2cpOwogIH0KfTsKCmNsYXNzIEVycldyaXRlciB7CiAgaW50IGM7CgogIHZvaWQgd3JpdGUoc3RyaW5nIG1zZykgewogICAgdGhpcy5jKys7CiAgICBiMnN0eWxlOjpzdGRfZXJyKG1zZyk7CiAgfQp9OwoKdGVtcGxhdGUgPFc+CmNsYXNzIExvZ2dlciB7CiAgVyB3OwoKICB2b2lkIGxvZyhzdHJpbmcgbXNnKSB7CiAgICB0aGlzLncud3JpdGUobXNnKTsKICB9CgogIGludCBjb3VudF9vZl9sb2dfbGluZXMoKSB7CiAgICByZXR1cm4gdGhpcy53LmM7CiAgfQp9OwoKdHlwZWRlZiBMb2dnZXI8T3V0V3JpdGVyPiBPdXRMb2dnZXI7CnR5cGVkZWYgTG9nZ2VyPEVycldyaXRlcj4gRXJyTG9nZ2VyOwoKdm9pZCBtYWluKCkgewogIE91dExvZ2dlciBvbDsKICBFcnJMb2dnZXIgZWw7CgogIG9sLmxvZygib3V0Iik7CiAg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PHRlc3RpbmcuaD4KCnZvaWQgbWFpbigpIHsKICBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8c3RyaW5nPihfX0ZJTEVfXywgX19GSUxFX18pOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KF9fZnVuY19fLCBfX2Z1bmNfXyk7CiAgLy8gVGhlIF9fTElORV9fIGlzIGhhbmRsZWQgYXMgdGhlIHN0YXJ0IG9mIGNoYXJhY3Rlciwgc28gdGhlIGZvbGxvd2luZyBhc3NlcnRpb24gd29uJ3QgcGFzcy4KICAvLyBiMnN0eWxlOjp0ZXN0aW5nOjphc3NlcnRfZXF1YWw8aW50PihfX0xJTkVfXywgX19MSU5FX18pOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF90cnVlKF9fTElORV9fID4gMCk7CiAgYjJzdHlsZTo6dGVzdGluZzo6YXNzZXJ0X2VxdWFsPHN0cmluZz4oX19TVEFURU1FTlRfXywgX19TVEFURU1FTlRfXyk7CiAgc3RyaW5nIHMgPSBfX1NUQVRFTUVOVF9fOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmFzc2VydF9lcXVhbDxzdHJpbmc+KHMsICJzdHJpbmcgcyA9IF9fU1RBVEVNRU5UX18gOyIpOwogIGIyc3R5bGU6OnRlc3Rpbmc6OmZpbmlzaGVkKCk7Cn0="
        ))
End Class
