
Imports osi.root.connector
Imports osi.service.configuration

Public Module _config
    Public ReadOnly config As config

    Sub New()
        Dim config_file As String = "sider.ini"
        Dim args() As String = Environment.GetCommandLineArgs()
        If array_size(args) > 1 AndAlso Not args(1).null_or_empty() Then
            config_file = args(1)
        End If
        raise_error("using configuration file ", config_file)
        assert_load(config_file)
        config = configuration.default(config_file)
    End Sub
End Module
