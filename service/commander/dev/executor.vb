
Imports osi.root.connector
Imports osi.root.procedure

Public Interface executor
    Function execute(ByVal i As command, ByVal o As command) As event_comb
End Interface
