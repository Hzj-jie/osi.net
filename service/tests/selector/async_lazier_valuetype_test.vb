
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector

Public Class async_lazier_valuetype_test
    Inherits [case]

    Private Shared Function run_case(Of T As _boolean)() As Boolean
        Const value As Int32 = 100
        Dim p As async_lazier(Of Int32, T) = Nothing
        p = New async_lazier(Of Int32, T)(Function(v As ref(Of Int32)) As event_comb
                                              Return sync_async(Sub()
                                                                    eva(v, value)
                                                                End Sub)
                                          End Function)
        p.wait_until_initialized()
        Dim r As Int32 = 0
        assert(p.get(r))
        assertion.equal(r, value)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of _true)() AndAlso
               run_case(Of _false)()
    End Function
End Class
