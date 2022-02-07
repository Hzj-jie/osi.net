Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class bstyle_lib
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(5465), 
            "BgAAAGNzdGRpb78AAADvu78NCiNpZm5kZWYgQlNUWUxFX0xJQl9DU1RESU8NCiNkZWZpbmUgQlNUWUxFX0xJQl9DU1RESU8NCg0KLy8gVE9ETzogTW92ZSB0byBiMnN0eWxlIG9uY2UgdGhlIGluY2x1ZGUgY2FuIGJlIGhhbmRsZWQgYnkgYjJzdHlsZSBhcyB3ZWxsLg0KDQojaW5jbHVkZSA8c3RkaW8uaD4NCg0KI2VuZGlmICAvLyBCU1RZTEVfTElCX0NTVERJTwcAAABydW4uY21kagAAAA0KZGVsIC9zICoudW5+DQouLlwuLlwuLlxyZXNvdXJjZVxnZW5cdGFyX2dlblxvc2kucm9vdC51dHQgdGFyX2dlbiAtLW91dHB1dD1ic3R5bGVfbGliDQptb3ZlIC9ZICoudmIgLi5cDQoIAAAAYnN0eWxlLmiTCAAA77u/DQojaWZuZGVmIEJTVFlMRV9MSUJfQlNUWUxFX0gNCiNkZWZpbmUgQlNUWUxFX0xJQl9CU1RZTEVfSA0KDQojaW5jbHVkZSA8YnN0eWxlX3R5cGVzLmg+DQojaW5jbHVkZSA8YnN0eWxlX2NvbnN0YW50cy5oPg0KDQovLyBVc2UgZG91YmxlLXVuZGVyc2NvcmUgdG8gYWxsb3cgYnN0eWxlIHRvIGFjY2VzcyB0aGVzZSBmdW5jdGlvbnMgd2l0aCBic3R5bGU6OiBuYW1lc3BhY2UgZm9ybWF0Lg0KaW50IGJzdHlsZV9fZ2V0Y2hhcigpIHsNCiAgaW50IHI7DQogIGxvZ2ljICJpbnRlcnJ1cHQgZ2V0Y2hhciBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyByIjsNCiAgcmV0dXJuIHI7DQp9DQoNCmludCBic3R5bGVfX3B1dGNoYXIoaW50IGkpIHsNCiAgbG9naWMgImludGVycnVwdCBwdXRjaGFyIGkgQEBwcmVmaXhlc0B0ZW1wc0BzdHJpbmciOw0KICByZXR1cm4gaTsNCn0NCg0KaW50IGJzdHlsZV9fZW9mKCkgew0KICBpbnQgcjsNCiAgbG9naWMgImNvcHkgciBAQHByZWZpeGVzQGNvbnN0YW50c0Blb2YiOw0KICByZXR1cm4gcjsNCn0NCg0KaW50IGJzdHlsZV9fY3VycmVudF9tcygpIHsNCiAgaW50IHJlc3VsdDsNCiAgbG9naWMgImludGVycnVwdCBjdXJyZW50X21zIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIHJlc3VsdCI7DQogIHJldHVybiByZXN1bHQ7DQp9DQoNCnN0cmluZyBic3R5bGVfX3N0cl9jb25jYXQoc3RyaW5nIGksIHN0cmluZyBqKSB7DQogIGxvZ2ljICJhcHBlbmQgaSBqIjsNCiAgcmV0dXJuIGk7DQp9DQoNCnN0cmluZyBic3R5bGVfX3N0cl9jb25jYXQoc3RyaW5nIGksIGJ5dGUgaikgew0KICBsb2dpYyAiYXBwZW5kIGkgaiI7DQogIHJldHVybiBpOw0KfQ0KDQpzdHJpbmcgYnN0eWxlX190b19zdHIoYnl0ZSBpKSB7DQogIHN0cmluZyBzOw0KICByZXR1cm4gYnN0eWxlX19zdHJfY29uY2F0KHMsIGkpOw0KfQ0KDQpiaWd1aW50IGJzdHlsZV9fdG9fYmlndWludChpbnQgaSkgew0KICByZXR1cm4gaTsNCn0NCg0KYmlndWludCBic3R5bGVfX3RvX2JpZ3VpbnQobG9uZyBpKSB7DQogIHJldHVybiBpOw0KfQ0KDQpsb25nIGJzdHlsZV9fdG9fbG9uZyhpbnQgaSkgew0KICByZXR1cm4gaTsNCn0NCg0KbG9uZyBic3R5bGVfX2ZpdF9pbl9sb25nKGxvbmcmIHgpIHsNCiAgbG9naWMgImN1dF9sZW4geCB4IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfbG9uZyI7DQogIHJldHVybiB4Ow0KfQ0KDQppbnQgYnN0eWxlX19maXRfaW5faW50KGludCYgeCkgew0KICBsb2dpYyAiY3V0X2xlbiB4IHggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzAgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9pbnQiOw0KICByZXR1cm4geDsNCn0NCg0KYnl0ZSBic3R5bGVfX2ZpdF9pbl9ieXRlKGJ5dGUmIHgpIHsNCiAgbG9naWMgImN1dF9sZW4geCB4IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfYnl0ZSI7DQogIHJldHVybiB4Ow0KfQ0KDQpsb25nIGJzdHlsZV9fdG9fbG9uZyhiaWd1aW50IHgpIHsNCiAgbG9uZyB5Ow0KICBsb2dpYyAiY3V0X2xlbiB5IHggQEBwcmVmaXhlc0Bjb25zdGFudHNAaW50XzAgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9sb25nIjsNCiAgcmV0dXJuIHk7DQp9DQoNCmludCBic3R5bGVfX3RvX2ludChiaWd1aW50IHgpIHsNCiAgaW50IHk7DQogIGxvZ2ljICJjdXRfbGVuIHkgeCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2ludCI7DQogIHJldHVybiB5Ow0KfQ0KDQpieXRlIGJzdHlsZV9fdG9fYnl0ZShpbnQgeCkgew0KICBieXRlIHk7DQogIGxvZ2ljICJjdXRfbGVuIHkgeCBAQHByZWZpeGVzQGNvbnN0YW50c0BpbnRfMCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2J5dGUiOw0KICByZXR1cm4geTsNCn0NCg0Kdm9pZCBic3R5bGVfX2xvYWRfbWV0aG9kKHN0cmluZyBtKSB7DQogIG0gPSBic3R5bGVfX3N0cl9jb25jYXQoDQoJCSAgIm9zaS5zZXJ2aWNlLmludGVycHJldGVyLnByaW1pdGl2ZS5sb2FkZWRfbWV0aG9kcywgb3NpLnNlcnZpY2UuaW50ZXJwcmV0ZXI6IiwNCgkJICBtKTsNCiAgbG9naWMgImludGVycnVwdCBsb2FkX21ldGhvZCBtIEBAcHJlZml4ZXNAdGVtcHNAc3RyaW5nIjsNCn0NCg0KI2VuZGlmICAvLyBCU1RZTEVfTElCX0JTVFlMRV9IDQoOAAAAYnN0eWxlX3R5cGVzLmhCAQAA77u/DQojaWZuZGVmIEJTVFlMRV9MSUJfQlNUWUxFX1RZUEVTX0gNCiNkZWZpbmUgQlNUWUxFX0xJQl9CU1RZTEVfVFlQRVNfSA0KDQp0eXBlZGVmIEludGVnZXIgaW50Ow0KbG9naWMgInR5cGUgbG9uZyA4IjsNCnR5cGVkZWYgQm9vbGVhbiBib29s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dHNAaW50XzEgSW50ZWdlciI7DQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfaW50IEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2xvbmcgSW50ZWdlciI7DQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfYm9vbCBJbnRlZ2VyIjsNCmxvZ2ljICJkZWZpbmUgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9ieXRlIEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2Zsb2F0IEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQGNvbnN0YW50c0Blb2YgSW50ZWdlciI7DQoNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8wIGkwIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGludF8xIGkxIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfaW50IGk0IjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfbG9uZyBpOCI7DQpsb2dpYyAiY29weV9jb25zdCBAQHByZWZpeGVzQGNvbnN0YW50c0BzaXplX29mX2Jvb2wgaTEiOw0KbG9naWMgImNvcHlfY29uc3QgQEBwcmVmaXhlc0Bjb25zdGFudHNAc2l6ZV9vZl9ieXRlIGkxIjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQHNpemVfb2ZfZmxvYXQgaTE2IjsNCmxvZ2ljICJjb3B5X2NvbnN0IEBAcHJlZml4ZXNAY29uc3RhbnRzQGVvZiBpLTEiOw0KDQpsb2dpYyAiZGVmaW5lIEBAcHJlZml4ZXNAdGVtcHNAYmlndWludCBCaWdVbnNpZ25lZEludGVnZXIiOw0KbG9naWMgImRlZmluZSBAQHByZWZpeGVzQHRlbXBzQHN0cmluZyBTdHJpbmciOw0KDQojZW5kaWYgIC8vIEJTVFlMRV9MSUJfQlNUWUxFX0NPTlNUQU5UU19IDQo="
        ))
End Class
