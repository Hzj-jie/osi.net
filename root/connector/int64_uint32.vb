
Option Explicit On
Option Infer Off
Option Strict Off

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _int64_uint32
    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function low_uint32(ByVal this As UInt64) As UInt32
        Dim v As union_int64 = Nothing
        v.u_value = this
        If BitConverter.IsLittleEndian Then
            Return v.first_u32
        End If
        Return v.second_u32
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function high_uint32(ByVal this As UInt64) As UInt32
        Dim v As union_int64 = Nothing
        v.u_value = this
        If BitConverter.IsLittleEndian Then
            Return v.second_u32
        End If
        Return v.first_u32
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function low_uint32(ByVal this As Int64) As UInt32
        Dim v As union_int64 = Nothing
        v.s_value = this
        If BitConverter.IsLittleEndian Then
            Return v.first_u32
        End If
        Return v.second_u32
    End Function
End Module
