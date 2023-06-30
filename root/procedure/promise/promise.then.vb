
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Partial Public NotInheritable Class promise
    Public Function [then](ByVal on_resolve As Func(Of Object, Object)) As promise
        Return [then](on_resolve, Nothing)
    End Function

    Public Function [then](ByVal on_resolve As Action(Of Object), ByVal on_reject As Action(Of Object)) As promise
        assert(Not on_resolve Is Nothing)
        Return [then](Function(ByVal result As Object) As Object
                          on_resolve(result)
                          Return result
                      End Function,
                      on_reject)
    End Function

    Public Function [then](ByVal on_resolve As Action(Of Object)) As promise
        Return [then](on_resolve, Nothing)
    End Function

    Public Function [then](ByVal on_resolve As Action) As promise
        Return [then](on_resolve.parameter_erasure(Of Object)(), Nothing)
    End Function
End Class
