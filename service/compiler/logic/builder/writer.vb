
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class writer
        Private ReadOnly s As StringBuilder
        Private ReadOnly e As vector(Of String)

        Public Sub New()
            s = New StringBuilder()
            e = New vector(Of String)()
        End Sub

        Public Sub append(ByVal s As String)
            Me.s.Append(s).Append(character.blank)
        End Sub

        Public Sub append(ByVal i As UInt32)
            append(Convert.ToString(i))
        End Sub

        Public Sub append(ByVal v As vector(Of String))
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                append(v(i))
                i += uint32_1
            End While
        End Sub

        Public Sub append(ByVal v As vector(Of pair(Of String, String)))
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                append(v(i).first)
                append(v(i).second)
                i += uint32_1
            End While
        End Sub

        Public Function append(ByVal a As Func(Of Boolean)) As Boolean
            assert(Not a Is Nothing)
            Return a()
        End Function

        Public Sub append(ByVal d As data_block)
            assert(Not d Is Nothing)
            append(Convert.ToString(d))
        End Sub

        Public Sub err(ByVal ParamArray s() As Object)
            e.emplace_back(strcat(s))
        End Sub

        Public Function dump() As String
            Dim r As String = Nothing
            r = Convert.ToString(s)
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
