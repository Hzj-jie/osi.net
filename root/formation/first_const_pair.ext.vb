
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public NotInheritable Class first_const_pair
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function first_compare(Of T1, T2)(ByVal this As first_const_pair(Of T1, T2),
                                                    ByVal that As first_const_pair(Of T1, T2)) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return compare(this.first, that.first)
    End Function

    Public NotInheritable Class first_hasher(Of T1, T2, _HASHER As _to_uint32(Of T1))
        Inherits _to_uint32(Of first_const_pair(Of T1, T2))

        Private Shared ReadOnly hasher As _HASHER

        Shared Sub New()
            hasher = alloc(Of _HASHER)()
        End Sub

        Public Overrides Function at(ByRef k As first_const_pair(Of T1, T2)) As UInt32
            assert(Not k Is Nothing)
            Return hasher(k.first)
        End Function

        Public Overrides Function reverse(ByVal i As UInt32) As first_const_pair(Of T1, T2)
            assert(False)
            Return Nothing
        End Function
    End Class

    Public NotInheritable Class first_equaler(Of T1, T2, _EQUALER As _equaler(Of T1))
        Inherits _equaler(Of first_const_pair(Of T1, T2))

        Private Shared ReadOnly equaler As _EQUALER

        Shared Sub New()
            equaler = alloc(Of _EQUALER)()
        End Sub

        Public Overrides Function at(ByRef i As first_const_pair(Of T1, T2),
                                     ByRef j As first_const_pair(Of T1, T2)) As Boolean
            Dim c As Int32 = 0
            c = object_compare(i, j)
            If c <> object_compare_undetermined Then
                Return c = 0
            End If
            assert(Not i Is Nothing)
            assert(Not j Is Nothing)
            Return equaler.at(i.first, j.first)
        End Function
    End Class
End Class
