
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices
Imports osi.root.constants

Public NotInheritable Class send_input
    <StructLayout(LayoutKind.Sequential)>
    Public Structure mouse_input
        Public Enum flag As UInt32
            absolute = &H8000
            move = &H1
            no_coalesce = &H2000
            left_down = &H2
            left_up = &H4
            right_down = &H8
            right_up = &H10
            middle_down = &H20
            middle_up = &H40
            virtual_desk = &H4000
            wheel = &H800
            x_down = &H80
            x_up = &H100
        End Enum

        Public dx As Int32
        Public dy As Int32
        Public mouse_data As UInt32
        Public flags As UInt32
        Public time As UInt32
        Public extra_info As UIntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure keyboard_input
        Public Enum flag As UInt32
            extended_key = &H1
            key_up = &H2
            scan_code = &H8
            unicode = &H4
        End Enum

        Public Shared Function from_scan_code(ByVal code As UInt16) As keyboard_input
            Dim r As keyboard_input = Nothing
            r = New keyboard_input()
            r.scan_code = code
            r.flags = flag.scan_code
            If (code >> bit_count_in_byte) <> 0 Then
                r.flags = r.flags Or flag.extended_key
            End If
            Return r
        End Function

        Public virtual_key As UShort
        Public scan_code As UShort
        Public flags As UInt32
        Public time As UInt32
        Public extra_info As UIntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure hardware_input
        Public msg As UInt32
        Public param_low As UShort
        Public param_high As UShort
    End Structure

    Private Enum input_type As UInt32
        mouse = 0
        keyboard = 1
        hardware = 2
    End Enum

    <StructLayout(LayoutKind.Explicit)>
    Private Structure input
        Public Shared ReadOnly size As Int32

        Shared Sub New()
            size = sizeof(Of input)()
            assert(size > 0)
        End Sub

        <FieldOffset(0)>
        Public type As input_type
        <FieldOffset(4)>
        Public mouse_input As mouse_input
        <FieldOffset(4)>
        Public keyboard_input As keyboard_input
        <FieldOffset(4)>
        Public hardware_input As hardware_input
    End Structure

    Private Declare Function SendInput _
        Lib "user32.dll" _
        (ByVal input_count As UInt32,
         <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.Struct, SizeParamIndex:=0)>
         ByVal inputs() As input,
         ByVal input_size As Int32) As UInt32

    Private Shared Function send_input(Of T)(ByVal e() As T, ByVal attach As Action(Of input, T)) As UInt32
        assert(Not attach Is Nothing)
        Dim o() As input = Nothing
        Dim c As UInt32 = 0
        For i As Int32 = 0 To array_size_i(e) - 1
            If Not e(i) Is Nothing Then
                c += uint32_1
            End If
        Next

        If c = uint32_0 Then
            Return uint32_0
        End If

        ReDim o(CInt(c) - 1)
        Dim j As Int32 = 0
        For i As Int32 = 0 To array_size_i(e) - 1
            If Not e(i) Is Nothing Then
                o(j) = New input()
                attach(o(j), e(i))
                j += 1
            End If
        Next

        Try
            Return SendInput(c, o, input.size)
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute SendInput() function, ex ", ex)
            Return uint32_0
        End Try
    End Function

    Public Shared Function mouse(ByVal ParamArray inputs() As mouse_input) As UInt32
        Return send_input(inputs, Sub(ByVal input As input, ByVal mouse_input As mouse_input)
                                      input.type = input_type.mouse
                                      input.mouse_input = mouse_input
                                  End Sub)
    End Function

    Public Shared Function keyboard(ByVal ParamArray inputs() As keyboard_input) As UInt32
        Return send_input(inputs, Sub(ByVal input As input, ByVal keyboard_input As keyboard_input)
                                      input.type = input_type.keyboard
                                      input.keyboard_input = keyboard_input
                                  End Sub)
    End Function

    Public Shared Function hardware(ByVal ParamArray inputs() As hardware_input) As UInt32
        Return send_input(inputs, Sub(ByVal input As input, ByVal hardware_input As hardware_input)
                                      input.type = input_type.hardware
                                      input.hardware_input = hardware_input
                                  End Sub)
    End Function

    Private Sub New()
    End Sub
End Class
