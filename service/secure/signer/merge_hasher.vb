
Imports osi.root.template
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

Namespace sign
    Public MustInherit Class merge_hasher(Of _New As __do(Of merge_method, merge_hasher(Of _New)))
        Inherits hasher

        Public Shared ReadOnly concat As merge_hasher(Of _New)
        Public Shared ReadOnly ring As merge_hasher(Of _New)
        Public Shared ReadOnly [xor] As merge_hasher(Of _New)

        Public Enum merge_method
            concat
            ring
            [xor]
        End Enum

        Shared Sub New()
            Dim [New] As _do(Of merge_method, merge_hasher(Of _New)) = Nothing
            [New] = -alloc(Of _New)()
            concat = [New](merge_method.concat)
            ring = [New](merge_method.ring)
            [xor] = [New](merge_method.xor)
        End Sub

        Private ReadOnly m As merge_method

        Protected Sub New(ByVal merge_method As merge_method)
            MyBase.New()
            m = merge_method
        End Sub

        Protected MustOverride Overloads Function compute(ByVal i() As Byte) As Byte()

        Private Function merge(ByVal key() As Byte,
                               ByVal i() As Byte,
                               ByVal offset As UInt32,
                               ByVal count As UInt32) As Byte()
            Select Case m
                Case merge_method.concat
                    Dim b() As Byte = Nothing
                    ReDim b(array_size(key) + count - uint32_1)
                    memcpy(b, key)
                    memcpy(b, array_size(key), i, offset, count)
                    Return compute(b)
                Case merge_method.ring
                    Dim o() As Byte = Nothing
                    assert(encrypt.ring.instance.encrypt(key, i, offset, count, o))
                    Return o
                Case merge_method.[xor]
                    Dim o() As Byte = Nothing
                    assert(encrypt.xor.instance.encrypt(key, i, offset, count, o))
                    Return o
                Case Else
                    assert(False)
                    Return Nothing
            End Select
        End Function

        Protected NotOverridable Overrides Function compute(ByVal key() As Byte,
                                                            ByVal i() As Byte,
                                                            ByVal offset As UInt32,
                                                            ByVal count As UInt32) As Byte()
            Return compute(merge(key, i, offset, count))
        End Function

        Private Shared Function create(ByVal v As var, ByRef o As merge_hasher(Of _New)) As Boolean
            If v Is Nothing Then
                Return False
            Else
                Const merger As String = "merger"
                v.bind(merger)
                Dim m As String = Nothing
                m = v(merger)
                If strsame(m, "ring", False) Then
                    o = merge_hasher(Of _New).ring
                ElseIf strsame(m, "xor", False) Then
                    o = merge_hasher(Of _New).xor
                Else
                    o = merge_hasher(Of _New).concat
                End If
                Return True
            End If
        End Function

        Public Shared Sub register(ByVal type As String)
            assert(constructor.register(type,
                                        Function(v As var, ByRef o As hasher) As Boolean
                                            Return create(v, o)
                                        End Function))
        End Sub
    End Class
End Namespace
