
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.resource

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class oneplus
            Private ReadOnly m As unordered_map(Of String, Double)
            Private ReadOnly l As UInt32
            Private ReadOnly t As onebound(Of String).trainer
            Private ReadOnly sample_rate As Double

            Public Sub New(ByVal m As unordered_map(Of String, Double), ByVal sample_rate As Double)
                assert(Not m Is Nothing)
                assert(Not m.empty())
                assert(sample_rate >= 0 AndAlso sample_rate <= 1)
                Me.m = m
                Me.l = (+(m.begin())).first.strlen()
                Me.t = New onebound(Of String).trainer()
                Me.sample_rate = sample_rate
            End Sub

            Public Sub New(ByVal m As onebound(Of Char).model, ByVal sample_rate As Double)
                Me.New(m.to_map(Of String)(), sample_rate)
            End Sub

            Public Sub New(ByVal m As onebound(Of String).model, ByVal sample_rate As Double)
                Me.New(m.to_map(Of String)(), sample_rate)
            End Sub

            Public Sub New(ByVal m As unordered_map(Of String, Double))
                Me.New(m, 1.0)
            End Sub

            Public Sub New(ByVal m As onebound(Of Char).model)
                Me.New(m.to_map(Of String)())
            End Sub

            Public Sub New(ByVal m As onebound(Of String).model)
                Me.New(m.to_map(Of String)())
            End Sub

            Private Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)
                assert([end] >= start)
                If [end] - start <= l Then
                    Return
                End If
                If sample_rate < 1 AndAlso thread_random.ref().NextDouble() >= sample_rate Then
                    Return
                End If
                For i As UInt32 = start To [end] - l - uint32_1
                    Dim it As unordered_map(Of String, Double).iterator = m.find(s.strmid(i, l))
                    If it = m.end() Then
                        Continue For
                    End If
                    If m.find(s.strmid(i + uint32_1, l)) = m.end() Then
                        Continue For
                    End If
                    t.accumulate(s.strmid(i, l), s.strmid(i + l, uint32_1), (+it).second)
                Next
            End Sub

            Public Function train(ByVal s As String) As onebound(Of String).model
                assert(Not s.null_or_whitespace())
                Return train({s})
            End Function

            Private Sub one_str(ByVal s As String)
                If s.null_or_whitespace() Then
                    Return
                End If
                s.strsep(AddressOf _character.not_cjk,
                             Sub(ByVal l As UInt32, ByVal i As UInt32)
                                 sentence(s, l, i)
                             End Sub)
            End Sub

            Public Function train(ByVal ss As IEnumerable(Of String)) As onebound(Of String).model
                For Each s As String In ss
                    one_str(s)
                Next
                Return t.dump()
            End Function

            Public Function train(ByVal reader As tar.reader) As onebound(Of String).model
                assert(Not reader Is Nothing)
                reader.foreach(Sub(ByVal name As String, ByVal p As Double, ByVal r As StreamReader)
                                   If p < 0.8 Then
                                       raise_error(error_type.user,
                                                   "ignroe ",
                                                   name,
                                                   ", the encoding possibility is ",
                                                   p)
                                       Return
                                   End If
                                   Dim line As String = r.ReadLine()
                                   While Not line Is Nothing
                                       one_str(line)
                                       line = r.ReadLine()
                                   End While
                               End Sub)
                Return t.dump()
            End Function
        End Class
    End Class
End Class
