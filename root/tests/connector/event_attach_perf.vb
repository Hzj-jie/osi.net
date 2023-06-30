
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.template
Imports osi.root.utt

' Unfortunately, [Delegate].GetInvocationList in .Net (the internal implementation of event) is an O(N) algorithm.
Public Class event_attach_perf
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(array_concat(c(Of _1)(), c(Of _10)(), c(Of _1000)()))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If os.windows_major <= os.windows_major_t._5 Then
            If isdebugbuild() Then
                Return loosen_bound({141, 1861, 711, 1848, 59854, 1837}, i, j)
            Else
                Return loosen_bound({170, 681, 1039, 681, 90961, 699}, i, j)
            End If
        Else
            If isdebugbuild() Then
                Return loosen_bound({183, 1466, 548, 1487, 36821, 1445}, i, j)
            Else
                Return loosen_bound({188, 466, 935, 511, 82558, 466}, i, j)
            End If
        End If
    End Function

    Private Shared Function c(Of _SIZE As _int64)() As [case]()
        Return {r(New event_case(Of _SIZE)()), r(New array_case(Of _SIZE)())}
    End Function

    Private Shared Function r(ByVal i As [case]) As [case]
        Return repeat(i, 1024 * 1024 * 2)
    End Function

    Private Class array_case(Of _SIZE As _int64)
        Inherits [case]

        Private Shared ReadOnly size As UInt32
        Private a() As String

        Shared Sub New()
            size = CUInt(+alloc(Of _SIZE)())
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim a(CInt(size - uint32_1))
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            ' array_size is too fast.
            For i As Int32 = 0 To 99
                Dim x As UInt32 = uint32_0
                x = array_size(a)
            Next
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            Erase a
            Return MyBase.finish()
        End Function
    End Class

    Private Class event_case(Of _SIZE As _int64)
        Inherits [case]

        Private Shared ReadOnly size As UInt32
        Private Event e()
        Private a() As eEventHandler

        Shared Sub New()
            size = CUInt(+alloc(Of _SIZE)())
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim a(CInt(size - uint32_1))
                For i As UInt32 = uint32_0 To size - uint32_1
                    Dim x As UInt32 = uint32_0
                    x = i
                    a(CInt(i)) = Sub()
                                     Dim j As UInt32 = uint32_0
                                     j += x
                                 End Sub
                    AddHandler e, a(CInt(i))
                Next
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim x As UInt32 = 0
            x = attached_delegate_count(eEvent)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            For i As UInt32 = uint32_0 To size - uint32_1
                RemoveHandler e, a(CInt(i))
            Next
            Erase a
            Return MyBase.finish()
        End Function
    End Class
End Class
