
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates

' Stop using the following classes or anyone derived from them.
' Use either Action or Func for function parameters,
' or func_t as the type parameter (should be quite rare) or quite performance sensitive.
' TODO: Remove the following classes and the entire template project.
Public NotInheritable Class __do
    Public NotInheritable Class default_of(Of T As New)
        Inherits __do(Of T)

        Protected Overrides Function at() As T
            Return New T()
        End Function
    End Class

    Private Sub New()
    End Sub
End Class

Public MustInherit Class __do(Of T)
    Protected MustOverride Function at() As T

    Public Shared Operator +(ByVal this As __do(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.at()
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T)) As Func(Of T)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Widening Operator CType(ByVal this As __do(Of T)) As T
        Return +this
    End Operator
End Class

Public MustInherit Class __do(Of T, RT)
    Public MustOverride Function at(ByRef k As T) As RT

    Default Public ReadOnly Property _at(ByVal k As T) As RT
        Get
            Return at(k)
        End Get
    End Property

    Public Shared Operator +(ByVal this As __do(Of T, RT), ByVal k As T) As RT
        If this Is Nothing Then
            Return Nothing
        End If
        Return this(k)
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T, RT)) As _do(Of T, RT)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator
End Class

Public MustInherit Class __do(Of T1, T2, RT)
    Public MustOverride Function at(ByRef i As T1, ByRef j As T2) As RT

    Default Public ReadOnly Property _at(ByVal i As T1, ByVal j As T2) As RT
        Get
            Return at(i, j)
        End Get
    End Property

    Public Shared Operator +(ByVal this As __do(Of T1, T2, RT)) As _do(Of T1, T2, RT)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T1, T2, RT)) As _do(Of T1, T2, RT)
        Return +this
    End Operator
End Class

Public MustInherit Class __do(Of T1, T2, T3, RT)
    Public MustOverride Function at(ByRef i As T1, ByRef j As T2, ByRef k As T3) As RT

    Default Public ReadOnly Property _at(ByVal i As T1, ByVal j As T2, ByVal k As T3) As RT
        Get
            Return at(i, j, k)
        End Get
    End Property

    Public Shared Operator +(ByVal this As __do(Of T1, T2, T3, RT)) As _do(Of T1, T2, T3, RT)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T1, T2, T3, RT)) As _do(Of T1, T2, T3, RT)
        Return +this
    End Operator
End Class

Public MustInherit Class __do(Of T1, T2, T3, T4, RT)
    Public MustOverride Function at(ByRef i As T1, ByRef j As T2, ByRef k As T3, ByRef l As T4) As RT

    Default Public ReadOnly Property _at(ByVal i As T1, ByVal j As T2, ByVal k As T3, ByVal l As T4) As RT
        Get
            Return at(i, j, k, l)
        End Get
    End Property

    Public Shared Operator +(ByVal this As __do(Of T1, T2, T3, T4, RT)) As _do(Of T1, T2, T3, T4, RT)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T1, T2, T3, T4, RT)) As _do(Of T1, T2, T3, T4, RT)
        Return +this
    End Operator
End Class

Public MustInherit Class __void
    Public MustOverride Sub at()

    Public Sub invoke()
        at()
    End Sub

    Public Shared Operator +(ByVal this As __void) As Action
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator
End Class

Public MustInherit Class __void(Of T)
    Public MustOverride Sub at(ByRef i As T)

    Default Public ReadOnly Property _at(ByVal i As T) As Byte
        Get
            at(i)
            Return 0
        End Get
    End Property

    Public Sub invoke(ByVal i As T)
        at(i)
    End Sub

    Public Shared Operator +(ByVal this As __void(Of T), ByVal i As T) As Byte
        If this Is Nothing Then
            Return 0
        End If
        Return this(i)
    End Operator

    Public Shared Operator -(ByVal this As __void(Of T)) As void(Of T)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator
End Class

Public MustInherit Class __void(Of T1, T2)
    Public MustOverride Sub at(ByRef i As T1, ByRef j As T2)

    Default Public ReadOnly Property _at(ByVal i As T1, ByVal j As T2) As Byte
        Get
            at(i, j)
            Return 0
        End Get
    End Property

    Public Sub invoke(ByVal i As T1, ByVal j As T2)
        at(i, j)
    End Sub

    Public Shared Operator +(ByVal this As __void(Of T1, T2)) As void(Of T1, T2)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Operator -(ByVal this As __void(Of T1, T2)) As void(Of T1, T2)
        Return +this
    End Operator
End Class

Public MustInherit Class __void(Of T1, T2, T3)
    Public MustOverride Sub at(ByRef i As T1, ByRef j As T2, ByRef k As T3)

    Default Public ReadOnly Property _at(ByVal i As T1, ByVal j As T2, ByVal k As T3) As Byte
        Get
            at(i, j, k)
            Return 0
        End Get
    End Property

    Public Sub invoke(ByVal i As T1, ByVal j As T2, ByVal k As T3)
        at(i, j, k)
    End Sub

    Public Shared Operator +(ByVal this As __void(Of T1, T2, T3)) As void(Of T1, T2, T3)
        If this Is Nothing Then
            Return Nothing
        End If
        Return AddressOf this.at
    End Operator

    Public Shared Operator -(ByVal this As __void(Of T1, T2, T3)) As void(Of T1, T2, T3)
        Return +this
    End Operator
End Class