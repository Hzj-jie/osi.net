
Partial Public NotInheritable Class queue_runner
    Public Shared Function once(ByVal d As Action) As Boolean
        Return d IsNot Nothing AndAlso
               push_only(Function() As Boolean
                             d()
                             Return False
                         End Function)
    End Function
End Class
