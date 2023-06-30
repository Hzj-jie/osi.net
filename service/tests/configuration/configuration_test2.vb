
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.configuration
Imports c = osi.service.configuration
Imports envs = osi.root.envs

Public NotInheritable Class configuration_test2
    Inherits [case]

    Private ReadOnly config_file As String =
        Path.Combine(temp_folder, strcat(guid_str(), filesystem.extension_prefix, "ini"))

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            If Not test_config.sync_export(config_file) Then
                Return False
            End If
            Return c.default().load(config_file)
        Else
            Return False
        End If
    End Function

    Private Function config() As config
        Return c.default()(config_file)
    End Function

    Private NotInheritable Class variants_builder
        Private lang As String
        Private brand As String
        Private time As Int32
        Private percentage As Double
        Private name As String
        Private email As String
        Private user_name As String
        Private reverse_host As String
        Private path As String
        Private password As String
        Private processor_usage As Double

        Public Shared Function [New]() As variants_builder
            Return New variants_builder()
        End Function

        Public Sub New()
            time = -1
            percentage = -1
            processor_usage = -1
        End Sub

        Public Function with_lang(ByVal s As String) As variants_builder
            lang = s
            Return Me
        End Function

        Public Function with_brand(ByVal s As String) As variants_builder
            brand = s
            Return Me
        End Function

        Public Function with_time(ByVal t As Int32) As variants_builder
            time = t
            Return Me
        End Function

        Public Function with_percentage(ByVal p As Double) As variants_builder
            percentage = p
            Return Me
        End Function

        Public Function with_name(ByVal s As String) As variants_builder
            name = s
            Return Me
        End Function

        Public Function with_email(ByVal s As String) As variants_builder
            email = s
            Return Me
        End Function

        Public Function with_user_name(ByVal s As String) As variants_builder
            user_name = s
            Return Me
        End Function

        Public Function with_reverse_host(ByVal s As String) As variants_builder
            reverse_host = s
            Return Me
        End Function

        Public Function with_path(ByVal s As String) As variants_builder
            path = s
            Return Me
        End Function

        Public Function with_password(ByVal s As String) As variants_builder
            password = s
            Return Me
        End Function

        Public Function with_processor_usage(ByVal u As Double) As variants_builder
            processor_usage = u
            Return Me
        End Function

        Public Function build() As vector(Of pair(Of String, String))
            Dim r As vector(Of pair(Of String, String)) = Nothing
            r = New vector(Of pair(Of String, String))()
            If Not lang Is Nothing Then
                assert(append_variant(r, "lang", lang))
            End If
            If Not brand Is Nothing Then
                assert(append_variant(r, "brand", brand))
            End If
            If time <> -1 Then
                assert(append_variant(r, "time", time))
            End If
            If percentage <> -1 Then
                assert(append_variant(r, "percentage", percentage))
            End If
            If Not name Is Nothing Then
                assert(append_variant(r, "name", name))
            End If
            If Not email Is Nothing Then
                assert(append_variant(r, "email", email))
            End If
            If Not user_name Is Nothing Then
                assert(append_variant(r, "user-name", user_name))
            End If
            If Not reverse_host Is Nothing Then
                assert(append_variant(r, "reverse-host", reverse_host))
            End If
            If Not path Is Nothing Then
                assert(append_variant(r, "path", path))
            End If
            If Not password Is Nothing Then
                assert(append_variant(r, "password", password))
            End If
            If processor_usage <> -1 Then
                assert(append_variant(r, "processor-usage", processor_usage))
            End If
            Return r
        End Function
    End Class

    Public Overrides Function run() As Boolean
        ' Test [build] section
        assertion.is_true(config()("build")(envs.build).to(Of Boolean)())
        For i As UInt32 = 0 To config()("build").keys().size() - uint32_1
            Dim key As String = Nothing
            key = config()("build").keys()(i)
            assertion.equal(config()("build")(key).to(Of Boolean)(), strsame(envs.build, key))
        Next

        ' Test [application_directory] section
        If String.Equals(envs.deploys.service_name, "Debug") OrElse strsame(envs.deploys.service_name, "Release") Then
            assertion.equal(config()("application_directory")("value"), envs.deploys.service_name)
        Else
            assertion.equal(config()("application_directory")("value"), "Unknown")
        End If

        ' Test [environment] section
        If strsame(envs.deploys.service_name, "Debug") OrElse strsame(envs.deploys.service_name, "Release") Then
            assertion.equal(config()("environment")("value"), "dev")
        Else
            assertion.equal(config()("environment")("value"), "utt-run")
        End If

        ' Test [section] section
        Dim variants As variants_builder = Nothing
        variants = variants_builder.[New]().
                                    with_lang("en").
                                    with_brand("lave").
                                    with_time(0).
                                    with_percentage(0.8).
                                    with_name("Jack").
                                    with_email("JackR@Lave.com").
                                    with_user_name("JackR").
                                    with_reverse_host("com.lave.www").
                                    with_path("search?q=blabla").
                                    with_password("thisisapassword").
                                    with_processor_usage(20)
        Dim v As vector(Of pair(Of String, String)) = Nothing
        v = variants.build()
        assertion.equal(config()("section", v)("welcome", v), "welcome")
        v = variants.with_lang("zh").build()
        assertion.equal(config()("section", v)("welcome", v), "欢迎")
        v = variants.with_lang("du").build()
        assertion.equal(config()("section", v)("welcome", v), "welkom")
        v = variants.with_lang("JA").build()
        assertion.equal(config()("section", v)("welcome", v), "yookoso")
        v = variants.with_lang("??").build()
        assertion.equal(config()("section", v)("welcome", v), "Welcome")

        v = variants.build()
        assertion.equal(config()("section", v)("title", v), "Lave Search")
        v = variants.with_brand("ding search").build()
        assertion.equal(config()("section", v)("title", v), "Ding Search")
        v = variants.with_brand("lave???").build()
        assertion.equal(config()("section", v)("title", v), "Lave Search")
        v = variants.with_brand("??").build()
        assertion.equal(config()("section", v)("title", v), "No Brand Search")

        v = variants.build()
        assertion.equal(config()("section", v)("lucky", v).to(Of Boolean)(), False)
        v = variants.with_time(1).build()
        assertion.equal(config()("section", v)("lucky", v).to(Of Boolean)(), True)
        v = variants.with_time(10).build()
        assertion.equal(config()("section", v)("lucky", v), "unknown")

        v = variants.build()
        assertion.equal(config()("section", v)("round", v).to(Of Int32)(), 1)
        v = variants.with_percentage(0.2).build()
        assertion.equal(config()("section", v)("round", v).to(Of Int32)(), 0)
        v = variants.with_percentage(10).build()
        assertion.equal(config()("section", v)("round", v).to(Of Int32)(), -1)

        v = variants.build()
        assertion.equal(config()("section", v)("gender", v), "male")
        v = variants.with_name("lucy").build()
        assertion.equal(config()("section", v)("gender", v), "female")
        v = variants.with_name("lucy,").build()
        assertion.equal(config()("section", v)("gender", v), "other")

        v = variants.build()
        assertion.equal(config()("section", v)("domain", v), "macrosoft.com")
        v = variants.with_email("JackR@jmail.com").build()
        assertion.equal(config()("section", v)("domain", v), "joojle.com")
        v = variants.with_email("JackR@mymail.com").build()
        assertion.equal(config()("section", v)("domain", v), "example.com")

        v = variants.build()
        assertion.equal(config()("section", v)("top50", v).to(Of Boolean)(), True)
        assertion.equal(config()("section", v)("bottom50", v).to(Of Boolean)(), False)
        v = variants.with_user_name("aaa").build()
        assertion.equal(config()("section", v)("top50", v).to(Of Boolean)(), True)
        assertion.equal(config()("section", v)("bottom50", v).to(Of Boolean)(), False)
        v = variants.with_user_name("zzz").build()
        assertion.equal(config()("section", v)("top50", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("bottom50", v).to(Of Boolean)(), True)
        v = variants.with_user_name("mzzzzzzzzzzzzzzzzzzzz").build()
        assertion.equal(config()("section", v)("top50", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("bottom50", v).to(Of Boolean)(), False)

        v = variants.build()
        assertion.equal(config()("section", v)("priority", v).to(Of Int32)(), 2)
        v = variants.with_reverse_host("com.joojle.www").build()
        assertion.equal(config()("section", v)("priority", v).to(Of Int32)(), 1)
        v = variants.with_reverse_host("com.inlook.wap").build()
        assertion.equal(config()("section", v)("priority", v).to(Of Int32)(), 3)
        v = variants.with_reverse_host("com.unknown.3g").build()
        assertion.equal(config()("section", v)("priority", v).to(Of Int32)(), 99999)

        v = variants.build()
        assertion.equal(config()("section", v)("search", v).to(Of Boolean)(), True)
        v = variants.with_path("s?lang=en&q=foooooo").build()
        assertion.equal(config()("section", v)("search", v).to(Of Boolean)(), True)
        v = variants.with_path("SEARCH?lang=en&q=foooooo").build()
        assertion.equal(config()("section", v)("search", v).to(Of Boolean)(), True)
        v = variants.with_path("lookup?lang=en&q=foooooo").build()
        assertion.equal(config()("section", v)("search", v).to(Of Boolean)(), False)

        v = variants.build()
        assertion.equal(config()("section", v)("encrypted", v).to(Of Boolean)(), False)
        v = variants.with_password("ThisIsAPassword").build()
        assertion.equal(config()("section", v)("encrypted", v).to(Of Boolean)(), True)

        v = variants.build()
        assertion.equal(config()("section", v)("restricted", v).to(Of Boolean)(), False)
        v = variants.with_processor_usage(100).build()
        assertion.equal(config()("section", v)("restricted", v).to(Of Boolean)(), True)

        v = variants.with_lang("en").
                     with_brand("Lave Search").
                     with_path("search?q=blabla").
                     with_reverse_host("com.lave.www").
                     build()
        assertion.equal(config()("section", v)("message", v), "Welcome to Use Lave Search (http://www.lave.com)")
        v = variants.with_path("lookup").build()
        assertion.equal(config()("section", v)("message", v), "Welcome to Use Lave (http://www.lave.com)")
        v = variants.with_reverse_host("com.ding.www").build()
        assertion.equal(config()("section", v)("message", v), "Welcome to Use Lave")
        v = variants.with_lang("zh").build()
        assertion.equal(config()("section", v)("message", v), "欢迎使用Lave")

        v = variants.with_email("annoymous@annoymous.com").
                     with_path("download?file=a-very-small-file.txt").
                     with_reverse_host("com.annoymous.www").
                     build()
        assertion.equal(config()("section", v)("welcome", v), "Not Welcome")
        assertion.equal(config()("section", v)("title", v), "Not Welcome")
        assertion.equal(config()("section", v)("lucky", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("round", v).to(Of Int32)(), -1)
        assertion.equal(config()("section", v)("gender", v), "other")
        assertion.equal(config()("section", v)("domain", v), "annoymous.com")
        assertion.equal(config()("section", v)("top50", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("bottom50", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("priority", v).to(Of Int32)(), 99999999)
        assertion.equal(config()("section", v)("search", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("encrypted", v).to(Of Boolean)(), False)
        assertion.equal(config()("section", v)("restricted", v).to(Of Boolean)(), True)
        assertion.equal(config()("section", v)("message", v), "You Are Not Welcomed To Use Our Service")
        assertion.equal(config()("section", v)("message-head", v), "Yes?")

        assertion.equal(config()("section", v)("unknown-key", v), default_str)
        assertion.equal(config()("section", v)("unknown-key2", v), default_str)

        Return True
    End Function

    Public Overrides Function finish() As Boolean
        c.default().unload(config_file)
        sleep()
        Return do_(Function() As Boolean
                       File.Delete(config_file)
                       Return True
                   End Function,
                   False) AndAlso
               MyBase.finish()
    End Function
End Class
