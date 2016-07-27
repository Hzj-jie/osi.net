
Public Module _signature
    Public Const signature_2 As Byte = 1
    Public Const signature_4 As Byte = (signature_2 << 2) + signature_2
    Public Const signature_8 As Byte = (signature_4 << 4) + signature_4
    Public Const signature_16 As Int16 = (CType(signature_8, Int16) << 8) + 0
    Public Const signature_32 As Int32 = (CType(signature_16, Int32) << 16) + signature_16
    Public Const signature_64 As Int64 = (CType(signature_32, Int64) << 32) + signature_32
End Module
