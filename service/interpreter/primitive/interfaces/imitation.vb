
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.math

Namespace primitive
    Public Interface imitation
        Inherits executor
        Function interrupts() As interrupts
        Sub push_stack()
        Sub pop_stack()
        Sub store_state()
        Sub restore_state()
        Function alloc(ByVal size As UInt64) As UInt64
        Sub dealloc(ByVal pos As UInt64)
        Overloads Sub instruction_ref(ByVal v As Int64)  ' data_ref.offset() returns int64
        Overloads Sub carry_over(ByVal v As Boolean)
        Overloads Sub divided_by_zero(ByVal v As Boolean)
        Overloads Sub imaginary_number(ByVal v As Boolean)
        Sub advance_instruction_ref(ByVal v As Int64)
        Sub do_not_advance_instruction_ref()
        Sub [stop]()
    End Interface

    Public Module _imitation
        <Extension()> Public Function access_ref_as_uint32(ByVal this As imitation, ByVal r As ref(Of Byte())) As UInt32
            assert(this IsNot Nothing)
            assert(r IsNot Nothing)
            Dim b As New big_uint(+r)
            Dim overflow As Boolean = False
            Dim o As UInt32 = b.as_uint32(overflow)
            this.carry_over(overflow)
            Return o
        End Function

        <Extension()> Public Function access_ref_as_uint64(ByVal this As imitation, ByVal r As ref(Of Byte())) As UInt64
            assert(this IsNot Nothing)
            assert(r IsNot Nothing)
            Dim b As New big_uint(+r)
            Dim overflow As Boolean = False
            Dim o As UInt64 = b.as_uint64(overflow)
            this.carry_over(overflow)
            Return o
        End Function

        <Extension()> Public Function access_ref_as_int64(ByVal this As imitation, ByVal r As ref(Of Byte())) As Int64
            assert(this IsNot Nothing)
            assert(r IsNot Nothing)
            Dim b As New big_uint(+r)
            Dim overflow As Boolean = False
            Dim o As Int64 = b.as_int64(overflow)
            this.carry_over(overflow)
            Return o
        End Function

        <Extension()> Public Sub instruction_ref(ByVal this As imitation, ByVal v As UInt64)
            assert(this IsNot Nothing)
            If v > max_int64 Then
                executor_stop_error.throw(executor.error_type.instruction_ref_overflow)
            Else
                this.instruction_ref(CLng(v))
            End If
        End Sub

        <Extension()> Public Function access_as_uint32(ByVal this As imitation, ByVal p As data_ref) As UInt32
            assert(this IsNot Nothing)
            Return this.access_ref_as_uint32(this.access(p))
        End Function

        <Extension()> Public Function access_as_uint64(ByVal this As imitation, ByVal p As data_ref) As UInt64
            assert(this IsNot Nothing)
            Return this.access_ref_as_uint64(this.access(p))
        End Function

        <Extension()> Public Function access_as_int64(ByVal this As imitation, ByVal p As data_ref) As Int64
            assert(this IsNot Nothing)
            Return this.access_ref_as_int64(this.access(p))
        End Function

        <Extension()> Public Function access_as_uint32(ByVal this As imitation,
                                                       ByVal p1 As data_ref,
                                                       ByVal p2 As data_ref,
                                                       ByVal ParamArray ps() As data_ref) As UInt32()
            assert(this IsNot Nothing)
            Dim r(CInt(array_size(ps)) + 1) As UInt32
            Dim co As Boolean = False
            For i As Int32 = 0 To CInt(array_size(ps)) + 1
                Dim o As Boolean = False
                If i = 0 Then
                    r(i) = this.access_as_uint32(p1)
                ElseIf i = 1 Then
                    r(i) = this.access_as_uint32(p2)
                Else
                    r(i) = this.access_as_uint32(ps(i - 2))
                End If
                If this.carry_over() Then
                    co = True
                End If
            Next
            this.carry_over(co)
            Return r
        End Function
    End Module
End Namespace
