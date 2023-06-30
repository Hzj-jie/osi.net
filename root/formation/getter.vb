
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Interface getter(Of T)
    Function [get](ByRef k As T) As Boolean
End Interface

Public NotInheritable Class not_null_getter
    Public Shared Function [New](Of T)(ByVal g As getter(Of T)) As not_null_getter(Of T)
        Return New not_null_getter(Of T)(g)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class not_null_getter(Of T)
    Implements getter(Of T)

    Private ReadOnly g As getter(Of T)

    Public Sub New(ByVal g As getter(Of T))
        assert(Not g Is Nothing)
        Me.g = g
    End Sub

    Public Function [get](ByRef k As T) As Boolean Implements getter(Of T).get
        If g.get(k) Then
            Return type_info(Of T).is_valuetype OrElse Not k Is Nothing
        Else
            Return False
        End If
    End Function
End Class

Public NotInheritable Class predefined_instance_getter
    Public Shared Function [New](Of T As Class)(ByVal v As T) As predefined_instance_getter(Of T)
        Return New predefined_instance_getter(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class predefined_instance_getter(Of T As Class)
    Inherits predefined_getter(Of T)

    Public Sub New(ByVal v As T)
        MyBase.New(v, Not v Is Nothing)
    End Sub
End Class

Public NotInheritable Class predefined_getter
    Public Shared Function [New](Of T)(ByVal v As T, ByVal r As Boolean) As predefined_getter(Of T)
        Return New predefined_getter(Of T)(v, r)
    End Function

    Public Shared Function [New](Of T)(ByVal v As T) As predefined_getter(Of T)
        Return New predefined_getter(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class predefined_getter(Of T)
    Implements getter(Of T)

    Private ReadOnly v As T
    Private ReadOnly r As Boolean

    Public Sub New(ByVal v As T, ByVal r As Boolean)
        Me.v = v
        Me.r = r
    End Sub

    Public Sub New(ByVal v As T)
        Me.New(v, True)
    End Sub

    Public Function [get](ByRef k As T) As Boolean Implements getter(Of T).get
        k = v
        Return r
    End Function
End Class

Public Module _getter
    <Extension()> Public Function [get](Of T)(ByVal this As getter(Of T)) As T
        assert(Not this Is Nothing)
        Dim r As T = Nothing
        assert(this.[get](r))
        Return r
    End Function
End Module