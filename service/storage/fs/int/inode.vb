
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure

Public Interface inode
    Function path() As String
    Function properties(ByVal r As ref(Of vector(Of String))) As event_comb
    Function subnodes(ByVal r As ref(Of vector(Of String))) As event_comb
    Function create(ByVal name As String,
                    ByVal o As ref(Of iproperty),
                    Optional ByVal wait_ms As Int64 = npos) As event_comb
    Function open(ByVal name As String, ByVal o As ref(Of iproperty)) As event_comb
End Interface
