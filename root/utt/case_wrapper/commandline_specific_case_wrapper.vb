﻿
Imports osi.root.connector

Public Class commandline_specific_case_wrapper
    Inherits case_wrapper

    Public Sub New(ByVal c As [case])
        MyBase.New(c)
    End Sub

    Public NotOverridable Overrides Function prepare() As Boolean
        If commandline_specific() Then
            Return MyBase.prepare()
        Else
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        If commandline_specific() Then
            Return MyBase.run()
        Else
            raise_error("did not select ", name, " with commandline arguemnts, ignore")
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        If commandline_specific() Then
            Return MyBase.finish()
        Else
            Return True
        End If
    End Function
End Class
