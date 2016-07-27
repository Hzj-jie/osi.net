
Imports osi.root.constants
Imports osi.root.connector

Namespace sign
    Public MustInherit Class hasher
        Implements signer

        Protected MustOverride Function compute(ByVal key() As Byte,
                                                ByVal i() As Byte,
                                                ByVal offset As UInt32,
                                                ByVal count As UInt32) As Byte()

        Public Function sign(ByVal key() As Byte,
                             ByVal i() As Byte,
                             ByVal offset As UInt32,
                             ByVal count As UInt32,
                             ByRef o() As Byte) As Boolean Implements signer.sign
            If isemptyarray(key) OrElse
               array_size(i) < offset + count Then
                Return False
            ElseIf count = uint32_0 Then
                ReDim o(npos)
                Return True
            Else
                o = compute(key, i, offset, count)
                Return True
            End If
        End Function
    End Class
End Namespace
