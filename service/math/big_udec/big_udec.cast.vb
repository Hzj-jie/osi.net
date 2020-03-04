
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Public Function as_bytes() As Byte()
        Dim r As MemoryStream = Nothing
        r = New MemoryStream(CInt(n.byte_size() + d.byte_size() + byte_count_in_uint32))
        assert(r.write(uint32_bytes(n.byte_size())))
        assert(r.Position() = byte_count_in_uint32)
        assert(r.write(n.as_bytes()))
        assert(r.write(d.as_bytes()))
        Return r.export()
    End Function
End Class
