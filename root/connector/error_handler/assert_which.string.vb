
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure string_assertion
        Private ReadOnly i As String

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal i As String)
            Me.i = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_uint32() As UInt32
            Try
                Return Convert.ToUInt32(i)
            Catch ex As FormatException
                assert(False, ex)
            Catch ex As OverflowException
                assert(False, ex)
            End Try
            Return 0
        End Function
    End Structure
End Class
