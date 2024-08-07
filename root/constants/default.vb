
Option Explicit On
Option Infer Off
Option Strict On

Public Module _default
    Public Const default_sbyte As SByte = Nothing
    Public Const default_int8 As SByte = Nothing
    Public Const default_int16 As Int16 = Nothing
    Public Const default_int32 As Int32 = Nothing
    Public Const default_int64 As Int64 = Nothing
    Public Const default_byte As Byte = Nothing
    Public Const default_uint8 As Byte = Nothing
    Public Const default_uint16 As UInt16 = Nothing
    Public Const default_uint32 As UInt32 = Nothing
    Public Const default_uint64 As UInt64 = Nothing
    Public Const default_str As String = Nothing
    Public Const default_string As String = Nothing
    Public ReadOnly default_strings() As String = Nothing
    Public ReadOnly default_bytes() As Byte = Nothing
End Module

' Shared Sub New() significant impacts performance. See default_null_perf.
Public NotInheritable Class [default](Of T)
    Public Shared ReadOnly null As T = Nothing
End Class

Public NotInheritable Class default_array(Of T)
    Public Shared ReadOnly empty(-1) As T
End Class