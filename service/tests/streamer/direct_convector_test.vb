
Imports osi.root.constants
Imports osi.service.streamer
Imports osi.tests.service.device

Public Class direct_convector_test
    Inherits convector_test

    Protected Overrides Function create_convector(ByVal dev1 As mock_dev_int,
                                                  ByVal dev2 As mock_dev_int) As convector(Of Int32)
        Return New convector(Of Int32)(dev1, dev2, npos, False, npos)
    End Function
End Class
