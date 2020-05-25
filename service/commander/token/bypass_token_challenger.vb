
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public NotInheritable Class bypass_token_challenger
    Implements itoken_challenger

    Public Shared ReadOnly instance As bypass_token_challenger

    Shared Sub New()
        instance = New bypass_token_challenger()
    End Sub

    Public Shared Function [New](Of COLLECTION, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 ByVal p As COLLECTION,
                                 ByVal c As CONNECTION) As bypass_token_challenger
        Return instance
    End Function

    Private Sub New()
    End Sub

    Default Public ReadOnly Property challenge(ByVal accepted As ref(Of Boolean)) As event_comb _
                                              Implements itoken_challenger.challenge
        Get
            Return sync_async(Sub()
                                  eva(accepted, True)
                              End Sub)
        End Get
    End Property
End Class