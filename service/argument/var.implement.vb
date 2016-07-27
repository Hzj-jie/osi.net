
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports cc = osi.root.connector

Partial Public Class var
    Implements ICloneable, IComparable, IComparable(Of var)

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim m As map(Of String, vector(Of String)) = Nothing
        copy(m, Me.raw)
        assert(m.insert(Me.binded))
        Return New var(Me.c, m, Nothing, copy(Me.others))
    End Function

    Public Shared Function compare(ByVal this As var, ByVal that As var) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            c = cc.compare(this.raw, that.raw)
            If c = 0 Then
                c = cc.compare(this.binded, that.binded)
                If c = 0 Then
                    c = cc.compare(this.others, that.others)
                    Return c
                Else
                    Return c
                End If
            Else
                Return c
            End If
        Else
            Return c
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of var)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As var) As Int32 Implements IComparable(Of var).CompareTo
        Return compare(Me, other)
    End Function
End Class
