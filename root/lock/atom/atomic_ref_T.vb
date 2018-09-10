
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Public NotInheritable Class atomic_ref(Of T)
    Private ReadOnly r As atomic_ref

    Public Sub New(ByVal r As atomic_ref)
        assert(Not r Is Nothing)
        Me.r = r
    End Sub

    Public Sub New()
        Me.New(New atomic_ref())
    End Sub

    Public Sub New(ByVal i As T)
        Me.New(New atomic_ref(i))
    End Sub

    Public Function [get]() As T
        Return direct_cast(Of T)(r.get())
    End Function

    Public Sub modify(ByVal d As void(Of T))
        r.modify(Sub(ByRef i As Object)
                     Dim v As T = Nothing
                     v = direct_cast(Of T)(i)
                     d(v)
                     i = v
                 End Sub)
    End Sub

    Public Function exchange(ByVal value As T) As T
        Dim v As Object = Nothing
        v = r.exchange(value)
        Return direct_cast(Of T)(v)
    End Function

    Public Function compare_exchange(ByVal value As T, ByVal comparand As T) As T
        Return direct_cast(Of T)(r.compare_exchange(value, comparand))
    End Function

    Public Shared Operator +(ByVal this As atomic_ref(Of T)) As T
        Return If(this Is Nothing, Nothing, this.get())
    End Operator
End Class
