
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/gen/gen.exe with
'case1 case2 case3 case4 heap callee_ref
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports osi.root.connector

Friend Module _import_executor_cases
    Public ReadOnly case1() As Byte
    Public ReadOnly case2() As Byte
    Public ReadOnly case3() As Byte
    Public ReadOnly case4() As Byte
    Public ReadOnly heap() As Byte
    Public ReadOnly callee_ref() As Byte

    Sub New()
        case1 = Convert.FromBase64String(strcat_hint(CUInt(1532), _
        "DQojIyBkZWZpbmUgYSB0eXBlIHVpbnQgb2YgNCBieXRlcw0KdHlwZSB1aW50IDQNCiMjIGRlZmluZSBhIHR5cGUgYm9vbCBvZiAxIGJ5dGUNCnR5cGUgYm9vbCAxDQoNCmNhbGxlZSBjb3B5LW9uZS1ieXRlMiB0eXBlKiAoIHNvdXJjZSB0eXBlKiB0YXJnZXQgdHlwZSogcG9zIHVpbnQgbGVuIHVpbnQgKSB7DQogICAgZGVmaW5lIGNoYXIgdHlwZSoNCiAgICBjdXRfbGVuIGNoYXIgc291cmNlIHBvcyBsZW4NCiAgICBhcHBlbmQgdGFyZ2V0IGNoYXINCiAgICByZXR1cm4gY29weS1vbmUtYnl0ZTIgdGFyZ2V0DQp9DQoNCmRlZmluZSBzb3VyY2UgdHlwZSoNCmRlZmluZSB0YXJnZXQxIHR5cGUqDQpkZWZpbmUgdGFyZ2V0MiB0eXBlKg0KDQpjYWxsZWUgY29weS1vbmUtYnl0ZTEgdHlwZTAgKCBzb3VyY2UgdHlwZSogcG9zIHVpbnQgbGVuIHVpbnQgKSB7DQogICAgZGVmaW5lIGNoYXIgdHlwZSoNCiAgICBjdXRfbGVuIGNoYXIgc291cmNlIHBvcyBsZW4NCiAgICBhcHBlbmQgdGFyZ2V0MSBjaGFyDQogICAgcmV0dXJuIGNvcHktb25lLWJ5dGUxICoNCn0NCg0KIyMgY29weSBhIGNvbnN0IHN0cmluZyB0byBzb3VyY2UNCmNvcHlfY29uc3Qgc291cmNlIEVoZWxsb1x4MjB3b3JsZFxuDQoNCmRlZmluZSAwIHVpbnQNCmNvcHlfY29uc3QgMCBpMA0KZGVmaW5lIDEgdWludA0KY29weV9jb25zdCAxIGkxDQoNCmRlZmluZSBsZW5ndGggdWludA0Kc2l6ZW9mIGxlbmd0aCBzb3VyY2UNCg0KZGVmaW5lIGkgdWludA0KY29weSBpIGxlbmd0aA0KZGVmaW5lIG5vdC1maW5pc2hlZCBib29sDQptb3JlIG5vdC1maW5pc2hlZCBpIDANCndoaWxlX3RoZW4gbm90LWZpbmlzaGVkIHsNCiAgICBzdWJ0cmFjdCBpIGkgMQ0KICAgIGNhbGxlciBjb3B5LW9uZS1ieXRlMSAoIHNvdXJjZSBpIDEgKQ0KICAgIG1vcmUgbm90LWZpbmlzaGVkIGkgMA0KfQ0KDQpjb3B5IGkgbGVuZ3RoDQptb3JlIG5vdC1maW5pc2hlZCBpIDANCndoaWxlX3RoZW4gbm90LWZpbmlzaGVkIHsNCiAgICBzdWJ0cmFjdCBpIGkgMQ0KICAgIGRlZmluZSByZXN1bHQgdHlwZSoNCiAgICBjYWxsZXIgY29weS1vbmUtYnl0ZTIgcmVzdWx0ICggc291cmNlIHRhcmdldDIgaSAxICkNCiAgICBtb3ZlIHRhcmdldDIgcmVzdWx0DQogICAgbW9yZSBub3QtZmluaXNoZWQgaSAwDQp9DQo="))
        case2 = Convert.FromBase64String(strcat_hint(CUInt(2344), _
        "77u/CiMjIFVzZSBhIHN0dXBpZCB3YXkgdG8gY2FsY3VsYXRlIG4hIGZyb20gMSB0byAxMDAgYW5kIHB1c2ggdGhlIHJlc3VsdHMgaW50byBhCiMjIHNsaWNlIGJ1ZmZlci4KCmRlZmluZSBuISB0eXBlKgoKdHlwZSB1aW50IDQKdHlwZSBib29sIDEKCmRlZmluZSAwIHVpbnQKY29weV9jb25zdCAwIGkwCmRlZmluZSAxIHVpbnQKY29weV9jb25zdCAxIGkxCmRlZmluZSAyIHVpbnQKY29weV9jb25zdCAyIGkyCmRlZmluZSAzIHVpbnQKY29weV9jb25zdCAzIGkzCgpjYWxsZWUgbWV0aG9kMSB0eXBlMCAoIG4gdHlwZSogKSB7CiAgZGVmaW5lIHJlc3VsdCB0eXBlKgogIGNvcHlfY29uc3QgcmVzdWx0IGkxCiAgZGVmaW5lIGkgdHlwZSoKICBjb3B5X2NvbnN0IGkgaTAKICBkZWZpbmUgbm90LWNvbnRpbnVlIGJvb2wKICBlcXVhbCBub3QtY29udGludWUgaSBuCiAgZG9fdW50aWwgbm90LWNvbnRpbnVlIHsKICAgIGFkZCBpIGkgMQogICAgbXVsdGlwbHkgcmVzdWx0IHJlc3VsdCBpCiAgICBlcXVhbCBub3QtY29udGludWUgaSBuCiAgfQogIGFwcGVuZF9zbGljZSBuISByZXN1bHQKfQoKY2FsbGVlIG1ldGhvZDIgdHlwZTAgKCBuIHR5cGUqICkgewogIGRlZmluZSByZXN1bHQgdHlwZSoKICBjb3B5X2NvbnN0IHJlc3VsdCBpMQogIGRlZmluZSBpIHR5cGUqCiAgY29weV9jb25zdCBpIGkwCiAgZGVmaW5lIGNvbnRpbnVlIGJvb2wKICBsZXNzIGNvbnRpbnVlIGkgbgogIGRvX3doaWxlIGNvbnRpbnVlIHsKICAgIGFkZCBpIGkgMQogICAgbXVsdGlwbHkgcmVzdWx0IHJlc3VsdCBpCiAgICBsZXNzIGNvbnRpbnVlIGkgbgogIH0KICBhcHBlbmRfc2xpY2UgbiEgcmVzdWx0Cn0KCmNhbGxlZSBtZXRob2QzLWltcGwgdHlwZSogKCBuIHR5cGUqICkgewogIGRlZmluZSByZXN1bHQgdHlwZSoKICBjb3B5X2NvbnN0IHJlc3VsdCBpMQogIGRlZmluZSBpIHR5cGUqCiAgY29weV9jb25zdCBpIGkyCiAgZGVmaW5lIGNvbnRpbnVlIGJvb2wKICBsZXNzX29yX2VxdWFsIGNvbnRpbnVlIGkgbgogIHdoaWxlX3RoZW4gY29udGludWUgewogICAgbXVsdGlwbHkgcmVzdWx0IHJlc3VsdCBpCiAgICBhZGQgaSBpIDEKICAgIGxlc3Nfb3JfZXF1YWwgY29udGludWUgaSBuCiAgfQogIHJldHVybiBtZXRob2QzLWltcGwgcmVzdWx0Cn0KCmNhbGxlZSBtZXRob2QzIHR5cGUwICggbiB0eXBlKiApIHsKICBkZWZpbmUgcmVzdWx0IHR5cGUqCiAgY2FsbGVyIG1ldGhvZDMtaW1wbCByZXN1bHQgKCBuICkKICBhcHBlbmRfc2xpY2UgbiEgcmVzdWx0IAp9CgpkZWZpbmUgMTAwIHVpbnQKY29weV9jb25zdCAxMDAgaTEwMApkZWZpbmUgaSB0eXBlKgpjb3B5X2NvbnN0IGkgaTEKZGVmaW5lIGNvbnRpbnVlIGJvb2wKbGVzc19vcl9lcXVhbCBjb250aW51ZSBpIDEwMAp3aGlsZV90aGVuIGNvbnRpbnVlIHsKICBkZWZpbmUgcm5kIHR5cGUqCiAgaW50ZXJydXB0IGN1cnJlbnRfbXMgMSBybmQKICBkZWZpbmUgcmVzdWx0IHR5cGUqCiAgZGl2aWRlIHJlc3VsdCBybmQgcm5kIDMKCiAgZGVmaW5lIGNob29zZSBib29sCiAgZXF1YWwgY2hvb3NlIHJuZCAwCiAgaWYgY2hvb3NlIHsKICAgIGNhbGxlciBtZXRob2QxICggaSApCiAgfQogIGVxdWFsIGNob29zZSBybmQgMQogIGlmIGNob29zZSB7CiAgICBjYWxsZXIgbWV0aG9kMiAoIGkgKQogIH0KICBlcXVhbCBjaG9vc2Ugcm5kIDIKICBpZiBjaG9vc2UgewogICAgY2FsbGVyIG1ldGhvZDMgKCBpICkKICB9CgogIGFkZCBpIGkgMQogIGxlc3Nfb3JfZXF1YWwgY29udGludWUgaSAxMDAKfQo="))
        case3 = Convert.FromBase64String(strcat_hint(CUInt(1212), _
        "77u/DQpkZWZpbmUgb3V0cHV0IHR5cGUqDQoNCnR5cGUgdWludCA0DQp0eXBlIGJvb2wgMQ0KDQpkZWZpbmUgMCB1aW50DQpjb3B5X2NvbnN0IDAgaTANCmRlZmluZSAxIHVpbnQNCmNvcHlfY29uc3QgMSBpMQ0KZGVmaW5lIDIgdWludA0KY29weV9jb25zdCAyIGkyDQpkZWZpbmUgMyB1aW50DQpjb3B5X2NvbnN0IDMgaTMNCg0KY2FsbGVlIG1ldGhvZDEgdHlwZTAgKCBuIHR5cGUqICkgew0KICBhcHBlbmRfc2xpY2Ugb3V0cHV0IG4NCn0NCg0KY2FsbGVlIG1ldGhvZDIgdHlwZTAgKCBuIHR5cGUqICkgew0KICBhcHBlbmRfc2xpY2Ugb3V0cHV0IG4NCn0NCg0KY2FsbGVlIG1ldGhvZDMgdHlwZTAgKCBuIHR5cGUqICkgew0KICBhcHBlbmRfc2xpY2Ugb3V0cHV0IG4NCn0NCg0KZGVmaW5lIDEwMCB1aW50DQpjb3B5X2NvbnN0IDEwMCBpMTAwDQpkZWZpbmUgaSB0eXBlKg0KY29weV9jb25zdCBpIGkxDQpkZWZpbmUgY29udGludWUgYm9vbA0KbGVzc19vcl9lcXVhbCBjb250aW51ZSBpIDEwMA0Kd2hpbGVfdGhlbiBjb250aW51ZSB7DQogIGRlZmluZSBybmQgdHlwZSoNCiAgaW50ZXJydXB0IGN1cnJlbnRfbXMgMSBybmQNCiAgZGVmaW5lIHJlc3VsdCB0eXBlKg0KICBkaXZpZGUgcmVzdWx0IHJuZCBybmQgMw0KDQogIGRlZmluZSBjaG9vc2UgYm9vbA0KICBlcXVhbCBjaG9vc2Ugcm5kIDANCiAgaWYgY2hvb3NlIHsNCiAgICBjYWxsZXIgbWV0aG9kMSAoIGkgKQ0KICB9DQogIGVxdWFsIGNob29zZSBybmQgMQ0KICBpZiBjaG9vc2Ugew0KICAgIGNhbGxlciBtZXRob2QyICggaSApDQogIH0NCiAgZXF1YWwgY2hvb3NlIHJuZCAyDQogIGlmIGNob29zZSB7DQogICAgY2FsbGVyIG1ldGhvZDMgKCBpICkNCiAgfQ0KDQogIGFkZCBpIGkgMQ0KICBsZXNzX29yX2VxdWFsIGNvbnRpbnVlIGkgMTAwDQp9DQo="))
        case4 = Convert.FromBase64String(strcat_hint(CUInt(276), _
        "77u/CmRlZmluZSB0ZW1wX3N0cmluZyB0eXBlKgoKY2FsbGVlIG1haW4gdHlwZTAgKCApIHsKICBkZWZpbmUgcmVzdWx0IHR5cGUqCiAgY29weV9jb25zdCByZXN1bHQgRWhlbGxvXHgyMHdvcmxkCiAgaW50ZXJydXB0IHN0ZG91dCByZXN1bHQgdGVtcF9zdHJpbmcKICBpbnRlcnJ1cHQgc3RkZXJyIHJlc3VsdCB0ZW1wX3N0cmluZwp9CgpjYWxsZXIgbWFpbiAoICk="))
        heap = Convert.FromBase64String(strcat_hint(CUInt(616), _
        "DQp0eXBlIHVpbnQgNA0KdHlwZSBib29sIDENCg0KZGVmaW5lIHRydWUgYm9vbA0KY29weV9jb25zdCB0cnVlIGIxDQoNCiMjIEFsd2F5cyB1c2UgYSBuZXcgc2NvcGUgdG8gZW5zdXJlIHRoZSBoZWFwIGFsbG9jYXRpb24gY2FuIGJlIGZyZWVkLg0KaWYgdHJ1ZSB7DQoNCiAgICBkZWZpbmUgdWludF8xMDAgdWludA0KICAgIGNvcHlfY29uc3QgdWludF8xMDAgaTEwMA0KICAgIGRlZmluZV9oZWFwIGggdWludCB1aW50XzEwMA0KICAgIGRlZmluZSB1aW50Xzk5IHVpbnQNCiAgICBjb3B5X2NvbnN0IHVpbnRfOTkgaTk5DQogICAgY29weV9oZWFwX2luIGggdWludF85OSB1aW50Xzk5DQogICAgZGVmaW5lIG91dCB1aW50DQogICAgY29weV9oZWFwX291dCBvdXQgaCB1aW50Xzk5DQoNCiAgICBkZWZpbmUgdGVtcF9zdHJpbmcgdHlwZSoNCiAgICBpbnRlcnJ1cHQgcHV0Y2hhciBvdXQgdGVtcF9zdHJpbmcNCg0KfQ0K"))
        callee_ref = Convert.FromBase64String(strcat_hint(CUInt(328), _
        "77u/DQpjYWxsZWUgbSB0eXBlMCAoIG4gdHlwZSomIGkgdHlwZSogKSB7DQogIGFwcGVuZCBuIGkNCn0NCg0KZGVmaW5lIG4gdHlwZSoNCmRlZmluZSBpIHR5cGUqDQpjb3B5X2NvbnN0IGkgc2QNCg0KY2FsbGVyIG0gKCBuIGkgKQ0KY2FsbGVyIG0gKCBuIGkgKQ0KY2FsbGVyIG0gKCBuIGkgKQ0KY2FsbGVyIG0gKCBuIGkgKQ0KDQpkZWZpbmUgdGVtcF9zdHJpbmcgdHlwZSoNCmludGVycnVwdCBzdGRvdXQgbiB0ZW1wX3N0cmluZw0K"))
    End Sub
End Module
