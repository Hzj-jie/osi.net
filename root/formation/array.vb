
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public NotInheritable Class array
    Public Shared Function [New](Of T)(ByVal v() As T) As array(Of T)
        Return New array(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class array(Of T, SIZE As _int64)
    Inherits const_array(Of T, SIZE)
    Implements ICloneable(Of array(Of T, SIZE)), ICloneable

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal size As Int64)
        MyBase.New(size)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal v() As T)
        MyBase.New(v)
    End Sub

    Public Shared Shadows Function move(ByVal i As array(Of T, SIZE)) As array(Of T, SIZE)
        Return const_array(Of T, SIZE).move(Of array(Of T, SIZE))(i)
    End Function

    Default Public Shadows Property data(ByVal i As UInt32) As T
        Get
            Return MyBase.[get](i)
        End Get
        Set(ByVal v As T)
            assert(i < size())
            Me.v(CInt(i)) = v
        End Set
    End Property

    Public Shadows Function as_array() As T()
        Return v
    End Function

    Public Shadows Function CloneT() As array(Of T, SIZE) Implements ICloneable(Of array(Of T, SIZE)).Clone
        Return MyBase.clone(Of array(Of T, SIZE))()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shared Shadows Widening Operator CType(ByVal this As array(Of T, SIZE)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.as_array()
    End Operator
End Class

Public Class array(Of T)
    Inherits const_array(Of T)
    Implements ICloneable(Of array(Of T)), ICloneable

    Public Sub New(ByVal size As Int64)
        MyBase.New(size)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal v() As T)
        MyBase.New(v)
    End Sub

    Public Shared Shadows Function move(ByVal i As array(Of T)) As array(Of T)
        Return const_array(Of T, _NPOS).move(Of array(Of T))(i)
    End Function

    Default Public Shadows Property data(ByVal i As UInt32) As T
        Get
            Return MyBase.[get](i)
        End Get
        Set(ByVal v As T)
            assert(i < size())
            Me.v(CInt(i)) = v
        End Set
    End Property

    Public Shadows Function CloneT() As array(Of T) Implements ICloneable(Of array(Of T)).Clone
        Return MyBase.clone(Of array(Of T))()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Overloads Shared Widening Operator CType(ByVal this() As T) As array(Of T)
        Return New array(Of T)(this)
    End Operator
End Class
