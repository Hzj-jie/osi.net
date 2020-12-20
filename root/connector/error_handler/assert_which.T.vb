
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure T_assertion(Of T)
        Private ReadOnly obj As T

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal obj As T)
            Me.obj = obj
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_not_null() As T
            assert(Not obj Is Nothing)
            Return obj
        End Function
    End Structure
End Class
