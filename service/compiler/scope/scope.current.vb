
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class scope(Of T As scope(Of T))
    Public Function includes() As includes_t
        Return from_root(Function(ByVal i As T) As includes_t
                             assert(Not i Is Nothing)
                             Return i.accessor().includes()
                         End Function)
    End Function

    Public Function defines() As define_t
        Return from_root(Function(ByVal i As T) As define_t
                             assert(Not i Is Nothing)
                             Return i.accessor().defines()
                         End Function)
    End Function
End Class
