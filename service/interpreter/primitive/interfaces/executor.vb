
Imports System.IO
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

        ' exportable.load_bitcode is not reasonable
        <Extension()> Public Function load_bitcode(ByVal e As executor, ByVal f As String) As Boolean
            Dim a() As Byte = Nothing
            Try
                a = File.ReadAllBytes(f)
            Catch ex As Exception
                raise_error(error_type.warning, "failed to read from file ", f)
                Return False
            End Try
            Return assert(Not e Is Nothing) AndAlso
                   e.import(a)
        End Function

        <Extension()> Public Function load_asccode(ByVal e As executor, ByVal f As String) As Boolean
            Dim s As String = Nothing
            Try
                s = File.ReadAllText(f)
            Catch ex As Exception
                raise_error(error_type.warning, "failed to read from file ", f)
                Return False
            End Try
            Return assert(Not e Is Nothing) AndAlso
                   e.import(s)
        End Function
    End Module
End Namespace
