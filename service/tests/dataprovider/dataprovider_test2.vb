
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.dataprovider

<test>
Public NotInheritable Class dataprovider_test2
    <test>
    Private Shared Sub name_test()
        Dim n As String = Nothing
        n = test_dataprovider.name("a-file-name",
                                   test_dataprovider.parameter("key1", 1),
                                   test_dataprovider.parameter("key2", "value2"),
                                   test_dataprovider.parameter("key3", False),
                                   test_dataprovider.parameter("key4", test_enum.enum1))
        assert_true(strcontains(n, "a-file-name"))
        assert_true(strcontains(n, "key1"))
        assert_true(strcontains(n, "1"))
        assert_true(strcontains(n, "key2"))
        assert_true(strcontains(n, "value2"))
        assert_true(strcontains(n, "key3"))
        assert_true(strcontains(n, "False"))
        assert_true(strcontains(n, "key4"))
        assert_true(strcontains(n, "enum1"))
        assert_false(strcontains(n, "key5"))
    End Sub

    Private Sub New()
    End Sub

    Private Enum test_enum
        enum1
        enum2
    End Enum

    Private NotInheritable Class test_dataprovider
        Inherits dataprovider(Of Byte)

        Private Sub New()
            MyBase.New(Nothing, Nothing, Nothing)
        End Sub

        Public Shared Shadows Function name(ByVal file As String, ByVal ParamArray p() As pair(Of String, Object)) As String
            Return dataprovider(Of Byte).name(GetType(test_dataprovider), file, p)
        End Function

        Public Shared Shadows Function parameter(ByVal name As String, ByVal value As Object) As pair(Of String, Object)
            Return dataprovider(Of Byte).parameter(name, value)
        End Function
    End Class
End Class
