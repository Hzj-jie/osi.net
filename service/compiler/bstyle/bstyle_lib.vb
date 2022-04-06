Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class bstyle_lib
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(5096), 
            "CAAAAGJzdHlsZS5omQAAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9IDQojZGVmaW5lIEJTVFlMRV9MSUJfQlNUWUxFX0gNCg0KI2luY2x1ZGUgPGJzdHlsZS90eXBlcy5oPg0KI2luY2x1ZGUgPGJzdHlsZS9jb25zdC5oPg0KDQojZW5kaWYgIC8vIEJTVFlMRV9MSUJfQlNUWUxFX0gNCgYAAABjc3RkaW+/AAAA77u/DQojaWZuZGVmIEJTVFlMRV9MSUJfQ1NURElPDQojZGVmaW5lIEJTVFlMRV9MSUJfQ1NURElPDQoNCi8vIFRPRE86IE1vdmUgdG8gYjJzdHlsZSBvbmNlIHRoZSBpbmNsdWRlIGNhbiBiZSBoYW5kbGVkIGJ5IGIyc3R5bGUgYXMgd2VsbC4NCg0KI2luY2x1ZGUgPHN0ZGlvLmg+DQoNCiNlbmRpZiAgLy8gQlNUWUxFX0xJQl9DU1RESU8HAAAAcnVuLmNtZGoAAAANCmRlbCAvcyAqLnVufg0KLi5cLi5cLi5ccmVzb3VyY2VcZ2VuXHRhcl9nZW5cb3NpLnJvb3QudXR0IHRhcl9nZW4gLS1vdXRwdXQ9YnN0eWxlX2xpYg0KbW92ZSAvWSAqLnZiIC4uXA0KBwAAAHN0ZGlvLmiuAQAA77u/DQojaWZuZGVmIEJTVFlMRV9MSUJfU1RESU9fSA0KI2RlZmluZSBCU1RZTEVfTElCX1NURElPX0gNCg0KI2luY2x1ZGUgPGJzdHlsZS9jb25zdC5oPg0KI2luY2x1ZGUgPGJzdHlsZS90eXBlcy5oPg0KDQppbnQgZ2V0Y2hhcigpIHsNCiAgaW50IHI7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZ2V0Y2hhciBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyByIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmludCBwdXRjaGFyKGludCBpKSB7DQogIGxvZ2ljICJpbnRlcnJ1cHQgcHV0Y2hhciBpIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIjsNCiAgcmV0dXJuIGk7DQp9DQoNCmludCBlb2YoKSB7DQogIGludCByOw0KICBsb2dpYyAiY29weSByIEBAcHJlZml4ZXNAY29uc3RhbnRzQGVvZiI7DQogIHJldHVybiByOw0KfQ0KDQojZW5kaWYgIC8vIEJTVFlMRV9MSUJfU1RESU9fSA4AAABic3R5bGUvY29uc3QuaG4EAADvu78NCiNpZm5kZWYgQlNUWUxFX0xJQl9CU1RZTEVfQ09OU1RBTlRTX0gNCiNkZWZpbmUgQlNUWUxFX0xJQl9CU1RZTEVfQ09OU1RBTlRTX0gNCg0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMCBJbnRlZ2VyIjsNCmxvZ2ljICJkZWZpbmUgQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzEgSW50ZWdlciI7DQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfaW50IEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2xvbmcgSW50ZWdlciI7DQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfYm9vbCBJbnRlZ2VyIjsNCmxvZ2ljICJkZWZpbmUgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9ieXRlIEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2Zsb2F0IEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0Blb2YgSW50ZWdlciI7DQoNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIGkwIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIGkxIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfaW50IGk0IjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfbG9uZyBpOCI7DQpsb2dpYyAiY29weV9jb25zdCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2Jvb2wgaTEiOw0KbG9naWMgImNvcHlfY29uc3QgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9ieXRlIGkxIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfZmxvYXQgaTE2IjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGVvZiBpLTEiOw0KDQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAdGVtcHNAYmlndWludCBCaWdVbnNpZ25lZEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBTdHJpbmciOw0KDQojZW5kaWYgIC8vIEJTVFlMRV9MSUJfQlNUWUxFX0NPTlNUQU5UU19IDQoMAAAAYnN0eWxlL2ludC5omgQAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9JTlRfSA0KI2RlZmluZSBCU1RZTEVfTElCX0JTVFlMRV9JTlRfSA0KDQojaW5jbHVkZSA8YnN0eWxlL2NvbnN0Lmg+DQojaW5jbHVkZSA8YnN0eWxlL3R5cGVzLmg+DQoNCmJpZ3VpbnQgYnN0eWxlX190b19iaWd1aW50KGludCBpKSB7DQogIHJldHVybiBpOw0KfQ0KDQpiaWd1aW50IGJzdHlsZV9fdG9fYmlndWludChsb25nIGkpIHsNCiAgcmV0dXJuIGk7DQp9DQoNCmxvbmcgYnN0eWxlX190b19sb25nKGludCBpKSB7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGJzdHlsZV9fZml0X2luX2xvbmcobG9uZyYgeCkgew0KICBsb2dpYyAiY3V0X2xlbiB4IHggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzAgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9sb25nIjsNCiAgcmV0dXJuIHg7DQp9DQoNCmludCBic3R5bGVfX2ZpdF9pbl9pbnQoaW50JiB4KSB7DQogIGxvZ2ljICJjdXRfbGVuIHggeCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2ludCI7DQogIHJldHVybiB4Ow0KfQ0KDQpieXRlIGJzdHlsZV9fZml0X2luX2J5dGUoYnl0ZSYgeCkgew0KICBsb2dpYyAiY3V0X2xlbiB4IHggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzAgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9ieXRlIjsNCiAgcmV0dXJuIHg7DQp9DQoNCmxvbmcgYnN0eWxlX190b19sb25nKGJpZ3VpbnQgeCkgew0KICBsb25nIHk7DQogIGxvZ2ljICJjdXRfbGVuIHkgeCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2xvbmciOw0KICByZXR1cm4geTsNCn0NCg0KaW50IGJzdHlsZV9fdG9faW50KGJpZ3VpbnQgeCkgew0KICBpbnQgeTsNCiAgbG9naWMgImN1dF9sZW4geSB4IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfaW50IjsNCiAgcmV0dXJuIHk7DQp9DQoNCmJ5dGUgYnN0eWxlX190b19ieXRlKGludCB4KSB7DQogIGJ5dGUgeTsNCiAgbG9naWMgImN1dF9sZW4geSB4IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfYnl0ZSI7DQogIHJldHVybiB5Ow0KfQ0KDQojZW5kaWYgIC8vIEJTVFlMRV9MSUJfQlNUWUxFX0lOVF9IFAAAAGJzdHlsZS9sb2FkX21ldGhvZC5ohAEAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9MT0FERURfTUVUSE9EX0gNCiNkZWZpbmUgQlNUWUxFX0xJQl9CU1RZTEVfTE9BREVEX01FVEhPRF9IDQoNCiNpbmNsdWRlIDxic3R5bGUvdHlwZXMuaD4NCg0Kdm9pZCBic3R5bGVfX2xvYWRfbWV0aG9kKHN0cmluZyBtKSB7DQogIG0gPSBic3R5bGVfX3N0cl9jb25jYXQoDQoJCSAgIm9zaS5zZXJ2aWNlLmludGVycHJldGVyLnByaW1pdGl2ZS5sb2FkZWRfbWV0aG9kcywgb3NpLnNlcnZpY2UuaW50ZXJwcmV0ZXI6IiwNCgkJICBtKTsNCiAgbG9naWMgImludGVycnVwdCBsb2FkX21ldGhvZCBtIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIjsNCn0NCg0KI2VuZGlmICAvLyBCU1RZTEVfTElCX0JTVFlMRV9MT0FERURfTUVUSE9EX0gMAAAAYnN0eWxlL3N0ci5owQIAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9TVFJfSA0KI2RlZmluZSBCU1RZTEVfTElCX0JTVFlMRV9TVFJfSA0KDQojaW5jbHVkZSA8YnN0eWxlL3R5cGVzLmg+DQoNCnN0cmluZyBic3R5bGVfX3N0cl9jb25jYXQoc3RyaW5nIGksIHN0cmluZyBqKSB7DQogIGxvZ2ljICJhcHBlbmQgaSBqIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnN0cmluZyBic3R5bGVfX3N0cl9jb25jYXQoc3RyaW5nIGksIGJ5dGUgaikgew0KICBsb2dpYyAiYXBwZW5kIGkgaiI7DQogIHJldHVybiBpOw0KfQ0KDQpzdHJpbmcgYnN0eWxlX190b19zdHIoYnl0ZSBpKSB7DQogIHN0cmluZyBzOw0KICByZXR1cm4gYnN0eWxlX19zdHJfY29uY2F0KHMsIGkpOw0KfQ0KDQppbnQgYnN0eWxlX19zdHJfbGVuKHN0cmluZyBzKSB7DQogIGludCByOw0KICBsb2dpYyAic2l6ZW9mIHIgcyI7DQogIHJldHVybiByOw0KfQ0KDQpib29sIGJzdHlsZV9fc3RyX2VtcHR5KHN0cmluZyBzKSB7DQogIC8vIE5vdGUsIGVtcHR5IG1lYW5zIHRoZSBzID09IG51bGwgcmF0aGVyIHRoYW4gcy5sZW5ndGggPT0gMC4NCiAgaW50IHIgPSBic3R5bGVfX3N0cl9sZW4ocyk7DQogIGJvb2wgcmVzdWx0Ow0KICBpbnQgemVybyA9IDA7DQogIGxvZ2ljICJlcXVhbCByZXN1bHQgciB6ZXJvIjsNCiAgcmV0dXJuIHJlc3VsdDsNCn0NCg0KI2VuZGlmICAvLyBCU1RZTEVfTElCX0JTVFlMRV9TVFJfSA0AAABic3R5bGUvdGltZS5oKAEAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9USU1FX0gNCiNkZWZpbmUgQlNUWUxFX0xJQl9CU1RZTEVfVElNRV9IDQoNCiNpbmNsdWRlIDxic3R5bGUvY29uc3QuaD4NCiNpbmNsdWRlIDxic3R5bGUvdHlwZXMuaD4NCg0KaW50IGJzdHlsZV9fY3VycmVudF9tcygpIHsNCiAgaW50IHJlc3VsdDsNCiAgbG9naWMgImludGVycnVwdCBjdXJyZW50X21zIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIHJlc3VsdCI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCiNlbmRpZiAgLy8gQlNUWUxFX0xJQl9CU1RZTEVfVElNRV9IDgAAAGJzdHlsZS90eXBlcy5oQgEAAO+7vw0KI2lmbmRlZiBCU1RZTEVfTElCX0JTVFlMRV9UWVBFU19IDQojZGVmaW5lIEJTVFlMRV9MSUJfQlNUWUxFX1RZUEVTX0gNCg0KdHlwZWRlZiBJbnRlZ2VyIGludDsNCmxvZ2ljICJ0eXBlIGxvbmcgOCI7DQp0eXBlZGVmIEJvb2xlYW4gYm9vbDsNCmxvZ2ljICJ0eXBlIGJ5dGUgMSI7DQp0eXBlZGVmIEJpZ1Vuc2lnbmVkSW50ZWdlciBiaWd1aW50Ow0KdHlwZWRlZiBCaWdVbnNpZ25lZEZsb2F0IHVmbG9hdDsNCnR5cGVkZWYgU3RyaW5nIHN0cmluZzsNCnR5cGVkZWYgdHlwZTAgdm9pZDsNCg0KI2VuZGlmICAvLyBCU1RZTEVfTElCX0JTVFlMRV9UWVBFU19IDQo="
        ))
End Class
