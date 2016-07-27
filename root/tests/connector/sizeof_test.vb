
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.utt

Public Class sizeof_test
    Inherits [case]

    Private Structure s1
    End Structure

    Private Structure s2
        Public a As Int32
        Public b As String
        Public c As Object
    End Structure

    Private Class c1
    End Class

    Private Class c2
        Public a As Int32
        Public b As String
        Public c As Object
    End Class

    Public Overrides Function run() As Boolean
        assert_equal(sizeof("abc"), npos)
        assert_equal(sizeof(Of String)(), npos)
        assert_equal(sizeof(1), 32 \ bit_count_in_byte)
        assert_equal(sizeof(Of Int32)(), 32 \ bit_count_in_byte)
        assert_equal(sizeof(Of s1)(), 1)
        assert_equal(sizeof(New s1()), 1)
        assert_equal(sizeof(Of s2)(), 3 * cpu_address_width \ bit_count_in_byte)
        assert_equal(sizeof(New s2()), 3 * cpu_address_width \ bit_count_in_byte)
        assert_equal(sizeof(Of c1)(), npos)
        assert_equal(sizeof(New c1()), npos)
        assert_equal(sizeof(Of c2)(), npos)
        assert_equal(sizeof(New c2()), npos)
        Return True
    End Function
End Class
