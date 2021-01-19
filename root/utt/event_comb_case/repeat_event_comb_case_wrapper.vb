
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
        Me.New(c, assert_which.of(test_size).can_cast_to_uint64())
    End Sub

    Protected Overridable Function test_size() As UInt64
        Return size
    End Function

    Public NotOverridable Overrides Function create() As event_comb
        assert(test_size() > 0)
        assert(test_size() <= max_uint32)
        Return event_comb.[of](AddressOf MyBase.create).repeat(CUInt(test_size()))
    End Function
End Class
