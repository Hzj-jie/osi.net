
Imports osi.root.connector

Public Class case_wrapper
    Inherits [case]

    Private ReadOnly c As [case] = Nothing

    Protected Sub New(ByVal c As [case])
        MyBase.New()
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    ' TODO: Be smarter, if GetType() is not any wrapper, c.name should be used.
    Public Sub New(ByVal c As [case],
                   ByVal full_name As String,
                   ByVal assembly_qualified_name As String,
                   ByVal name As String)
        MyBase.New(full_name, assembly_qualified_name, name)
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Overrides Function run() As Boolean
        Return c.run()
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return c.reserved_processors()
    End Function

    Protected Function mybase_prepare() As Boolean
        Return MyBase.prepare()
    End Function

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso c.prepare()
    End Function

    Protected Function mybase_finish() As Boolean
        Return MyBase.finish()
    End Function

    Public Overrides Function finish() As Boolean
        Return c.finish() AndAlso MyBase.finish()
    End Function
End Class
