
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.streamer
Imports osi.service.transmitter

Public NotInheritable Class direct_flower_test
    Inherits flower_test

    Protected Overrides Function create_flower(ByVal first As T_receiver(Of Int32),
                                               ByVal last As T_sender(Of Int32)) As flower(Of Int32)
        Return New direct_flower(Of Int32)(first, last)
    End Function
End Class
