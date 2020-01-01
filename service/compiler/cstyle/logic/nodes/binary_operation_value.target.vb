
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class cstyle
    Partial Public NotInheritable Class binary_operation_value
        Inherits builder_wrapper
        Implements builder

        Private Shared current_left As value.target
        Private Shared current_right As value.target

        Private Shared Function with_current_targets(ByVal left As value.target, ByVal right As value.target) As IDisposable
            assert(Not left Is Nothing)
            assert(Not right Is Nothing)
            current_left = left
            current_right = right
            Return deferring.to(Sub()
                                    current_left = Nothing
                                    current_right = Nothing
                                End Sub)
        End Function

        Public Shared Function current_left_target() As String
            assert(Not current_left Is Nothing)
            Return current_left.value_name
        End Function

        Public Shared Function current_right_target() As String
            assert(Not current_right Is Nothing)
            Return current_right.value_name
        End Function
    End Class
End Class
