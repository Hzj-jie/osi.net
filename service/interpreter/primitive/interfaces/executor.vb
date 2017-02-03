
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.service.math

Namespace primitive
    Public Class executor_stop_error
        Inherits Exception

        Public ReadOnly error_types() As executor.error_type

        Public Sub New(ByVal ParamArray error_types() As executor.error_type)
            Me.error_types = error_types
        End Sub

        Public Shared Sub [throw](ByVal ParamArray error_types() As executor.error_type)
            Throw New executor_stop_error(error_types)
        End Sub

        Public Overrides Function ToString() As String
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            r.Append("executor_stop_error: ")
            For i As Int32 = 0 To array_size_i(error_types) - 1
                If i > 0 Then
                    r.Append(", ")
                End If
                r.Append(error_types(i))
            Next
            Return Convert.ToString(r)
        End Function
    End Class

    Public Interface executor
        Inherits exportable

        Enum error_type
            instruction_pointer_overflow
            stack_access_out_of_boundary
            undefined_extern_function
            unsupported_feature
            invalid_buffer_size
        End Enum

        Structure state
            Public Shared ReadOnly empty As state
            Public ReadOnly instruction_pointer As UInt64
            Public ReadOnly stack_size As UInt64

            Public Sub New(ByVal instruction_pointer As UInt64, ByVal stack_size As UInt64)
                Me.instruction_pointer = instruction_pointer
                Me.stack_size = stack_size
            End Sub
        End Structure

        Function access_stack(ByVal p As data_ref) As pointer(Of Byte())
        Function stack_size() As UInt64
        Function instruction_pointer() As UInt64
        Function carry_over() As Boolean
        Function divided_by_zero() As Boolean
        Function imaginary_number() As Boolean
        Function access_states(ByVal p As UInt64) As state
        Function states_size() As UInt64

        Function halt() As Boolean
        Function errors() As vector(Of error_type)

        Sub execute()
    End Interface

    Public Module _executor
        <Extension()> Public Function convert_stack_to_uint32(ByVal this As executor,
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

        <Extension()> Public Function convert_stack_to_uint64(ByVal this As executor,
                                                              ByVal p As data_ref,
                                                              ByRef overflow As Boolean) As UInt64
            assert(Not this Is Nothing)
            Dim d As pointer(Of Byte()) = Nothing
            d = this.access_stack(p)
            assert(Not d Is Nothing)
            Dim b As big_uint = Nothing
            b = New big_uint(+d)
            Return b.as_uint64(overflow)
        End Function

        <Extension()> Public Function access_stack_as_bool(ByVal this As executor, ByVal p As data_ref) As Boolean
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

        <Extension()> Public Function stack_top(ByVal this As executor) As pointer(Of Byte())
            assert(Not this Is Nothing)
            Return this.access_stack(data_ref.rel(0))
        End Function

        <Extension()> Public Function convert_stack_top_to_uint64(ByVal this As executor,
                                                                  ByRef overflow As Boolean) As UInt64
            Return this.convert_stack_to_uint64(data_ref.rel(0), overflow)
        End Function

        <Extension()> Public Function current_state(ByVal this As executor) As executor.state
            assert(Not this Is Nothing)
            Return New executor.state(this.instruction_pointer() + uint64_1, this.stack_size())
        End Function
    End Module
End Namespace
