
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation

Public Interface lang_parser
    Function parse(ByVal txt As String,
                   Optional ByRef words As vector(Of typed_word) = Nothing,
                   Optional ByRef root As typed_node = Nothing) As Boolean

    Function type_id(ByVal name As String, ByRef o As UInt32) As Boolean
    Function type_name(ByVal i As UInt32, ByRef name As String) As Boolean
End Interface

Public Module _lang_parser
    <Extension()> Public Function type_name(ByVal lp As lang_parser,
                                            ByVal n As typed_node,
                                            ByRef name As String) As Boolean
        throws.not_null(lp)
        assert(Not n Is Nothing)
        Return lp.type_name(n.type, name)
    End Function

    <Extension()> Public Function type_id(ByVal lp As lang_parser, ByVal name As String) As UInt32
        throws.not_null(lp)
        assert(Not name.null_or_whitespace())
        Dim o As UInt32 = 0
        assert(lp.type_id(name, o))
        Return o
    End Function

    <Extension()> Public Function type_name(ByVal lp As lang_parser, ByVal id As UInt32) As String
        throws.not_null(lp)
        Dim o As String = Nothing
        assert(lp.type_name(id, o))
        Return o
    End Function

    <Extension()> Public Function type_name(ByVal lp As lang_parser, ByVal n As typed_node) As String
        assert(Not n Is Nothing)
        Return type_name(lp, n.type)
    End Function
End Module
