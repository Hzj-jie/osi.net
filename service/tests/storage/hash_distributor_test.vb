
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.storage

Public NotInheritable Class hash_distributor_test
    Inherits istrkeyvt_case

    Private Const strkeyvt_count As Int32 = 16
    Private ReadOnly istrkeyvt_name_base As String

    Public Sub New()
        MyBase.New()
        istrkeyvt_name_base = guid_str()
    End Sub

    Private Function istrkeyvt_name(ByVal i As Int32) As String
        Return strcat(istrkeyvt_name_base, i)
    End Function

    Protected Overrides Function create_istrkeyvt(ByVal p As ref(Of istrkeyvt)) As event_comb
        Dim ecs() As event_comb = Nothing
        Dim fs() As ref(Of istrkeyvt) = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim ecs(strkeyvt_count - 1)
                                  ReDim fs(strkeyvt_count - 1)
                                  For i As Int32 = 0 To strkeyvt_count - 1
                                      fs(i) = New ref(Of istrkeyvt)()
                                      ecs(i) = fces.memory_fces(fs(i))
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ecs.end_result()) Then
                                      Dim names() As String = Nothing
                                      ReDim names(strkeyvt_count - 1)
                                      For i As Int32 = 0 To strkeyvt_count - 1
                                          names(i) = istrkeyvt_name(i)
                                          assertion.is_true(manager.register(names(i), +(fs(i))))
                                      Next
                                      Return eva(p, New hash_distributor(names)) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Protected Overrides Function clean_up() As event_comb
        Return sync_async(Sub()
                              For i As Int32 = 0 To strkeyvt_count - 1
                                  assertion.is_true(manager.erase(istrkeyvt_name(i), [default](Of istrkeyvt).null))
                              Next
                          End Sub)
    End Function
End Class
