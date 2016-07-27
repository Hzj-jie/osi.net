
Imports osi.root.connector

Public Class case_wrapper
    Inherits [case]

    Private ReadOnly c As [case] = Nothing

    Protected Sub New(ByVal c As [case])
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Overrides Function run() As Boolean
        Return c.run()
    End Function

    Public Overrides Function preserved_processors() As Int16
        Return c.preserved_processors()
    End Function

    Protected Function case_prepare_proxy() As Boolean
        Return MyBase.prepare()
    End Function

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso c.prepare()
    End Function

    Protected Function case_finish_proxy() As Boolean
        Return MyBase.finish()
    End Function

    Public Overrides Function finish() As Boolean
        Return c.finish() AndAlso MyBase.finish()
    End Function
End Class
