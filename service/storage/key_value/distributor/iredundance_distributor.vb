
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure

Public Interface iredundance_distributor
    Function wrappered() As key_locked_istrkeyvt
    Function expired() As ref(Of singleentry)
    Function read(ByVal key As String,
                  ByVal result As ref(Of Byte()),
                  ByVal ts As ref(Of Int64),
                  ByVal nodes_result As ref(Of Boolean())) As event_comb
    Function modify(ByVal key As String,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal excluded_nodes() As Boolean) As event_comb
End Interface
