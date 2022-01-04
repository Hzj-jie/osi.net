
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template

Public Class object_list_writer
    Protected ReadOnly v As New vector(Of Object)()

    Public Function append(ByVal s As String) As Boolean
        ' Allow appending newline characters.
        assert(Not s.null_or_empty())
        v.emplace_back(s)
        Return True
    End Function

    Public Function append(ByVal v As vector(Of pair(Of String, String))) As Boolean
        assert(Not v Is Nothing)
        Return append(lazier.of(Function() As String
                                    Return v.str(Function(ByVal x As pair(Of String, String)) As String
                                                     assert(Not x Is Nothing)
                                                     Return strcat(x.first, character.blank, x.second)
                                                 End Function,
                                                 character.blank)
                                End Function))
    End Function

    Public Function append(Of WRITER As object_list_writer)(ByVal a As Func(Of WRITER, Boolean)) As Boolean
        assert(Not a Is Nothing)
        Return a(direct_cast(Of WRITER)(Me))
    End Function

    Public Function append(Of T)(ByVal obj As T) As Boolean
        assert(Not obj Is Nothing)
        v.emplace_back(obj)
        Return True
    End Function
End Class

Public Class object_list_writer(Of DEBUG_DUMP_T As __void(Of String))
    Inherits object_list_writer
    Private Shared ReadOnly debug_dump As Action(Of String) = AddressOf alloc(Of DEBUG_DUMP_T)().invoke

    Public Function dump() As String
        Dim r As String = v.str(character.blank)
        debug_dump(r)
        Return r
    End Function

    ' Allows an object_list_writer to be injected into another object_list_writer.
    Public NotOverridable Overrides Function ToString() As String
        Return dump()
    End Function
End Class
