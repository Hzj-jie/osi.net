
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.procedure

' The legacy implementation of module to support returning an event_comb with a boolean value to consume the context.
Public MustInherit Class procedure_module
    Implements module_handle.module

    Protected MustOverride Function process(ByVal context As server.context, ByRef ec As event_comb) As Boolean

    Public Function context_received(ByVal context As server.context) As Boolean _
                                    Implements module_handle.module.context_received
        Return procedure_handle.process_context(context, AddressOf process)
    End Function
End Class
