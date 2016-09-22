
Imports osi.root.delegates

Public MustInherit Class __do(Of T)
    Protected MustOverride Function at() As T

    Public Shared Operator +(ByVal this As __do(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.at()
        End If
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T)) As Func(Of T)
        If this Is Nothing Then
            Return Nothing
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return this(k)
        End If
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T, RT)) As _do(Of T, RT)
        If this Is Nothing Then
            Return Nothing
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return AddressOf this.at
        End If
    End Operator

    Public Shared Operator -(ByVal this As __do(Of T1, T2, T3, RT)) As _do(Of T1, T2, T3, RT)
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
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return this(i)
        End If
    End Operator

    Public Shared Operator -(ByVal this As __void(Of T)) As void(Of T)
        If this Is Nothing Then
            Return Nothing
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return AddressOf this.at
        End If
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
        Else
            Return AddressOf this.at
        End If
    End Operator

    Public Shared Operator -(ByVal this As __void(Of T1, T2, T3)) As void(Of T1, T2, T3)
        Return +this
    End Operator
End Class