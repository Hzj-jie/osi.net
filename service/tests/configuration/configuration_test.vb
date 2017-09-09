
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.configuration
Imports osi.service.convertor
Imports envs = osi.root.envs
Imports c = osi.service.configuration

Public Class configuration_test
    Inherits [case]

    Private Class config_writer
        Public Shared ReadOnly file As String
        Private Shared write_times As Int32 = 0

        Shared Sub New()
            file = strcat(guid_str(), ".ini")
        End Sub

        Public Shared Sub write()
            Using o As TextWriter = New StreamWriter(file)
                o.WriteLine("spf=str-pat")
                o.WriteLine("spcf=str-pat-cache")
                o.WriteLine("sf=string")
                o.WriteLine("iif=int-int")
                o.WriteLine("ssf=str-ser")
                o.WriteLine("dcf=dbl-com")

                o.WriteLine("[section]")
                o.WriteLine("; try static filters first")
                o.WriteLine("b:unknownbuild$buildmode=unknownbuild")
                o.WriteLine("b:debugbuild$buildmode=debugbuild")
                o.WriteLine("b:releasebuild$buildmode=releasebuild")

                'it's a design issue, the >= and <= cannot work in default characters setting
                o.WriteLine("; try static filters and int_compare_selector")
                o.WriteLine("pc:>7$machine_type=great")
                o.WriteLine("pc:>3$machine_type=powerful")
                o.WriteLine("pc:>2$machine_type=strange")
                o.WriteLine("pc:>1$machine_type=normal")
                o.WriteLine("pc:<2$machine_type=lowend")

                'the string pattern match should be covered by match_pattern_test
                o.WriteLine("key1@spf:*p*a*t*t*e*r*n*1*=pattern1")
                o.WriteLine("key1@spf:*p*a*t*t*e*r*n*2*=pattern2")
                o.WriteLine("key1@spf:*p*a*t*t*e*r*n*3*=pattern3")
                o.WriteLine("key1=key1 no match")

                o.WriteLine("key2@spcf:*p*a*t*t*e*r*n*1*=pattern1")
                o.WriteLine("key2@spcf:*p*a*t*t*e*r*n*2*=pattern2")
                o.WriteLine("key2@spcf:*p*a*t*t*e*r*n*3*=pattern3")
                o.WriteLine("key2=key2 no match")

                o.WriteLine("key3@sf:string1=string1")
                o.WriteLine("key3@sf:string2=string2")
                o.WriteLine("key3@sf:string3=string3")
                o.WriteLine("key3=key3 no match")

                o.WriteLine("key4@iif:[0,100)=100")
                o.WriteLine("key4@iif:[100,200)=200")
                o.WriteLine("key4@iif:[200,300)=300")
                o.WriteLine("key4=400")

                o.WriteLine("key5@ssf:string1,string2,string3=string1,string2,string3")
                o.WriteLine("key5@ssf:string4,string5,string6=string4,string5,string6")
                o.WriteLine("key5@ssf:string7,string8,string9=string7,string8,string9")
                o.WriteLine("key5=string10")

                o.WriteLine("key6@dcf:>0&dcf:<100.1=100.1")
                o.WriteLine("key6@dcf:>100.1&dcf:<200.2=200.2")
                o.WriteLine("key6@dcf:>200.2&dcf:<300.3=300.3")
                o.WriteLine("key6@dcf:>300.3&dcf:<400.4=400.4")
                o.WriteLine("key6=500.5")

                o.WriteLine("write_times=" + Convert.ToString(_inc(write_times)))
            End Using
        End Sub
    End Class

    Private Shared Function machine_type() As String
        If Environment.ProcessorCount() > 7 Then
            Return "great"
        ElseIf Environment.ProcessorCount() > 3 Then
            Return "powerful"
        ElseIf Environment.ProcessorCount() > 2 Then
            Return "strange"
        ElseIf Environment.ProcessorCount() > 1 Then
            Return "normal"
        Else
            Return "lowend"
        End If
    End Function

    Private Shared Function s() As sections
        Return c.default()(config_writer.file)("section")
    End Function

    Public Overrides Function run() As Boolean
        config_writer.write()
        assert_load(seconds_to_milliseconds(100), config_writer.file)

        assert_equal(s()("buildmode"), envs.build)

        assert_equal(s()("machine_type"), machine_type())

        assert_equal(s()("key1", create_variants("spf", "!pattern1")), "pattern1")
        assert_equal(s()("key1", create_variants("spf", "p!attern2")), "pattern2")
        assert_equal(s()("key1", create_variants("spf", "pa!ttern3")), "pattern3")
        assert_equal(s()("key1", create_variants("spf", "pattern4")), "key1 no match")

        assert_equal(s()("key2", create_variants("spcf", "!pattern1")), "pattern1")
        assert_equal(s()("key2", create_variants("spcf", "p!attern2")), "pattern2")
        assert_equal(s()("key2", create_variants("spcf", "pa!ttern3")), "pattern3")
        assert_equal(s()("key2", create_variants("spcf", "pattern4")), "key2 no match")

        assert_equal(s()("key3", create_variants("sf", "string1")), "string1")
        assert_equal(s()("key3", create_variants("sf", "string2")), "string2")
        assert_equal(s()("key3", create_variants("sf", "string3")), "string3")
        assert_equal(s()("key3", create_variants("sf", "string4")), "key3 no match")

        assert_equal(s()("key4", create_variants("iif", rnd_int(0, 100))).to_int32(), 100)
        assert_equal(s()("key4", create_variants("iif", rnd_int(100, 200))).to_int32(), 200)
        assert_equal(s()("key4", create_variants("iif", rnd_int(200, 300))).to_int32(), 300)
        assert_equal(s()("key4", create_variants("iif", rnd_int(300, 400))).to_int32(), 400)

        assert_equal(s()("key5", create_variants("ssf", "string1")), "string1,string2,string3")
        assert_equal(s()("key5", create_variants("ssf", "string2")), "string1,string2,string3")
        assert_equal(s()("key5", create_variants("ssf", "string3")), "string1,string2,string3")
        assert_equal(s()("key5", create_variants("ssf", "string4")), "string4,string5,string6")
        assert_equal(s()("key5", create_variants("ssf", "string5")), "string4,string5,string6")
        assert_equal(s()("key5", create_variants("ssf", "string6")), "string4,string5,string6")
        assert_equal(s()("key5", create_variants("ssf", "string7")), "string7,string8,string9")
        assert_equal(s()("key5", create_variants("ssf", "string8")), "string7,string8,string9")
        assert_equal(s()("key5", create_variants("ssf", "string9")), "string7,string8,string9")
        assert_equal(s()("key5", create_variants("ssf", "string10")), "string10")

        assert_equal(s()("key6", create_variants("dcf", rnd_double(0, 100) + 0.1)).to_double(), 100.1)
        assert_equal(s()("key6", create_variants("dcf", rnd_double(100.1, 200.1) + 0.1)).to_double(), 200.2)
        assert_equal(s()("key6", create_variants("dcf", rnd_double(200.2, 300.2) + 0.1)).to_double(), 300.3)
        assert_equal(s()("key6", create_variants("dcf", rnd_double(300.3, 400.3) + 0.1)).to_double(), 400.4)
        assert_equal(s()("key6", create_variants("dcf", rnd_double(400.4, 500.4))).to_double(), 500.5)
        assert_equal(s()("key6", create_variants("dcf", rnd_double(-100, -1))).to_double(), 500.5)
        assert_equal(s()("key6", create_variants("dcf", 0)).to_double(), 500.5)
        assert_equal(s()("key6", create_variants("dcf", 100.1)).to_double(), 500.5)
        assert_equal(s()("key6", create_variants("dcf", 200.2)).to_double(), 500.5)
        assert_equal(s()("key6", create_variants("dcf", 300.3)).to_double(), 500.5)

        assert_equal(s()("write_times").to_int32(), 1)

        config_writer.write()
        sleep_seconds(10)
        assert_equal(s()("write_times").to_int32(), 2)

        c.default().unload(config_writer.file)
        sleep()
        assert_true(do_(Function() As Boolean
                            IO.File.Delete(config_writer.file)
                            Return True
                        End Function,
                        False))
        Return True
    End Function
End Class
