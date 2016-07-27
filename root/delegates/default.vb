
Public Module _default
    Public ReadOnly func_bool_null As Func(Of Boolean) = Nothing
    Public ReadOnly func_bool_true As Func(Of Boolean) = Function() As Boolean
                                                             Return True
                                                         End Function
    Public ReadOnly func_bool_false As Func(Of Boolean) = Function() As Boolean
                                                              Return False
                                                          End Function
    Public ReadOnly action_null As Action = Nothing
    Public ReadOnly action_empty As Action = Sub()
                                             End Sub
End Module
