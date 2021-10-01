
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

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
        <Extension()> Public Sub instruction_ref(ByVal this As imitation, ByVal v As UInt64)
            assert(Not this Is Nothing)
            If v > max_int64 Then
                executor_stop_error.throw(executor.error_type.instruction_ref_overflow)
            Else
                this.instruction_ref(CLng(v))
            End If
        End Sub

        <Extension()> Public Function access_stack_as_uint32(ByVal this As imitation, ByVal p As data_ref) As UInt32
            assert(Not this Is Nothing)
            Dim o As Boolean = False
            Dim r As UInt32 = this.convert_stack_to_uint32(p, o)
            this.carry_over(o)
            Return r
        End Function

        <Extension()> Public Function access_stack_as_uint64(ByVal this As imitation, ByVal p As data_ref) As UInt64
            assert(Not this Is Nothing)
            Dim o As Boolean = False
            Dim r As UInt64 = this.convert_stack_to_uint64(p, o)
            this.carry_over(o)
            Return r
        End Function

        <Extension()> Public Function access_stack_top_as_uint64(ByVal this As imitation) As UInt64
            Return this.access_stack_as_uint64(data_ref.rel(0))
        End Function

        <Extension()> Public Function access_heap(ByVal this As imitation, ByVal p As data_ref) As ref(Of Byte())
            Return this.access_heap(this.access_stack_as_uint64(p))
        End Function

        <Extension()> Public Function access_heap_as_uint32(ByVal this As imitation, ByVal p As UInt64) As UInt32
            assert(Not this Is Nothing)
            Dim o As Boolean = False
            Dim r As UInt32 = this.convert_heap_to_uint32(p, o)
            this.carry_over(o)
            Return r
        End Function

        <Extension()> Public Function access_heap_as_uint64(ByVal this As imitation, ByVal p As UInt64) As UInt64
            assert(Not this Is Nothing)
            Dim o As Boolean = False
            Dim r As UInt64 = this.convert_heap_to_uint64(p, o)
            this.carry_over(o)
            Return r
        End Function

        <Extension()> Public Function access_stack_as_uint32(ByVal this As imitation,
                                                             ByVal p1 As data_ref,
                                                             ByVal p2 As data_ref,
                                                             ByVal ParamArray ps() As data_ref) As UInt32()
            assert(Not this Is Nothing)
            Dim r(CInt(array_size(ps)) + 1) As UInt32
            For i As Int32 = 0 To CInt(array_size(ps)) + 1
                Dim o As Boolean = False
                If i = 0 Then
                    r(i) = this.convert_stack_to_uint32(p1, o)
                ElseIf i = 1 Then
                    r(i) = this.convert_stack_to_uint32(p2, o)
                Else
                    r(i) = this.convert_stack_to_uint32(ps(i - 2), o)
                End If
                If o Then
                    this.carry_over(o)
                End If
            Next
            Return r
        End Function
    End Module
End Namespace
