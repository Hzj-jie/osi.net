
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Interface exporter(Of IMPL_T As sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T))
        Event new_sharedtransmitter_exported(ByVal d As IMPL_T)
    End Interface
End Class
