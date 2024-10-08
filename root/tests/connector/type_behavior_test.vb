
Imports osi.root.formation
Imports osi.root.utt

Public Class type_behavior_test
    Inherits [case]

    Private Class C(Of T)
    End Class

    Public Overrides Function run() As Boolean
        assertion.is_true(GetType(C(Of Int32)).FullName().Contains(GetType(Int32).Name()))
        assertion.is_true(GetType(C(Of String)).FullName().Contains(GetType(String).Name()))
        assertion.is_true(GetType(C(Of C(Of Int32))).FullName().Contains(GetType(C(Of Int32)).Name()))
        assertion.is_true(GetType(vector(Of Int32)).FullName().Contains(GetType(Int32).Name()))
        assertion.is_true(GetType(vector(Of String)).FullName().Contains(GetType(String).Name()))
        assertion.is_true(GetType(vector(Of C(Of Int32))).FullName().Contains(GetType(C(Of Int32)).Name()))

        assertion.equal(GetType(C(Of Int32)).GUID(), GetType(C(Of String)).GUID())
        assertion.equal(GetType(vector(Of Int32)).GUID(), GetType(vector(Of String)).GUID())
        assertion.equal(GetType(vector(Of C(Of Int32))).GUID(), GetType(vector(Of C(Of String))).GUID())

        assertion.is_false(GetType(C(Of Int32)) Is GetType(C(Of )))
        assertion.is_false(GetType(C(Of )) Is GetType(C(Of Int32)))
        Return True
    End Function
End Class
