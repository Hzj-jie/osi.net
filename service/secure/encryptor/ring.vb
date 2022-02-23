
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

Namespace encrypt
    <global_init(global_init_level.services)>
    Public Class ring
        Implements encryptor

        Public Shared ReadOnly instance As ring
        Private Const max_uint8_p_1 As UInt16 = max_uint8 + uint16_1
        Private Const max_uint8_m_1 As Byte = max_uint8 - uint8_1

        Shared Sub New()
            instance = New ring()
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function create(ByVal parameters As var, ByRef o As encryptor) As Boolean
            o = instance
            Return True
        End Function

        Private Shared Function length(ByVal key() As Byte) As Byte
            Dim r As Byte = 0
            For i As Int32 = 0 To min(8, array_size(key)) - 1
                Dim v As UInt16 = 0
                v = r
                v += If(key(i) > max_uint8_m_1, key(i) - max_uint8_m_1, key(i))
                If v > max_uint8_m_1 Then
                    v -= max_uint8_m_1
                End If
                assert(v <= max_uint8_m_1 AndAlso v >= 0)
                r = v
            Next
            Return r + uint8_1
        End Function

        Private Shared Function encrypt(ByVal key As Byte,
                                        ByVal i As Byte,
                                        ByRef o As Byte) As Boolean
            Dim v As UInt16 = 0
            v = i
            v += key
            If v > max_uint8 Then
                v -= max_uint8_p_1
            End If
            assert(v <= max_uint8 AndAlso v >= 0)
            o = v
            Return True
        End Function

        Private Shared Function decrypt(ByVal key As Byte,
                                        ByVal i As Byte,
                                        ByRef o As Byte) As Boolean
            Dim v As Int16 = 0
            v = i
            v -= key
            If v < 0 Then
                v += max_uint8_p_1
            End If
            assert(v <= max_uint8 AndAlso v >= 0)
            o = v
            Return True
        End Function

        Private Shared Function ring(ByVal key() As Byte,
                                     ByVal i() As Byte,
                                     ByVal offset As Int32,
                                     ByVal count As Int32,
                                     ByRef o() As Byte,
                                     ByVal r As _do_val_val_ref(Of Byte, Byte, Byte, Boolean)) As Boolean
            assert(r IsNot Nothing)
            If array_size(i) < offset + count OrElse
               isemptyarray(key) Then
                Return False
            Else
                Dim k As Byte = 0
                k = length(key)
                ReDim o(count - 1)
                For j As UInt32 = 0 To count - 1
                    If Not r(k, i(j + offset), o(j)) Then
                        Return False
                    End If
                Next
                Return True
            End If
        End Function

        Public Function decrypt(ByVal key() As Byte,
                                ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte) As Boolean Implements encryptor.decrypt
            Return ring(key, i, offset, count, o, AddressOf decrypt)
        End Function

        Public Function encrypt(ByVal key() As Byte,
                                ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte) As Boolean Implements encryptor.encrypt
            Return ring(key, i, offset, count, o, AddressOf encrypt)
        End Function

        Private Shared Sub init()
            assert(constructor.register("ring",
                                        Function(v As var, ByRef o As encryptor) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
