
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils

Public Module _lifetime_event_comb
    Public Function lifetime_event_comb(ByVal stop_result As Boolean,
                                        ByVal control As expiration_controller,
                                        ByVal ParamArray d() As Func(Of Boolean)) As event_comb
        assert(Not control Is Nothing)
        Dim d2() As Func(Of Boolean) = Nothing
        ReDim d2(array_size_i(d) - 1)
        For i As Int32 = 0 To array_size_i(d) - 1
            Dim j As Int32 = 0
            j = i
            d2(i) = Function() As Boolean
                        If control.expired() Then
                            Return stop_result AndAlso
                                   goto_end()
                        Else
                            Return d(j)()
                        End If
                    End Function
        Next
        Return New event_comb(d2)
    End Function

    Public Function success_lifetime_event_comb(ByVal control As expiration_controller,
                                                ByVal ParamArray d() As Func(Of Boolean)) As event_comb
        Return lifetime_event_comb(True, control, d)
    End Function

    Public Function lifetime_event_comb(ByVal control As expiration_controller,
                                        ByVal ParamArray d() As Func(Of Boolean)) As event_comb
        Return lifetime_event_comb(False, control, d)
    End Function

    Public Function application_lifetime_event_comb(ByVal ParamArray d() As Func(Of Boolean)) As event_comb
        Return lifetime_event_comb(True, application_lifetime.expiration_controller(), d)
    End Function
End Module
