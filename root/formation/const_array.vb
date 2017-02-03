
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector

Public Module _const_array
    <Extension()> Public Function null_or_empty(Of T, SIZE As _int64)(ByVal this As const_array(Of T, SIZE)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module

Public NotInheritable Class const_array
    Public Shared Function [New](Of T)(ByVal v() As T) As const_array(Of T)
        Return New const_array(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class const_array(Of T, __SIZE As _int64)
    Private Shared ReadOnly _size As Int64
    Protected ReadOnly v() As T

    Shared Sub New()
        _size = +alloc(Of __SIZE)()
    End Sub

    Protected Sub New()
        Me.New(_size)
    End Sub

    Protected Sub New(ByVal size As Int64)
        assert(size > 0)
        ReDim v(CInt(size) - 1)
    End Sub

    Public Sub New(ByVal v() As T)
        assert(array_size(v) = _size OrElse _size = npos)
        Me.v = v
    End Sub

    Default Public ReadOnly Property [get](ByVal i As UInt32) As T
        Get
            assert(i < size())
            Return v(CInt(i))
        End Get
    End Property

    Public Function size() As UInt32
        If _size < 0 Then
            Return array_size(v)
        Else
            Return CUInt(_size)
        End If
    End Function

    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Public Function as_array() As T()
        Dim r() As T = Nothing
        ReDim r(CInt(size() - uint32_1))
        memcpy(r, v)
        Return r
    End Function

    Public Shared Widening Operator CType(ByVal this As const_array(Of T, __SIZE)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.as_array()
    End Operator

    Public Shared Widening Operator CType(ByVal this() As T) As const_array(Of T, __SIZE)
        If isemptyarray(this) Then
            Return Nothing
        End If
        Return New const_array(Of T, __SIZE)(this)
    End Operator
End Class

Public Class const_array(Of T)
    Inherits const_array(Of T, _NPOS)

    Public Sub New(ByVal size As Int64)
        MyBase.New(size)
    End Sub

    Public Sub New(ByVal v() As T)
        MyBase.New(v)
    End Sub

    Public Shared Shadows Widening Operator CType(ByVal this() As T) As const_array(Of T)
        If isemptyarray(this) Then
            Return Nothing
        End If

        Return New const_array(Of T)(this)
    End Operator
End Class
