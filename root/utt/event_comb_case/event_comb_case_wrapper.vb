
Imports osi.root.procedure
Imports osi.root.connector

Public Class event_comb_case_wrapper
    Inherits event_comb_case

    Private ReadOnly c As event_comb_case = Nothing

    Protected Sub New(ByVal c As event_comb_case)
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Overrides Function create() As event_comb
        Return c.create()
    End Function

    Public Overrides Function preserved_processors() As Int16
        Return c.preserved_processors()
    End Function

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso c.prepare()
    End Function

    Public Overrides Function finish() As Boolean
        Return MyBase.finish() AndAlso c.finish()
    End Function
End Class
