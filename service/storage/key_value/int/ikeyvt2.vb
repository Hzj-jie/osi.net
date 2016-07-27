
Imports osi.root.formation
Imports osi.root.procedure

Public Interface ikeyvt2(Of SEEK_RESULT)
    Function read_existing_timestamp(ByVal key As SEEK_RESULT,
                                     ByVal ts As pointer(Of Int64)) As event_comb
    Function delete_existing_timestamp(ByVal key As SEEK_RESULT,
                                       ByVal result As pointer(Of Boolean)) As event_comb
    Function write_new_timestamp(ByVal key() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As pointer(Of Boolean)) As event_comb
    Function read_existing(ByVal key As SEEK_RESULT,
                           ByVal result As pointer(Of Byte())) As event_comb
    Function append_existing(ByVal key As SEEK_RESULT,
                             ByVal value() As Byte,
                             ByVal result As pointer(Of Boolean)) As event_comb
    Function delete_existing(ByVal key As SEEK_RESULT,
                             ByVal result As pointer(Of Boolean)) As event_comb
    Function seek(ByVal key() As Byte,
                  ByVal r As pointer(Of SEEK_RESULT),
                  ByVal result As pointer(Of Boolean)) As event_comb
    Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb
    Function write_new(ByVal key() As Byte, ByVal value() As Byte, ByVal result As pointer(Of Boolean)) As event_comb
    Function sizeof_existing(ByVal key As SEEK_RESULT, ByVal result As pointer(Of Int64)) As event_comb
    Function full(ByVal result As pointer(Of Boolean)) As event_comb
    Function empty(ByVal result As pointer(Of Boolean)) As event_comb
    Function retire() As event_comb
    Function capacity(ByVal result As pointer(Of Int64)) As event_comb
    Function valuesize(ByVal result As pointer(Of Int64)) As event_comb
    Function keycount(ByVal result As pointer(Of Int64)) As event_comb
    Function heartbeat() As event_comb
    Function [stop]() As event_comb
End Interface
