
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

Namespace encrypt
    <global_init(global_init_level.services)>
    Public Class [xor]
        Implements encryptor

        Public Shared ReadOnly instance As [xor]

        Shared Sub New()
            instance = New [xor]()
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function create(ByVal parameter As var, ByRef o As encryptor) As Boolean
            o = instance
            Return True
        End Function

        Public Function decrypt(ByVal key() As Byte,
                                ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte) As Boolean Implements encryptor.decrypt
            If array_size(i) < offset + count OrElse
               isemptyarray(key) Then
                Return False
            Else
                Dim ki As Int32 = 0
                Dim kl As Int32 = 0
                kl = array_size(key)
                ReDim o(count - 1)
                For j As UInt32 = 0 To count - 1
                    o(j) = i(j + offset) Xor key(ki)
                    ki += 1
                    If ki = kl Then
                        ki = 0
                    End If
                Next
                Return True
            End If
        End Function

        Public Function encrypt(ByVal key() As Byte,
                                ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte) As Boolean Implements encryptor.encrypt
            Return decrypt(key, i, offset, count, o)
        End Function

        Private Shared Sub init()
            assert(constructor.register("xor",
                                        Function(v As var, ByRef o As encryptor) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
