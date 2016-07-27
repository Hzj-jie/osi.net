
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector

Public Class sync_lazier_valuetype_test
    Inherits [case]

    Private Shared Function run_case(Of T As _boolean)() As Boolean
        Const value As Int32 = 100
        Dim p As sync_lazier(Of Int32, T) = Nothing
        p = New sync_lazier(Of Int32, T)(Function() As Int32
                                             Return value
                                         End Function)
        Dim r As Int32 = 0
        r = p.get()
        assert_equal(r, value)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of _true)() AndAlso
               run_case(Of _false)()
    End Function
End Class
