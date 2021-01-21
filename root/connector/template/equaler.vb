
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.template

Public NotInheritable Class default_equaler(Of T1, T2)
    Inherits _equaler(Of T1, T2)

    Public Overrides Function at(ByRef i As T1, ByRef j As T2) As Boolean
        Return equal(i, j)
    End Function
End Class

Public NotInheritable Class default_equaler(Of T)
    Inherits _equaler(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Boolean
        Return equal(i, j)
    End Function
End Class

Public NotInheritable Class default_non_null_equaler(Of T)
    Inherits _equaler(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Boolean
        Return non_null_equal(i, j)
    End Function
End Class

Public NotInheritable Class equality_comparer_equaler(Of T)
    Inherits _equaler(Of T)

    Public Overrides Function at(ByRef i As T, ByRef j As T) As Boolean
        Return EqualityComparer(Of T).Default.Equals(i, j)
    End Function
End Class

Public NotInheritable Class equaler_comparer(Of T, EQUALER As _equaler(Of T))
    Implements IEqualityComparer(Of T)

    Public Shared ReadOnly instance As equaler_comparer(Of T, EQUALER) = New equaler_comparer(Of T, EQUALER)()
    Private Shared ReadOnly e As EQUALER = alloc(Of EQUALER)()

    Private Sub New()
    End Sub

    Public Overloads Function Equals(ByVal x As T, ByVal y As T) As Boolean Implements IEqualityComparer(Of T).Equals
        Return e(x, y)
    End Function

    Public Overloads Function GetHashCode(ByVal obj As T) As Int32 Implements IEqualityComparer(Of T).GetHashCode
        If type_info(Of EQUALER, type_info_operators.equal, equality_comparer_equaler(Of T)).v Then
            Return EqualityComparer(Of T).Default.GetHashCode(obj)
        End If

        Return If(obj Is Nothing, 0, obj.GetHashCode())
    End Function
End Class