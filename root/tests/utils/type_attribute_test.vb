
Imports osi.root.utils
Imports osi.root.utt

Public Class type_attribute_test
    Inherits [case]

    <type_attribute()>
    Private Class C1(Of T)
        Shared Sub New()
            type_attribute.of(Of C1(Of T)).set(GetType(T).FullName())
        End Sub
    End Class

    Private Shared Function run_case(Of T)() As Boolean
        Dim x As C1(Of T) = Nothing
        x = New C1(Of T)()
        assert_true(type_attribute.has(Of C1(Of T))())
        assert_equal(type_attribute.of(Of C1(Of T)).get(Of String)(), GetType(T).FullName())
        Dim y As Object = Nothing
        y = x
        assert_true(type_attribute.has(y))
        assert_equal(type_attribute.of(y).get(Of String)(), GetType(T).FullName())
        Return True
    End Function

    Private Class C2
    End Class

    <type_attribute()>
    Private Class C3
    End Class

    Private Shared Function no_attribute_cases() As Boolean
        assert_false(type_attribute.has(Of String)())
        assert_false(type_attribute.has(Of Int32)())
        assert_false(type_attribute.has(Of Int64)())
        assert_false(type_attribute.has(Of Boolean)())
        assert_false(type_attribute.has(Of Char)())
        assert_false(type_attribute.has(Of C2)())
        assert_false(type_attribute.has(Of C3)())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of Int32)() AndAlso
               run_case(Of Int64)() AndAlso
               run_case(Of String)() AndAlso
               run_case(Of C1(Of Boolean))() AndAlso
               run_case(Of type_attribute_test)() AndAlso
               no_attribute_cases()
    End Function
End Class
