
Imports osi.root.connector

Public Class realtime_wrapper
    Inherits case_wrapper

    Public Sub New(ByVal c As [case])
        MyBase.New(c)
    End Sub

    Public NotOverridable Overrides Function run() As Boolean
        Using New realtime()
            Return MyBase.run()
        End Using
    End Function
End Class
