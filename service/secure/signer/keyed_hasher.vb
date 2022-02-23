
Imports System.Security.Cryptography
Imports osi.root.template
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

Namespace sign
    Public Class keyed_hasher(Of T As __do(Of KeyedHashAlgorithm))
        Inherits hasher

        Public Shared ReadOnly instance As keyed_hasher(Of T)
        Private Shared ReadOnly create As Func(Of KeyedHashAlgorithm)
        <ThreadStatic> Private Shared h As KeyedHashAlgorithm

        Shared Sub New()
            create = -alloc(Of T)()
            instance = New keyed_hasher(Of T)()
        End Sub

        Private Sub New()
        End Sub

        Protected NotOverridable Overrides Function compute(ByVal key() As Byte,
                                                            ByVal i() As Byte,
                                                            ByVal offset As UInt32,
                                                            ByVal count As UInt32) As Byte()
            If h Is Nothing Then
                h = create()
            End If
            assert(h IsNot Nothing)
            h.Key() = key
            Return h.ComputeHash(i, offset, count)
        End Function

        Public Shared Sub register(ByVal type As String)
            assert(constructor.register(type,
                                        Function(v As var) As hasher
                                            Return instance
                                        End Function))
        End Sub
    End Class
End Namespace
