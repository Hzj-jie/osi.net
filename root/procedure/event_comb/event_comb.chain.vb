
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class event_comb
    Private Shared Function accept_ec(ByVal ec As event_comb,
                                      ByVal accept_null_ec As Boolean,
                                      ByVal d As Func(Of Boolean)) As Boolean
        assert(d IsNot Nothing)
        Return (accept_null_ec AndAlso ec Is Nothing) OrElse d()
    End Function

    Private Shared Function chain_waitfor(ByVal ec As event_comb, ByVal accept_null_ec As Boolean) As Boolean
        Return accept_ec(ec, accept_null_ec, Function() As Boolean
                                                 Return waitfor(ec)
                                             End Function) AndAlso
               goto_next()
    End Function

    Private Shared Function ec_result(ByVal ec As event_comb,
                                      ByVal accept_null_ec As Boolean,
                                      ByVal [goto] As Func(Of Boolean)) As Boolean
        Return accept_ec(ec, accept_null_ec, Function() As Boolean
                                                 Return ec.end_result()
                                             End Function) AndAlso
               [goto]()
    End Function

    Private Shared Function end_result(ByVal ec As event_comb, ByVal accept_null_ec As Boolean) As Boolean
        Return ec_result(ec, accept_null_ec, AddressOf goto_end)
    End Function

    Public Shared Function chain_before(ByVal d As Func(Of Boolean),
                                        ByVal c As Func(Of event_comb),
                                        Optional ByVal accept_null_ec As Boolean = False) As event_comb
        assert(d IsNot Nothing)
        assert(c IsNot Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If d() Then
                                      ec = c()
                                      Return chain_waitfor(ec, accept_null_ec)
                                  Else
                                      Return False
                                  End If
                              End Function,
                             Function() As Boolean
                                 Return end_result(ec, accept_null_ec)
                             End Function)
    End Function

    Public Shared Function chain_before(ByVal d As Func(Of Boolean),
                                        ByVal ec As event_comb,
                                        Optional ByVal accept_null_ec As Boolean = False) As event_comb
        Return chain_before(d,
                            Function() As event_comb
                                Return ec
                            End Function,
                            accept_null_ec)
    End Function

    Public Shared Function chain_after(ByVal c As Func(Of event_comb),
                                       ByVal before As Func(Of Boolean),
                                       ByVal after As Func(Of Boolean),
                                       Optional ByVal accept_null_ec As Boolean = False) As event_comb
        assert(c IsNot Nothing)
        assert(before IsNot Nothing OrElse after IsNot Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = c()
                                  Return chain_waitfor(ec, accept_null_ec)
                              End Function,
                             Function() As Boolean
                                 Dim rtn As Boolean = True
                                 If before IsNot Nothing Then
                                     rtn = before()
                                 End If
                                 rtn = rtn AndAlso end_result(ec, accept_null_ec)
                                 If after IsNot Nothing Then
                                     rtn = rtn AndAlso after()
                                 End If
                                 Return rtn
                             End Function)
    End Function

    Public Shared Function chain_after(ByVal c As Func(Of event_comb),
                                       ByVal d As Func(Of Boolean),
                                       Optional ByVal accept_null_ec As Boolean = False) As event_comb
        Return chain_after(c, Nothing, d, accept_null_ec)
    End Function

    Public Shared Function chain_after_always(ByVal c As Func(Of event_comb),
                                              ByVal d As Func(Of Boolean),
                                              Optional ByVal accept_null_ec As Boolean = False) As event_comb
        Return chain_after(c, d, Nothing, accept_null_ec)
    End Function

    Public Shared Function chain_after(ByVal ec As event_comb,
                                       ByVal d As Func(Of Boolean),
                                       Optional ByVal accept_null_ec As Boolean = False) As event_comb
        Return chain_after(Function() As event_comb
                               Return ec
                           End Function,
                           d,
                           accept_null_ec)
    End Function

    Public Shared Function chain_after_always(ByVal ec As event_comb,
                                              ByVal d As Func(Of Boolean),
                                              Optional ByVal accept_null_ec As Boolean = False) As event_comb
        Return chain_after_always(Function() As event_comb
                                      Return ec
                                  End Function,
                                  d,
                                  accept_null_ec)
    End Function

    Private Shared Function chain_result(ByVal ec As event_comb, ByVal accept_null_ec As Boolean) As Boolean
        Return ec_result(ec, accept_null_ec, AddressOf goto_prev)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As void(Of event_comb, Boolean, Boolean, Boolean)) As event_comb
        assert(cs IsNot Nothing AndAlso cs.Length() > 0)
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Dim accept_null_ec As Boolean = False
        Dim predict As Boolean = False
        Dim asPredict As Boolean = False
        Return New event_comb(Function() As Boolean
                                  cs(i)(ec, accept_null_ec, predict, asPredict)
                                  i += 1
                                  If asPredict Then
                                      Return predict AndAlso (i < cs.Length() OrElse goto_end())
                                  Else
                                      Return chain_waitfor(ec, accept_null_ec)
                                  End If
                              End Function,
                             Function() As Boolean
                                 Return (i = cs.Length() AndAlso end_result(ec, accept_null_ec)) OrElse
                                        (i < cs.Length() AndAlso chain_result(ec, accept_null_ec))
                             End Function)
    End Function

    Public Shared Function as_predict(ByVal d As Func(Of Boolean)) As void(Of event_comb, Boolean, Boolean, Boolean)
        assert(d IsNot Nothing)
        Return Sub(ByRef ec As event_comb,
                   ByRef accept_null_ec As Boolean,
                   ByRef predict As Boolean,
                   ByRef asPredict As Boolean)
                   ec = Nothing
                   accept_null_ec = False
                   predict = d()
                   asPredict = True
               End Sub
    End Function

    Public Shared Function as_event_comb(ByVal d As Func(Of event_comb),
                                         Optional ByVal accept_null_ec As Boolean = False) _
                                        As void(Of event_comb, Boolean, Boolean, Boolean)
        assert(d IsNot Nothing)
        Return Sub(ByRef ec As event_comb,
                   ByRef ane As Boolean,
                   ByRef predict As Boolean,
                   ByRef asPredict As Boolean)
                   ec = d()
                   ane = accept_null_ec
                   predict = False
                   asPredict = False
               End Sub
    End Function

    Public Shared Function as_nullable_event_comb(ByVal d As Func(Of event_comb)) _
                                                 As void(Of event_comb, Boolean, Boolean, Boolean)
        Return as_event_comb(d, True)
    End Function

    Public Shared Function chain(ByVal before As Func(Of Boolean),
                                 ByVal cs As _do(Of Int32, event_comb, Boolean, Boolean),
                                 ByVal after As Func(Of Boolean)) As event_comb
        assert(before IsNot Nothing)
        assert(cs IsNot Nothing)
        assert(after IsNot Nothing)
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Dim accept_null_ec As Boolean = False
        Return New event_comb(Function() As Boolean
                                  If before() Then
                                      Return goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                             Function() As Boolean
                                 If cs(i, ec, accept_null_ec) Then
                                     i += 1
                                     Return chain_waitfor(ec, accept_null_ec)
                                 Else
                                     assert(i > 0)
                                     Return end_result(ec, accept_null_ec) AndAlso after()
                                 End If
                             End Function,
                             Function() As Boolean
                                 Return chain_result(ec, accept_null_ec)
                             End Function)
    End Function

    Private Shared ReadOnly t As Func(Of Boolean) =
        Function() As Boolean
            Return True
        End Function

    Public Shared Function chain(ByVal cs As _do(Of Int32, event_comb, Boolean, Boolean)) As event_comb
        Return chain(t, cs, t)
    End Function

    Private Shared Function chain_response(Of T)(ByVal cs() As T,
                                                 ByVal i As Int32,
                                                 ByRef ec As event_comb,
                                                 ByRef accept_null_ec As Boolean,
                                                 ByVal d As void(Of Int32, event_comb, Boolean)) As Boolean
        assert(cs IsNot Nothing AndAlso cs.Length() > 0)
        assert(d IsNot Nothing)
        If i = cs.Length() Then
            Return False
        Else
            d(i, ec, accept_null_ec)
            Return True
        End If
    End Function

    Private Shared Function chain(Of T)(ByVal cs() As T,
                                        ByVal before As Func(Of Boolean),
                                        ByVal c As _do(Of Int32, event_comb),
                                        ByVal after As Func(Of Boolean),
                                        ByVal accept_null_ec As _do(Of Int32, Boolean)) As event_comb
        assert(cs IsNot Nothing AndAlso cs.Length() > 0)
        assert(c IsNot Nothing)
        assert(accept_null_ec IsNot Nothing)
        Return chain(before,
                     Function(ByRef i As Int32, ByRef ec As event_comb, ByRef ane As Boolean) As Boolean
                         Return chain_response(cs, i, ec, ane,
                                               Sub(ByRef j As Int32, ByRef e As event_comb, ByRef a As Boolean)
                                                   e = c(j)
                                                   a = accept_null_ec(j)
                                               End Sub)
                     End Function,
                     after)
    End Function

    Private Shared Function chain(Of T)(ByVal cs() As T,
                                        ByVal c As _do(Of Int32, event_comb),
                                        ByVal accept_null_ec As _do(Of Int32, Boolean)) As event_comb
        Return chain(cs, event_comb.t, c, event_comb.t, accept_null_ec)
    End Function

    Public Shared Function chain(ByVal before As Func(Of Boolean),
                                 ByVal cs() As _do(Of event_comb, Boolean),
                                 ByVal after As Func(Of Boolean)) As event_comb
        assert(cs IsNot Nothing AndAlso cs.Length() > 0)
        Return chain(before,
                     Function(ByRef i As Int32, ByRef ec As event_comb, ByRef accept_null_ec As Boolean) As Boolean
                         Return chain_response(cs, i, ec, accept_null_ec,
                                               Sub(ByRef j As Int32, ByRef e As event_comb, ByRef ane As Boolean)
                                                   ane = cs(j)(e)
                                               End Sub)
                     End Function,
                     after)
    End Function

    Public Shared Function chain(ByVal before As Func(Of Boolean),
                                 ByVal after As Func(Of Boolean),
                                 ByVal ParamArray cs() As _do(Of event_comb, Boolean)) As event_comb
        Return chain(before, cs, after)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As _do(Of event_comb, Boolean)) As event_comb
        Return chain(t, t, cs)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As pair(Of Func(Of event_comb), Boolean)) As event_comb
        Return chain(cs,
                     Function(ByRef i As Int32) As event_comb
                         Return cs(i).first()
                     End Function,
                     Function(ByRef i As Int32) As Boolean
                         Return cs(i).second
                     End Function)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As void(Of event_comb, Boolean)) As event_comb
        assert(cs IsNot Nothing AndAlso cs.Length() > 0)
        Return chain(Function(ByRef i As Int32, ByRef ec As event_comb, ByRef accept_null_ec As Boolean) As Boolean
                         Return chain_response(cs, i, ec, accept_null_ec,
                                               Sub(ByRef j As Int32, ByRef e As event_comb, ByRef a As Boolean)
                                                   cs(j)(e, a)
                                               End Sub)
                     End Function)
    End Function

    Public Shared Function chain(ByVal accept_null_ec As Boolean,
                                 ByVal ParamArray cs() As Func(Of event_comb)) As event_comb
        Return chain(cs,
                     Function(ByRef i As Int32) As event_comb
                         Return cs(i)()
                     End Function,
                     Function(ByRef i As Int32) As Boolean
                         Return accept_null_ec
                     End Function)
    End Function

    Public Shared Function chain(ByVal before As Func(Of Boolean),
                                 ByVal after As Func(Of Boolean),
                                 ByVal accept_null_ec As Boolean,
                                 ByVal ParamArray cs() As Func(Of event_comb)) As event_comb
        Return chain(cs,
                     before,
                     Function(ByRef i As Int32) As event_comb
                         Return cs(i)()
                     End Function,
                     after,
                     Function(ByRef i As Int32) As Boolean
                         Return accept_null_ec
                     End Function)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As Func(Of event_comb)) As event_comb
        Return chain(False, cs)
    End Function

    Public Shared Function chain(ByVal accept_null_ec As Boolean,
                                 ByVal ParamArray cs() As event_comb) As event_comb
        Return chain(cs,
                     Function(ByRef i As Int32) As event_comb
                         Return cs(i)
                     End Function,
                     Function(ByRef i As Int32) As Boolean
                         Return accept_null_ec
                     End Function)
    End Function

    Public Shared Function chain(ByVal ParamArray cs() As event_comb) As event_comb
        Return chain(False, cs)
    End Function
End Class
