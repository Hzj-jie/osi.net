
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

Namespace sign
    <global_init(global_init_level.services)>
    Public Class [xor]
        Implements signer

        Public Shared ReadOnly instance As [xor]

        Shared Sub New()
            instance = New [xor]()
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function create(ByVal parameter As var, ByRef o As signer) As Boolean
            o = instance
            Return True
        End Function

        Public Function sign(ByVal key() As Byte,
                             ByVal i() As Byte,
                             ByVal offset As UInt32,
                             ByVal count As UInt32,
                             ByRef o() As Byte) As Boolean Implements signer.sign
            Return encrypt.[xor].instance.encrypt(key, i, offset, count, o)
        End Function

        Private Shared Sub init()
            assert(constructor.register("xor",
                                        Function(v As var, ByRef o As signer) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
