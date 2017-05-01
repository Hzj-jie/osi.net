
Imports System.Runtime.CompilerServices

Public Interface getter(Of T)
    Function [get](ByRef k As T) As Boolean
End Interface

Public NotInheritable Class predefined_instance_getter(Of T As Class)
    Inherits predefined_getter(Of T)

    Public Sub New(ByVal v As T)
        MyBase.New(v, Not v Is Nothing)
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