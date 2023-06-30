
Imports osi.root.connector

Partial Public Class type_attribute
    Private Class forward_signal
        Private ReadOnly s As type_attribute

        Public Sub New(ByVal s As type_attribute)
            assert(Not s Is Nothing)
            assert(Not s.s Is Nothing)
            Me.s = s
        End Sub

        Private Shared Function enable_forward_set(ByVal forward_mode As forward_mode) As Boolean
            Return forward_mode = type_attribute.forward_mode.set OrElse
                   forward_mode = type_attribute.forward_mode.any
        End Function

        Private Shared Function enable_forward_get(ByVal forward_mode As forward_mode) As Boolean
            Return forward_mode = type_attribute.forward_mode.get OrElse
                   forward_mode = type_attribute.forward_mode.any
        End Function

        Public Shared Function [set](ByVal i As Object,
                                     ByVal v As Object,
                                     ByVal forward_mode As forward_mode) As Boolean
            Dim fs As forward_signal = Nothing
            fs = TryCast(i, forward_signal)
            If fs Is Nothing Then
                Return False
            Else
                assert(enable_forward_set(forward_mode))
                fs.s.set(v)
                Return True
            End If
        End Function

        Public Shared Function [get](ByVal i As Object,
                                     ByRef o As Object,
                                     ByVal forward_mode As forward_mode) As Boolean
            Dim fs As forward_signal = Nothing
            fs = TryCast(i, forward_signal)
            If fs Is Nothing Then
                Return False
            Else
                assert(enable_forward_get(forward_mode))
                o = fs.s.get()
                Return True
            End If
        End Function
    End Class
End Class
