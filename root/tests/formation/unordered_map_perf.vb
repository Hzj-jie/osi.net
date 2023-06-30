
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class unordered_map_perf(Of KEY_T, VALUE_T)
    Inherits performance_comparison_case_wrapper

    Public Sub New(ByVal percentages() As Double,
                   ByVal rnd_key As Func(Of KEY_T),
                   ByVal rnd_value As Func(Of VALUE_T))
        MyBase.New(r(New unordered_map_case(percentages, rnd_key, rnd_value)),
                   r(New map_case(percentages, rnd_key, rnd_value)))
    End Sub

    Protected Shared Function low_item_count_percentages() As Double()
        ' ~239 items
        Return {0.2445, 0.2445, 0.25, 0.26, 0.001}
    End Function

    Protected Shared Function high_item_count_percentages() As Double()
        ' ~25000 items
        Return {0.25, 0.25, 0.25, 0.24999, 0.00001}
    End Function

    Protected Shared Function small_range_uint() As Func(Of UInt32)
        Return Function() As UInt32
                   Return rnd_uint(min_uint16, max_uint16)
               End Function
    End Function

    Protected Shared Function large_range_uint() As Func(Of UInt32)
        Return Function() As UInt32
                   Return rnd_uint()
               End Function
    End Function

    Protected Shared Function small_range_string() As Func(Of String)
        Return Function() As String
                   Return rnd_en_chars(3)
               End Function
    End Function

    Protected Shared Function large_range_string() As Func(Of String)
        Return Function() As String
                   Return rnd_en_chars(6)
               End Function
    End Function

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 1024 * 256)
    End Function

    Private MustInherit Class perf_case
        Inherits random_run_case

        Private ReadOnly rnd_key As Func(Of KEY_T)
        Private ReadOnly rnd_value As Func(Of VALUE_T)

        Public Sub New(ByVal percentages() As Double,
                       ByVal rnd_key As Func(Of KEY_T),
                       ByVal rnd_value As Func(Of VALUE_T))
            assert(Not rnd_key Is Nothing)
            assert(Not rnd_value Is Nothing)
            Me.rnd_key = rnd_key
            Me.rnd_value = rnd_value
            insert_call(percentages(0), AddressOf insert)
            insert_call(percentages(1), AddressOf emplace)
            insert_call(percentages(2), AddressOf find)
            insert_call(percentages(3), AddressOf [erase])
            insert_call(percentages(4), AddressOf clear)
        End Sub

        Public Overrides Function finish() As Boolean
            clear()
            Return MyBase.finish()
        End Function

        Protected MustOverride Sub insert(ByVal k As KEY_T, ByVal v As VALUE_T)
        Protected MustOverride Sub emplace(ByVal k As KEY_T, ByVal v As VALUE_T)
        Protected MustOverride Sub [erase](ByVal k As KEY_T)
        Protected MustOverride Sub clear()
        Protected MustOverride Sub find(ByVal k As KEY_T)

        Private Sub insert()
            insert(rnd_key(), rnd_value())
        End Sub

        Private Sub emplace()
            emplace(rnd_key(), rnd_value())
        End Sub

        Private Sub [erase]()
            [erase](rnd_key())
        End Sub

        Private Sub find()
            find(rnd_key())
        End Sub
    End Class

    Private NotInheritable Class unordered_map_case
        Inherits perf_case

        Private ReadOnly m As unordered_map(Of KEY_T, VALUE_T)

        Public Sub New(ByVal percentages() As Double,
                       ByVal rnd_key As Func(Of KEY_T),
                       ByVal rnd_value As Func(Of VALUE_T))
            MyBase.New(percentages, rnd_key, rnd_value)
            m = New unordered_map(Of KEY_T, VALUE_T)()
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub

        Protected Overrides Sub emplace(ByVal k As KEY_T, ByVal v As VALUE_T)
            m.emplace(k, v)
        End Sub

        Protected Overrides Sub [erase](ByVal k As KEY_T)
            m.erase(k)
        End Sub

        Protected Overrides Sub find(ByVal k As KEY_T)
            m.find(k)
        End Sub

        Protected Overrides Sub insert(ByVal k As KEY_T, ByVal v As VALUE_T)
            m.insert(k, v)
        End Sub
    End Class

    Private NotInheritable Class map_case
        Inherits perf_case

        Private ReadOnly m As map(Of KEY_T, VALUE_T)

        Public Sub New(ByVal percentages() As Double,
                       ByVal rnd_key As Func(Of KEY_T),
                       ByVal rnd_value As Func(Of VALUE_T))
            MyBase.New(percentages, rnd_key, rnd_value)
            m = New map(Of KEY_T, VALUE_T)()
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub

        Protected Overrides Sub emplace(ByVal k As KEY_T, ByVal v As VALUE_T)
            m.emplace(k, v)
        End Sub

        Protected Overrides Sub [erase](ByVal k As KEY_T)
            m.erase(k)
        End Sub

        Protected Overrides Sub find(ByVal k As KEY_T)
            m.find(k)
        End Sub

        Protected Overrides Sub insert(ByVal k As KEY_T, ByVal v As VALUE_T)
            m.insert(k, v)
        End Sub
    End Class
End Class
