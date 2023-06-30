
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class bypass_token_defender
    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal powerpoints As object_unique_set(Of COLLECTION),
                                 ByVal info As token_info(Of COLLECTION, CONNECTION)) _
                                As bypass_token_defender(Of COLLECTION, CONNECTION)
        Return New bypass_token_defender(Of COLLECTION, CONNECTION)(powerpoints, info)
    End Function

    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 Optional ByVal max_connecting As UInt32 = uint32_0,
                                 Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0) _
                                As bypass_token_defender(Of COLLECTION, CONNECTION)
        Return New bypass_token_defender(Of COLLECTION, CONNECTION)(info)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class bypass_token_defender(Of COLLECTION As Class, CONNECTION)
    Inherits itoken_defender(Of COLLECTION, CONNECTION)

    Public Sub New(ByVal powerpoints As object_unique_set(Of COLLECTION),
                   ByVal info As token_info(Of COLLECTION, CONNECTION))
        MyBase.New(powerpoints, info, max_uint32, int64_1)
    End Sub

    Public Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION))
        MyBase.New(info, max_uint32, int64_1)
    End Sub

    Protected Overrides Function verify_token(ByVal c As CONNECTION,
                                              ByVal p As COLLECTION,
                                              ByVal r As ref(Of COLLECTION)) As event_comb
        Return sync_async(Sub()
                              eva(r, p)
                          End Sub)
    End Function
End Class
