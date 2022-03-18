' This file is created by genall with
' C:\Users\Hzj_j\git\osi.net\service\resource\gen\gen.exe
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
        [nlexer_rule] = Convert.FromBase64String(strcat_hint(CUInt(1024), _
        "77u/DQprdy1pZiBpZg0Ka3ctZWxzZSBlbHNlDQprdy1mb3IgZm9yDQprdy13aGlsZSB3aGlsZQ0Ka3ctZG8gZG8NCmt3LWxvb3AgbG9vcA0Ka3ctcmV0dXJuIHJldHVybg0Ka3ctYnJlYWsgYnJlYWsNCmt3LWxvZ2ljIGxvZ2ljDQprdy1pbmNsdWRlICNpbmNsdWRlDQprdy1pZm5kZWYgI2lmbmRlZg0Ka3ctZGVmaW5lICNkZWZpbmUNCmt3LWVuZGlmICNlbmRpZg0Ka3ctdHlwZWRlZiB0eXBlZGVmDQprdy1zdHJ1Y3Qgc3RydWN0DQprdy1yZWludGVycHJldC1jYXN0IHJlaW50ZXJwcmV0X2Nhc3QNCmt3LWRlbGVnYXRlIGRlbGVnYXRlDQprdy11bmRlZmluZSB1bmRlZmluZQ0Ka3ctZGVhbGxvYyBkZWFsbG9jDQoNCnNpbmdsZS1saW5lLWNvbW1lbnQgLy9bKnxcbl0qDQptdWx0aS1saW5lLWNvbW1lbnQgL1wqWyp8XCovXSpcKi8NCg0KYmxhbmsgW1xiXSsNCg0KaW5jbHVkZS13aXRoLWZpbGUgI2luY2x1ZGVbXGJdKzxbKnw+XSs+DQoNCmJvb2wgW3RydWUsZmFsc2VdDQppbnRlZ2VyIFsrLC1dP1tcZF0rDQpiaWd1aW50IFtcZF0rW2wsTF0NCnVmbG9hdCBbXGRdKi5bXGRdKw0Kc3RyaW5nICJbXCIsKnwiXSoiDQoNCmNvbW1hIFwsDQpjb2xvbiA6DQpxdWVzdGlvbi1tYXJrID8NCnN0YXJ0LXBhcmFncmFwaCB7DQplbmQtcGFyYWdyYXBoIH0NCnN0YXJ0LWJyYWNrZXQgKA0KZW5kLWJyYWNrZXQgKQ0Kc3RhcnQtc3F1YXJlLWJyYWNrZXQgXFsNCmVuZC1zcXVhcmUtYnJhY2tldCBcXQ0Kc2VtaS1jb2xvbiA7DQphc3NpZ25tZW50ID0NCg0KZG90IC4NCg0K"))
        [nlexer_rule2] = Convert.FromBase64String(strcat_hint(CUInt(44), _
        "77u/DQpyYXctbmFtZSBbXHcsX11bXHcsXGQsX10qDQo="))
        [syntaxer_rule] = Convert.FromBase64String(strcat_hint(CUInt(3976), _
        "77u/DQpJR05PUkVfVFlQRVMgYmxhbmssIHNpbmdsZS1saW5lLWNvbW1lbnQsIG11bHRpLWxpbmUtY29tbWVudA0KUk9PVF9UWVBFUyByb290LXR5cGUNCg0KZnVuY3Rpb24gdHlwZS1uYW1lIG5hbWUgc3RhcnQtYnJhY2tldCBbcGFyYW1saXN0XT8gZW5kLWJyYWNrZXQgbXVsdGktc2VudGVuY2UtcGFyYWdyYXBoDQpwYXJhbSBwYXJhbXR5cGUgbmFtZQ0KcGFyYW1saXN0IFtwYXJhbS13aXRoLWNvbW1hXSogcGFyYW0NCnBhcmFtLXdpdGgtY29tbWEgcGFyYW0gY29tbWENCg0KcGFyYWdyYXBoIFtzZW50ZW5jZSwgbXVsdGktc2VudGVuY2UtcGFyYWdyYXBoXQ0Kc2VudGVuY2UgW3NlbnRlbmNlLXdpdGgtc2VtaS1jb2xvbiwgY29uZGl0aW9uLCB3aGlsZSwgZm9yLWxvb3BdDQoNCm11bHRpLXNlbnRlbmNlLXBhcmFncmFwaCBzdGFydC1wYXJhZ3JhcGggW3BhcmFncmFwaF0qIGVuZC1wYXJhZ3JhcGgNCg0KdmFsdWUtZGVmaW5pdGlvbiB0eXBlLW5hbWUgbmFtZSBhc3NpZ25tZW50IHZhbHVlDQp2YWx1ZS1kZWNsYXJhdGlvbiB0eXBlLW5hbWUgbmFtZQ0KaGVhcC1kZWNsYXJhdGlvbiB0eXBlLW5hbWUgaGVhcC1uYW1lDQp2YWx1ZS1jbGF1c2UgdmFyaWFibGUtbmFtZSBhc3NpZ25tZW50IHZhbHVlDQoNCmNvbmRpdGlvbiBrdy1pZiBzdGFydC1icmFja2V0IHZhbHVlIGVuZC1icmFja2V0IHBhcmFncmFwaCBbZWxzZS1jb25kaXRpb25dPw0KZWxzZS1jb25kaXRpb24ga3ctZWxzZSBwYXJhZ3JhcGgNCg0Kd2hpbGUga3ctd2hpbGUgc3RhcnQtYnJhY2tldCB2YWx1ZSBlbmQtYnJhY2tldCBwYXJhZ3JhcGgNCg0KYmFzZS1mb3ItaW5jcmVhc2UgW3ZhbHVlLWNsYXVzZSwgaWdub3JlLXJlc3VsdC1mdW5jdGlvbi1jYWxsXQ0KZm9yLWxvb3Aga3ctZm9yIHN0YXJ0LWJyYWNrZXQgW3ZhbHVlLWRlZmluaXRpb24sIHZhbHVlLWRlY2xhcmF0aW9uXT8gc2VtaS1jb2xvbiBbdmFsdWVdPyBzZW1pLWNvbG9uIFtmb3ItaW5jcmVhc2VdPyBlbmQtYnJhY2tldCBwYXJhZ3JhcGggDQoNCnZhbHVlIFt2YWx1ZS13aXRoLWJyYWNrZXQsIHZhbHVlLXdpdGhvdXQtYnJhY2tldF0NCmJhc2UtdmFsdWUtd2l0aG91dC1icmFja2V0IFtmdW5jdGlvbi1jYWxsLCB2YXJpYWJsZS1uYW1lLCBpbnRlZ2VyLCBiaWd1aW50LCB1ZmxvYXQsIGJvb2wsIHN0cmluZ10KdmFsdWUtd2l0aC1icmFja2V0IHN0YXJ0LWJyYWNrZXQgdmFsdWUgZW5kLWJyYWNrZXQNCg0KaGVhcC1uYW1lIG5hbWUgc3RhcnQtc3F1YXJlLWJyYWNrZXQgdmFsdWUgZW5kLXNxdWFyZS1icmFja2V0DQoNCmlnbm9yZS1yZXN1bHQtZnVuY3Rpb24tY2FsbCBmdW5jdGlvbi1jYWxsDQoNCmZ1bmN0aW9uLWNhbGwgbmFtZSBzdGFydC1icmFja2V0IFt2YWx1ZS1saXN0XT8gZW5kLWJyYWNrZXQNCnZhbHVlLWxpc3QgW3ZhbHVlLXdpdGgtY29tbWFdKiB2YWx1ZQ0KdmFsdWUtd2l0aC1jb21tYSB2YWx1ZSBjb21tYQ0KDQpyZXR1cm4tY2xhdXNlIGt3LXJldHVybiBbdmFsdWVdPw0KDQojIEVtYmVkIGEgbG9naWMgc3RhdGVtZW50IGRpcmVjdGx5LCBsaWtlIGxvZ2ljICJpbnQgc3Rkb3V0IGFfc3RyaW5nIg0KbG9naWMga3ctbG9naWMgc3RyaW5nDQoNCmluY2x1ZGUtd2l0aC1zdHJpbmcga3ctaW5jbHVkZSBzdHJpbmcNCmluY2x1ZGUgW2luY2x1ZGUtd2l0aC1zdHJpbmcsIGluY2x1ZGUtd2l0aC1maWxlXQ0KDQppZm5kZWYtd3JhcHBlZCBrdy1pZm5kZWYgbmFtZSBbcm9vdC10eXBlXSoga3ctZW5kaWYNCmRlZmluZSBrdy1kZWZpbmUgbmFtZQ0KDQojIEFsbG93IHR5cGUqIHRvIGJlIHVzZWQgYXMgYSBzdHJpbmcuDQp0eXBlZGVmLXR5cGUtc3RyIHN0cmluZw0KdHlwZWRlZi10eXBlLW5hbWUgdHlwZS1uYW1lDQp0eXBlZGVmLXR5cGUgW3R5cGVkZWYtdHlwZS1uYW1lLCB0eXBlZGVmLXR5cGUtc3RyXQ0KdHlwZWRlZiBrdy10eXBlZGVmIHR5cGVkZWYtdHlwZSB0eXBlZGVmLXR5cGUNCg0KIyBUT0RPOiBTdXBwb3J0IHZhbHVlLWRlZmluaXRpb24NCnN0cnVjdC1ib2R5IFt2YWx1ZS1kZWNsYXJhdGlvbl0/IHNlbWktY29sb24NCnN0cnVjdCBrdy1zdHJ1Y3QgdHlwZS1uYW1lIHN0YXJ0LXBhcmFncmFwaCBbc3RydWN0LWJvZHldKiBlbmQtcGFyYWdyYXBoIHNlbWktY29sb24NCg0KIyBGdW5jdGlvbi1saWtlIGluc3RydWN0aW9ucy4NCnJlaW50ZXJwcmV0LWNhc3Qga3ctcmVpbnRlcnByZXQtY2FzdCBzdGFydC1icmFja2V0IHZhcmlhYmxlLW5hbWUgY29tbWEgdHlwZS1uYW1lIGVuZC1icmFja2V0DQp1bmRlZmluZSBrdy11bmRlZmluZSBzdGFydC1icmFja2V0IHZhcmlhYmxlLW5hbWUgZW5kLWJyYWNrZXQNCmRlYWxsb2Mga3ctZGVhbGxvYyBzdGFydC1icmFja2V0IHZhcmlhYmxlLW5hbWUgZW5kLWJyYWNrZXQNCg0Kcm9vdC10eXBlLXdpdGgtc2VtaS1jb2xvbiBbdmFsdWUtZGVmaW5pdGlvbiwgaGVhcC1kZWNsYXJhdGlvbiwgdmFsdWUtZGVjbGFyYXRpb24sIGxvZ2ljLCB0eXBlZGVmLCBkZWxlZ2F0ZV0/IHNlbWktY29sb24NCmJhc2Utcm9vdC10eXBlIFtyb290LXR5cGUtd2l0aC1zZW1pLWNvbG9uLCBmdW5jdGlvbiwgaW5jbHVkZSwgZGVmaW5lLCBpZm5kZWYtd3JhcHBlZCwgc3RydWN0XQ0KYmFzZS1zZW50ZW5jZS13aXRoLXNlbWktY29sb24gW2lnbm9yZS1yZXN1bHQtZnVuY3Rpb24tY2FsbCwgdmFsdWUtZGVmaW5pdGlvbiwgaGVhcC1kZWNsYXJhdGlvbiwgdmFsdWUtZGVjbGFyYXRpb24sIHZhbHVlLWNsYXVzZSwgcmV0dXJuLWNsYXVzZSwga3ctYnJlYWssIGxvZ2ljLCB0eXBlZGVmLCByZWludGVycHJldC1jYXN0LCBkZWxlZ2F0ZV0/IHNlbWktY29sb24NCg0KZGVsZWdhdGUga3ctZGVsZWdhdGUgdHlwZS1uYW1lIG5hbWUgc3RhcnQtYnJhY2tldCBbcGFyYW10eXBlbGlzdF0/IGVuZC1icmFja2V0DQpwYXJhbXR5cGUgdHlwZS1uYW1lIFtyZWZlcmVuY2VdPw0KcGFyYW10eXBlbGlzdCBbcGFyYW10eXBlLXdpdGgtY29tbWFdKiBwYXJhbXR5cGUNCnBhcmFtdHlwZS13aXRoLWNvbW1hIHBhcmFtdHlwZSBjb21tYQ0K"))
    End Sub
End Module
