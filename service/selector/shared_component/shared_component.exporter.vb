﻿
Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Interface exporter(Of IMPL_T As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T))
        Event new_shared_component_exported(ByVal d As IMPL_T)
    End Interface
End Class
