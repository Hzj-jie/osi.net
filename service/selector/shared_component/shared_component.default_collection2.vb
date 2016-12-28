
Imports osi.root.template
Imports osi.root.formation

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class collection(Of _RESOURCE_COUNT As _int64,
                                           _PORT_TO_UINT32 As _to_uint32(Of PORT_T))
        Inherits collection(Of _RESOURCE_COUNT,
                               _PORT_TO_UINT32,
                               shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T))

        Protected NotOverridable Overrides Function new_shared_component(
                    ByVal p As PARAMETER_T,
                    ByVal local_port As PORT_T,
                    ByVal component As ref_instance(Of COMPONENT_T),
                    ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                    ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                    ByVal buff As DATA_T) _
                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
            Return shared_component.[New](p, local_port, component, dispenser, accepter, buff)
        End Function
    End Class
End Class
