
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.interpreter.primitive

Public Class lazy_list_writer
    Protected ReadOnly v As New vector(Of Func(Of String))()

    Public Function append(ByVal s As String) As Boolean
        ' Allow appending newline characters.
        assert(Not s.null_or_empty())
        Return append(Function() As String
                          Return s
                      End Function)
    End Function

    Public Function append(ByVal v As UInt32) As Boolean
        Return append(Convert.ToString(v))
    End Function

    Public Function append(ByVal d As data_block) As Boolean
        assert(Not d Is Nothing)
        Return append(AddressOf d.ToString)
    End Function

    Public Function append(ByVal v As vector(Of String)) As Boolean
        assert(Not v Is Nothing)
        Return append(Function() As String
                          Return v.str()
                      End Function)
    End Function

    Public Function append(ByVal v As vector(Of pair(Of String, String))) As Boolean
        assert(Not v Is Nothing)
        Return append(Function() As String
                          Return v.str(Function(ByVal x As pair(Of String, String)) As String
                                           assert(Not x Is Nothing)
                                           Return strcat(x.first, character.blank, x.second)
                                       End Function,
                                       character.blank)
                      End Function)
    End Function

    Public Function append(Of WRITER As lazy_list_writer)(ByVal a As Func(Of WRITER, Boolean)) As Boolean
        assert(Not a Is Nothing)
        Return a(direct_cast(Of WRITER)(Me))
    End Function

    Public Function append(ByVal f As Func(Of String)) As Boolean
        assert(Not f Is Nothing)
        v.emplace_back(f)
        Return True
    End Function

    Public Function append(ByVal w As lazy_list_writer) As Boolean
        assert(Not w Is Nothing)
        assert(Not Object.ReferenceEquals(Me, w))
        Return append(AddressOf w.str)
    End Function

    Public Function append(ByVal r As ref(Of String)) As Boolean
        assert(Not r Is Nothing)
        Return append(AddressOf r.ToString)
    End Function

    ' Allows a lazy_list_writer to be injected into another lazy_list_writer. Note, the debug_dump_t won't work for
    ' this function, the outer lazy_list_writer should take care of it.
    Public Function str() As String
        Return v.str(Function(ByVal x As Func(Of String)) As String
                         assert(Not x Is Nothing)
                         Return x()
                     End Function,
                     character.blank)
    End Function

    ' Used by code_gen.dump.
    Public NotOverridable Overrides Function ToString() As String
        Return str()
    End Function
End Class

Public Class lazy_list_writer(Of DEBUG_DUMP_T As __void(Of String))
    Inherits lazy_list_writer
    Private Shared ReadOnly debug_dump As Action(Of String) = AddressOf alloc(Of DEBUG_DUMP_T)().invoke

    Public Function dump() As String
        Dim r As String = str()
        debug_dump(r)
        Return r
    End Function
End Class
