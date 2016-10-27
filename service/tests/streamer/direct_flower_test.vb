
Imports osi.service.transmitter
Imports osi.service.streamer

Public Class direct_flower_test
    Inherits flower_test

    Protected Overrides Function create_flower(ByVal first As T_receiver(Of Int32),
                                               ByVal last As T_sender(Of Int32)) As flower(Of Int32)
        Return New direct_flower(Of Int32)(first, last)
    End Function
End Class
