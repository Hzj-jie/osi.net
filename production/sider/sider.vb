
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.threadpool
Imports osi.service.device

Public Module sider
    Sub New()
        debugpause()
        global_init.execute()
    End Sub

    Public Sub main()
        init()
        garbage_collector.trigger()
    End Sub
End Module
