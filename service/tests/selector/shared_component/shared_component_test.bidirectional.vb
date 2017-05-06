
#If TODO Then
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class bidirectional_test
        Inherits [case]

        Private ReadOnly c As shared_component(Of UInt16, UInt16, component, Int32, parameter).
                 collection(Of _max_uint16, _uint16_to_uint32, functor)

        Public Sub New()
            MyBase.New()
            c = _new(c)
        End Sub

        Public Overrides Function run() As Boolean

        End Function
    End Class
End Class
#End If
