
Imports osi.root.connector
Imports osi.service.tcp

Public Module remote_console
    Public Sub main()
        Dim pp As powerpoint = powerpoint.create(osi.service.argument.var.application)
        If argument.server Then
            server.run(pp)
        Else
            client.run(pp)
        End If
        garbage_collector.trigger()
    End Sub
End Module
