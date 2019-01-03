
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class assert_test
    Inherits [case]

    Private Shared Function assert_return_case() As Boolean
        Dim i As Int32 = 0
        assertion.equal(assert_return(eva(i, 100), i), 100)
        Dim o As Object = Nothing
        assertion.is_not_null(assert_return(eva(o, New Object()), o))
        Dim x As Object = Nothing
        x = assert_return(eva(o, New Object()), o)
        assertion.is_not_null(x)
        assertion.reference_equal(x, o)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return assert_return_case()
    End Function
End Class
