
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt

Public NotInheritable Class unordered_set_perf
    Public Class small_range_uint
        Inherits __do(Of UInt32)

        Protected Overrides Function at() As UInt32
            Return rnd_uint(min_uint16, max_uint16)
        End Function
    End Class

    Public Class large_range_uint
        Inherits __do(Of UInt32)

        Protected Overrides Function at() As UInt32
            Return rnd_uint()
        End Function
    End Class

    Public Class small_range_string
        Inherits __do(Of String)

        Protected Overrides Function at() As String
            Return rnd_en_chars(3)
        End Function
    End Class

    Public Class large_range_string
        Inherits __do(Of String)

        Protected Overrides Function at() As String
            Return rnd_en_chars(6)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class

Public Class unordered_set_perf(Of T, _RND As __do(Of T))
    Inherits performance_comparison_case_wrapper

    Private Shared ReadOnly rnd As Func(Of T)

    Shared Sub New()
        rnd = -alloc(Of _RND)()
    End Sub

    Public Sub New(ByVal percentages() As Double)
        MyBase.New(R(New unordered_set_case(percentages)), R(New set_case(percentages)))
    End Sub

    Protected Shared Function low_item_count_percentages() As Double()
        ' ~239 items
        Return {0.2445, 0.2445, 0.25, 0.26, 0.001}
    End Function

    Protected Shared Function high_item_count_percentages() As Double()
        ' ~25000 items
        Return {0.25, 0.25, 0.25, 0.24999, 0.00001}
    End Function

    Private Shared Function R(ByVal c As [case]) As [case]
        Return repeat(c, 1024 * 256)
    End Function

    Private MustInherit Class perf_case
        Inherits random_run_case

        Public Sub New(ByVal percentages() As Double)
            insert_call(percentages(0), AddressOf insert)
            insert_call(percentages(1), AddressOf emplace)
            insert_call(percentages(2), AddressOf find)
            insert_call(percentages(3), AddressOf [erase])
            insert_call(percentages(4), AddressOf clear)
        End Sub

        Protected MustOverride Sub insert(ByVal v As T)
        Protected MustOverride Sub emplace(ByVal v As T)
        Protected MustOverride Sub [erase](ByVal v As T)
        Protected MustOverride Sub clear()
        Protected MustOverride Sub find(ByVal v As T)

        Private Sub insert()
            insert(rnd())
        End Sub

        Private Sub emplace()
            emplace(rnd())
        End Sub

        Private Sub [erase]()
            [erase](rnd())
        End Sub

        Private Sub find()
            find(rnd())
        End Sub
    End Class

    Private Class unordered_set_case
        Inherits perf_case

        Private ReadOnly s As unordered_set(Of T)

        Public Sub New(ByVal percentages() As Double)
            MyBase.New(percentages)
            s = New unordered_set(Of T)()
        End Sub

        Protected Overrides Sub clear()
            s.clear()
        End Sub

        Protected Overrides Sub emplace(ByVal v As T)
            s.emplace(v)
        End Sub

        Protected Overrides Sub [erase](ByVal v As T)
            s.erase(v)
        End Sub

        Protected Overrides Sub find(ByVal v As T)
            s.find(v)
        End Sub

        Protected Overrides Sub insert(ByVal v As T)
            s.insert(v)
        End Sub
    End Class

    Private Class set_case
        Inherits perf_case

        Private ReadOnly s As [set](Of T)

        Public Sub New(ByVal percentages() As Double)
            MyBase.New(percentages)
            s = New [set](Of T)()
        End Sub

        Protected Overrides Sub clear()
            s.clear()
        End Sub

        Protected Overrides Sub emplace(ByVal v As T)
            s.emplace(v)
        End Sub

        Protected Overrides Sub [erase](ByVal v As T)
            s.erase(v)
        End Sub

        Protected Overrides Sub find(ByVal v As T)
            s.find(v)
        End Sub

        Protected Overrides Sub insert(ByVal v As T)
            s.insert(v)
        End Sub
    End Class
End Class
