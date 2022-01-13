' This file is created by genall with
' C:\Users\Hzj_jie\git\osi\service\resource\gen\gen.exe
' shared_rules "[nlexer_rule]" "nlexer_rule.txt" "[nlexer_rule2]" "nlexer_rule2.txt" "[syntaxer_rule]" "syntaxer_rule.txt"

Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'[nlexer_rule] [nlexer_rule2] [syntaxer_rule]
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module shared_rules
    Public ReadOnly [nlexer_rule]() As Byte
    Public ReadOnly [nlexer_rule2]() As Byte
    Public ReadOnly [syntaxer_rule]() As Byte

    Sub New()
        [nlexer_rule] = Convert.FromBase64String(strcat_hint(CUInt(940), _
        "77u/DQprdy1pZiBpZg0Ka3ctZWxzZSBlbHNlDQprdy1mb3IgZm9yDQprdy13aGlsZSB3aGlsZQ0Ka3ctZG8gZG8NCmt3LWxvb3AgbG9vcA0Ka3ctcmV0dXJuIHJldHVybg0Ka3ctYnJlYWsgYnJlYWsNCmt3LWxvZ2ljIGxvZ2ljDQprdy1pbmNsdWRlICNpbmNsdWRlDQprdy1pZm5kZWYgI2lmbmRlZg0Ka3ctZGVmaW5lICNkZWZpbmUNCmt3LWVuZGlmICNlbmRpZg0Ka3ctdHlwZWRlZiB0eXBlZGVmDQprdy1zdHJ1Y3Qgc3RydWN0DQprdy1yZWludGVycHJldC1jYXN0IHJlaW50ZXJwcmV0X2Nhc3QNCg0Kc2luZ2xlLWxpbmUtY29tbWVudCAvL1sqfFxuXSoNCm11bHRpLWxpbmUtY29tbWVudCAvXCpbKnxcKi9dKlwqLw0KDQpibGFuayBbXGJdKw0KDQppbmNsdWRlLXdpdGgtZmlsZSAjaW5jbHVkZVtcYl0rPFsqfD5dKz4NCg0KYm9vbCBbdHJ1ZSxmYWxzZV0NCmludGVnZXIgWyssLV0/W1xkXSsNCmJpZ3VpbnQgW1xkXStbbCxMXQ0KdWZsb2F0IFtcZF0qLltcZF0rDQpzdHJpbmcgIltcIiwqfCJdKiINCg0KY29tbWEgXCwNCmNvbG9uIDoNCnF1ZXN0aW9uLW1hcmsgPw0Kc3RhcnQtcGFyYWdyYXBoIHsNCmVuZC1wYXJhZ3JhcGggfQ0Kc3RhcnQtYnJhY2tldCAoDQplbmQtYnJhY2tldCApDQpzdGFydC1zcXVhcmUtYnJhY2tldCBcWw0KZW5kLXNxdWFyZS1icmFja2V0IFxdDQpzZW1pLWNvbG9uIDsNCmFzc2lnbm1lbnQgPQ0KDQpkb3QgLg0KDQo="))
        [nlexer_rule2] = Convert.FromBase64String(strcat_hint(CUInt(44), _
        "77u/DQpyYXctbmFtZSBbXHcsX11bXHcsXGQsX10qDQo="))
        [syntaxer_rule] = Convert.FromBase64String(strcat_hint(CUInt(3584), _
        "77u/DQpJR05PUkVfVFlQRVMgYmxhbmssIHNpbmdsZS1saW5lLWNvbW1lbnQsIG11bHRpLWxpbmUtY29tbWVudA0KUk9PVF9UWVBFUyByb290LXR5cGUNCg0KZnVuY3Rpb24gdHlwZS1uYW1lIG5hbWUgc3RhcnQtYnJhY2tldCBbcGFyYW1saXN0XT8gZW5kLWJyYWNrZXQgbXVsdGktc2VudGVuY2UtcGFyYWdyYXBoDQpwYXJhbSB0eXBlLW5hbWUgW3JlZmVyZW5jZV0/IG5hbWUNCnBhcmFtbGlzdCBbcGFyYW0td2l0aC1jb21tYV0qIHBhcmFtDQpwYXJhbS13aXRoLWNvbW1hIHBhcmFtIGNvbW1hDQoNCnBhcmFncmFwaCBbc2VudGVuY2UsIG11bHRpLXNlbnRlbmNlLXBhcmFncmFwaF0NCnNlbnRlbmNlIFtzZW50ZW5jZS13aXRoLXNlbWktY29sb24sIGNvbmRpdGlvbiwgd2hpbGUsIGZvci1sb29wXQ0KDQptdWx0aS1zZW50ZW5jZS1wYXJhZ3JhcGggc3RhcnQtcGFyYWdyYXBoIFtwYXJhZ3JhcGhdKiBlbmQtcGFyYWdyYXBoDQoNCnZhbHVlLWRlZmluaXRpb24gdHlwZS1uYW1lIG5hbWUgYXNzaWdubWVudCB2YWx1ZQ0KdmFsdWUtZGVjbGFyYXRpb24gdHlwZS1uYW1lIG5hbWUNCmhlYXAtZGVjbGFyYXRpb24gdHlwZS1uYW1lIGhlYXAtbmFtZQ0KdmFsdWUtY2xhdXNlIHZhcmlhYmxlLW5hbWUgYXNzaWdubWVudCB2YWx1ZQ0KIyBVc2UgaW4gc3RydWN0IGFuZCBjbGFzcy4NCnZhbHVlLWRlY2xhcmF0aW9uLXdpdGgtc2VtaS1jb2xvbiB2YWx1ZS1kZWNsYXJhdGlvbiBzZW1pLWNvbG9uDQoNCmNvbmRpdGlvbiBrdy1pZiBzdGFydC1icmFja2V0IHZhbHVlIGVuZC1icmFja2V0IHBhcmFncmFwaCBbZWxzZS1jb25kaXRpb25dPw0KZWxzZS1jb25kaXRpb24ga3ctZWxzZSBwYXJhZ3JhcGgNCg0Kd2hpbGUga3ctd2hpbGUgc3RhcnQtYnJhY2tldCB2YWx1ZSBlbmQtYnJhY2tldCBwYXJhZ3JhcGgNCg0KYmFzZS1mb3ItaW5jcmVhc2UgW3ZhbHVlLWNsYXVzZSwgaWdub3JlLXJlc3VsdC1mdW5jdGlvbi1jYWxsXQ0KZm9yLWxvb3Aga3ctZm9yIHN0YXJ0LWJyYWNrZXQgW3ZhbHVlLWRlZmluaXRpb24sIHZhbHVlLWRlY2xhcmF0aW9uXT8gc2VtaS1jb2xvbiBbdmFsdWVdPyBzZW1pLWNvbG9uIFtmb3ItaW5jcmVhc2VdPyBlbmQtYnJhY2tldCBwYXJhZ3JhcGggDQoNCnZhbHVlIFt2YWx1ZS13aXRoLWJyYWNrZXQsIHZhbHVlLXdpdGhvdXQtYnJhY2tldF0NCmJhc2UtdmFsdWUtd2l0aG91dC1icmFja2V0IFtmdW5jdGlvbi1jYWxsLCB2YXJpYWJsZS1uYW1lLCBpbnRlZ2VyLCBiaWd1aW50LCB1ZmxvYXQsIGJvb2wsIHN0cmluZ10KdmFsdWUtd2l0aC1icmFja2V0IHN0YXJ0LWJyYWNrZXQgdmFsdWUgZW5kLWJyYWNrZXQNCg0KaGVhcC1uYW1lIG5hbWUgc3RhcnQtc3F1YXJlLWJyYWNrZXQgdmFsdWUgZW5kLXNxdWFyZS1icmFja2V0DQoNCmlnbm9yZS1yZXN1bHQtZnVuY3Rpb24tY2FsbCBmdW5jdGlvbi1jYWxsDQoNCmZ1bmN0aW9uLWNhbGwgbmFtZSBzdGFydC1icmFja2V0IFt2YWx1ZS1saXN0XT8gZW5kLWJyYWNrZXQNCnZhbHVlLWxpc3QgW3ZhbHVlLXdpdGgtY29tbWFdKiB2YWx1ZQ0KdmFsdWUtd2l0aC1jb21tYSB2YWx1ZSBjb21tYQ0KDQpyZXR1cm4tY2xhdXNlIGt3LXJldHVybiBbdmFsdWVdPw0KDQojIEVtYmVkIGEgbG9naWMgc3RhdGVtZW50IGRpcmVjdGx5LCBsaWtlIGxvZ2ljICJpbnQgc3Rkb3V0IGFfc3RyaW5nIg0KbG9naWMga3ctbG9naWMgc3RyaW5nDQoNCmluY2x1ZGUtd2l0aC1zdHJpbmcga3ctaW5jbHVkZSBzdHJpbmcNCmluY2x1ZGUgW2luY2x1ZGUtd2l0aC1zdHJpbmcsIGluY2x1ZGUtd2l0aC1maWxlXQ0KDQppZm5kZWYtd3JhcHBlZCBrdy1pZm5kZWYgbmFtZSBbcm9vdC10eXBlXSoga3ctZW5kaWYNCmRlZmluZSBrdy1kZWZpbmUgbmFtZQ0KDQojIEFsbG93IHR5cGUqIHRvIGJlIHVzZWQgYXMgYSBzdHJpbmcuDQp0eXBlZGVmLXR5cGUtc3RyIHN0cmluZw0KdHlwZWRlZi10eXBlLW5hbWUgdHlwZS1uYW1lDQp0eXBlZGVmLXR5cGUgW3R5cGVkZWYtdHlwZS1uYW1lLCB0eXBlZGVmLXR5cGUtc3RyXQ0KdHlwZWRlZiBrdy10eXBlZGVmIHR5cGVkZWYtdHlwZSB0eXBlZGVmLXR5cGUNCg0KIyBUT0RPOiBTdXBwb3J0IHZhbHVlLWRlZmluaXRpb24NCnN0cnVjdCBrdy1zdHJ1Y3QgdHlwZS1uYW1lIHN0YXJ0LXBhcmFncmFwaCBbdmFsdWUtZGVjbGFyYXRpb24td2l0aC1zZW1pLWNvbG9uXSogZW5kLXBhcmFncmFwaCBzZW1pLWNvbG9uDQoNCnJlaW50ZXJwcmV0LWNhc3Qga3ctcmVpbnRlcnByZXQtY2FzdCBzdGFydC1icmFja2V0IHZhcmlhYmxlLW5hbWUgY29tbWEgdHlwZS1uYW1lIGVuZC1icmFja2V0DQoNCnJvb3QtdHlwZS13aXRoLXNlbWktY29sb24gW3ZhbHVlLWRlZmluaXRpb24sIGhlYXAtZGVjbGFyYXRpb24sIHZhbHVlLWRlY2xhcmF0aW9uLCBsb2dpYywgdHlwZWRlZl0/IHNlbWktY29sb24NCmJhc2Utcm9vdC10eXBlIFtyb290LXR5cGUtd2l0aC1zZW1pLWNvbG9uLCBmdW5jdGlvbiwgaW5jbHVkZSwgZGVmaW5lLCBpZm5kZWYtd3JhcHBlZCwgc3RydWN0XQ0KYmFzZS1zZW50ZW5jZS13aXRoLXNlbWktY29sb24gW2lnbm9yZS1yZXN1bHQtZnVuY3Rpb24tY2FsbCwgdmFsdWUtZGVmaW5pdGlvbiwgaGVhcC1kZWNsYXJhdGlvbiwgdmFsdWUtZGVjbGFyYXRpb24sIHZhbHVlLWNsYXVzZSwgcmV0dXJuLWNsYXVzZSwga3ctYnJlYWssIGxvZ2ljLCB0eXBlZGVmLCByZWludGVycHJldC1jYXN0XT8gc2VtaS1jb2xvbg0K"))
    End Sub
End Module
