
Public Module _unref
    Public Function unref(Of T)(ByRef a As T) As T
        Return a
    End Function
End Module
