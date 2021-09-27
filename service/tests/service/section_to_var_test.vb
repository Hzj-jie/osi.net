
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.configuration
Imports config = osi.service.configuration.config

Public NotInheritable Class section_to_var_test
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
        assert_load(seconds_to_milliseconds(300), config_writer.file)
        Dim c As config = configuration.default(config_writer.file)
        Dim ss As vector(Of section) = c.sections("section")
        If assertion.is_not_null(ss) AndAlso
           assertion.equal(ss.size(), CUInt(3)) Then
            Dim v As var = Nothing
            v = New var(ss(0).values())
            assertion.is_true(v.other_values() Is Nothing OrElse v.other_values().empty())
            assertion.equal(v("key0"), "value0")
            assertion.equal(v("key1"), " value1")
            assertion.is_true(v.switch("A"))
            assertion.is_true(v.switch("B"))
            assertion.is_true(v.switch("C"))
            assertion.is_true(v.switch("DEF"))
            assertion.is_true(v.switch("GHI"))
            assertion.is_false(v.switch("D"))
            assertion.is_false(v.switch("E"))
            assertion.is_false(v.switch("F"))
            assertion.is_false(v.switch("G"))
            assertion.is_false(v.switch("H"))
            assertion.is_false(v.switch("I"))

            v = New var(ss(1).values())
            If assertion.is_false(v.other_values().null_or_empty()) AndAlso
               assertion.equal(v.other_values().size(), uint32_1) Then
                assertion.equal(v.other_values()(0), "value4")
            End If
            assertion.equal(v("key2"), "value2")
            assertion.equal(v("key3"), " value3")
            assertion.is_true(v.switch("a"))
            assertion.is_true(v.switch("b"))
            assertion.is_true(v.switch("c"))
            assertion.is_true(v.switch("def"))
            assertion.is_true(v.switch("ghi"))
            assertion.is_false(v.switch("d"))
            assertion.is_false(v.switch("e"))
            assertion.is_false(v.switch("f"))
            assertion.is_false(v.switch("g"))
            assertion.is_false(v.switch("h"))
            assertion.is_false(v.switch("i"))

            v = New var(ss(2).values())
            If assertion.is_not_null(v.other_values()) AndAlso
               assertion.is_false(v.other_values().empty()) AndAlso
               assertion.equal(v.other_values().size(), CUInt(3)) Then
                assertion.equal(v.other_values()(0), "value1 value2")
                assertion.equal(v.other_values()(1), "value3 value4")
                assertion.equal(v.other_values()(2), "value5")
            End If
        End If

        configuration.default.unload(config_writer.file)
        sleep()
        assertion.is_true(do_(Function() As Boolean
                                  File.Delete(config_writer.file)
                                  Return True
                              End Function,
                              False))
        Return True
    End Function
End Class
