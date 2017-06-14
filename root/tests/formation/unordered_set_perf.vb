
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt

Public Class unordered_set_perf(Of T, _RND As __do(Of T))
    Inherits performance_comparison_case_wrapper

    Private Shared ReadOnly rnd As Func(Of T)

    Shared Sub New()
        rnd = -alloc(Of _RND)()
    End Sub

    Public Sub New()
        MyBase.New(R(New unordered_set_case()), R(New set_case()))
    End Sub

    Private Shared Function R(ByVal c As [case]) As [case]
        Return repeat(c, 1024 * 1024 * 16)
    End Function

    Private MustInherit Class perf_case
        Inherits random_run_case

        Public Sub New()
            insert_call(0.22, AddressOf insert)
            insert_call(0.22, AddressOf emplace)
            insert_call(0.25, AddressOf find)
            insert_call(0.26, AddressOf [erase])
            insert_call(0.05, AddressOf clear)
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

        Public Sub New()
            MyBase.New()
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

        Public Sub New()
            MyBase.New()
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

Public Class unordered_set_uint_perf
    Inherits unordered_set_perf(Of UInt32, rnd)

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.9},
                {2.5, 0}}
    End Function

    Public Class rnd
        Inherits __do(Of UInt32)

        Protected Overrides Function at() As UInt32
            Return rnd_uint(min_uint16, max_uint16)
        End Function
    End Class
End Class

Public Class unordered_set_string_perf
    Inherits unordered_set_perf(Of String, rnd)

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 0.6},
                {4, 0}}
    End Function

    Public Class rnd
        Inherits __do(Of String)

        Protected Overrides Function at() As String
            Return rnd_en_chars(3)
        End Function
    End Class
End Class
