
' TODO Remove
Imports osi.root.constants

Friend Module _bytes_array
    Public Function validate(Of T)(ByVal b() As Byte,
                                   ByVal io As T,
                                   ByVal offset As UInt32,
                                   ByRef sz As UInt32) As Boolean
        sz = get_size(io)
        Return array_size(b) >= offset + sz
    End Function

    Public Function get_size(Of T)(ByVal io As T) As UInt32
        Dim r As Int32 = 0
        r = sizeof(Of T)()
        assert(r > 0)
        Return r
    End Function
End Module

Public Module _bytes_bytes
    Public Function bytes_bytes_ref(ByVal i() As Byte,
                                    ByVal ii As UInt32,
                                    ByVal len As UInt32,
                                    ByRef o() As Byte) As Boolean
        If array_size(i) < ii + len Then
            Return False
        Else
            If len = uint32_0 Then
                ReDim o(-1)
            Else
                ReDim o(len - uint32_1)
                memcpy(o, uint32_0, i, ii, len)
            End If
            Return True
        End If
    End Function

    Public Function bytes_bytes_ref(ByVal i() As Byte,
                                    ByRef o() As Byte,
                                    Optional ByRef offset As UInt32 = 0) As Boolean
        If offset > array_size(i) Then
            Return False
        Else
            If isemptyarray(i) Then
                assert(offset = 0)
                o = Nothing
            Else
                ReDim o(array_size(i) - offset - uint32_1)
                memcpy(o, uint32_0, i, offset, array_size(i) - offset)
                offset = array_size(i)
            End If
            Return True
        End If
    End Function

    Public Function bytes_bytes_val(ByVal i() As Byte,
                                    ByVal o() As Byte,
                                    Optional ByRef offset As UInt32 = 0) As Boolean
        If offset + array_size(i) > array_size(o) Then
            Return False
        Else
            memcpy(o, offset, i)
            offset += array_size(i)
            Return True
        End If
    End Function

    Public Function bytes_bytes_val(ByVal i() As Byte,
                                    ByRef ii As UInt32,
                                    ByVal o() As Byte,
                                    ByRef oi As UInt32) As Boolean
        If array_size(i) <= ii OrElse array_size(o) <= oi Then
            Return False
        Else
            Dim c As UInt32 = 0
            c = min(array_size(i) - ii, array_size(o) - oi)
            assert(c > 0)
            memcpy(o, oi, i, ii, c)
            ii += c
            oi += c
            Return True
        End If
    End Function

    Public Function bytes_bytes_val(ByVal i() As Byte,
                                    ByVal ii As UInt32,
                                    ByVal len As UInt32,
                                    ByVal o() As Byte,
                                    ByRef oi As UInt32) As Boolean
        If array_size(i) < ii + len OrElse
           array_size(o) < oi + len Then
            Return False
        Else
            memcpy(i, ii, o, oi, len)
            oi += len
            Return True
        End If
    End Function
End Module
