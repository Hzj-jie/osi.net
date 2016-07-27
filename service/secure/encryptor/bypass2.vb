
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.device

'test purpose, it's not bypass, but do exactly as bypass
Namespace encrypt
    <global_init(global_init_level.services)>
    Public Class bypass2
        Implements encryptor

        Public Shared ReadOnly instance As bypass2

        Shared Sub New()
            instance = New bypass2()
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
            Return forward_bytes_transformer.forward(i, offset, count, o)
        End Function

        Public Function encrypt(ByVal key() As Byte,
                                ByVal i() As Byte,
                                ByVal offset As UInt32,
                                ByVal count As UInt32,
                                ByRef o() As Byte) As Boolean Implements encryptor.encrypt
            Return forward_bytes_transformer.forward(i, offset, count, o)
        End Function

        Private Shared Sub init()
            assert(constructor.register("bypass2",
                                        Function(v As var, ByRef o As encryptor) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
