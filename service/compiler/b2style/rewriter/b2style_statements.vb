
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'prefix suffix
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_statements
    Public ReadOnly prefix() As Byte
    Public ReadOnly suffix() As Byte

    Sub New()
        prefix = Convert.FromBase64String(strcat_hint(CUInt(1492), _
        "H4sIAAAAAAAEALVZS27bMBBdN0DuQGRlAVokRRcFjAJe9QTdC7JFJzQU0RWpxG6Rk3XRI/UK5UeURGlIkbaMLCJTM2/evCGpkfjvz9/7O8ZrUj2j7WfGzyXOMl43OBOD6Bt6+CGuH9ZTm31ess7ou/whre7v3igpeivGi4w2fNV6kwT9vr9DqKTPZIceSMVxXTdHjoSdMEMEbTbHGu/JCbMNx69HttGeEvsDhhcIIfDiKhp+S2nZpSB/dBHIHq3MNZqmO1EyWUvLD4SFTvNenbatm5+cFCCUnLKNJmd5BZEDRYN1JYm/tjMQ0qSHECXvbXZNXeOKZ69sZZQRt2vMmpKvwXnSe7hmSuv+oPxrzJu66hEVhfFCEb+zHa12eb8KUtReHewJmx+PuCrEND1Y+CQKenvm+FpgTiX2SkEZ6VsbNgQAmaSor4cqXmeVV0VbTkFT/j/YM7ZFPQxDqOkGw9E6BE3O8rUZPkyH5+NUFF79I080gtZI5Nmak0JZPbSSwwZwXA+3V0kH+9zYTd0cOskBfyArUfyzyctVG1yqqq+Mrsp4unyUVzs+mWL22oDC6YxSpP4fbJ5jZoCKSQrJdEgcU1ADtfkNk7tZxOca52J/yfhLXkVr+0prfIm0VtAghW2aC6cdoPfS8UvM2GWaS89LNO8jBgk+ILhktgFSLx+Z1nN7x5gDHXQUA/KpcEgnq1WOBoUPEv42oQN0XyqwWSrXqW4v0OtJRGu/NIHICkSHt5+ue8IzUuknrLpzsluuXcMzVpIdRifxN+gmRZvEeF5xthFMs0f4FiO/cEb3Ct7agE5gp9uSMW3FTagIE5iJ6hLHVOSg7h9vQkYih7CRG5hkMlRFWZzXIKfzkpzOrnYuLwr3mjUteyH7dWfHbk9GCQiuPy8YOJcd71IyArDAovD14wUulJJEvW443mCCIuhaJy7dX0nVsFnlWbPldb7j4fJrXF8BPJgxVdCB3HWICDMqxlgp0eWQY3meFcsYRohloH16eWCj9DKx3JJFRPJLVpA3UmBPt6l/TvtN7Wg6Ttd3B38nakvcUnEI3H3EuJ4EWAuNARekZQaUY0ltoMKNWE0mPPVsyXOl838riiqd5HFB3SIZRNdN0oot2pWcAop2pO+4nt2ilFX4/qRBfZuTCzBmZ9JR3NtSaAz/nrQVRuqj21yzUUU0GwbU23D4AUGSnvbCjwZlTefnBY2YFC2kL2U/HETQnbAfa5wvw6Vsz3cm49O0dfL22k9wJ20r0AWBXnYiQ4AL5QQvlC7u9ZlBa+fkXDsqcIFdqnat1kLSykiQtJfEidZXBl8oR7/I44MG6d8eNgw/U1tnDe8vpMTI/eL+mHRnXSoz9G303ErR06Nk8IkMb5lWpLtrOar3kRR9+apuseGtwVnH5MBk/N65TZIUse6krFOJGUHsMy8pR3cuNtBjeq4GCdh/qoBR1VGZC9U64wtB9fK0Epk54PMCDI/3/gN2PDNErB4AAA=="))

        assert(prefix.ungzip(prefix))
        suffix = Convert.FromBase64String(strcat_hint(CUInt(32), _
        "H4sIAAAAAAAEAHu/ez8A4ZcQAQMAAAA="))

        assert(suffix.ungzip(suffix))
    End Sub
End Module
