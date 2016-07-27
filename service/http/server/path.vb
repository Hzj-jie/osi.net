
Imports System.Runtime.CompilerServices
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _path
    Private ReadOnly path_separators() As Char = {constants.uri.path_separator}

    <Extension()> Public Function parse_path(ByVal ctx As HttpListenerContext,
                                             ByRef result As vector(Of String),
                                             Optional ByVal e As Text.Encoding = Nothing) As Boolean
        If ctx Is Nothing Then
            Return False
        Else
            If result Is Nothing Then
                result = New vector(Of String)()
            Else
                result.clear()
            End If
            If Not String.IsNullOrEmpty(ctx.Request().Url().AbsolutePath()) Then
                Dim ss() As String = Nothing
                ss = ctx.Request().Url().AbsolutePath().Split(path_separators,
                                                              StringSplitOptions.RemoveEmptyEntries)
                If isemptyarray(ss) Then
                    'odd, should not happen
                    Return False
                Else
                    For i As Int32 = 0 To array_size(ss)
                        result.emplace_back(uri_path_decode(ss(i), e))
                    Next
                End If
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function path(ByVal ctx As HttpListenerContext,
                                       Optional ByVal e As Text.Encoding = Nothing) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        assert(parse_path(ctx, r, e))
        Return r
    End Function
End Module
