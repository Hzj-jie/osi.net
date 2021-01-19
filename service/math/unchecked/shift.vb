
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public NotInheritable Class shift
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function left(ByVal v As UInt64, ByVal moves As UInt32) As UInt64
        If moves > max_int32 Then
            v <<= max_int32
            moves -= CUInt(max_int32)
        End If
        If moves <> 0 Then
            v <<= CInt(moves)
        End If
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function left(ByVal v As UInt64, ByVal moves As Byte) As UInt64
        v <<= moves
        Return v
    End Function

    ' TODO: Move from connector.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_left(ByVal v As UInt32, ByVal moves As Byte) As UInt32
        Return root.connector.left_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_right(ByVal v As UInt32, ByVal moves As Byte) As UInt32
        Return root.connector.right_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_left(ByVal v As Int32, ByVal moves As Byte) As Int32
        Return root.connector.left_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_right(ByVal v As Int32, ByVal moves As Byte) As Int32
        Return root.connector.right_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_left(ByVal v As UInt64, ByVal moves As Byte) As UInt64
        Return root.connector.left_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_right(ByVal v As UInt64, ByVal moves As Byte) As UInt64
        Return root.connector.right_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_left(ByVal v As Int64, ByVal moves As Byte) As Int64
        Return root.connector.left_shift(v, moves)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function rotate_right(ByVal v As Int64, ByVal moves As Byte) As Int64
        Return root.connector.right_shift(v, moves)
    End Function

    Private Sub New()
    End Sub
End Class
