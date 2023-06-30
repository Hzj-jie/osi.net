
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Interface iquestioner
    Default ReadOnly Property question(ByVal request As command,
                                       ByVal response As ref(Of command)) As event_comb
    Default ReadOnly Property question(ByVal request() As Byte,
                                       ByVal response As ref(Of Byte())) As event_comb
    Default ReadOnly Property question(ByVal request As command,
                                       ByVal response As Func(Of command, Boolean)) As event_comb
    Default ReadOnly Property question(ByVal request As command,
                                       ByVal response As Func(Of Boolean)) As event_comb
    Default ReadOnly Property question(ByVal request As command) As event_comb
End Interface

Public Module _iquestioner
    Public Function question_redirect(ByVal this As iquestioner,
                                      ByVal request() As Byte,
                                      ByVal response As ref(Of Byte())) As event_comb
        assert(Not this Is Nothing)
        Dim r As ref(Of command) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of command)()
                                  ec = this(command.[New](request), r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not (+r) Is Nothing AndAlso
                                         eva(response, (+r).action()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function question_redirect(ByVal this As iquestioner,
                                      ByVal request As command,
                                      ByVal response As Func(Of command, Boolean)) As event_comb
        assert(Not this Is Nothing)
        Dim r As ref(Of command) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of command)()
                                  ec = this(request, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not (+r) Is Nothing AndAlso
                                         (+r).action_is(constants.response.success) AndAlso
                                         (response Is Nothing OrElse response(+r)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function question_redirect(ByVal this As iquestioner,
                                      ByVal request As command,
                                      ByVal response As Func(Of Boolean)) As event_comb
        assert(Not this Is Nothing)
        Return this(request,
                    Function(c As command) As Boolean
                        Return response Is Nothing OrElse response()
                    End Function)
    End Function

    Public Function question_redirect(ByVal this As iquestioner, ByVal request As command) As event_comb
        assert(Not this Is Nothing)
        Return this(request, root.delegates.func_bool_null)
    End Function
End Module
