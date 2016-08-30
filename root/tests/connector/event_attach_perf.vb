
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt

' Unfortunately, [Delegate].GetInvocationList in .Net (the internal implementation of event) is an O(N) algorithm.
Public Class event_attach_perf
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(array_concat(c(Of _1)(), c(Of _10)(), c(Of _1000)()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{-1, 0.2, -1, -1, -1, -1},
                {20, -1, -1, -1, -1, -1},
                {-1, -1, -1, 1, -1, -1},
                {-1, -1, 4, -1, -1, -1},
                {-1, -1, -1, -1, -1, 66.7},
                {-1, -1, -1, -1, 0.06, -1}}
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
            size = +alloc(Of _SIZE)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim a(size - uint32_1)
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
            size = +alloc(Of _SIZE)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim a(size - uint32_1)
                For i As UInt32 = uint32_0 To size - uint32_1
                    Dim x As UInt32 = uint32_0
                    x = i
                    a(i) = Sub()
                               Dim j As UInt32 = uint32_0
                               j += x
                           End Sub
                    AddHandler e, a(i)
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
                RemoveHandler e, a(i)
            Next
            Erase a
            Return MyBase.finish()
        End Function
    End Class
End Class
