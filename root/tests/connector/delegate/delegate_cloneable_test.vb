
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt

Public Class delegate_cloneable_test
    Inherits [case]

    Private Delegate Sub a_delegate()

    Public Overrides Function run() As Boolean
        assertion.is_true(type_info(Of Action).is_cloneable)
        assertion.is_true(type_info(Of Func(Of Int32)).is_cloneable)
        assertion.is_true(type_info(Of Action(Of Int32)).is_cloneable)
        assertion.is_true(type_info(Of Func(Of Int32, Double)).is_cloneable)
        assertion.is_true(type_info(Of _func(Of Int32, Double, Int32, Int32, String, Double)).is_cloneable)
        Return True
    End Function
End Class
