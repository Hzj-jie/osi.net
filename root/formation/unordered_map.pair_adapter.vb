
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class unordered_map(Of KEY_T,
                                      VALUE_T,
                                      _HASHER As _to_uint32(Of KEY_T),
                                      _EQUALER As _equaler(Of KEY_T))
    Public Class first_const_pair_hasher
        Inherits _to_uint32(Of first_const_pair(Of KEY_T, VALUE_T))

        Private Shared ReadOnly hasher As _HASHER

        Shared Sub New()
            hasher = alloc(Of _HASHER)()
        End Sub

        Public Overrides Function at(ByRef k As first_const_pair(Of KEY_T, VALUE_T)) As UInt32
            assert(Not k Is Nothing)
            Return hasher(k.first)
        End Function

        Public Overrides Function reverse(ByVal i As UInt32) As first_const_pair(Of KEY_T, VALUE_T)
            assert(False)
            Return Nothing
        End Function
    End Class

    Public Class first_const_pair_equaler
        Inherits _equaler(Of first_const_pair(Of KEY_T, VALUE_T))

        Private Shared ReadOnly equaler As _EQUALER

        Shared Sub New()
            equaler = alloc(Of _EQUALER)()
        End Sub

        Public Overrides Function at(ByRef i As first_const_pair(Of KEY_T, VALUE_T),
                                     ByRef j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
            Dim c As Int32 = 0
            c = object_compare(i, j)
            If c = object_compare_undetermined Then
                assert(Not i Is Nothing)
                assert(Not j Is Nothing)
                Return equaler(i.first, j.first)
            Else
                Return c = 0
            End If
        End Function
    End Class
End Class
