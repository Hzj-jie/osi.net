
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.connector
Imports osi.service.selector

Partial Public Class shared_component_test
    Private NotInheritable Class collection
        Inherits shared_component(Of Byte, Byte, component, Int32, parameter).
                 collection(Of _max_uint16, _byte_to_uint32, functor)
    End Class
End Class
