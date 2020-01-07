
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class binary_operation_value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private Shared ReadOnly ws As write_scoped(Of target_ref)

        Shared Sub New()
            ws = New write_scoped(Of target_ref)()
        End Sub

        Private NotInheritable Class target_ref
            Public ReadOnly left As String
            Public ReadOnly right As String

            Public Sub New(ByVal left As String, ByVal right As String)
                assert(Not left Is Nothing)
                assert(Not right Is Nothing)
                Me.left = left
                Me.right = right
            End Sub
        End Class

        Private Shared Function with_current_targets(ByVal left As String, ByVal right As String) As IDisposable
            Return ws.push(New target_ref(left, right))
        End Function

        Public Shared Function current_left_target() As String
            Return ws.current().left
        End Function

        Public Shared Function current_right_target() As String
            Return ws.current().right
        End Function
    End Class
End Class
