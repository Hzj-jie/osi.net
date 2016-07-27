
Imports osi.root.connector
Imports osi.root.procedure

Public Class multi_procedure_case_wrapper
    Inherits event_comb_case_wrapper

    Private ReadOnly pc As Int32 = 0

    Public Sub New(ByVal c As event_comb_case, Optional ByVal procedure_count As Int32 = 8)
        MyBase.New(c)
        pc = procedure_count
    End Sub

    Protected Overridable Function procedure_count() As Int32
        Return pc
    End Function

    Public NotOverridable Overrides Function create() As event_comb
        assert(procedure_count() > 1)
        Dim ecs() As event_comb = Nothing
        ReDim ecs(procedure_count() - 1)
        Return New event_comb(Function() As Boolean
                                  For i As Int32 = 0 To procedure_count() - 1
                                      ecs(i) = MyBase.create()
                                      If Not waitfor(ecs(i)) Then
                                          Return False
                                      End If
                                  Next
                                  Return goto_next()
                              End Function,
                              Function() As Boolean
                                  For i As Int32 = 0 To procedure_count() - 1
                                      If Not ecs(i).end_result() Then
                                          Return False
                                      End If
                                  Next
                                  Return goto_end()
                              End Function)
    End Function
End Class
