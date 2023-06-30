
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Interface istrkeyvt
    Function read(ByVal key As String,
                  ByVal result As ref(Of Byte()),
                  ByVal ts As ref(Of Int64)) As event_comb
    Function append(ByVal key As String,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As ref(Of Boolean)) As event_comb
    Function delete(ByVal key As String, ByVal result As ref(Of Boolean)) As event_comb
    Function seek(ByVal key As String, ByVal result As ref(Of Boolean)) As event_comb
    Function list(ByVal result As ref(Of vector(Of String))) As event_comb
    Function modify(ByVal key As String,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As ref(Of Boolean)) As event_comb
    Function sizeof(ByVal key As String, ByVal result As ref(Of Int64)) As event_comb
    Function full(ByVal result As ref(Of Boolean)) As event_comb
    Function empty(ByVal result As ref(Of Boolean)) As event_comb
    Function retire() As event_comb
    Function capacity(ByVal result As ref(Of Int64)) As event_comb
    Function valuesize(ByVal result As ref(Of Int64)) As event_comb
    Function keycount(ByVal result As ref(Of Int64)) As event_comb
    Function heartbeat() As event_comb
    Function [stop]() As event_comb
    Function unique_write(ByVal key As String,
                          ByVal value() As Byte,
                          ByVal ts As Int64,
                          ByVal result As ref(Of Boolean)) As event_comb
End Interface
