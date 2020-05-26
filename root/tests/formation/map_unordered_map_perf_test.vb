
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

' Compare perf of map, hashmap, unordered_map, unordered_map2 and Dictionary
Public NotInheritable Class map_unordered_map_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const manual_case_rounds As Int64 = 20000000
    Private Shared ReadOnly strs As samples(Of String)

    Shared Sub New()
        strs = samples.of(Function() As String
                              Return rnd_chars(rnd_int(20, 50))
                          End Function,
                          100000)
    End Sub

    Public Sub New()
        MyBase.New(r(New map_case()),
                   r(New dictionary_case()),
                   r(New unordered_map_case()))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({19713, 1155, 5559}, i, j)
    End Function

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 1000000)
    End Function

    Private MustInherit Class run_case
        Inherits random_run_case

        Public Sub New()
            MyBase.New()
            ' ~30000 items
            insert_call(0.5, AddressOf insert)
            insert_call(0.3, AddressOf find)
            insert_call(0.2, AddressOf [erase])
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            clear()
            strs.reset()
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            clear()
            Return MyBase.finish()
        End Function

        Protected MustOverride Sub insert()
        Protected MustOverride Sub find()
        Protected MustOverride Sub [erase]()
        Protected MustOverride Sub clear()
    End Class

    Public NotInheritable Class map_manual_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(repeat(New map_case(), manual_case_rounds))
        End Sub
    End Class

    Private NotInheritable Class map_case
        Inherits run_case

        Private ReadOnly m As map(Of String, String)

        Public Sub New()
            MyBase.New()
            m = New map(Of String, String)()
        End Sub

        Protected Overrides Sub insert()
            m.emplace(strs.next(), strs.next())
        End Sub

        Protected Overrides Sub find()
            m.find(strs.next())
        End Sub

        Protected Overrides Sub [erase]()
            m.erase(strs.next())
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub
    End Class

    Public NotInheritable Class dictionary_manual_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(repeat(New dictionary_case(), manual_case_rounds))
        End Sub
    End Class

    Private NotInheritable Class dictionary_case
        Inherits run_case

        Private m As Dictionary(Of String, String)

        Protected Overrides Sub insert()
            m(strs.next()) = strs.next()
        End Sub

        Protected Overrides Sub find()
            m.ContainsKey(strs.next())
        End Sub

        Protected Overrides Sub [erase]()
            m.Remove(strs.next())
        End Sub

        Protected Overrides Sub clear()
            ' Dictionary.Clear does not really release the memory.
            m = New Dictionary(Of String, String)()
        End Sub
    End Class

    Public NotInheritable Class unordered_map_manual_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(repeat(New unordered_map_case(), manual_case_rounds))
        End Sub
    End Class

    Private NotInheritable Class unordered_map_case
        Inherits run_case

        Private ReadOnly m As unordered_map(Of String, String) = Nothing

        Public Sub New()
            MyBase.New()
            m = New unordered_map(Of String, String)()
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub

        Protected Overrides Sub [erase]()
            m.erase(strs.next())
        End Sub

        Protected Overrides Sub find()
            m.find(strs.next())
        End Sub

        Protected Overrides Sub insert()
            m.emplace(strs.next(), strs.next())
        End Sub
    End Class
End Class
