
Imports System.DateTime
Imports osi.root.connector

Public Class reverse_date
    Inherits reverse(Of Date)
    Implements IComparable(Of reverse_date)

    Public Sub New()
        Me.New(Now())
    End Sub

    Public Sub New(ByVal i As Date)
        MyBase.New(i)
    End Sub

    Public Sub New(ByVal i As reverse(Of Date))
        MyBase.New(i)
    End Sub

    Public Overloads Function CompareTo(ByVal other As reverse_date) As Int32 _
                                       Implements IComparable(Of reverse_date).CompareTo
        Return MyBase.CompareTo(other)
    End Function

    Public Shared Operator +(ByVal left As reverse_date, ByVal right As reverse_date) As reverse_date
        Return New reverse_date(New Date((+left).Ticks() + (+right).Ticks()))
    End Operator
End Class
