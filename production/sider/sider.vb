
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.threadpool
Imports osi.service.device

Public Module sider
    Sub New()
        debugpause()
        global_init.execute(load_assemblies:=True)
    End Sub

    Public Sub main()
        init()
        gc_trigger()
    End Sub
End Module
