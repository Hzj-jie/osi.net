
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
    End Class
End Namespace
