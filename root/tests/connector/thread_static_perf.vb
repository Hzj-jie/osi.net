
Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public MustInherit Class thread_static_perf
    Inherits multithreading_case_wrapper

    Private Const thread_count As Int32 = 32
    Private Const repeat_count As Int32 = 1024 * 1024

    Protected Sub New(ByVal c As thread_static_perf_case)
        MyBase.New(repeat(c, repeat_count), thread_count)
    End Sub

    Protected MustInherit Class thread_static_perf_case
        Inherits [case]

        Protected MustOverride Function [set]() As Boolean
        Protected MustOverride Function [get]() As Boolean

        Public NotOverridable Overrides Function run() As Boolean
            Return If(rnd_bool(), [set](), [get]())
        End Function
    End Class

    Protected MustInherit Class thread_static_perf_reference_type_case
        Inherits thread_static_perf_case

        Protected Class test_class
            Public a As Int32
            Public b As Int32
            Public c As Object
            Public d As String
            Public e As Exception
        End Class

        Protected Shared Function next_sample() As test_class
            Return If(rnd_bool(), New test_class(), Nothing)
        End Function
    End Class
End Class

Public Class connector_thread_static_valuetype_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_case

        Private ReadOnly ts As thread_static(Of Int32)

        Public Sub New()
            ts = New thread_static(Of Int32)()
        End Sub

        Protected Overrides Function [get]() As Boolean
            Dim i As Int32 = 0
            Return ts.get(i)
        End Function

        Protected Overrides Function [set]() As Boolean
            Return ts.set(rnd_int(min_int32, max_int32))
        End Function
    End Class
End Class

Public Class internal_thread_static_valuetype_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_case

        <ThreadStatic()> Private Shared value As Int32

        Protected Overrides Function [get]() As Boolean
            Dim i As Int32 = 0
            i = value
            Return True
        End Function

        Protected Overrides Function [set]() As Boolean
            value = rnd_int(min_int32, max_int32)
            Return True
        End Function
    End Class
End Class

Public Class utils_thread_static2_valuetype_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_case

        Private ReadOnly ts As thread_static2(Of Int32)

        Public Sub New()
            ts = New thread_static2(Of Int32)()
        End Sub

        Protected Overrides Function [get]() As Boolean
            Dim i As Int32 = 0
            i = ts.get()
            Return True
        End Function

        Protected Overrides Function [set]() As Boolean
            ts.set(rnd_int(min_int32, max_int32))
            Return True
        End Function
    End Class
End Class

Public Class connector_thread_static_reference_type_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_reference_type_case

        Private ReadOnly ts As thread_static(Of test_class)

        Public Sub New()
            ts = New thread_static(Of test_class)()
        End Sub

        Protected Overrides Function [get]() As Boolean
            Dim i As test_class = Nothing
            Return ts.get(i)
        End Function

        Protected Overrides Function [set]() As Boolean
            Return ts.set(next_sample())
        End Function
    End Class
End Class

Public Class internal_thread_static_reference_type_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_reference_type_case

        <ThreadStatic()> Private Shared value As test_class

        Protected Overrides Function [get]() As Boolean
            Dim i As test_class = Nothing
            i = value
            Return True
        End Function

        Protected Overrides Function [set]() As Boolean
            value = next_sample()
            Return True
        End Function
    End Class
End Class

Public Class utils_thread_static2_reference_type_perf
    Inherits thread_static_perf

    Public Sub New()
        MyBase.New(New [case]())
    End Sub

    Protected Class [case]
        Inherits thread_static_perf_reference_type_case

        Private ReadOnly ts As thread_static2(Of test_class)

        Public Sub New()
            ts = New thread_static2(Of test_class)()
        End Sub

        Protected Overrides Function [get]() As Boolean
            Dim i As test_class = Nothing
            i = ts.get()
            Return True
        End Function

        Protected Overrides Function [set]() As Boolean
            ts.set(next_sample())
            Return True
        End Function
    End Class
End Class
