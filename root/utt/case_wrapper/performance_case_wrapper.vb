﻿
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation

Public Class performance_case_wrapper
    Inherits case_wrapper

    Public Const undetermined_max_loops As UInt64 = max_uint64
    Private ReadOnly max_l As UInt64 = 0
    Private ReadOnly min_l As UInt64 = 0
    Private ReadOnly t As UInt64 = 0
    Private min As UInt64
    Private max As UInt64
    Private ave As UInt64

    Public Sub New(ByVal c As [case],
                   Optional ByVal max_loops As UInt64 = undetermined_max_loops,
                   Optional ByVal min_loops As UInt64 = 0,
                   Optional ByVal times As UInt64 = 1)
        MyBase.New(c)
        Me.max_l = max_loops
        Me.min_l = min_loops
        Me.t = times
    End Sub

    Public Function min_used_loops() As UInt64
        Return min
    End Function

    Public Function max_used_loops() As UInt64
        Return max
    End Function

    Public Function average_used_loops() As UInt64
        Return ave
    End Function

    Protected Overridable Function max_loops() As UInt64
        Return max_l
    End Function

    Protected Overridable Function min_loops() As UInt64
        Return min_l
    End Function

    Protected Overridable Function times() As UInt64
        Return t
    End Function

    Private Overloads Function run(ByRef min As UInt64, ByRef max As UInt64, ByRef ave As UInt64) As Boolean
        assert(times() > 0)
        min = max_uint64
        max = min_uint64
        ave = 0
        For i As Int64 = 0 To times() - 1
            Dim used_loops As pointer(Of Int64) = Nothing
            used_loops = New pointer(Of Int64)()
            Using New boost()
                Using New processor_loops_timing_counter(used_loops)
                    If Not MyBase.run() Then
                        Return False
                    End If
                End Using
            End Using

            assert((+used_loops) >= 0)
            ave += (+used_loops)
            If (+used_loops) < min Then
                min = (+used_loops)
            End If
            If (+used_loops) > max Then
                max = (+used_loops)
            End If
        Next

        ave \= times()
        Return True
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(max_loops() > 0)
        If min_loops() = 0 AndAlso max_loops() = undetermined_max_loops Then
            utt_raise_error("performance case ", name, " did not set min loops and max loops, consider to add it.")
        End If
        If run(min, max, ave) Then
            raise_error("finished performance case ", name, " with ", times(), " times, ",
                        "max used loops ", max, ", min used loops ", min, ", average used_loops ", ave)
            assert_less_or_equal(min, max_loops(),
                                 "performance case ",
                                 name,
                                 " is out of maximum expected loops ",
                                 max_loops(),
                                 ", min used loops ",
                                 min)
            assert_more_or_equal(max, min_loops(),
                                 "performance case ",
                                 name,
                                 " is less than minimum expected loops ",
                                 min_loops(),
                                 ", max used loops ",
                                 max)
            Return True
        Else
            Return False
        End If
    End Function

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
