
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Net
Imports System.Web
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _url_encoded_parameters
    Private ReadOnly argument_separators() As Char = {constants.uri.argument_separator}
    Private ReadOnly argument_name_value_separator As String = constants.uri.argument_name_value_separator
    'if result is nothing, create a new object,
    'otherwise injecting directly to it, which means existing key-value pair will be over-written
    <Extension()> Public Function parse_url_encoded_parameters(ByVal input As String,
                                                               ByRef result As map(Of String, String),
                                                               Optional ByVal e As Text.Encoding = Nothing) As Boolean
        If String.IsNullOrEmpty(input) Then
            Return False
        Else
            If result Is Nothing Then
                result = New map(Of String, String)()
            End If
            If e Is Nothing Then
                e = default_encoding
            End If
            Dim ss() As String = Nothing
            ss = input.Split(argument_separators, StringSplitOptions.RemoveEmptyEntries)
            If isemptyarray(ss) Then
                'odd, should not happen
                Return False
            Else
                For i As Int32 = 0 To array_size(ss) - 1
                    If Not String.IsNullOrEmpty(ss(i)) Then
                        Dim k As String = Nothing
                        Dim v As String = Nothing
                        If strsep(ss(i), k, v, argument_name_value_separator, False) Then
                            v = HttpUtility.UrlDecode(v, e)
                        Else
                            k = ss(i)
                            v = Nothing
                        End If
                        assert(Not String.IsNullOrEmpty(k))
                        k = HttpUtility.UrlDecode(k, e)
                        result(k) = v
                    End If
                Next
                Return True
            End If
        End If
    End Function

    <Extension()> Public Function parse_parameters(ByVal ctx As HttpListenerContext,
                                                   ByVal result As pointer(Of map(Of String, String)),
                                                   Optional ByVal e As Text.Encoding = Nothing,
                                                   Optional ByVal ls As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As map(Of String, String) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New map(Of String, String)()
                                  ec = parse_parameters(ctx, r, e, ls)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function parse_parameters(ByVal ctx As HttpListenerContext,
                                                   ByVal result As map(Of String, String),
                                                   Optional ByVal e As Text.Encoding = Nothing,
                                                   Optional ByVal ls As link_status = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim qs As String = Nothing
                                  If ctx Is Nothing OrElse
                                     (eva(qs,
                                          strmid(ctx.Request().Url().Query(),
                                                 strlen(constants.uri.query_mark))) AndAlso
                                      Not String.IsNullOrEmpty(qs) AndAlso
                                      Not parse_url_encoded_parameters(qs, result, e)) Then
                                      Return False
                                  ElseIf is_www_form_urlencoded(ctx) Then
                                      ec = parse_url_encoded_input_stream(ctx, result, ls)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function parse_url_encoded_input_stream(ByVal ctx As HttpListenerContext,
                                                                 ByVal result As pointer(Of map(Of String, String)),
                                                                 Optional ByVal ls As link_status = Nothing) _
                                                                As event_comb
        Dim ec As event_comb = Nothing
        Dim r As map(Of String, String) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New map(Of String, String)()
                                  ec = parse_url_encoded_input_stream(ctx, r, ls)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function parse_url_encoded_input_stream(ByVal ctx As HttpListenerContext,
                                                                 ByVal result As map(Of String, String),
                                                                 Optional ByVal ls As link_status = Nothing) _
                                                                As event_comb
        Dim ms As MemoryStream = Nothing
        Dim ec As event_comb = Nothing
        Dim charset As String = Nothing
        Return New event_comb(Function() As Boolean
                                  If ctx Is Nothing OrElse
                                     Not is_www_form_urlencoded(ctx, charset) Then
                                      Return False
                                  Else
                                      ms = New MemoryStream()
                                      ec = ctx.Request().read_request_body(ms, ls, Nothing)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim e As Text.Encoding = Nothing
                                  parse_encoding(charset, e)
                                  assert(Not e Is Nothing)
                                  Using sr As StreamReader = New StreamReader(ms, e, False)
                                      Dim r As Boolean = False
                                      r = parse_url_encoded_parameters(sr.ReadToEnd(), result, e)
                                      ms.Close()
                                      ms.Dispose()
                                      Return r AndAlso
                                             goto_end()
                                  End Using
                              End Function)
    End Function
End Module
