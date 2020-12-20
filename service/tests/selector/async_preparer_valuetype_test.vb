
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector

Public Class async_preparer_valuetype_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const value As Int32 = 100
        Dim p As async_preparer(Of Int32) = Nothing
        p = New async_preparer(Of Int32)(Function(v As ref(Of Int32)) As event_comb
                                             Return sync_async(Sub()
                                                                   eva(v, value)
                                                               End Sub)
                                         End Function)
        p.wait_until_initialized()
        Dim r As Int32 = 0
        assertion.is_true(p.get(r))
        assertion.equal(r, value)
        Return True
    End Function
End Class
