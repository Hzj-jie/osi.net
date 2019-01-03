
Imports osi.root.connector
Imports osi.root.utt

Public Class gettype_test
    Inherits [case]

    Private Interface I(Of T)
    End Interface

    Private Class A(Of T)
        Implements I(Of T)
    End Class

    Private Class B(Of T)
        Inherits A(Of T)
    End Class

    Private Sub type_compare(Of T)()
        assertion.is_false(GetType(T).GetGenericTypeDefinition() Is GetType(A(Of )))
        assertion.is_true(GetType(T).GetGenericTypeDefinition() Is GetType(B(Of )))
        assertion.is_false(GetType(T).GetGenericTypeDefinition() Is GetType(I(Of )))
        assertion.is_true(GetType(T).GetGenericTypeDefinition().is(GetType(A(Of ))))
        assertion.is_true(GetType(T).GetGenericTypeDefinition().is(GetType(B(Of ))))
        assertion.is_true(GetType(T).GetGenericTypeDefinition().is(GetType(I(Of ))))
        assertion.is_true(GetType(T).is(GetType(A(Of ))))
        assertion.is_true(GetType(T).is(GetType(B(Of ))))
        assertion.is_true(GetType(T).is(GetType(I(Of ))))
    End Sub

    Public Overrides Function run() As Boolean
        Dim x As A(Of String) = Nothing
        x = New A(Of String)()
        assertion.is_true(TypeOf x Is A(Of String))
        assertion.is_false(TypeOf DirectCast(x, Object) Is A(Of Int32))
        assertion.is_true(GetType(A(Of String)).GetGenericTypeDefinition() Is GetType(A(Of )))
        assertion.is_true(GetType(A(Of String)).GetGenericTypeDefinition().is(GetType(A(Of ))))
        assertion.is_false(GetType(A(Of String)).GetGenericTypeDefinition() Is GetType(I(Of )))
        assertion.is_true(GetType(A(Of String)).GetGenericTypeDefinition().is(GetType(I(Of ))))
        assertion.is_true(GetType(A(Of String)).is(GetType(A(Of ))))
        assertion.is_true(GetType(A(Of String)).is(GetType(I(Of ))))
        assertion.is_true(GetType(A(Of String)) Is GetType(A(Of String)))
        assertion.is_true(x.GetType().GetGenericTypeDefinition() Is GetType(A(Of )))
        assertion.is_false(x.GetType().GetGenericTypeDefinition() Is GetType(I(Of )))
        assertion.is_true(x.GetType().GetGenericTypeDefinition().is(GetType(A(Of ))))
        assertion.is_true(x.GetType().GetGenericTypeDefinition().is(GetType(I(Of ))))
        assertion.is_true(x.GetType().is(GetType(A(Of ))))
        assertion.is_true(x.GetType().is(GetType(I(Of ))))
        assertion.is_true(GetType(B(Of String)).inherit(GetType(A(Of ))))
        assertion.is_true(GetType(B(Of String)).implement(GetType(I(Of ))))
        assertion.is_true(GetType(B(Of )).inherit(GetType(A(Of ))))
        assertion.is_true(GetType(B(Of )).implement(GetType(I(Of ))))
        type_compare(Of B(Of String))()
        type_compare(Of B(Of Int32))()
        Return True
    End Function
End Class
