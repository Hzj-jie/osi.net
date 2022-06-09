
Imports osi.root.connector
Imports osi.service.configuration

Public Module _config
    Public ReadOnly config As config

    Sub New()
        Dim config_file As String = "sider.ini"
        If Not My.Application().CommandLineArgs() Is Nothing AndAlso
           My.Application().CommandLineArgs().Count() > 0 AndAlso
           Not My.Application(.null_or_empty().CommandLineArgs()(0)) Then
            config_file = My.Application().CommandLineArgs()(0)
        End If
        raise_error("using configuration file ", config_file)
        assert_load(config_file)
        config = configuration.default(config_file)
    End Sub
End Module
