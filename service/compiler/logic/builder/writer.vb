
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class writer
        Private ReadOnly v As vector(Of Object)
        Private ReadOnly e As vector(Of String)

        Public Sub New()
            v = New vector(Of Object)()
            e = New vector(Of String)()
        End Sub

        Public Sub append(ByVal s As UInt32)
            v.emplace_back(s)
        End Sub

        Public Sub append(ByVal s As String)
            ' Allow appending newline characters.
            assert(Not s.null_or_empty())
            v.emplace_back(s)
        End Sub

        ' Provide a way to delay the construction of the commands.
        Public MustInherit Class delayed
            Public MustOverride Overrides Function ToString() As String
        End Class

        Public Sub append(ByVal s As delayed)
            assert(Not s Is Nothing)
            v.emplace_back(s)
        End Sub

        Public Sub append(ByVal s As data_block)
            assert(Not s Is Nothing)
            v.emplace_back(s)
        End Sub

        Public Sub append(ByVal v As vector(Of String))
            assert(Not v Is Nothing)
            If Not v.empty() Then
                append(v.str(character.blank))
            End If
        End Sub

        Public Sub append(ByVal v As vector(Of pair(Of String, String)))
            assert(Not v Is Nothing)
            If Not v.empty() Then
                append(v.str(Function(ByVal x As pair(Of String, String)) As String
                                 assert(Not x Is Nothing)
                                 Return strcat(x.first, character.blank, x.second)
                             End Function,
                         character.blank))
            End If
        End Sub

        Public Function append(ByVal a As Func(Of Boolean)) As Boolean
            assert(Not a Is Nothing)
            Return a()
        End Function

        Public Sub err(ByVal ParamArray s() As Object)
            e.emplace_back(strcat(s))
            e.emplace_back(newline.incode())
        End Sub

        Public Function dump() As String
            Dim r As String = Nothing
            r = v.str(character.blank)
            If builders.debug_dump Then
                raise_error(error_type.user, "Debug dump of logic ", r)
            End If
            Return r
        End Function

        Public Function dump(ByVal importer As importer, ByVal v As vector(Of exportable)) As Boolean
            assert(Not importer Is Nothing)
            assert(Not v Is Nothing)
            If Not importer.import(dump(), v) Then
                Return False
            End If
            If v.empty() Then
                Return False
            End If
            ' No harmful to always append a stop at the end.
            v.emplace_back(New [stop]())
            Return True
        End Function

        Public Function dump(ByVal functions As interrupts, ByVal v As vector(Of exportable)) As Boolean
            Return dump(importer.[New](functions), v)
        End Function

        Public Function dump(ByVal v As vector(Of exportable)) As Boolean
            Return dump(importer.[New](), v)
        End Function
    End Class
End Namespace
