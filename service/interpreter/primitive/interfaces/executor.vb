
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

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
    End Class

    Public Interface executor
        Inherits exportable

        Enum error_type
            instruction_pointer_overflow
            stack_access_out_of_boundary
            undefined_extern_function
        End Enum

        Function access_stack(ByVal p As data_ref) As pointer(Of Byte())
        Sub push_stack(ByVal count As UInt32)
        Sub pop_stack(ByVal count As UInt32)
        Function instruction_pointer() As UInt64
        Function carry_over() As Boolean
        Function divided_by_zero() As Boolean
        Function imaginary_number() As Boolean

        Function halt() As Boolean
        Function errors() As vector(Of error_type)

        Sub execute()
    End Interface

    Public Module _executor
        <Extension()> Public Sub push_stack(ByVal e As executor)
            assert(Not e Is Nothing)
            e.push_stack(uint32_1)
        End Sub

        <Extension()> Public Sub pop_stack(ByVal e As executor)
            assert(Not e Is Nothing)
            e.pop_stack(uint32_1)
        End Sub
    End Module
End Namespace
