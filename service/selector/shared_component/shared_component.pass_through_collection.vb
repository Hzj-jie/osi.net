
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Private Class pass_through_collection
        Implements collection

        Private ReadOnly local_port As PORT_T
        Private ReadOnly component As ref_instance(Of COMPONENT_T)
        Private ReadOnly dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))

        Public Sub New(ByVal local_port As PORT_T,
                       ByVal component As ref_instance(Of COMPONENT_T),
                       ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))
            assert(Not local_port Is Nothing)
            assert(Not component Is Nothing)
            assert(Not dispenser Is Nothing)
            Me.local_port = local_port
            Me.component = component
            Me.dispenser = dispenser
        End Sub

        Public Function [New](ByVal p As PARAMETER_T,
                              ByRef local_port As PORT_T,
                              ByRef o As ref_instance(Of COMPONENT_T)) As Boolean Implements collection.New
            local_port = Me.local_port
            o = Me.component
            Return True
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByVal local_port As PORT_T,
                              ByVal i As ref_instance(Of COMPONENT_T),
                              ByRef dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) As Boolean _
                             Implements collection.New
            dispenser = Me.dispenser
            Return True
        End Function
    End Class
End Class
