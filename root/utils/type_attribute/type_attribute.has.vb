
Imports osi.root.connector

Partial Public Class type_attribute
    Public Shared Function has(Of T)() As Boolean
        Return [of](Of T)(True)
    End Function

    Public Shared Function has(Of T As signal)(ByVal i As T) As Boolean
        Return [of](Of T)(True, i)
    End Function

    Public Shared Function has(ByVal i As signal) As Boolean
        Return [of](True, i)
    End Function

    Public Shared Function has(ByVal t As Type) As Boolean
        Return [of](True, t)
    End Function

    Public Shared Function has(ByVal obj As Object) As Boolean
        Return [of](True, obj)
    End Function
End Class
