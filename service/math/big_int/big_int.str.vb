﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_int
    Public Function str(Optional ByVal base As Byte = default_str_base,
                        Optional ByVal require_positive_signal_mask As Boolean = False) As String
        Dim r As String = Nothing
        r = d.str(base)
        If negative() Then
            Return negative_signal_mask + r
        End If
        If require_positive_signal_mask Then
            Return positive_signal_mask + r
        End If
        Return r
    End Function

    Public Shared Function parse(ByVal s As String,
                                 ByRef r As big_int,
                                 Optional ByVal base As Byte = default_str_base) As Boolean
        Dim signal As Boolean = False
        signal = True
        If Not String.IsNullOrEmpty(s) Then
            If s(0) = negative_signal_mask Then
                signal = False
                s = s.Substring(1)
            ElseIf s(0) = positive_signal_mask Then
                signal = True
                s = s.Substring(1)
            Else
                assert(signal)
            End If
        End If
        Dim d As big_uint = Nothing
        If big_uint.parse(s, d, base) Then
            r = share(d)
            r.set_signal(signal)
            Return True
        End If
        Return False
    End Function
End Class
