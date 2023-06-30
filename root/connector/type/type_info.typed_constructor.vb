
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class type_info(Of T)
    Private NotInheritable Class typed_constructor
        Public ReadOnly raw As Func(Of Object)
        Public ReadOnly typed As Func(Of T)

        Public Sub New(ByVal raw As Func(Of Object))
            Me.raw = raw
            If Not raw Is Nothing Then
                typed = Function() As T
                            Return direct_cast(Of T)(raw())
                        End Function
            End If
        End Sub

        Public Function empty() As Boolean
            assert((raw Is Nothing) = (typed Is Nothing))
            Return raw Is Nothing
        End Function
    End Class

    Private NotInheritable Class typed_parameters_constructor
        Public ReadOnly raw As Func(Of Object(), Object)
        Public ReadOnly typed As Func(Of Object(), T)

        Public Sub New(ByVal raw As Func(Of Object(), Object))
            Me.raw = raw
            If Not raw Is Nothing Then
                typed = Function(ByVal parameters() As Object) As T
                            Return direct_cast(Of T)(raw(parameters))
                        End Function
            End If
        End Sub

        Public Function empty() As Boolean
            assert((raw Is Nothing) = (typed Is Nothing))
            Return raw Is Nothing
        End Function
    End Class
End Class
