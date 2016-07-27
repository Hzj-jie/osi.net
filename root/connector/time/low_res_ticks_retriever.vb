﻿
Imports System.DateTime
Imports osi.root.constants

Public Class low_res_ticks_retriever
    Private Const revise_interval_ms As UInt32 = 30 * minute_second * second_milli
    Private Shared last_ms As UInt32
    Private Shared last_revise_ms As UInt32
    Private Shared offset As Int64

    Shared Sub New()
        low_res_milliseconds()
    End Sub

    Private Sub New()
    End Sub

    Public Shared Function low_res_ticks() As Int64
        Return milliseconds_to_ticks(low_res_milliseconds())
    End Function

    Public Shared Function low_res_milliseconds() As Int64
        Dim lrs As UInt32 = 0
        lrs = last_revise_ms
        Dim c As UInt32 = 0
        c = environment_milliseconds_uint32()
        If offset = 0 OrElse
           c < last_ms OrElse
           c < lrs OrElse
           c - lrs >= revise_interval_ms Then
            'lrs will be larger than c when the environment_milliseconds_uint32 returns to 0
            For i As Int32 = 0 To 1
                Dim this As Int64 = 0
                this = (((Now().milliseconds() - environment_milliseconds_uint32()) -
                         (environment_milliseconds_uint32() - Now().milliseconds())) >> 1)
                assert(this > 0)
                If i = 0 OrElse
                   this < offset Then
                    offset = this
                End If
            Next
            c = environment_milliseconds_uint32()
            last_revise_ms = c
        End If
        last_ms = c
        assert(offset > 0)
        Return offset + c
    End Function
End Class
