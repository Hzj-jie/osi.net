
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template

Public Interface istrkeyvt
    Function read(ByVal key As String,
                  ByVal result As pointer(Of Byte()),
                  ByVal ts As pointer(Of Int64)) As event_comb
    Function append(ByVal key As String,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As pointer(Of Boolean)) As event_comb
    Function delete(ByVal key As String, ByVal result As pointer(Of Boolean)) As event_comb
    Function seek(ByVal key As String, ByVal result As pointer(Of Boolean)) As event_comb
    Function list(ByVal result As pointer(Of vector(Of String))) As event_comb
    Function modify(ByVal key As String,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As pointer(Of Boolean)) As event_comb
    Function sizeof(ByVal key As String, ByVal result As pointer(Of Int64)) As event_comb
    Function full(ByVal result As pointer(Of Boolean)) As event_comb
    Function empty(ByVal result As pointer(Of Boolean)) As event_comb
    Function retire() As event_comb
    Function capacity(ByVal result As pointer(Of Int64)) As event_comb
    Function valuesize(ByVal result As pointer(Of Int64)) As event_comb
    Function keycount(ByVal result As pointer(Of Int64)) As event_comb
    Function heartbeat() As event_comb
    Function [stop]() As event_comb
    Function unique_write(ByVal key As String,
                          ByVal value() As Byte,
                          ByVal ts As Int64,
                          ByVal result As pointer(Of Boolean)) As event_comb
End Interface
