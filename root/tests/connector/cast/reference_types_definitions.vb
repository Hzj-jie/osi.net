
Public Module _reference_types_definitions
    Friend Interface return_s
        Function s() As String
    End Interface

    Friend Class base_class
        Implements return_s

        Public ReadOnly s As String = Nothing

        Public Sub New(ByVal s As String)
            Me.s = s
        End Sub

        Public Function s1() As String Implements return_s.s
            Return s
        End Function
    End Class

    Friend Class inherit_class
        Inherits base_class
        Implements return_s

        Public Sub New(ByVal s As String)
            MyBase.New(s)
        End Sub
    End Class

    Friend Class other_class
        Implements return_s

        Public ReadOnly s As String = Nothing

        Public Sub New(ByVal s As String)
            Me.s = s
        End Sub

        Public Function s1() As String Implements return_s.s
            Return s
        End Function
    End Class

    Friend Class ctyped_class
        Implements return_s

        Public ReadOnly s As String = Nothing

        Public Sub New(ByVal s As String)
            Me.s = s
        End Sub

        Public Shared Widening Operator CType(ByVal this As ctyped_class) As inherit_class
            If this Is Nothing Then
                Return Nothing
            Else
                Return New inherit_class(this.s)
            End If
        End Operator

        Public Function s1() As String Implements return_s.s
            Return s
        End Function
    End Class

    Friend Class ctyped2_class
        Implements return_s

        Public ReadOnly s As String = Nothing

        Public Sub New(ByVal s As String)
            Me.s = s
        End Sub

        Public Shared Narrowing Operator CType(ByVal this As ctyped2_class) As inherit_class
            If this Is Nothing Then
                Return Nothing
            Else
                Return New inherit_class(this.s)
            End If
        End Operator

        Public Function s1() As String Implements return_s.s
            Return s
        End Function
    End Class
End Module
