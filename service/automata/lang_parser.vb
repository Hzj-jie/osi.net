
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Public Interface lang_parser
    Function parse(ByVal txt As String,
                   Optional ByRef words As vector(Of typed_word) = Nothing,
                   Optional ByRef root As typed_node = Nothing) As Boolean
End Interface
