
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Public Interface bytes_transformer
    Function transform(ByVal i() As Byte,
                       ByVal offset As UInt32,
                       ByVal count As UInt32,
                       ByRef o() As Byte) As Boolean
End Interface

Public Module _bytes_transformer
    Private Function transform(ByVal t As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, Byte(), Boolean),
                               ByRef i() As Byte,
                               ByRef offset As UInt32,
                               ByRef count As UInt32) As Boolean
        assert(Not t Is Nothing)
        Dim b() As Byte = Nothing
        If t(i, offset, count, b) Then
            i = b
            offset = 0
            count = array_size(b)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function transform(ByVal bt As bytes_transformer,
                                            ByRef i() As Byte,
                                            ByRef offset As UInt32,
                                            ByRef count As UInt32) As Boolean
        assert(Not bt Is Nothing)
        Return transform(AddressOf bt.transform, i, offset, count)
    End Function

    Private Function transform(ByVal bt As bytes_transformer_collection,
                               ByVal forward As Boolean,
                               ByRef i() As Byte,
                               ByRef offset As UInt32,
                               ByRef count As UInt32) As Boolean
        assert(Not bt Is Nothing)
        If bt.empty() Then
            Return True
        ElseIf forward Then
            Return transform(AddressOf bt.transform_forward, i, offset, count)
        Else
            Return transform(AddressOf bt.transform_backward, i, offset, count)
        End If
    End Function

    <Extension()> Public Function transform_forward(ByVal bt As bytes_transformer_collection,
                                                    ByRef i() As Byte,
                                                    ByRef offset As UInt32,
                                                    ByRef count As UInt32) As Boolean
        Return transform(bt, True, i, offset, count)
    End Function

    <Extension()> Public Function transform_backward(ByVal bt As bytes_transformer_collection,
                                                     ByRef i() As Byte,
                                                     ByRef offset As UInt32,
                                                     ByRef count As UInt32) As Boolean
        Return transform(bt, False, i, offset, count)
    End Function

    <Extension()> Public Function transform(ByVal bt As bytes_transformer,
                                            ByVal i() As Byte,
                                            ByVal count As UInt32,
                                            ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function transform(ByVal bt As bytes_transformer,
                                            ByVal i() As Byte,
                                            ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform(i, uint32_0, array_size(i), o)
    End Function

    <Extension()> Public Function transform_forward(ByVal bt As bytes_transformer_collection,
                                                    ByVal i() As Byte,
                                                    ByVal count As UInt32,
                                                    ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform_forward(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function transform_forward(ByVal bt As bytes_transformer_collection,
                                                    ByVal i() As Byte,
                                                    ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform_forward(i, uint32_0, array_size(i), o)
    End Function

    <Extension()> Public Function transform_backward(ByVal bt As bytes_transformer_collection,
                                                     ByVal i() As Byte,
                                                     ByVal count As UInt32,
                                                     ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform_backward(i, uint32_0, count, o)
    End Function

    <Extension()> Public Function transform_backward(ByVal bt As bytes_transformer_collection,
                                                     ByVal i() As Byte,
                                                     ByRef o() As Byte) As Boolean
        assert(Not bt Is Nothing)
        Return bt.transform_backward(i, uint32_0, array_size(i), o)
    End Function
End Module

Public Class bytes_transformer_wrapper
    Implements bytes_transformer

    Private ReadOnly v As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, Byte(), Boolean)

    Public Sub New(ByVal v As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, Byte(), Boolean))
        Me.v = v
    End Sub

    Public Function transform(ByVal i() As Byte,
                              ByVal offset As UInt32,
                              ByVal count As UInt32,
                              ByRef o() As Byte) As Boolean Implements bytes_transformer.transform
        If v Is Nothing Then
            Return False
        Else
            Return v(i, offset, count, o)
        End If
    End Function
End Class

Public Class forward_bytes_transformer
    Implements bytes_transformer

    Public Shared Function forward(ByVal i() As Byte,
                                   ByVal offset As UInt32,
                                   ByVal count As UInt32,
                                   ByRef o() As Byte) As Boolean
        Dim p As piece = Nothing
        If piece.create(i, offset, count, p) Then
            o = p.export()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function transform(ByVal i() As Byte,
                              ByVal offset As UInt32,
                              ByVal count As UInt32,
                              ByRef o() As Byte) As Boolean Implements bytes_transformer.transform
        Return forward(i, offset, count, o)
    End Function
End Class

Public Class bytes_transformer_collection
    Implements bytes_transformer
    Private ReadOnly trans As vector(Of bytes_transformer)

    Public Sub New()
        trans = New vector(Of bytes_transformer)()
    End Sub

    Public Sub New(ByVal ParamArray vs() As bytes_transformer)
        Me.New()
        assert(chain(vs))
    End Sub

    Public Function empty() As Boolean
        Return trans.empty()
    End Function

    Public Function size() As UInt32
        Return trans.size()
    End Function

    Public Sub clear()
        trans.clear()
    End Sub

    Public Function chain(ByVal ParamArray v() As bytes_transformer) As Boolean
        For i As Int32 = 0 To array_size_i(v) - 1
            If v(i) Is Nothing Then
                Return False
            Else
                trans.emplace_back(v(i))
            End If
        Next
        Return True
    End Function

    Public Function transform(ByVal i() As Byte,
                              ByVal offset As UInt32,
                              ByVal count As UInt32,
                              ByRef o() As Byte) As Boolean Implements bytes_transformer.transform
        Return transform_forward(i, offset, count, o)
    End Function

    Public Function transform(ByVal i() As Byte,
                              ByVal offset As UInt32,
                              ByVal count As UInt32,
                              ByVal forward As Boolean,
                              ByRef o() As Byte) As Boolean
        If empty() Then
            Return forward_bytes_transformer.forward(i, offset, count, o)
        Else
            Dim s As UInt32 = 0
            Dim e As UInt32 = 0
            Dim g As Int32 = 0
            If forward Then
                s = 0
                e = trans.size() - uint32_1
                g = 1
            Else
                s = trans.size() - uint32_1
                e = 0
                g = -1
            End If
            For j As Int32 = CInt(s) To CInt(e) Step g
                If trans(CUInt(j)).transform(i, offset, count, o) Then
                    i = o
                    offset = 0
                    count = array_size(i)
                Else
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function transform_forward(ByVal i() As Byte,
                                      ByVal offset As UInt32,
                                      ByVal count As UInt32,
                                      ByRef o() As Byte) As Boolean
        Return transform(i, offset, count, True, o)
    End Function

    Public Function transform_backward(ByVal i() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32,
                                       ByRef o() As Byte) As Boolean
        Return transform(i, offset, count, False, o)
    End Function
End Class
