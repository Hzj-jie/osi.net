
Imports osi.root.utils
Imports osi.root.utt

Public Class type_initializer_behavior_test
    Inherits [case]

    Private Class nested_type_case_classes
        Public Class holder
            Private Shared v As Boolean

            Shared Sub New()
                v = False
            End Sub

            Public Shared Sub trigger()
                v = True
            End Sub

            Public Shared Function triggered() As Boolean
                Return v
            End Function
        End Class

        Public Class caller
            Shared Sub New()
                holder.trigger()
            End Sub

            Public Class nested_type
                Public Shared Sub trigger()
                End Sub
            End Class
        End Class
    End Class

    Private Shared Function nested_type_case() As Boolean
        assert_false(nested_type_case_classes.holder.triggered())
        nested_type_case_classes.caller.nested_type.trigger()
        assert_false(nested_type_case_classes.holder.triggered())
        Return True
    End Function

    Private Class delegate_case_classes
        Public Class holder
            Private Shared v As Boolean

            Shared Sub New()
                v = False
            End Sub

            Public Shared Sub trigger()
                v = True
            End Sub

            Public Shared Function triggered() As Boolean
                Return v
            End Function
        End Class

        Public Class caller
            Shared Sub New()
                holder.trigger()
            End Sub

            Public Shared Sub trigger()
            End Sub
        End Class
    End Class

    Private Shared Function delegate_case() As Boolean
        assert_false(delegate_case_classes.holder.triggered())
        Dim v As Action = Nothing
        v = AddressOf delegate_case_classes.caller.trigger
        assert_false(delegate_case_classes.holder.triggered())
        Return True
    End Function

    Private Class delegate_case2_classes
        Public Class holder
            Private Shared v As Boolean

            Shared Sub New()
                v = False
            End Sub

            Public Shared Sub trigger()
                v = True
            End Sub

            Public Shared Function triggered() As Boolean
                Return v
            End Function
        End Class

        Public Class caller
            Shared Sub New()
                holder.trigger()
            End Sub

            Public Shared Sub trigger()
            End Sub
        End Class
    End Class

    Private Shared Function delegate_case2() As Boolean
        assert_false(delegate_case2_classes.holder.triggered())
        Dim v As Action = Nothing
        v = Sub()
                delegate_case2_classes.caller.trigger()
            End Sub
        assert_false(delegate_case2_classes.holder.triggered())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return nested_type_case() AndAlso
               delegate_case() AndAlso
               delegate_case2()
    End Function
End Class
