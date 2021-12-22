' This file is created by genall with
' C:\Users\Hzj_j\git\osi.net\service\resource\gen\gen.exe
' shared_rules "[nlexer_rule]" "nlexer_rule.txt" "[syntaxer_rule]" "syntaxer_rule.txt"

Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'[nlexer_rule] [syntaxer_rule]
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module shared_rules
    Public ReadOnly [nlexer_rule]() As Byte
    Public ReadOnly [syntaxer_rule]() As Byte

    Sub New()
        [nlexer_rule] = Convert.FromBase64String(strcat_hint(CUInt(888), _
        "77u/DQprdy1pZiBpZg0Ka3ctZWxzZSBlbHNlDQprdy1mb3IgZm9yDQprdy13aGlsZSB3aGlsZQ0Ka3ctZG8gZG8NCmt3LWxvb3AgbG9vcA0Ka3ctcmV0dXJuIHJldHVybg0Ka3ctYnJlYWsgYnJlYWsNCmt3LWxvZ2ljIGxvZ2ljDQprdy1pbmNsdWRlICNpbmNsdWRlDQprdy1pZm5kZWYgI2lmbmRlZg0Ka3ctZGVmaW5lICNkZWZpbmUNCmt3LWVuZGlmICNlbmRpZg0Ka3ctdHlwZWRlZiB0eXBlZGVmDQprdy1zdHJ1Y3Qgc3RydWN0DQoNCnNpbmdsZS1saW5lLWNvbW1lbnQgLy9bKnxcbl0qDQptdWx0aS1saW5lLWNvbW1lbnQgL1wqWyp8XCovXSpcKi8NCg0KYmxhbmsgW1xiXSsNCg0KaW5jbHVkZS13aXRoLWZpbGUgI2luY2x1ZGVbXGJdKzxbKnw+XSs+DQoNCmJvb2wgW3RydWUsZmFsc2VdDQppbnRlZ2VyIFsrLC1dP1tcZF0rDQpiaWd1aW50IFtcZF0rW2wsTF0NCnVmbG9hdCBbXGRdKi5bXGRdKw0Kc3RyaW5nICJbXCIsKnwiXSoiDQoNCmNvbW1hIFwsDQpjb2xvbiA6DQpxdWVzdGlvbi1tYXJrID8NCnN0YXJ0LXBhcmFncmFwaCB7DQplbmQtcGFyYWdyYXBoIH0NCnN0YXJ0LWJyYWNrZXQgKA0KZW5kLWJyYWNrZXQgKQ0Kc3RhcnQtc3F1YXJlLWJyYWNrZXQgXFsNCmVuZC1zcXVhcmUtYnJhY2tldCBcXQ0Kc2VtaS1jb2xvbiA7DQphc3NpZ25tZW50ID0NCg0KZG90IC4NCg=="))
        [syntaxer_rule] = Convert.FromBase64String(strcat_hint(CUInt(2648), _
        "77u/DQpJR05PUkVfVFlQRVMgYmxhbmssIHNpbmdsZS1saW5lLWNvbW1lbnQsIG11bHRpLWxpbmUtY29tbWVudA0KUk9PVF9UWVBFUyByb290LXR5cGUNCg0KZnVuY3Rpb24gbmFtZSBuYW1lIHN0YXJ0LWJyYWNrZXQgW3BhcmFtbGlzdF0/IGVuZC1icmFja2V0IG11bHRpLXNlbnRlbmNlLXBhcmFncmFwaA0KcGFyYW1saXN0IFtwYXJhbS13aXRoLWNvbW1hXSogcGFyYW0NCnBhcmFtLXdpdGgtY29tbWEgcGFyYW0gY29tbWENCg0KcGFyYWdyYXBoIFtzZW50ZW5jZSwgbXVsdGktc2VudGVuY2UtcGFyYWdyYXBoXQ0Kc2VudGVuY2UgW3NlbnRlbmNlLXdpdGgtc2VtaS1jb2xvbiwgc2VudGVuY2Utd2l0aG91dC1zZW1pLWNvbG9uXQ0Kc2VudGVuY2Utd2l0aG91dC1zZW1pLWNvbG9uIFtjb25kaXRpb24sIHdoaWxlLCBmb3ItbG9vcF0NCg0KbXVsdGktc2VudGVuY2UtcGFyYWdyYXBoIHN0YXJ0LXBhcmFncmFwaCBbcGFyYWdyYXBoXSogZW5kLXBhcmFncmFwaA0KDQp2YWx1ZS1kZWZpbml0aW9uIG5hbWUgbmFtZSBhc3NpZ25tZW50IHZhbHVlDQp2YWx1ZS1kZWNsYXJhdGlvbiBuYW1lIG5hbWUNCmhlYXAtZGVjbGFyYXRpb24gbmFtZSBoZWFwLW5hbWUNCnZhbHVlLWNsYXVzZSB2YXJpYWJsZS1uYW1lIGFzc2lnbm1lbnQgdmFsdWUNCg0KdmFsdWUtZGVmaW5pdGlvbi13aXRoLXNlbWktY29sb24gdmFsdWUtZGVmaW5pdGlvbiBzZW1pLWNvbG9uDQp2YWx1ZS1kZWNsYXJhdGlvbi13aXRoLXNlbWktY29sb24gdmFsdWUtZGVjbGFyYXRpb24gc2VtaS1jb2xvbg0KaGVhcC1kZWNsYXJhdGlvbi13aXRoLXNlbWktY29sb24gaGVhcC1kZWNsYXJhdGlvbiBzZW1pLWNvbG9uDQoNCmNvbmRpdGlvbiBrdy1pZiBzdGFydC1icmFja2V0IHZhbHVlIGVuZC1icmFja2V0IHBhcmFncmFwaCBbZWxzZS1jb25kaXRpb25dPw0KZWxzZS1jb25kaXRpb24ga3ctZWxzZSBwYXJhZ3JhcGgNCg0Kd2hpbGUga3ctd2hpbGUgc3RhcnQtYnJhY2tldCB2YWx1ZSBlbmQtYnJhY2tldCBwYXJhZ3JhcGgNCg0KdmFsdWUgW3ZhbHVlLXdpdGgtYnJhY2tldCwgdmFsdWUtd2l0aG91dC1icmFja2V0XQ0KdmFsdWUtd2l0aC1icmFja2V0IHN0YXJ0LWJyYWNrZXQgdmFsdWUgZW5kLWJyYWNrZXQNCg0KaGVhcC1uYW1lIG5hbWUgc3RhcnQtc3F1YXJlLWJyYWNrZXQgdmFsdWUgZW5kLXNxdWFyZS1icmFja2V0DQoNCmlnbm9yZS1yZXN1bHQtZnVuY3Rpb24tY2FsbCBmdW5jdGlvbi1jYWxsDQoNCmZ1bmN0aW9uLWNhbGwgbmFtZSBzdGFydC1icmFja2V0IFt2YWx1ZS1saXN0XT8gZW5kLWJyYWNrZXQNCnZhbHVlLWxpc3QgW3ZhbHVlLXdpdGgtY29tbWFdKiB2YWx1ZQ0KdmFsdWUtd2l0aC1jb21tYSB2YWx1ZSBjb21tYQ0KDQpyZXR1cm4tY2xhdXNlIGt3LXJldHVybiBbdmFsdWVdPw0KDQojIEVtYmVkIGEgbG9naWMgc3RhdGVtZW50IGRpcmVjdGx5LCBsaWtlIGxvZ2ljICJpbnQgc3Rkb3V0IGFfc3RyaW5nIg0KbG9naWMga3ctbG9naWMgc3RyaW5nDQpsb2dpYy13aXRoLXNlbWktY29sb24gbG9naWMgc2VtaS1jb2xvbg0KDQppbmNsdWRlLXdpdGgtc3RyaW5nIGt3LWluY2x1ZGUgc3RyaW5nDQppbmNsdWRlIFtpbmNsdWRlLXdpdGgtc3RyaW5nLCBpbmNsdWRlLXdpdGgtZmlsZV0NCg0KaWZuZGVmLXdyYXBwZWQga3ctaWZuZGVmIG5hbWUgW3Jvb3QtdHlwZV0qIGt3LWVuZGlmDQpkZWZpbmUga3ctZGVmaW5lIG5hbWUNCg0KIyBBbGxvdyB0eXBlKiB0byBiZSB1c2VkIGFzIGEgc3RyaW5nLg0KdHlwZWRlZi10eXBlIFtuYW1lLCBzdHJpbmddDQp0eXBlZGVmIGt3LXR5cGVkZWYgdHlwZWRlZi10eXBlIHR5cGVkZWYtdHlwZQ0KdHlwZWRlZi13aXRoLXNlbWktY29sb24gdHlwZWRlZiBzZW1pLWNvbG9uDQoNCiMgVE9ETzogU3VwcG9ydCB2YWx1ZS1kZWZpbml0aW9uDQpzdHJ1Y3Qga3ctc3RydWN0IG5hbWUgc3RhcnQtcGFyYWdyYXBoIFt2YWx1ZS1kZWNsYXJhdGlvbi13aXRoLXNlbWktY29sb25dKiBlbmQtcGFyYWdyYXBoIHNlbWktY29sb24NCg=="))
    End Sub
End Module
