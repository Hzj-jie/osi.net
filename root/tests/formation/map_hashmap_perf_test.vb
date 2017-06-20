﻿
Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

' Compare perf of map, hashmap and Dictionary
Public Class map_hashmap_perf_test
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(R(New map_case()), R(New hashmap_case()), R(New dictionary_case()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        If isdebugmode() Then
            Return {{0, 4, 10},
                    {0.5, 0, -1},
                    {0.2, -1, 0}}
        Else
            Return {{0, 3, 4},
                    {0.7, 0, -1},
                    {0.5, -1, 0}}
        End If
    End Function

    Private Shared Function R(ByVal c As [case]) As [case]
        Return repeat(c, 100000)
    End Function

    Private Shared Function rnd_str() As String
        Return rnd_chars(rnd_int(20, 50))
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
            If MyBase.prepare() Then
                clear()
                Return True
            Else
                Return False
            End If
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

    Public Class map_manual_perf
        Inherits commandline_specific_case_wrapper

        Public Sub New()
            MyBase.New(repeat(New map_case(), 1000000000))
        End Sub
    End Class

    Private Class map_case
        Inherits run_case

        Private ReadOnly m As map(Of String, String)

        Public Sub New()
            MyBase.New()
            m = New map(Of String, String)()
        End Sub

        Protected Overrides Sub insert()
            m.emplace(rnd_str(), rnd_str())
        End Sub

        Protected Overrides Sub find()
            m.find(rnd_str())
        End Sub

        Protected Overrides Sub [erase]()
            m.erase(rnd_str())
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub
    End Class

    Public Class hashmap_manual_perf
        Inherits commandline_specific_case_wrapper

        Public Sub New()
            MyBase.New(repeat(New hashmap_case(), 1000000000))
        End Sub
    End Class

    Private Class hashmap_case
        Inherits run_case

        Private ReadOnly m As hashmap(Of String, String)

        Public Sub New()
            MyBase.New()
            m = New hashmap(Of String, String)()
        End Sub

        Protected Overrides Sub insert()
            m.emplace(rnd_str(), rnd_str())
        End Sub

        Protected Overrides Sub find()
            m.find(rnd_str())
        End Sub

        Protected Overrides Sub [erase]()
            m.erase(rnd_str())
        End Sub

        Protected Overrides Sub clear()
            m.clear()
        End Sub
    End Class

    Private Class dictionary_case
        Inherits run_case

        Private m As Dictionary(Of String, String)

        Protected Overrides Sub insert()
            m.Add(rnd_str(), rnd_str())
        End Sub

        Protected Overrides Sub find()
            m.ContainsKey(rnd_str())
        End Sub

        Protected Overrides Sub [erase]()
            m.Remove(rnd_str())
        End Sub

        Protected Overrides Sub clear()
            ' Dictionary.Clear does not really release the memory.
            m = New Dictionary(Of String, String)()
        End Sub
    End Class
End Class
