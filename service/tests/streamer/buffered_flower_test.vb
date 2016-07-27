
Imports osi.service.device
Imports osi.service.streamer

Public Class buffered_flower_test
    Inherits flower_test

    Protected Overrides Function create_flower(ByVal first As T_receiver(Of Int32),
                                               ByVal last As T_sender(Of Int32)) As flower(Of Int32)
        Return New buffered_flower(Of Int32)(first, last)
    End Function
End Class
