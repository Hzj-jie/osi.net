﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure

Public Class repeat_event_comb_case_wrapper
    Inherits event_comb_case_wrapper

    Private ReadOnly size As UInt64 = 0

    Public Sub New(ByVal c As event_comb_case, Optional ByVal test_size As UInt64 = 1)
        MyBase.New(c)
        size = test_size
    End Sub

    Public Sub New(ByVal c As event_comb_case, ByVal test_size As Int64)
        Me.New(c, assert_return(test_size >= 0, CULng(test_size)))
        raise_error(error_type.deprecated,
                    "repeat_event_comb_case_wrapper([case], int64) is deprecated, use uint64 overloads: ",
                    backtrace())
    End Sub

    Protected Overridable Function test_size() As UInt64
        Return size
    End Function

    Public NotOverridable Overrides Function create() As event_comb
        assert(test_size() > 0)
        assert(test_size() <= max_int32)
        Return event_comb.repeat(CInt(test_size()), Function() MyBase.create())
    End Function
End Class
