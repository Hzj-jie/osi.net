
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.service.streamer

<global_init(global_init_level.test)>
Friend Module _flower_int32_initializer
    Sub New()
        flower(Of Int32).register_is_eos(Function(ByVal i As Int32) As Boolean
                                             Return i = max_int32
                                         End Function)
    End Sub

    Private Sub init()
    End Sub
End Module
