
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

Namespace sign
    <global_init(global_init_level.services)>
    Public Class bypass
        Implements signer

        Public Shared ReadOnly instance As bypass

        Shared Sub New()
            instance = New bypass()
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
            Return encrypt.bypass.instance.encrypt(key, i, offset, count, o)
        End Function

        Private Shared Sub init()
            assert(constructor.register("bypass",
                                        Function(v As var, ByRef o As signer) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
