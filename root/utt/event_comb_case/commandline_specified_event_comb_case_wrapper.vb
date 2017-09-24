
Imports osi.root.procedure
Imports osi.root.connector

Public Class commandline_specified_event_comb_case_wrapper
    Inherits event_comb_case_wrapper

    Public Sub New(ByVal c As event_comb_case)
        MyBase.New(c)
    End Sub

    Public Overrides Function create() As event_comb
        If commandline.specified(Me) Then
            Return MyBase.create()
        Else
            raise_error("did not select ", name, " with commandline arguemnts, ignore")
            Return event_comb.succeeded()
        End If
    End Function
End Class
