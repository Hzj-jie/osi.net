
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.connector
Imports osi.service.sharedtransmitter

Partial Public Class sharedtransmitter_test
    Private NotInheritable Class collection
        Inherits sharedtransmitter(Of Byte, Byte, component, Int32, parameter).
                 collection(Of _max_uint16, _byte_to_uint32, functor)
    End Class
End Class
