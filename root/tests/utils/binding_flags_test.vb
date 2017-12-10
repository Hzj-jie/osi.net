
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class binding_flags_test
    <test>
    Private Shared Sub return_default()
        Dim bf As BindingFlags = Nothing
        assert_true(bf.from_str(default_str))
        assert_equal(bf, BindingFlags.Default)

        bf = Nothing
        assert_true(bf.from_str(""))
        assert_equal(bf, BindingFlags.Default)

        bf = Nothing
        assert_true(bf.from_str("|,    " + character.tab))
        assert_equal(bf, BindingFlags.Default)

        bf = Nothing
        assert_true(bf.method_from_str(default_str))
        assert_equal(bf, BindingFlags.Default Or BindingFlags.InvokeMethod)

        bf = Nothing
        assert_true(bf.method_from_str(""))
        assert_equal(bf, BindingFlags.Default Or BindingFlags.InvokeMethod)

        bf = Nothing
        assert_true(bf.method_from_str("|,"))
        assert_equal(bf, BindingFlags.Default Or BindingFlags.InvokeMethod)
    End Sub

    <test>
    Private Shared Sub should_fail()
        Dim bf As BindingFlags = Nothing
        assert_false(bf.from_str("what's this"))
        assert_false(bf.from_str("private|public?"))
    End Sub

    <test>
    Private Shared Sub predefined_cases()
        Dim bf As BindingFlags = Nothing
        assert_true(bf.from_str("private"))
        assert_equal(bf, BindingFlags.NonPublic)

        bf = Nothing
        assert_true(bf.from_str("protected"))
        assert_equal(bf, BindingFlags.NonPublic)
    End Sub

    <test>
    <repeat(1000)>
    Private Shared Sub random_case()
        Dim separators() As String = Nothing
        separators = strsplitter.with_default_separators(character.sheffer, character.comma)
        Dim random_separator As Func(Of String) = Function() As String
                                                      Return separators(rnd_int(0, array_size_i(separators)))
                                                  End Function
        Dim bf As BindingFlags = Nothing
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        assert(enum_traversal(Of BindingFlags)(Sub(x As BindingFlags, y As String)
                                                   If rnd_bool() Then
                                                       bf = bf Or x
                                                       s.Append(random_separator())
                                                       If rnd_bool() Then
                                                           s.Append(random_separator())
                                                       End If
                                                       s.Append(y)
                                                       If rnd_bool() Then
                                                           s.Append(random_separator())
                                                       End If
                                                   End If
                                               End Sub))
        Dim nbf As BindingFlags = Nothing
        assert_true(nbf.from_str(Convert.ToString(s)))
        assert_equal(nbf, bf)

        nbf = Nothing
        assert_true(nbf.method_from_str(Convert.ToString(s)))
        assert_equal(nbf, bf Or BindingFlags.InvokeMethod)
    End Sub

    Private Sub New()
    End Sub
End Class
