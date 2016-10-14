
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.storage

Public Class redundance_distributor_case3_test
    Inherits redundance_distributor_test

    Public Sub New()
        MyBase.New(New default_istrkeyvt_case3())
    End Sub
End Class

Public Class redundance_distributor_test
    Inherits istrkeyvt_case

    Private ReadOnly istrkeyvt1_name As String
    Private ReadOnly istrkeyvt2_name As String

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
        istrkeyvt1_name = guid_str()
        istrkeyvt2_name = guid_str()
    End Sub

    Public Sub New()
        Me.New(New fast_istrkeyvt_case2())
    End Sub

    Private Function register_istrkeyvt(ByVal n As String, ByVal broken As Boolean) As Boolean
        Dim i As istrkeyvt = Nothing
        i = memory.ctor()
        Return assert_not_nothing(i) AndAlso
               assert_true(manager.register(n, If(broken, New broken_istrkeyvt(i), i)))
    End Function

    Protected Overrides Function clean_up(ByVal i As istrkeyvt) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  assert_true(manager.erase(istrkeyvt1_name, [default](Of istrkeyvt).null))
                                  assert_true(manager.erase(istrkeyvt2_name, [default](Of istrkeyvt).null))
                                  ec = MyBase.clean_up(i)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        If register_istrkeyvt(istrkeyvt1_name, False) AndAlso
           register_istrkeyvt(istrkeyvt2_name, True) Then
            Return redundance_distributor.ctor(istrkeyvt1_name, istrkeyvt2_name)
        Else
            Return Nothing
        End If
    End Function
End Class
