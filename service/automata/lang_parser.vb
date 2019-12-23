
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Public Interface lang_parser
    Function parse(ByVal txt As String,
                   Optional ByRef words As vector(Of typed_word) = Nothing,
                   Optional ByRef root As typed_node = Nothing) As Boolean

    Function type_id(ByVal name As String, ByRef o As UInt32) As Boolean
    Function type_name(ByVal i As UInt32, ByRef name As String) As Boolean
End Interface
