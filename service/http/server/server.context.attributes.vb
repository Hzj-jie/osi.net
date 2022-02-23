
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class server
    Partial Public NotInheritable Class context
        Public NotInheritable Class www_form_urlencoded
            Public ReadOnly owner As context
            Public ReadOnly is_www_form_urlencoded As Boolean
            Public ReadOnly charset As String
            Public ReadOnly encoder As Encoding
            Public ReadOnly query As map(Of String, vector(Of String))

            Public Sub New(ByVal owner As context,
                           ByVal is_www_form_urlencoded As Boolean,
                           ByVal charset As String,
                           ByVal encoder As Encoding,
                           ByVal query As map(Of String, vector(Of String)))
                assert(owner IsNot Nothing)
                assert(encoder IsNot Nothing)
                assert(query IsNot Nothing)
                Me.owner = owner
                Me.is_www_form_urlencoded = is_www_form_urlencoded
                Me.charset = charset
                Me.encoder = encoder
                Me.query = query
            End Sub
        End Class
    End Class
End Class
