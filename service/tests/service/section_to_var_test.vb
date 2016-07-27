
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.configuration
Imports config = osi.service.configuration.config

Public Class section_to_var_test
    Inherits [case]

    Private Class config_writer
        Public Shared ReadOnly file As String

        Shared Sub New()
            file = strcat(guid_str(), ".ini")
        End Sub

        Public Shared Sub write()
            Using o As TextWriter = New StreamWriter(file)
                o.WriteLine("[section0]")
                o.WriteLine("key0=value0")
                o.WriteLine("key1= value1")
                o.WriteLine("-ABC")
                o.WriteLine("~DEF")
                o.WriteLine("~GHI")

                o.WriteLine("[section1]")
                o.WriteLine("key2=value2")
                o.WriteLine("key3= value3")
                o.WriteLine("-abc")
                o.WriteLine("~def")
                o.WriteLine("~ghi")
                o.WriteLine("value4")

                o.WriteLine("[section2]")
                o.WriteLine("value1 value2")
                o.WriteLine("value3 value4")
                o.WriteLine("value5")
            End Using
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        config_writer.write()
        assert_load(seconds_to_milliseconds(100), config_writer.file)
        Dim c As config = Nothing
        c = configuration.default(config_writer.file)
        Dim ss As vector(Of section) = Nothing
        ss = c.sections("section")
        If assert_not_nothing(ss) AndAlso
           assert_equal(ss.size(), CUInt(3)) Then
            Dim v As var = Nothing
            v = New var(ss(0).values())
            assert_true(v.other_values() Is Nothing OrElse v.other_values().empty())
            assert_equal(v("key0"), "value0")
            assert_equal(v("key1"), " value1")
            assert_true(v.switch("A"))
            assert_true(v.switch("B"))
            assert_true(v.switch("C"))
            assert_true(v.switch("DEF"))
            assert_true(v.switch("GHI"))
            assert_false(v.switch("D"))
            assert_false(v.switch("E"))
            assert_false(v.switch("F"))
            assert_false(v.switch("G"))
            assert_false(v.switch("H"))
            assert_false(v.switch("I"))

            v = New var(ss(1).values())
            If assert_false(v.other_values().null_or_empty()) AndAlso
               assert_equal(v.other_values().size(), uint32_1) Then
                assert_equal(v.other_values()(0), "value4")
            End If
            assert_equal(v("key2"), "value2")
            assert_equal(v("key3"), " value3")
            assert_true(v.switch("a"))
            assert_true(v.switch("b"))
            assert_true(v.switch("c"))
            assert_true(v.switch("def"))
            assert_true(v.switch("ghi"))
            assert_false(v.switch("d"))
            assert_false(v.switch("e"))
            assert_false(v.switch("f"))
            assert_false(v.switch("g"))
            assert_false(v.switch("h"))
            assert_false(v.switch("i"))

            v = New var(ss(2).values())
            If assert_not_nothing(v.other_values()) AndAlso
               assert_false(v.other_values().empty()) AndAlso
               assert_equal(v.other_values().size(), CUInt(3)) Then
                assert_equal(v.other_values()(0), "value1 value2")
                assert_equal(v.other_values()(1), "value3 value4")
                assert_equal(v.other_values()(2), "value5")
            End If
        End If

        configuration.default.unload(config_writer.file)
        sleep()
        assert_true(do_(Function() As Boolean
                            File.Delete(config_writer.file)
                            Return True
                        End Function,
                        False))
        Return True
    End Function
End Class
