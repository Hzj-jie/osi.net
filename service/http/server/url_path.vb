
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class url_path
    Private Shared ReadOnly path_separators() As Char = {constants.uri.path_separator}

    Public Shared Function parse(ByVal request_absolute_path As String,
                                 ByRef result As vector(Of String),
                                 Optional ByVal e As Text.Encoding = Nothing) As Boolean
        result.renew()
        If Not request_absolute_path.null_or_empty() Then
            Dim ss() As String = Nothing
            ss = request_absolute_path.Split(path_separators, StringSplitOptions.RemoveEmptyEntries)
            If isemptyarray(ss) Then
                ' Odd. This should not happen.
                Return False
            Else
                For i As Int32 = 0 To array_size_i(ss) - 1
                    result.emplace_back(uri.path_encoder.decode(ss(i), e))
                Next
            End If
        End If
        Return True
    End Function

    Public Shared Function parse(ByVal request As HttpListenerRequest,
                                 ByRef result As vector(Of String),
                                 Optional ByVal e As Text.Encoding = Nothing) As Boolean
        If request Is Nothing Then
            Return False
        Else
            Return parse(request.Url().AbsolutePath(), result, e)
        End If
    End Function

    Public Shared Function parse(ByVal request As HttpListenerRequest,
                                 Optional ByVal e As Text.Encoding = Nothing) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        assert(parse(request, r, e))
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
