
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.lock

Public Module _async_model
    Public Function begin(ByVal action As callback_action) As Boolean
        Return callback_manager.global.begin(action)
    End Function

    Public Sub assert_begin(ByVal action As callback_action)
        assert(begin(action))
    End Sub

    Public Function begin(ByVal action As callback_action, ByVal timeout_ms As Int64) As Boolean
        Return callback_manager.global.begin(action, timeout_ms)
    End Function

    Public Sub assert_begin(ByVal action As callback_action, ByVal timeout_ms As Int64)
        assert(begin(action, timeout_ms))
    End Sub

    Public Function begin(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        Return event_driver.begin(ec, timeout_ms)
    End Function

    Public Function begin(ByVal ec As event_comb) As Boolean
        Return event_driver.begin(ec)
    End Function

    Public Sub assert_begin(ByVal ec As event_comb, ByVal timeout_ms As Int64)
        assert(begin(ec, timeout_ms))
    End Sub

    Public Sub assert_begin(ByVal ec As event_comb)
        assert(begin(ec))
    End Sub

    Public Sub begin_lifetime_event_comb(ByVal control As expiration_controller,
                                         ByVal ParamArray d() As Func(Of Boolean))
        assert_begin(lifetime_event_comb(control, d))
    End Sub

    Public Sub begin_application_lifetime_event_comb(ByVal ParamArray d() As Func(Of Boolean))
        assert_begin(application_lifetime_event_comb(d))
    End Sub
End Module
