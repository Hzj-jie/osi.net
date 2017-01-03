
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.math

Namespace primitive
    Public Interface imitation
        Inherits executor
        Function extern_functions() As extern_functions
        Overloads Sub instruction_pointer(ByVal v As Int64)  ' data_ref.offset() returns int64
        Overloads Sub carry_over(ByVal v As Boolean)
        Overloads Sub divided_by_zero(ByVal v As Boolean)
        Overloads Sub imaginary_number(ByVal v As Boolean)
        Sub advance_instruction_pointer(ByVal v As Int64)
        Sub do_not_advance_instruction_pointer()
        Sub [stop]()
    End Interface

    Public Module _imitation
        <Extension()> Public Function convert_stack_to_uint32(ByVal this As imitation,
                                                              ByVal p As data_ref,
                                                              ByRef overflow As Boolean) As UInt32
            assert(Not this Is Nothing)
            Dim d As pointer(Of Byte()) = Nothing
            d = this.access_stack(p)
            assert(Not d Is Nothing)
            Dim b As big_uint = Nothing
            b = New big_uint(+d)
            Return b.as_uint32(overflow)
        End Function

        <Extension()> Public Function access_stack_as_bool(ByVal this As imitation, ByVal p As data_ref) As Boolean
            assert(Not this Is Nothing)
            Dim d As pointer(Of Byte()) = Nothing
            d = this.access_stack(p)
            assert(Not d Is Nothing)
            Dim o As Boolean = False
            ' If the data slot is empty, treat it as false.
            If Not bytes_bool(+d, o) Then
                o = False
            End If
            Return o
        End Function

        <Extension()> Public Function access_stack_as_uint32(ByVal this As imitation, ByVal p As data_ref) As UInt32
            assert(Not this Is Nothing)
            Dim o As Boolean = False
            Dim r As UInt32 = 0
            r = this.convert_stack_to_uint32(p, o)
            this.carry_over(o)
            Return r
        End Function

        <Extension()> Public Function access_stack_as_uint32(ByVal this As imitation,
                                                             ByVal p1 As data_ref,
                                                             ByVal p2 As data_ref,
                                                             ByVal ParamArray ps() As data_ref) As UInt32()
            assert(Not this Is Nothing)
            Dim r() As UInt32 = Nothing
            ReDim r(array_size(ps) + uint32_1)
            For i As UInt32 = 0 To array_size(ps) + uint32_1
                Dim o As Boolean = False
                If i = 0 Then
                    r(i) = this.convert_stack_to_uint32(p1, o)
                ElseIf i = 1 Then
                    r(i) = this.convert_stack_to_uint32(p2, o)
                Else
                    r(i) = this.convert_stack_to_uint32(ps(i - uint32_2), o)
                End If
                If o Then
                    this.carry_over(o)
                End If
            Next
            Return r
        End Function
    End Module
End Namespace
