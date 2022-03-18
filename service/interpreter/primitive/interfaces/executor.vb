
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace primitive
    Public NotInheritable Class executor_stop_error
        Inherits Exception

        Public ReadOnly error_type As executor.error_type

        Public Sub New(ByVal error_type As executor.error_type)
            Me.error_type = error_type
        End Sub

        Public Shared Sub [throw](ByVal error_type As executor.error_type)
            Throw New executor_stop_error(error_type)
        End Sub

        Public Shared Sub [throw](ByVal error_type As executor.error_type, ByVal ParamArray log_msg() As Object)
            raise_error(root.constants.error_type.user, log_msg)
            [throw](error_type)
        End Sub

        Public Overrides Function ToString() As String
            Return "executor_stop_error: " + error_type.ToString()
        End Function
    End Class

    Public Interface executor
        Inherits exportable

        Enum error_type
            instruction_ref_overflow
            stack_access_out_of_boundary
            undefined_interrupt
            unsupported_feature
            invalid_buffer_size
            interrupt_failure
            interrupt_implementation_failure
            heap_access_out_of_boundary
            out_of_heap_memory
        End Enum

        Structure state
            Public Shared ReadOnly empty As state
            Public ReadOnly instruction_ref As UInt64
            Public ReadOnly stack_size As UInt64

            Public Sub New(ByVal instruction_ref As UInt64, ByVal stack_size As UInt64)
                Me.instruction_ref = instruction_ref
                Me.stack_size = stack_size
            End Sub
        End Structure

        Function access(ByVal p As data_ref) As ref(Of Byte())
        Function stack_size() As UInt64
        Function instruction_ref() As UInt64
        Function carry_over() As Boolean
        Function divided_by_zero() As Boolean
        Function imaginary_number() As Boolean
        Function access_states(ByVal p As UInt64) As state
        Function states_size() As UInt64

        Function halt() As Boolean
        Function halt_error() As error_type

        Sub execute()
    End Interface

    Public Module _executor
        <Extension()> Public Function access_as_bool(ByVal this As executor, ByVal p As data_ref) As Boolean
            assert(Not this Is Nothing)
            Dim d As ref(Of Byte()) = this.access(p)
            assert(Not d Is Nothing)
            Dim o As Boolean = False
            ' If the data slot is empty, treat it as false.
            If Not bytes_bool(+d, o) Then
                o = False
            End If
            Return o
        End Function

        <Extension()> Public Function stack_top(ByVal this As executor) As ref(Of Byte())
            assert(Not this Is Nothing)
            Return this.access(data_ref.rel(0))
        End Function

        <Extension()> Public Function current_state(ByVal this As executor) As executor.state
            assert(Not this Is Nothing)
            Return New executor.state(this.instruction_ref() + uint64_1, this.stack_size())
        End Function
    End Module
End Namespace
